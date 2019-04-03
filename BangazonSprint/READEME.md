# BangazonAPI
Welcome to **Bangazon!** The new virtual marketplace. This marketplace allows customers to buy and sell their products through a single page application web page and its data is tracked through a powerful, hand crafted and solely dedicated API. 
## Software Requirements
Sql Server Manangment Studio
Visual Studio Community 2017
## Enitity Relationship Diagram
![alt text](https://i.imgur.com/z0rFYz0.png)


## Database Setup
In Visual Studio right click on ```BangazonSprint``` and select ```Add -> New Item...```
when the window pops up select ```Data``` underneath ```ASP.NET Core``` and choose ```JSON File``` and name it ```appsettings.json``` then click ```add```
copy the contents of the ```BoilerplateAppSettings.json``` into ```appsettings.json``` then open ```SSMS``` and copy the contents of the ```Server name``` text box and paste where it says ```INSERT_DATABASE_CONNECTION_HERE```
then replace ```INSERT_DATABASE_NAME``` with the name of your database that you've created. 



### Http Request Methods

### 3. PaymentType
Use the command ```dotnet run``` to start the program, BangazonAPI. Once the program is running, open up the Postman desktop app and run the following commands for each request method:
 ##### GET
 To GET all payment types, select GET in Postman then paste ```localhost:5000/PaymentType``` into the field and click send. The result should be an array of all the payment type in the database that should look like:
 ```
 [
  {
    "Id": 1,
    "Name": "Visa",
	"AcctNumber": 12345667,
	"CustomerId": 1
  },
  {
    "Id": 2,
    "Name": "MasterCard",
	"AcctNumber": 23452346,
	"CustomerId": 2
  },
  {
    "Id": 3,
    "Name": "BlackCard",
	"AcctNumber": 123375457,
	"CustomerId": 1
  }
 ]
 ```
 To GET a specific, single payment type, add an /{id} to the end of the ```localhost:5000/PaymentType``` URL. The result should only include the single payment type with the Id you added like the below:  
```
[
  {
    "Id": 1,
    "Name": "Visa",
	"AcctNumber": 123375457,
	"CustomerId": 1
  }
]
```
 ##### POST
 To POST a new object to your existing array for PaymentType, select POST, then paste ```localhost:5000/PaymentType``` into the field. Then click Body underneath the field, select raw, and then paste this below snippet or make one similar then click send. The result should be the new PaymentType you made:
```
{
	"Name": "Visa",
	"AcctNumber": 123375457,
	"CustomerId": 1
}
```
##### PUT
 To update an existing PaymentType, select PUT then paste ```localhost:5000/paymentType/2``` or any other existing order. Then follow the same directions as the POST example, and change the values then click send: 
```
{
	"Name": "Visa",
	"AcctNumber": 123375457,
	"CustomerId": 1
}
```
You should get nothing back from this besides an OK status. When you run the GET query the computer you specified in your PUT query should show the updated, edited information you gave it.
 
 ##### DELETE
 To DELETE an existing product type, select DELETE then paste ```localhost:5000/PaymentType/2``` or any other existing PaymentType then click send. You should get nothing back from this besides an OK status. When you run the GET query the order with the Id you specified in your DELETE query should no longer exist.

 ### 3. ProductType
Use the command ```dotnet run``` to start the program, BangazonAPI. Once the program is running, open up the Postman desktop app and run the following commands for each request method:
 ##### GET
 To GET all product types, select GET in Postman then paste ```localhost:5000/ProductType``` into the field and click send. The result should be an array of all the product types in the database that should look like:
 ```
 [
  {
    "Id": 1,
    "Name": "Toys"
  },
  {
    "Id": 2,
    "Name": "Tools"
  },
  {
    "Id": 3,
    "Name": "Outdoors"
  }
 ]
 ```
 To GET a specific, single product type, add an /{id} to the end of the ```localhost:5000/ProductType``` URL. The result should only include the single product type with the Id you added like the below:  
```
[
  {
    "Id": 1,
    "Name": "Toys"
  }
]
```
 ##### POST
 To POST a new object to your existing array for ProductType, select POST, then paste ```localhost:5000/ProductType``` into the field. Then click Body underneath the field, select raw, and then paste this below snippet or make one similar then click send. The result should be the new ProductType you made:
```
{
	"Name": "Kitchen"
}
```
##### PUT
 To update an existing ProductType, select PUT then paste ```localhost:5000/productType/2``` or any other existing order. Then follow the same directions as the POST example, and change the values then click send: 
```
{
	"Name": "Furniture"
}
```
You should get nothing back from this besides an OK status. When you run the GET query the computer you specified in your PUT query should show the updated, edited information you gave it.
 
 ##### DELETE
 To DELETE an existing product type, select DELETE then paste ```localhost:5000/ProductType/2``` or any other existing ProductType then click send. You should get nothing back from this besides an OK status. When you run the GET query the order with the Id you specified in your DELETE query should no longer exist.

