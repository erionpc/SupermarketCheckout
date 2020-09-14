using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Server.MapperProfiles
{
    public class ItemPriceProfile : Profile
    {
        public ItemPriceProfile()
        {
            CreateMap<Data.Entities.ItemPrice, Models.ItemPriceDto>();
        }
    }
}
