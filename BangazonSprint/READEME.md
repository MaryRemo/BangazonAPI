# BangazonAPI
Welcome to **Bangazon!** The new virtual marketplace. This marketplace allows customers to buy and sell their products through a single page application web page and its data is tracked through a powerful, hand crafted and solely dedicated API. 
## Software Requirements
Sql Server Manangment Studio
Visual Studio Community 2017
Postman


## Senior Dev:
##### Leah Hoefling

## Contributors:
##### Austin Blade
##### Hunter Metts
##### Hannah Neal
##### Mary Remo

## Entity Relationship Diagram
![alt text](https://i.imgur.com/z0rFYz0.png)

## Database Setup
In Visual Studio right click on ```BangazonSprint``` and select ```Add -> New Item...```
when the window pops up select ```Data``` underneath ```ASP.NET Core``` and choose ```JSON File``` and name it ```appsettings.json``` then click ```add```
copy the contents of the ```BoilerplateAppSettings.json``` into ```appsettings.json``` then open ```SSMS``` and copy the contents of the ```Server name``` text box and paste where it says ```INSERT_DATABASE_CONNECTION_HERE```
then replace ```INSERT_DATABASE_NAME``` with the name of your database that you've created. 

### Project Set Up
+ Open the BangazonAPI project in Visual Studio.
+ Open Postman.
+ Follow the instructions above (in "Database Setup") to set up the database in Microsoft SQL Server Management Studio. 
+ The primary server address for all controllers in this project is ```localhost:5000/```. The endpoints for individual controllers are defined in their respective sections.
+ The controllers generally have a GET, POST, PUT, and DELETE method; these are defined in their respective sections.
+ The presence or absence of query string parameters are also defined in each section. 
.+ _To check query string parameters... _ *Someone please update this section*
.+ To test a controller's GET method, select GET from the dropdown menu in Postman and paste the primary address and appropriate endpoint in the address field. Click send and ensure you are able to see the database items in the results section. To see a specific item, use the server address, appropriate endpoint and the specific id in the address field.
.+ To test a controller's POST method, choose PUT from the dropdown menu and paste the server address and endpoint in the address field in Postman. In the body field, create a new object in the json format of the object in the database (this format is defined for each section below). When you are finished, click the send button. You should get nothing back from this besides an OK status. When you run the GET query the computer you specified in your PUT query should show the updated, edited information you gave it. To view results, perform another GET.
.+ To test a controllers's PUT method, choose PUT from the dropdown menu and paste the server address and endpoint with the specific id of the item you want to edit in the address field in Postman. Follow the json structure of your object and change or update desired values. When you are finished, click the send button. You should get nothing back from this besides an OK status. When you run the GET query the computer you specified in your PUT query should show the updated, edited information you gave it. To view results, perform another GET.
.+ To test a controllers's DELETE method, choose DELETE from the dropdown menu and paste the server address and endpoint with the specific id of the item you want to delete in the address field in Postman. Only do this if you truly want the object to disappear forever. It will be completely removed from the database. When you are sure, click the send button. You should get nothing back from this besides an OK status. When you run the GET query the computer you specified in your PUT query should show the updated, edited information you gave it. To view results, perform another GET.


### Http Request Methods

### 1. Customer
#### Created by Mary Remo (MR)
The endpoint for this is: ```api/customer```. A specific item's endpoint is: ```api/customer/{id}```, where "id" is the id of the specific item you are looking for. This controller makes use of GET, POST, and PUT methods. It also uses query string parameters.

### 2. Product
#### Created by Hannah Neal (HMN)
The endpoint for this is: ```api/product```. A specific item's endpoint is: ```api/product/{id}```, where "id" is the id of the specific item you are looking for. This controller makes use of GET, POST, PUT, and DELETE methods.

 ```
 
  {
    "id": 1,
    "price": 19,
    "title": "First Product",
    "description": "First product's description",
    "quantity": 1,
    "productTypeId": 1,
    "customerId": 1,
    "productType": {
      "id": 1,
      "name": "Toys"
    },
    "customer": {
      "id": 1,
      "firstName": "Gilda",
      "lastName": "Radnor",
      "payments": null,
      "products": null
    }
  },
  {
    "id": 2,
    "price": 199,
    "title": "Second Product",
    "description": "Second product's description",
    "quantity": 2,
    "productTypeId": 2,
    "customerId": 1,
    "productType": {
      "id": 2,
      "name": "Tools"
    },
    "customer": {
      "id": 1,
      "firstName": "John",
      "lastName": "Doe",
      "payments": null,
      "products": null
    }
  }
]
 ```
A specific item should look like this:
 
```
[
  {
    "id": 2,
    "price": 199,
    "title": "Second Product",
    "description": "Second product's description",
    "quantity": 2,
    "productTypeId": 2,
    "customerId": 1,
    "productType": {
      "id": 2,
      "name": "Tools"
    },
    "customer": {
      "id": 1,
      "firstName": "John",
      "lastName": "Doe",
      "payments": null,
      "products": null
    }
  }
]
```
To create a new object or edit a specific item already in the database, follow the structure below:
```
{
  {
    "price": 199,
    "title": "Second Product",
    "description": "Second product's description",
    "quantity": 2,
    "productTypeId": 2,
    "customerId": 1
  }
}
```


### 3. PaymentType
#### Created by Austin Blade (AB)
The endpoint for this is ```api/paymenttype```. A specific item's endpoint is: ```api/product/{id}```, where "id" is the id of the specific item you are looking for. This controller makes use of GET, POST, PUT, and DELETE methods.

 The structure of the objects of this controller should look like this:

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
A specific item should look like this:
 
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
To create a new object or edit a specific item already in the database, follow the structure below:
```
{
	"Name": "Visa",
	"AcctNumber": 123375457,
	"CustomerId": 1
}
```
### 4. Order
#### Created by
 The endpoint for this is ```api/?```. A specific item's endpoint is: ```api/?/{id}```, where "id" is the id of the specific item you are looking for. This controller makes use of GET, POST, PUT, and DELETE methods. It also uses query strings.

  The structure of the objects of this controller should look like this:
  ```


  ```
   A specific item should look like this:
   ```


   ```
   To create a new object or edit a specific item already in the database, follow the structure below:
   ```


   ```
 
### 5. ProductType
#### Created by Austin Blade (AB)

 The endpoint for this is ```api/paymenttype```. A specific item's endpoint is: ```api/product/{id}```, where "id" is the id of the specific item you are looking for. This controller makes use of GET, POST, PUT, and DELETE methods.

 The structure of the objects of this controller should look like this:
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
 A specific item should look like this:
```
[
  {
    "Id": 1,
    "Name": "Toys"
  }
]
```
To create a new object or edit a specific item already in the database, follow the structure below:
```
{
	"Name": "Kitchen"
}
```
### 6. Employee
#### Created by Mary Remo (MR)
 The endpoint for this is ```api/order```. A specific item's endpoint is: ```api/order/{id}```, where "id" is the id of the specific item you are looking for. This controller makes use of GET, POST, and PUT methods.

  The structure of the objects of this controller should look like this:
  ```


  ```
   A specific item should look like this:
   ```


   ```
   To create a new object or edit a specific item already in the database, follow the structure below:
   ```


   ```

### 7. Department
#### Created by Austin Blade (AB)
 The endpoint for this is ```api/order```. A specific item's endpoint is: ```api/order/{id}```, where "id" is the id of the specific item you are looking for. This controller makes use of GET, POST, PUT, and DELETE methods. It also uses query strings.

  The structure of the objects of this controller should look like this:
  ```


  ```
   A specific item should look like this:
   ```


   ```
   To create a new object or edit a specific item already in the database, follow the structure below:
   ```


   ```

### 8. Computer
#### Created by Hannah Neal (HN)
 The endpoint for this is ```api/order```. A specific item's endpoint is: ```api/order/{id}```, where "id" is the id of the specific item you are looking for. This controller makes use of GET, POST, PUT, and DELETE methods.

  The structure of the objects of this controller should look like this:
  ```


  ```
   A specific item should look like this:
   ```


   ```
   To create a new object or edit a specific item already in the database, follow the structure below:
   ```


   ```


### 9. TrainingProgram
#### Created by

 The endpoint for this is ```api/?```. A specific item's endpoint is: ```api/?/{id}```, where "id" is the id of the specific item you are looking for. This controller makes use of ... methods. It also uses query strings.

  The structure of the objects of this controller should look like this:
  ```
      {
        "id": 1,
        "purchaseDate": "1999-02-16T00:00:00",
        "decomissionDate": "1994-05-26T00:00:00",
        "make": "XPS 13",
        "manufacturer": "Dell"
    },
    {
        "id": 2,
        "purchaseDate": "1999-02-16T00:00:00",
        "decomissionDate": "1994-05-26T00:00:00",
        "make": "XPS 13",
        "manufacturer": "Dell"
    }

  ```
   A specific item should look like this:
   ```
       {
        "id": 2,
        "purchaseDate": "1999-02-16T00:00:00",
        "decomissionDate": "1994-05-26T00:00:00",
        "make": "XPS 13",
        "manufacturer": "Dell"
    }

   ```
   To create a new object or edit a specific item already in the database, follow the structure below:
   ```
    {
        "purchaseDate": "1999-02-16T00:00:00",
        "decomissionDate": "1994-05-26T00:00:00",
        "make": "XPS 13",
        "manufacturer": "Dell"
    }

   ```


### 10. Order
####Created by Hunter Metts (HM)

 The endpoint for this is ```api/order```. A specific item's endpoint is: ```api/order/{id}```, where "id" is the id of the specific item you are looking for. This controller makes use of GET, POST, PUT, and DELETE methods. It also uses query strings.

  The structure of the objects of this controller should look like this:
  ```


  ```
   A specific item should look like this:
   ```


   ```
   To create a new object or edit a specific item already in the database, follow the structure below:
   ```


   ```







