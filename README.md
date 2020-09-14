# SupermarketCheckout
Conceptual solution for a supermarket checkout

The aim of this project is to demonstrate Production Ready Good Quality code in implementing a supermarket checkout that calculates 
the total price of a number of items. 

In a normal supermarket, things are identified using Stock Keeping Units, or SKUs. In our store, we’ll use individual letters of the 
alphabet (A, B, C, and so on). Our goods are priced individually. In addition, some items are multipriced: buy n of them, and they’ll 
cost you y pounds. For example, item ‘A’ might cost 50 pounds individually, but this week we have a special offer: buy three ‘A’s and 
they’ll cost you 130. There can also be multiple offers for the same item. The test data is as follows:
<br/><br/>

| Item | #Price  | #Offer                           | 
| :---:| :-:     | :-:                              |
| A    | 50      | 3 for 130, 4 for 160, 5 for 180  |
| B    | 30      | 2 for 40                         |
| C    | 20      |                                  |
| D    | 15      |                                  |
<br/>
Our checkout accepts items in any order, so that if we scan a B, an A, and another B, we’ll recognize the two B’s and price them at 45 
(for a total price so far of 95).
<br/><br/>

For item A the price calculation combines the offers on the basis of the price defined for the largest quantity.
E.g. for 14 A the price is calculated as (180 * 2) + 160 = 520.
<br/><br/>
The ReceiptItemPriceCalculator class calculates the basket item price on the basis of the above logic. It also calculates the offer text on the basis of the price applied.
When more than 1 offer applies, the offer text becomes "multiple offers applied".
The CheckoutService class composes the receipt on the basis of the items in the basket and also calculates the total.
<br/><br/>
The BasketItemsController can be used to add 1 item at a time to the basket.
The BasketController can be used to create a new basket.
<br/><br/>
The solution is a RESTful web-api based on ASP.NET Core, Entity Framework Core and SqlServer (localdb).
There are unit tests based on xUnit which test the main logic with several input scenarios.
The database is seeded with demo data upon start-up.
<br/><br/>
To test the solution use PostMan (or any REST client). A PostMan collection and environment is supplied for this:<br/>
[SupermarketCheckout.postman_collection.json](SupermarketCheckout.postman_collection.json)<br/>
[env.postman_environment.json](env.postman_environment.json)

