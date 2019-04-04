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
 To GET all product types, select GET in Postman then paste ```localhost:5000/PaymentType``` into the field and click send. The result should be an array of all the payment type in the database that should look like:
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

### 4. Customer
Use the command ```dotnet run``` to start the program, BangazonAPI. Once the program is running, open up the Postman desktop app and run the following commands for each request method:
 ##### GET
 To GET all customers, select GET in Postman then paste ```localhost:5000/customer``` into the field and click send. The result should be an array of all the payment type in the database that should look like:
 ```
 [
  {
    "Id": 1,
    "FirstName": "Marie",
    "LastName": "Massie",
  },
  {
    "Id": 2,
    "FirstName": "Hannah",
    "LastName": "Fluff",
  },
 ]
 ```
 To GET a specific, single customer, add an /{id} to the end of the ```localhost:5000/customer``` URL. The result should only include the single payment type with the Id you added like the below:  
```
[
  {
    "Id": 1,
    "FirstName": "Mary",
    "LastName": "Massie",
  }
]
```
 ##### POST
 To POST a new object to your existing array for Customer, select POST, then paste ```localhost:5000/Customer``` into the field. Then click Body underneath the field, select raw, and then paste this below snippet or make one similar then click send. The result should be the new PaymentType you made:
```
{
	"FirstName": "George",
	"LastName": "Geo",
}
```
##### PUT
 To update an existing Customer, select PUT then paste ```localhost:5000/customer/2``` or any other existing order. Then follow the same directions as the POST example, and change the values then click send: 
```
{
	"FirstName": "Hanna",
	"LastName": "Fuff",
}
```
You should get nothing back from this besides an OK status. When you run the GET query the computer you specified in your PUT query should show the updated, edited information you gave it.

### 4. TrainingProgram
Use the command ```dotnet run``` to start the program, BangazonAPI. Once the program is running, open up the Postman desktop app and run the following commands for each request method:
 ##### GET
 To GET all training programs, select GET in Postman then paste ```localhost:5000/trainingprogram``` into the field and click send. The result should be an array of all the payment type in the database that should look like:
 To GET all training programs that their EndDate is in the future ```localhost:5000/trainingprogram?completed=false```
 ```
 [
  {
        "id": 1,
        "name": "How To Sell Cars",
        "startDate": "2020-02-14T00:00:00",
        "endDate": "2019-02-15T00:00:00",
        "maxAttendees": 15,
        "employeeList": [
            {
                "id": 1,
                "firstName": "Hunter",
                "lastName": "Metts",
                "departmentId": 1,
                "isSupervisor": false,
                "department": {
                    "id": 1,
                    "name": "Technology"
                }
            },
            {
                "id": 3,
                "firstName": "Gee",
                "lastName": "Remo",
                "departmentId": 1,
                "isSupervisor": false,
                "department": {
                    "id": 1,
                    "name": "Technology"
                }
            },
            {
                "id": 5,
                "firstName": "Gee",
                "lastName": "Blade",
                "departmentId": 1,
                "isSupervisor": false,
                "department": {
                    "id": 1,
                    "name": "Technology"
                }
            }
        ]
    },
    {
        "id": 5,
        "name": "How train your dragon",
        "startDate": "2019-03-04T00:00:00",
        "endDate": "2019-03-10T00:00:00",
        "maxAttendees": 20,
        "employeeList": [
            {
                "id": 1,
                "firstName": "Hunter",
                "lastName": "Metts",
                "departmentId": 1,
                "isSupervisor": false,
                "department": {
                    "id": 1,
                    "name": "Technology"
                }
            }
        ]
    },
    {
        "id": 3,
        "name": "How to ride a bike",
        "startDate": "2019-02-04T00:00:00",
        "endDate": "2020-02-04T00:00:00",
        "maxAttendees": 15,
        "employeeList": [
            {
                "id": 1,
                "firstName": "Hunter",
                "lastName": "Metts",
                "departmentId": 1,
                "isSupervisor": false,
                "department": {
                    "id": 1,
                    "name": "Technology"
                }
            }
        ]
    }
 ]
 ```
 To GET a specific, single payment type, add an /{id} to the end of the ```localhost:5000/trainingprogram``` URL. The result should only include the single payment type with the Id you added like the below:  
