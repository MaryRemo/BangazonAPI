using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
        //NOTE: HMN: The ProductType model has not been set up yet but it should be included in the query for GetAllProducts. When ProductType has been established, add to this query.
        //NOTE:  HMN: MR is working on Customer model; the customer_id should be included in this query. 

        [HttpGet]
        public List<Product> GetAllProducts(string q)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (q != null)
                    {
                        cmd.CommandText = $@"SELECT 
                                                p.id AS ProductId, 
                                                p.price AS ProductPrice, 
                                                p.title AS ProductTitle, 
                                                p.description AS ProductDescription, 
                                                p.quantity AS ProductQuantity
                                            FROM Product p";

                        //NOTE:HMN:  ProductType and CustomerId need to be added to the query!!!

                        cmd.Parameters.Add(new SqlParameter("@q", $"%{q}%"));
                        SqlDataReader reader = cmd.ExecuteReader();

                        List<Product> products = new List<Product>();
                        while (reader.Read())
                        {
                            Product product = new Product
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                price = reader.GetInt32(reader.GetOrdinal("productPrice")),
                                title = reader.GetString(reader.GetOrdinal("productTitle")),
                                description = reader.GetString(reader.GetOrdinal("productDescription")),
                                quantity = reader.GetInt32(reader.GetOrdinal("productQuantity"))
                            };

                            products.Add(product);
                        }

                        reader.Close();
                        return products;
                    }
                    else
                    {
                        cmd.CommandText = $@"SELECT 
                                                p.id AS ProductId, 
                                                p.price AS ProductPrice, 
                                                p.title AS ProductTitle, 
                                                p.description AS ProductDescription, 
                                                p.quantity AS ProductQuantity
                                            FROM Product p";
                        SqlDataReader reader = cmd.ExecuteReader();

                        List<Product> products = new List<Product>();

                        while (reader.Read())
                        {
                            Product product = new Product
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                price = reader.GetInt32(reader.GetOrdinal("productPrice")),
                                title = reader.GetString(reader.GetOrdinal("productTitle")),
                                description = reader.GetString(reader.GetOrdinal("productDescription")),
                                quantity = reader.GetInt32(reader.GetOrdinal("productQuantity"))
                            };
                            //NOTE: JOIN ProductType and Customer here

                            //NOTE : Create ProductType and Customer objects here

                            products.Add(product);
                        };

                        reader.Close();
                        return products;
                    }
                }
            }
        }

        // GET: api/Product/5
        [HttpGet("{id}", Name = "GetSingleProduct")]
        public Product Get(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT 
                                                p.id AS ProductId, 
                                                p.price AS ProductPrice, 
                                                p.title AS ProductTitle, 
                                                p.description AS ProductDescription, 
                                                p.quantity AS ProductQuantity
                                            FROM Product p
                                            WHERE p.id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Product product = null;
                    if (reader.Read())
                    {
                        product = new Product
                        {
                            id = reader.GetInt32(reader.GetOrdinal("id")),
                            price = reader.GetInt32(reader.GetOrdinal("productPrice")),
                            title = reader.GetString(reader.GetOrdinal("productTitle")),
                            description = reader.GetString(reader.GetOrdinal("productDescription")),
                            quantity = reader.GetInt32(reader.GetOrdinal("productQuantity"))
                        };
                        //NOTE: JOIN ProductType and Customer here
                        //NOTE: Create ProductType and Customer objects here
                    }

                    reader.Close();
                    return product;
                }
            }
        }

        // POST: api/Product
        [HttpPost]
        public ActionResult Post([FromBody] Product newProduct)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Product 
                                            (p.id, 
                                             p.price, 
                                             p.slackhandle, cohortid)
                                             OUTPUT INSERTED.Id
                                             VALUES (@firstname, @lastname, @slackhandle, @cohortid)";
                    cmd.Parameters.Add(new SqlParameter("@firstname", newInstructor.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastname", newInstructor.LastName));
                    cmd.Parameters.Add(new SqlParameter("@slackhandle", newInstructor.SlackHandle));
                    cmd.Parameters.Add(new SqlParameter("@cohortid", newInstructor.CohortId));

                    int newId = (int)cmd.ExecuteScalar();
                    newInstructor.Id = newId;
                    return CreatedAtRoute("GetSingleInstructor", new { id = newId }, newInstructor);
                }
            }


        // PUT: api/Product/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
