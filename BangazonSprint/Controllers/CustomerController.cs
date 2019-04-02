using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BangazonSprint.Models;
using BangazonSprintStartUp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BangazonSprint.Controllers
{ 

    // Created by MR
    /*GET
    POST
    PUT
    User should be able to GET a list of customers, and GET a single customer.
    If the query string parameter of ?_include=products is provided, then any products that
    the customer is selling should be included in the response.
    If the query string parameter of ?_include=payments is provided, then any payment 
    types that the customer has used to pay for an order should be included in the response.
    If the query string parameter of q is provided when querying the list of customers,
    then any customer that has property value that matches the pattern should be returned.
    If /customers?q=mic is requested, then any customer whose first name is Michelle, or Michael,
    or Domicio should be returned. Any customer whose last name is Michaelangelo, or Omici, Dibromic 
    should be returned. Every property of the customer object should be checked for a match.

    Testing Criteria
    Write a testing class and test methods that validate the GET single, GET all, POST, and PUT operations work as expected.*/

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly IConfiguration _config;

    public CustomerController(IConfiguration config)
    {
        _config = config;
    }

    public SqlConnection Connection
    {
        get
        {
            return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }
    }

        // GET: api/Customer
        [HttpGet]
        public List<Customer> GetCustomers(string include, string q)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (include == "products")
                    {
                        cmd.CommandText = $@"SELECT c.id AS customerid, c.FirstName, c.LastName, p.id AS productid, p.[Title] AS Title, p.[Description] AS Description,
                                        p.Price, p.Quantity
                                        FROM Customer c
                                        JOIN  Product p ON c.Id = p.CustomerId
                                        WHERE 1 = 1";
                    }
                   else if (include == "payments")
                    {
                        cmd.CommandText = $@"SELECT c.id AS customerid, c.FirstName, c.LastName, pt.id AS paymentid, pt.[Name], pt.AcctNumber
                                        FROM Customer c
                                        JOIN PaymentType pt on c.Id  = pt.CustomerId
                                        WHERE 1 = 1";
                    }
                    else
                    {
                        cmd.CommandText = $@"SELECT c.Id AS customerid, c.FirstName, c.LastName
                                        FROM Customer c
                                        WHERE 1 = 1";
                    }
                    if (!string.IsNullOrWhiteSpace(q))
                    {
                        cmd.CommandText += @" AND FirstName LIKE @b OR LastName LIKE @b";
                        cmd.Parameters.Add(new SqlParameter("@b", $"%{q}%"));

                    }
                    SqlDataReader reader = cmd.ExecuteReader();

                    Dictionary<int, Customer> customers = new Dictionary<int, Customer>();
                    while (reader.Read())
                    {
                        int customerid = reader.GetInt32(reader.GetOrdinal("customerid"));

                        if (!customers.ContainsKey(customerid))
                        {
                            Customer customer = new Customer
                            {
                                Id = customerid,
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                payments = new List<PaymentType>(),
                                products = new List<Product>(),
                            };
                            customers.Add(customerid, customer);
                        }

                        if (include == "payments")
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("paymentid")))
                            {
                                Customer currentCustomer = customers[customerid];
                                if (!currentCustomer.payments.Exists(x => x.Id == reader.GetInt32(reader.GetOrdinal("paymentid"))))
                                {
                                    currentCustomer.payments.Add(
                                    new PaymentType
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("paymentid")),
                                        AcctNumber = reader.GetInt32(reader.GetOrdinal("AcctNumber")),
                                        Name = reader.GetString(reader.GetOrdinal("Name")),
                                    });
                                }
                            }
                        }

                        if (include == "products")
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("productid")))
                            {
                                Customer currentCustomer = customers[customerid];
                                if (!currentCustomer.products.Exists(x => x.Id == reader.GetInt32(reader.GetOrdinal("productid"))))
                                {
                                    currentCustomer.products.Add(
                                        new Product
                                        {
                                            Id = reader.GetInt32(reader.GetOrdinal("productid")),
                                            Title = reader.GetString(reader.GetOrdinal("Title")),
                                            Price = reader.GetInt32(reader.GetOrdinal("Price")),
                                            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                            Description = reader.GetString(reader.GetOrdinal("Description")),
                                        });
                                }
                            }
                        }

                    }
                    reader.Close();
                    return customers.Values.ToList();
                }
            }
        }


        // GET: api/Customer/5
       [HttpGet("{id}", Name = "GetSingleCustomer")]
        public Customer GetCustomer(int id, string include)
        {
            using (SqlConnection conn = Connection)

            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (include == "payments")
                    {
                        cmd.CommandText = $@"SELECT c.id AS customerid, c.FirstName, c.LastName, pt.id AS paymentid, pt.[Name], pt.AcctNumber
                                        FROM Customer c
                                        JOIN PaymentType pt on c.Id  = pt.CustomerId
                                    WHERE c.id = @Id";
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    }
                    else if (include == "products")
                    {
                        cmd.CommandText = $@"SELECT c.id AS customerid, c.FirstName, c.LastName, p.id AS productid, p.[Title] AS Title,
                                        p.[Description] AS Description,
                                        p.Price, p.Quantity
                                        FROM Customer c
                                        JOIN  Product p ON c.Id = p.CustomerId
                                    WHERE c.id = @Id";
                        cmd.Parameters.Add(new SqlParameter("@Id", id));
                    }
                    else
                    {
                        cmd.CommandText = $@"SELECT c.Id AS customerid, c.FirstName, c.LastName
                                        FROM Customer c
                                        WHERE c.id = @Id";
                        cmd.Parameters.Add(new SqlParameter("@Id", id));
                    }

                    SqlDataReader reader = cmd.ExecuteReader();

                    Customer customer = null;

                    while (reader.Read())
                    {
                        customer = new Customer
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("customerid")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            payments = new List<PaymentType>(),
                            products = new List<Product>(),

                        };

                        if (include == "payments")
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("paymentid")))
                            {      
                                if (!customer.payments.Exists(x => x.Id == reader.GetInt32(reader.GetOrdinal("paymentid"))))
                                {
                                    customer.payments.Add(
                                    new PaymentType
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("paymentid")),
                                        AcctNumber = reader.GetInt32(reader.GetOrdinal("AcctNumber")),
                                        Name = reader.GetString(reader.GetOrdinal("Name")),
                                    });
                                }
                            }
                        }

                        if (include == "products")
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("productid")))
                            {
                                if (!customer.products.Exists(x => x.Id == reader.GetInt32(reader.GetOrdinal("productid"))))
                                {
                                    customer.products.Add(
                                        new Product
                                        {
                                            Id = reader.GetInt32(reader.GetOrdinal("productid")),
                                            Title = reader.GetString(reader.GetOrdinal("Title")),
                                            Price = reader.GetInt32(reader.GetOrdinal("Price")),
                                            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                            Description = reader.GetString(reader.GetOrdinal("Description")),
                                        });
                                }
                            }
                        }
                    }
                    reader.Close();
                    return customer;
                }
            }
        }

        // POST: api/Customer
       [HttpPost]
        public async Task<IActionResult> Post([FromBody] Customer newCustomer)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Customer (FirstName, LastName)
                                        OUTPUT INSERTED.Id
                                        VALUES (@FirstName, @LastName);";
                    cmd.Parameters.Add(new SqlParameter("@FirstName", newCustomer.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@LastName", newCustomer.LastName));

                    int newId = (int)cmd.ExecuteScalar();
                    newCustomer.Id = newId;
                    return CreatedAtRoute("GetSingleCustomer", new { id = newId }, newCustomer);

                }
            }
        }
        
        // PUT: api/Customer/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Customer customer)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Customer
                                        SET FirstName = @FirstName, 
                                            LastName = @LastName
                                        WHERE id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@FirstName", customer.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@LastName", customer.LastName));
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
