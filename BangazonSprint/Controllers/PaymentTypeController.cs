// CREATED BY: AB

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BangazonSprint.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BangazonSprint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentTypeController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public PaymentTypeController(IConfiguration configuration)
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
        // GET: api/PaymentType
        [HttpGet]
        public IActionResult GetAllPaymentTypes()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT pt.id, pt.name, pt.acctNumber, pt.customerId
                                        FROM PaymentType pt";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<PaymentType> paymentTypes = new List<PaymentType>();
                    while (reader.Read())
                    {
                        PaymentType paymentType = new PaymentType
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            AcctNumber = reader.GetInt32(reader.GetOrdinal("acctNumber")),
                            CustomerId = reader.GetInt32(reader.GetOrdinal("customerId")),
                        };

                        paymentTypes.Add(paymentType);
                    }

                    reader.Close();
                    if (paymentTypes.Count == 0)
                    {
                        return NoContent();
                    }
                    else
                    {
                        return Ok(paymentTypes);
                    }
                }
            }
        }

        // GET: api/PaymentType/5
        [HttpGet("{id}", Name = "GetSinglePaymentType")]

        public PaymentType Get(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT pt.id, pt.name, pt.acctNumber, pt.customerId
                                        FROM PaymentType pt
                                        WHERE pt.id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    PaymentType paymentType = null;
                    if (reader.Read())
                    {
                        paymentType = new PaymentType
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            AcctNumber = reader.GetInt32(reader.GetOrdinal("acctNumber")),
                            CustomerId = reader.GetInt32(reader.GetOrdinal("customerId"))
                        };
                    }

                    reader.Close();
                    return paymentType;
                }
            }
        }

        // POST: api/PaymentType
        [HttpPost]
        public ActionResult Post([FromBody] PaymentType newPaymentType)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO paymentType (name, acctNumber, customerId)
                                             OUTPUT INSERTED.Id
                                             VALUES (@name, @acctNumber, @customerId)";
                    cmd.Parameters.Add(new SqlParameter("@name", newPaymentType.Name));
                    cmd.Parameters.Add(new SqlParameter("@acctNumber", newPaymentType.AcctNumber));
                    cmd.Parameters.Add(new SqlParameter("@customerId", newPaymentType.CustomerId));

                    int newId = (int)cmd.ExecuteScalar();
                    newPaymentType.Id = newId;
                    return CreatedAtRoute("GetSinglePaymentType", new { id = newId }, newPaymentType);
                }
            }
        }

        // PUT: api/PaymentType/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] PaymentType paymentType)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE paymentType 
                                           SET name = @name, 
                                               acctNumber = @acctNumber,
                                               customerId = @customerId 
                                         WHERE id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@name", paymentType.Name));
                    cmd.Parameters.Add(new SqlParameter("@acctNumber", paymentType.AcctNumber));
                    cmd.Parameters.Add(new SqlParameter("@customerId", paymentType.CustomerId));
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
                    cmd.CommandText = "DELETE FROM paymentType WHERE id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
