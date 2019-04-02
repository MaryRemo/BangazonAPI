using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using BangazonSprint.Models;

namespace BangazonSprint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ProductController(IConfiguration config)
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

        // GET: api/Product

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //if (q != null)

                    //? QUESTION:
                    //NOTE: Included in this query are the Product Name and Customer Firstname and Lastname. The query was set up this way as a double-check step for verifying the correct ids for the various columns. However, including ProductName, CustomerFirstName and CustomerLastName have not been requested by the Product Manager. Before merging, clear this up with the PM.

                    cmd.CommandText = @"SELECT p.Id, p.ProductTypeId, pt.Name AS ProductTypeName, p.CustomerId, c.FirstName AS CustomerFirstName, c.LastName AS CustomerLastName, p.Price, p.Title, p.Description, p.Quantity
                                        FROM Product p
                                        INNER JOIN ProductType pt ON p.ProductTypeId = pt.Id
                                        LEFT JOIN Customer c ON p.CustomerId = c.Id";
                    //cmd.Parameters.Add(new SqlParameter("@q", $"%{q}%"));
                    SqlDataReader reader = cmd.ExecuteReader();

        List<Product> products = new List<Product>();
                        while (reader.Read())
                        {
                            Product product = new Product
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Price = reader.GetInt32(reader.GetOrdinal("Price")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),
                                ProductType = new ProductType
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),
                                    Name = reader.GetString(reader.GetOrdinal("ProductTypeName"))
                                },
                                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                Customer = new Customer
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                    FirstName = reader.GetString(reader.GetOrdinal("CustomerFirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("CustomerLastName"))
                                }
                            };
        products.Add(product);
                        }
    reader.Close();
                        return Ok(products);
}
                }
            }

        // GET: api/Product/5
        [HttpGet("{id}", Name = "GetSingleProduct")]
        public IActionResult GetSingleProduct([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT p.Id, p.ProductTypeId, pt.Name AS ProductTypeName, p.CustomerId, c.FirstName AS CustomerFirstName, c.LastName AS CustomerLastName, p.Price, p.Title, p.Description, p.Quantity
                                        FROM Product p
                                        INNER JOIN ProductType pt ON p.ProductTypeId = pt.Id
                                        LEFT JOIN Customer c ON p.CustomerId = c.Id
                                        WHERE p.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Product product = null;
                    if (reader.Read())
                    {
                        product = new Product
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Price = reader.GetInt32(reader.GetOrdinal("Price")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                            ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),
                            ProductType = new ProductType
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("ProductTypeName"))
                            },
                            CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                            Customer = new Customer
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                FirstName = reader.GetString(reader.GetOrdinal("CustomerFirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("CustomerLastName"))
                            }
                        };

                    }

                    reader.Close();
                    return Ok(product);
                }
            }
        }

        // POST: api/Product
        [HttpPost]
        public IActionResult PostProduct([FromBody] Product newProduct)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Product
                                            (ProductTypeId,
                                            CustomerId,
                                            Price, 
                                            Title, 
                                            Description, 
                                            Quantity)
                                        OUTPUT INSERTED.Id
                                        VALUES (@productTypeId, @customerId, @price, @title, @description, @quantity)";

                    cmd.Parameters.Add(new SqlParameter("@productTypeId", newProduct.ProductTypeId));
                    cmd.Parameters.Add(new SqlParameter("@customerId", newProduct.CustomerId));
                    cmd.Parameters.Add(new SqlParameter("@price", newProduct.Price));
                    cmd.Parameters.Add(new SqlParameter("@title", newProduct.Title));
                    cmd.Parameters.Add(new SqlParameter("@description", newProduct.Description));
                    cmd.Parameters.Add(new SqlParameter("@quantity", newProduct.Quantity));

                    int newId = (int)cmd.ExecuteScalar();
                    newProduct.Id = newId;
                    return CreatedAtRoute("GetSingleProduct", new { id = newId }, newProduct);

                    //? HMN: Doesn't the return need an "await"? See PostProduct on line 165.
                }
            }
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public void PutProduct(int id, [FromBody] Product editedProduct)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Product
                                           SET 
                                               ProductTypeId = @productTypeId,
                                               CustomerId = @customerId,
                                               Price = @price, 
                                               Title = @title,
                                               Description = @description,
                                               Quantity = @quantity
                                         WHERE Id = @id;";

                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    cmd.Parameters.Add(new SqlParameter("@productTypeId", editedProduct.ProductTypeId));
                    cmd.Parameters.Add(new SqlParameter("@customerId", editedProduct.CustomerId));
                    cmd.Parameters.Add(new SqlParameter("@price", editedProduct.Price));
                    cmd.Parameters.Add(new SqlParameter("@title", editedProduct.Title));
                    cmd.Parameters.Add(new SqlParameter("@description", editedProduct.Description));
                    cmd.Parameters.Add(new SqlParameter("@quantity", editedProduct.Quantity));

                    //int rowsAffected = cmd.ExecuteNonQuery();
                    ////NOTE: HMN: ExecuteNonQuery() executes a Transact-SQL statment against the connection and returns the number of rows affected.

                    //// HMN: If the rows affected are greater than 0, return an empty StatusCodes204 object indicating success; otherwise, if no rows are affected, return an error message (exception).
                    //if(rowsAffected > 0)
                    //{
                    //    return NoContent();
                    //    //NOTE: HMN: NoContent() comes from the ControllerBase; it creates an empty NoContent object that produces an empty StatusCodes.StatusCodes204NoContent response. An HTTP 204 No Content success status response code indicates that the request has succeeded, but that the client doesn't need to go away from its current page.
                    //    //HMN: MDN Resource: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/204
                    //}
                    //throw new Exception("No rows affected");

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Product WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if(rowsAffected > 0)
                    {
                        return NoContent();
                    }
                    throw new Exception("No rows affected");
                }
            }
        }
    }
}
