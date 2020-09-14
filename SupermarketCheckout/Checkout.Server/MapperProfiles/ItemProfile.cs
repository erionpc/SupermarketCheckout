using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Server.MapperProfiles
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Data.Entities.Item, Models.ItemDto>();
            CreateMap<Models.ItemForAddToBasketDto, Data.Entities.Item>();
        }
    }
}