```
[
  {
        "id": 3,
        "name": "How to ride a bike",
        "startDate": "2019-02-04T00:00:00",
        "endDate": "2020-02-04T00:00:00",
        "maxAttendees": 15,
        "employeeList": [
            {
                "id": 1,
                "firstName": "Hunter",
                "lastName": "Metts",
                "departmentId": 1,
                "isSupervisor": false,
                "department": {
                    "id": 1,
                    "name": "Technology"
                }
            }
        ]
    }
]
```
 ##### POST
 To POST a new object to your existing array for TrainingProgram, select POST, then paste ```localhost:5000/TrainingProgram``` into the field. Then click Body underneath the field, select raw, and then paste this below snippet or make one similar then click send. The result should be the new TrainingProgram you made:
```
{
	"name": "How to ride a bike",
        "startDate": "2019-02-04T00:00:00",
        "endDate": "2020-02-04T00:00:00",
        "maxAttendees": 15,
}
```
##### PUT
 To update an existing TrainingProgram, select PUT then paste ```localhost:5000/trainingprogram/1``` or any other existing order. Then follow the same directions as the POST example, and change the values then click send: 
```
{
	"name": "How to ride a bike",
        "startDate": "2019-02-04T00:00:00",
        "endDate": "2020-02-04T00:00:00",
        "maxAttendees": 15,
}
```
You should get nothing back from this besides an OK status. When you run the GET query the computer you specified in your PUT query should show the updated, edited information you gave it.
##### DELETE
 To DELETE an existing product type, select DELETE then paste ```localhost:5000/TrainingProgram/2``` or any other existing training program then click send. You should get nothing back from this besides an OK status. When you run the GET query the order with the Id you specified in your DELETE query should no longer exist.

### 5. Employee
Use the command ```dotnet run``` to start the program, BangazonAPI. Once the program is running, open up the Postman desktop app and run the following commands for each request method:
 ##### GET
 To GET all product types, select GET in Postman then paste ```localhost:5000/employee``` into the field and click send. The result should be an array of all the employees in the database that should look like:
 ```
 [
   {
        "id": 1,
        "firstName": "Hunter",
        "lastName": "Metts",
        "departmentId": 1,
        "isSupervisor": false,
        "department": {
            "id": 1,
            "name": "Technology"
        }

 ]
 ```
 To GET a specific, single payment type, add an /{id} to the end of the ```localhost:5000/employee``` URL. The result should only include the single customer with the Id you added like the below:  
```
[
  {
     {
        "id": 1,
        "firstName": "Hunter",
        "lastName": "Metts",
        "departmentId": 1,
        "isSupervisor": false,
        "department": {
            "id": 1,
            "name": "Technology"
        }
  }
]
```
 ##### POST
 To POST a new object to your existing array for PaymentType, select POST, then paste ```localhost:5000/employee``` into the field. Then click Body underneath the field, select raw, and then paste this below snippet or make one similar then click send. The result should be the new PaymentType you made:
```
{
	
        "firstName": "Hunter",
        "lastName": "Metts",
        "departmentId": 1,
        "isSupervisor": false,
      
}
```
##### PUT
 To update an existing Employee, select PUT then paste ```localhost:5000/employee/2``` or any other existing order. Then follow the same directions as the POST example, and change the values then click send: 
```
{
	
        "firstName": "Hunter",
        "lastName": "Metts",
        "departmentId": 1,
        "isSupervisor": false,

}
```
You should get nothing back from this besides an OK status. When you run the GET query the computer you specified in your PUT query should show the updated, edited information you gave it.
