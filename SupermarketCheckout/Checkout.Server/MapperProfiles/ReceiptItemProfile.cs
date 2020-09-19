using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Server.MapperProfiles
{
    public class ReceiptItemProfile : Profile
    {
        public ReceiptItemProfile()
        {
            CreateMap<Models.ReceiptItemDto, Data.Entities.ReceiptItem>();
        }
    }
}
