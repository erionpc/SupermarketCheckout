using Checkout.Server.Models;
using Checkout.Server.Services;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace Checkout.Server.UnitTests
{
    public class ReceiptItemPriceCalculatorTests
    {
        public static IEnumerable<object[]> CalculateReceiptItemPriceTestData()
        {
            var shopInventory = TestHelper.GetTestInventory();

            yield return new object[]
            {
                "test with null list of prices",
                null,
                5,
                null,
                new ArgumentException("prices not defined")
            };

            yield return new object[]
            {
                "test with empty list of prices",
                new List<ItemPriceDto>(),
                5,
                null,
                new ArgumentException("prices not defined")
            };

            yield return new object[]
            {
                "test with zero quantity",
                shopInventory["A"].Prices,
                0,
                new Tuple<decimal, string>(0, null)
            };

            yield return new object[]
            {
                "test with 1",
                shopInventory["A"].Prices,
                1,
                new Tuple<decimal, string>(50, null)
            };

            yield return new object[]
            {
                "test with 2",
                shopInventory["A"].Prices,
                2,
                new Tuple<decimal, string>(50 * 2, null)
            };

            yield return new object[]
            {
                "test with 3",
                shopInventory["A"].Prices,
                3,
                new Tuple<decimal, string>(130, "3 for 130")
            };

            yield return new object[]
            {
                "test with 4",
                shopInventory["A"].Prices,
                4,
                new Tuple<decimal, string>(160, "4 for 160")
            };

            yield return new object[]
            {
                "test with 5",
                shopInventory["A"].Prices,
                5,
                new Tuple<decimal, string>(180, "5 for 180")
            };

            yield return new object[]
            {
                "test with 6",
                shopInventory["A"].Prices,
                6,
                new Tuple<decimal, string>(180 + 50, "5 for 180")
            };

            yield return new object[]
            {
                "test with 7",
                shopInventory["A"].Prices,
                7,
                new Tuple<decimal, string>(180 + (50 * 2), "5 for 180")
            };

            yield return new object[]
            {
                "test with 8",
                shopInventory["A"].Prices,
                8,
                new Tuple<decimal, string>(180 + 130, "multiple offers applied")
            };

            yield return new object[]
            {
                "test with 9",
                shopInventory["A"].Prices,
                9,
                new Tuple<decimal, string>(180 + 160, "multiple offers applied")
            };

            yield return new object[]
            {
                "test with 10",
                shopInventory["A"].Prices,
                10,
                new Tuple<decimal, string>(180 * 2, "5 for 180")
            };

            yield return new object[]
            {
                "test with 11",
                shopInventory["A"].Prices,
                11,
                new Tuple<decimal, string>((180 * 2) + 50, "5 for 180")
            };

            yield return new object[]
            {
                "test with 12",
                shopInventory["A"].Prices,
                12,
                new Tuple<decimal, string>((180 * 2) + (50 * 2), "5 for 180")
            };

            yield return new object[]
            {
                "test with 13",
                shopInventory["A"].Prices,
                13,
                new Tuple<decimal, string>((180 * 2) + 130, "multiple offers applied")
            };

            yield return new object[]
            {
                "test with 14",
                shopInventory["A"].Prices,
                14,
                new Tuple<decimal, string>((180 * 2) + 160, "multiple offers applied")
            };

            yield return new object[]
            {
                "test with 15",
                shopInventory["A"].Prices,
                15,
                new Tuple<decimal, string>(180 * 3, "5 for 180")
            };

            yield return new object[]
            {
                "test with 16",
                shopInventory["A"].Prices,
                16,
                new Tuple<decimal, string>((180 * 3) + 50, "5 for 180")
            };

            yield return new object[]
            {
                "test with 17",
                shopInventory["A"].Prices,
                17,
                new Tuple<decimal, string>((180 * 3) + (50 * 2), "5 for 180")
            };

            yield return new object[]
            {
                "test with 18",
                shopInventory["A"].Prices,
                18,
                new Tuple<decimal, string>((180 * 3) + 130, "multiple offers applied")
            };

            yield return new object[]
            {
                "test with 19",
                shopInventory["A"].Prices,
                19,
                new Tuple<decimal, string>((180 * 3) + 160, "multiple offers applied")
            };

            yield return new object[]
            {
                "test with 20",
                shopInventory["A"].Prices,
                20,
                new Tuple<decimal, string>((180 * 4), "5 for 180")
            };
        }

        [Theory]
        [MemberData(nameof(CalculateReceiptItemPriceTestData))]        
        public void CalculateReceiptItemPriceTest(string testCaseName, ICollection<ItemPriceDto> prices, int quantity, Tuple<decimal, string> expectedResult, Exception expectedException = null)
        {
            var calculator = new ReceiptItemPriceCalculator();
            try
            {
                var actual = calculator.CalculateReceiptItemPrice(prices, quantity);
                // assert price
                Assert.Equal(expectedResult.Item1, actual.Item1);
                // assert offer text
                Assert.Equal(expectedResult.Item2, actual.Item2);
            }
            catch (CheckoutException ex)
            {
                Assert.Equal(expectedException.Message, ex.Message);
            }
        }
    }
}
