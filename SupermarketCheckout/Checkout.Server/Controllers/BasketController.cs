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
    [Route("api/basket")]
    public class BasketController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBasketRepository _basketRepo;

        public BasketController(IMapper mapper, IBasketRepository basketRepo)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _basketRepo = basketRepo ??
                throw new ArgumentNullException(nameof(basketRepo));
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> Create([FromBody] BasketForCreationDto basket)
        {
            var basketEntity = _mapper.Map<Data.Entities.Basket>(basket);
            await _basketRepo.CreateBasket(basketEntity);
            await _basketRepo.Save();

            return Ok(_mapper.Map<BasketDto>(basketEntity));
        }
    }
}