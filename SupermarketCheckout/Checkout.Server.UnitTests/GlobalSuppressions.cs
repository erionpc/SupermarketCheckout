// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters", Justification = "The testCaseName parameter is used to identify failing test cases more easily", Scope = "member", Target = "~M:Checkout.Server.UnitTests.ReceiptItemPriceCalculatorTests.CalculateReceiptItemPriceTest(System.String,System.Collections.Generic.ICollection{Checkout.Server.Models.ItemPriceDto},System.Int32,System.Tuple{System.Decimal,System.String},System.Exception)")]
[assembly: SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters", Justification = "The testCaseName parameter is used to identify failing test cases more easily", Scope = "member", Target = "~M:Checkout.Server.UnitTests.CheckoutServiceTests.CalculateTotalPriceTest(System.String,System.Collections.Generic.ICollection{Checkout.Server.Models.BasketItemDto},System.Decimal,System.String)")]
