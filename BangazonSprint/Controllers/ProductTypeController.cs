// CREATED BY: AB
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
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public ProductTypeController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            }
        }
        // GET: api/ProductType
        [HttpGet]
        public IActionResult GetAllPaymentTypes()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT pt.id, pt.name
                                        FROM ProductType pt";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<ProductType> productTypes = new List<ProductType>();
                    while (reader.Read())
                    {
                        ProductType productType = new ProductType
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("name"))
                        };

                        productTypes.Add(productType);
                    }

                    reader.Close();
                    if (productTypes.Count == 0)
                    {
                        return NoContent();
                    }
                    else
                    {
                        return Ok(productTypes);
                    }
                }
            }
        }

        // GET: api/ProductType/5
        [HttpGet("{id}", Name = "GetSingleProductType")]
        public ProductType Get(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT pt.id, pt.name
                                        FROM ProductType pt
                                        WHERE pt.id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    ProductType productType = null;
                    if (reader.Read())
                    {
                        productType = new ProductType
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("name"))
                        };
                    }

                    reader.Close();
                    return productType;
                }
            }
        }

        // POST: api/ProductType
        [HttpPost]
        public ActionResult Post([FromBody] ProductType newProductType)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO productType (name)
                                             OUTPUT INSERTED.Id
                                             VALUES (@name)";
                    cmd.Parameters.Add(new SqlParameter("@name", newProductType.Name));
                    
                    int newId = (int)cmd.ExecuteScalar();
                    newProductType.Id = newId;
                    return CreatedAtRoute("GetSingleProductType", new { id = newId }, newProductType);
                }
            }
        }

        // PUT: api/ProductType/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] ProductType productType)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE productType 
                                           SET name = @name 
                                         WHERE id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@name", productType.Name));
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM productType WHERE id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
