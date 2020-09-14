using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Server.MapperProfiles
{
    public class BasketItemProfile : Profile
    {
        public BasketItemProfile()
        {
            CreateMap<Data.Entities.BasketItem, Models.BasketItemDto>();
            CreateMap<Models.BasketItemForCreationDto, Data.Entities.BasketItem>();
        }
    }
}
