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
using Checkout.Server.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Checkout.Shared;

namespace Checkout.Server.Controllers
{
    [Authorize(Roles = UserRoles.Pos)]
    [ApiController]
    [Route("api/basket")]
    public class BasketController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBasketRepository _basketRepo;
        private readonly ICheckoutService _checkoutService;
        private readonly IReceiptItemsRepository _receiptItemsRepository;

        public BasketController(IMapper mapper, 
                                IBasketRepository basketRepo, 
                                ICheckoutService checkoutService,
                                IReceiptItemsRepository receiptItemsRepository)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _basketRepo = basketRepo ??
                throw new ArgumentNullException(nameof(basketRepo));
            _checkoutService = checkoutService ??
                throw new ArgumentNullException(nameof(checkoutService));
            _receiptItemsRepository = receiptItemsRepository ??
                throw new ArgumentNullException(nameof(receiptItemsRepository));
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> Create([FromBody] BasketForCreationDto basket)
        {
            var basketEntity = _mapper.Map<Data.Entities.Basket>(basket);
            await _basketRepo.CreateBasket(basketEntity);
            await _basketRepo.Save();

            return Ok(_mapper.Map<BasketDto>(basketEntity));
        }

        [HttpPost("{basketId}/checkout")]
        public async Task<ActionResult<BasketDto>> Checkout(Guid basketId)
        {
            var basketEntity = await _basketRepo.GetFullBasket(basketId);
            if (basketEntity == null)
            {
                return NotFound("Basket not found");
            }
            if (basketEntity.Status != Data.Entities.Enums.BasketStatus.Active)
            {
                return Conflict("Cannot checkout a closed basket");
            }

            var receipt = _checkoutService.CreateReceipt(_mapper.Map<ICollection<BasketItemDto>>(basketEntity.BasketItems), basketEntity.PosId);
            _basketRepo.CheckoutBasket(basketEntity, receipt.TotalPrice, _mapper.Map<ICollection<Data.Entities.ReceiptItem>>(receipt.Items));
            await _basketRepo.Save();

            return Ok(receipt);
        }

        [HttpPost("{basketId}/reopen")]
        public async Task<ActionResult<BasketDto>> Reopen(Guid basketId)
        {
            var basketEntity = await _basketRepo.GetBasket(basketId);
            if (basketEntity == null)
            {
                return NotFound("Basket not found");
            }
            if (basketEntity.Status == Data.Entities.Enums.BasketStatus.Active)
            {
                return Conflict("The basket is already open");
            }

            _basketRepo.ReopenBasket(basketEntity);
            
            // need to delete the receipt items for the reopened basket to avoid conflicts
            await _receiptItemsRepository.DeleteBasketReceiptItems(basketEntity);

            await _basketRepo.Save();

            return Ok();
        }
    }
}