using Checkout.Server.Models;
using Checkout.Server.Services;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Checkout.Server.UnitTests
{
    public class CheckoutServiceTests
    {
        public static IEnumerable<object[]> CalculateTotalPriceTestData()
        {
            var shopInventory = TestHelper.GetTestInventory();

            yield return new object[]
            {
                "test with null list of items",
                null,
                0
            };

            yield return new object[]
            {
                "test with empty list of items",
                new List<BasketItemDto>(),
                0
            };

            yield return new object[]
            {
                "test with null item",
                new List<BasketItemDto>()
                {
                    new BasketItemDto()
                },
                0,
                "Inventory not loaded"
            };

            yield return new object[]
            {
                "test with no prices",
                new List<BasketItemDto>()
                {
                    new BasketItemDto()
                    {
                        Item = new ItemDto()
                    }
                },
                0,
                "Prices not loaded"
            };

            yield return new object[]
            {
                "test with no unit price",
                new List<BasketItemDto>()
                {
                    new BasketItemDto()
                    {
                        Item = new ItemDto()
                        {
                            Prices = new List<ItemPriceDto>()
                            {
                                new ItemPriceDto()
                                {
                                    Quantity = 2,
                                    Amount = 30
                                }
                            }
                        }
                    }
                },
                0,
                "Unit prices not specified"
            };

            yield return new object[]
            {
                "test with A",
                new List<BasketItemDto>() 
                {
                    new BasketItemDto()
                    {
                        Item = shopInventory["A"]
                    }
                },
                shopInventory["A"].Prices.Single(x => x.Quantity == 1).Amount
            };

            yield return new object[]
            {
                "test with AB",
                new List<BasketItemDto>()
                {
                    new BasketItemDto()
                    {
                        Item = shopInventory["A"]
                    },
                    new BasketItemDto()
                    {
                        Item = shopInventory["B"]
                    }
                },
                shopInventory["A"].Prices.Single(x => x.Quantity == 1).Amount + 
                shopInventory["B"].Prices.Single(x => x.Quantity == 1).Amount
            };

            yield return new object[]
            {
                "test with CDBA",
                new List<BasketItemDto>()
                {
                    new BasketItemDto()
                    {
                        Item = shopInventory["C"]
                    },
                    new BasketItemDto()
                    {
                        Item = shopInventory["D"]
                    },
                    new BasketItemDto()
                    {
                        Item = shopInventory["B"]
                    },
                    new BasketItemDto()
                    {
                        Item = shopInventory["A"]
                    }
                },
                shopInventory["C"].Prices.Single(x => x.Quantity == 1).Amount +
                shopInventory["D"].Prices.Single(x => x.Quantity == 1).Amount +
                shopInventory["B"].Prices.Single(x => x.Quantity == 1).Amount +
                shopInventory["A"].Prices.Single(x => x.Quantity == 1).Amount
            };

            yield return new object[]
            {
                "test with AA",
                new List<BasketItemDto>()
                {
                    new BasketItemDto()
                    {
                        Item = shopInventory["A"]
                    },
                    new BasketItemDto()
                    {
                        Item = shopInventory["A"]
                    }
                },
                shopInventory["A"].Prices.FirstOrDefault(x => x.Quantity == 2)?.Amount ??
                shopInventory["A"].Prices.Single(x => x.Quantity == 1)?.Amount * 2
            };

            yield return new object[]
            {
                "test with AAA",
                new List<BasketItemDto>()
                {
                    new BasketItemDto()
                    {
                        Item = shopInventory["A"]
                    },
                    new BasketItemDto()
                    {
                        Item = shopInventory["A"]
                    },
                    new BasketItemDto()
                    {
                        Item = shopInventory["A"]
                    }
                },
                shopInventory["A"].Prices.FirstOrDefault(x => x.Quantity == 3)?.Amount ??
                shopInventory["A"].Prices.Single(x => x.Quantity == 1)?.Amount * 3
            };

            yield return new object[]
            {
                "test with AAABB",
                new List<BasketItemDto>()
                {
                    new BasketItemDto()
                    {
                        Item = shopInventory["A"]
                    },
                    new BasketItemDto()
                    {
                        Item = shopInventory["A"]
                    },
                    new BasketItemDto()
                    {
                        Item = shopInventory["A"]
                    },
                    new BasketItemDto()
                    {
                        Item = shopInventory["B"]
                    },
                    new BasketItemDto()
                    {
                        Item = shopInventory["B"]
                    }
                },
                (shopInventory["A"].Prices.FirstOrDefault(x => x.Quantity == 3)?.Amount ??
                 shopInventory["A"].Prices.Single(x => x.Quantity == 1)?.Amount * 3) +
                (shopInventory["B"].Prices.FirstOrDefault(x => x.Quantity == 2)?.Amount ??
                 shopInventory["B"].Prices.Single(x => x.Quantity == 1)?.Amount * 2)
            };
        }

        [Theory]
        [MemberData(nameof(CalculateTotalPriceTestData))]
        public void CalculateTotalPriceTest(string testCaseName, ICollection<BasketItemDto> basketItems, decimal expectedResult, string expectedExceptionMessage = null)
        {
            var calculator = new CheckoutService(new ReceiptItemPriceCalculator());
            try
            {
                var actual = calculator.CreateReceipt(basketItems, Guid.NewGuid())?.TotalPrice ?? 0;
                Assert.Equal(expectedResult, actual);
            }
            catch (CheckoutException ex)
            {
                Assert.Equal(expectedExceptionMessage, ex.Message);
            }
        }
    }
}
