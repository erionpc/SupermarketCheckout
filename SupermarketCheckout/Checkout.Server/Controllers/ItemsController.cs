using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Checkout.Server.Data.Repositories;
using Checkout.Server.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Checkout.Shared;

namespace Checkout.Server.Controllers
{
    [Authorize(Roles = UserRoles.Pos)]
    [ApiController]
    [Route("api/items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository _itemsRepo;
        private readonly IMapper _mapper;

        public ItemsController(IItemsRepository itemsRepo, IMapper mapper)
        {
            _itemsRepo = itemsRepo ?? 
                throw new ArgumentNullException(nameof(itemsRepo));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> Get()
        {
            var items = await _itemsRepo.GetItems();
            return Ok(_mapper.Map<IEnumerable<ItemDto>>(items));
        }
    }
}
