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
    public class ComputerController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ComputerController(IConfiguration config)
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

        // GET: api/Computer/5
        [HttpGet]
        public IActionResult GetAllComputers()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT comp.Id, comp.PurchaseDate, comp.DecomissionDate, comp.Make, comp.Manufacturer
                                        FROM Computer comp";
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    List<Computer> computers = new List<Computer>();
                    while (reader.Read())
                    {
                        Computer computer = new Computer
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                            DecomissionDate = reader.GetDateTime(reader.GetOrdinal("DecomissionDate")),
                            Make = reader.GetString(reader.GetOrdinal("Make")),
                            Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer"))
                        };
                        computers.Add(computer);
                    }
                    reader.Close();
                    return Ok(computers);
                }
            }
        }

        //HMN: Completed manual testing of GetAll and there were no issues.

        [HttpGet("{id}", Name = "GetSingleComputer")]
        public IActionResult GetSingleComputer([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT comp.Id, comp.PurchaseDate, comp.DecomissionDate, comp.Make, comp.Manufacturer
                                        FROM Computer comp
                                        WHERE comp.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Computer> computers = new List<Computer>();
                    while (reader.Read())
                    {
                        Computer computer = new Computer
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                            DecomissionDate = reader.GetDateTime(reader.GetOrdinal("DecomissionDate")),
                            Make = reader.GetString(reader.GetOrdinal("Make")),
                            Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer"))
                        };
                        computers.Add(computer);
                    }
                    reader.Close();
                    return Ok(computers);
                }
            }
        }

        //HMN: Manually tested the GetSingleComputer method and found no errors.

        // POST: api/Computer
        [HttpPost]
        public IActionResult PostProduct([FromBody] Computer newComputer)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Computer
                                            (PurchaseDate, DecomissionDate, Make, Manufacturer)
                                        OUTPUT INSERTED.Id
                                        VALUES (@purchaseDate, @decomissionDate, @make, @manufacturer)";
                    cmd.Parameters.Add(new SqlParameter("@purchaseDate", newComputer.PurchaseDate));
                    cmd.Parameters.Add(new SqlParameter("@decomissionDate", newComputer.DecomissionDate));
                    cmd.Parameters.Add(new SqlParameter("@make", newComputer.Make));
                    cmd.Parameters.Add(new SqlParameter("@manufacturer", newComputer.Manufacturer));

                    int newId = (int)cmd.ExecuteScalar();
                    newComputer.Id = newId;
                    return CreatedAtRoute("GetSingleComputer", new { id = newId }, newComputer);
                }
            }
        }
        //HMN: manually tested POST and found no errors.

        // PUT: api/Computer/5
        [HttpPut("{id}")]
        public void PutComputer(int id, [FromBody] Computer editedComputer)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Computer
                                        SET 
                                            PurchaseDate = @purchaseDate,
                                            DecomissionDate = @decomissionDate,
                                            Make = @make, 
                                            Manufacturer = @manufacturer
                                         WHERE Id = @id;";

                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    cmd.Parameters.Add(new SqlParameter("@purchaseDate", editedComputer.PurchaseDate));
                    cmd.Parameters.Add(new SqlParameter("@decomissionDate", editedComputer.DecomissionDate));
                    cmd.Parameters.Add(new SqlParameter("@make", editedComputer.Make));
                    cmd.Parameters.Add(new SqlParameter("@manufacturer", editedComputer.Manufacturer));

                    cmd.ExecuteNonQuery();
                }
            }
        }
        //HMN: Manually tested PUT and found no errors.

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
