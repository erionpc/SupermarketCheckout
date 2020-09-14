using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Server.MapperProfiles
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<Data.Entities.Basket, Models.BasketDto>();
            CreateMap<Models.BasketForCreationDto, Data.Entities.Basket>();
        }
    }
}
