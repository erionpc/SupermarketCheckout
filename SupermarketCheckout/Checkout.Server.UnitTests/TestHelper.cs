using Checkout.Server.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.Server.UnitTests
{
    public static class TestHelper
    {
        public static Dictionary<string, ItemDto> GetTestInventory() =>
            new Dictionary<string, ItemDto>()
            {
                {
                    "A",
                    new ItemDto()
                    {
                        SKU = "A",
                        Prices = new List<ItemPriceDto>()
                        {
                            new ItemPriceDto()
                            {
                                Amount = 50
                            },
                            new ItemPriceDto()
                            {
                                Quantity = 3,
                                Amount = 130
                            },
                            new ItemPriceDto()
                            {
                                Quantity = 4,
                                Amount = 160
                            },
                            new ItemPriceDto()
                            {
                                Quantity = 5,
                                Amount = 180
                            }
                        }
                    }
                },
                {
                    "B",
                    new ItemDto()
                    {
                        SKU = "B",
                        Prices = new List<ItemPriceDto>()
                        {
                            new ItemPriceDto()
                            {
                                Amount = 30
                            },
                            new ItemPriceDto()
                            {
                                Quantity = 2,
                                Amount = 45
                            }
                        }
                    }
                },
                {
                    "C",
                    new ItemDto()
                    {
                        SKU = "C",
                        Prices = new List<ItemPriceDto>()
                        {
                            new ItemPriceDto()
                            {
                                Amount = 20
                            }
                        }
                    }
                },
                {
                    "D",
                    new ItemDto()
                    {
                        SKU = "D",
                        Prices = new List<ItemPriceDto>()
                        {
                            new ItemPriceDto()
                            {
                                Amount = 15
                            }
                        }
                    }
                }
            };
    }
}
