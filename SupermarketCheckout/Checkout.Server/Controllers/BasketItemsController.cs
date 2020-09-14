

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Checkout.Server.Data.Repositories;
using Checkout.Server.Models;
using Checkout.Server.Services;

namespace Checkout.Server.Controllers
{
    [ApiController]
    [Route("api/basket/{basketId}/items")]
    public class BasketItemsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBasketRepository _basketRepo;
        private readonly IItemsRepository _itemsRepo;
        private readonly ICheckoutService _checkoutService;

        public BasketItemsController(IMapper mapper, IBasketRepository basketRepo, IItemsRepository itemsRepo, ICheckoutService checkoutService)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _basketRepo = basketRepo ??
                throw new ArgumentNullException(nameof(basketRepo));
            _itemsRepo = itemsRepo ??
                throw new ArgumentNullException(nameof(itemsRepo));
            _checkoutService = checkoutService ??
                throw new ArgumentNullException(nameof(checkoutService));
        }

        [HttpPost]
        public async Task<ActionResult<ReceiptDto>> AddBasketItem(Guid basketId, BasketItemForCreationDto basketItem)
        {
            var basketExists = await _basketRepo.Exists(basketId);
            if (!basketExists)
            {
                return NotFound("Basket not found");
            }    

            if (basketItem.Item == null)
            {
                return BadRequest("Item not specified");
            }

            var item = await _itemsRepo.GetItemBySKU(basketItem.Item.SKU);
            if (item == null)
            {
                return NotFound();
            }

            var basketItemEntity = _mapper.Map<Data.Entities.BasketItem>(basketItem);

            await _basketRepo.AddToBasket(basketItemEntity, basketId, item.Id);
            await _basketRepo.Save();

            var basketEntity = await _basketRepo.GetFullBasket(basketId);

            return Ok(_checkoutService.CreateReceipt(_mapper.Map<ICollection<Models.BasketItemDto>>(basketEntity.BasketItems), basketEntity.PosId));
        }
    }
}
