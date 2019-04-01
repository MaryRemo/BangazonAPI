/*
 * CREATED BY: HM
 */

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
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _config;

        public OrderController(IConfiguration config)
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

        // GET: api/Order?q=socks&include=paymentType
        [HttpGet]
        public IEnumerable<Order> Get(string include, string q)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (include == "products")
                    {
                        cmd.CommandText = @"
                                        select * from productType
                                        left join product 
                                        on productType.id = product.productTypeId
                                        left join orderProduct 
                                        on product.id = orderProduct.productid
                                        left join [order]
                                        on orderProduct.orderId = [order].id 
                                        left join PaymentType
                                        on [order].paymentTypeId = paymentType.id
                                        left join customer
                                        on paymentType.CustomerId = customer.id
                                         WHERE 1 = 1";
                    }
                    else
                    {
                        cmd.CommandText = @"
                                         select * from [order]
                                         WHERE 1 = 1";
                    }

                    if (!string.IsNullOrWhiteSpace(q))
                    {
                        cmd.CommandText += @" AND 
                                             ([order].CustomerId LIKE @q OR
                                              [order].PaymentTypeId LIKE @q)";
                        cmd.Parameters.Add(new SqlParameter("@q", $"%{q}%"));
                    }

                    SqlDataReader reader = cmd.ExecuteReader();

                    Dictionary<int, Order> orders = new Dictionary<int, Order>();
                    while (reader.Read())
                    {
                        int orderId = reader.GetInt32(reader.GetOrdinal("order.id"));
                        if (!orders.ContainsKey(orderId))
                        {
                            Order newOrder = new Order
                            {
                                id = orderId,
                                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                PaymentTypeId = reader.GetInt32(reader.GetOrdinal("PaymentTypeId")),
                            };

                            orders.Add(orderId, newOrder);
                        }

                        
                    }

                    reader.Close();

                    return orders.Values.ToList();
                }
            }
        }

        /*

        // GET: api/Students/5?include=exercise
        [HttpGet("{id}", Name = "GetSpecificOrder")]
        public Students Get(int id, string include)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (include == "exercise")
                    {
                        cmd.CommandText = @"select s.id as StudentId,
                                               s.FirstName,
                                               s.LastName,
                                               s.SlackHandle,
                                               s.CohortId,
                                               c.[Name] as CohortName,
                                               e.id as ExerciseId,
                                               e.[name] as ExerciseName,
                                               e.[Language]
                                          from student s
                                               left join Cohort c on s.CohortId = c.id
                                               left join StudentExercise se on s.id = se.studentid
                                               left join Exercise e on se.exerciseid = e.id";
                    }
                    else
                    {
                        cmd.CommandText = @"select s.id as StudentId,
                                               s.FirstName,
                                               s.LastName,
                                               s.SlackHandle,
                                               s.CohortId,
                                               c.[Name] as CohortName
                                          from student s
                                               left join Cohort c on s.CohortId = c.id";
                    }

                    cmd.CommandText += " WHERE s.id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();

                    Students student = null;
                    while (reader.Read())
                    {
                        if (student == null)
                        {
                            student = new Students
                            {
                                Id = id,
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                                CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                                Cohort = new Cohort
                                {
                                    id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                                    cohortName = reader.GetString(reader.GetOrdinal("CohortName"))
                                }
                            };
                        }

                        if (include == "exercise")
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("ExerciseId")))
                            {
                                student.Exercises.Add(
                                    new Exercises
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("ExerciseId")),
                                        Language = reader.GetString(reader.GetOrdinal("Language")),
                                        Name = reader.GetString(reader.GetOrdinal("ExerciseName")),
                                    }
                                );
                            }
                        }
                    }

                    reader.Close();

                    return student;
                }
            }
        }




        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Students student)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Student (FirstName, LastName, SlackHandle, cohortId)
                                        OUTPUT INSERTED.Id
                                        VALUES (@FirstName, @LastName, @SlackHandle, @cohortId)";
                    cmd.Parameters.Add(new SqlParameter("@FirstName", student.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@LastName", student.LastName));
                    cmd.Parameters.Add(new SqlParameter("@SlackHandle", student.SlackHandle));
                    cmd.Parameters.Add(new SqlParameter("@cohortId", student.CohortId));



                    int newId = (int)cmd.ExecuteScalar();
                    student.Id = newId;
                    return CreatedAtRoute("GetStudent", new { id = newId }, student);
                }
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Students student)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Student
                                            SET FirstName = @FirstName,
                                                LastName = @LastName,
                                                SlackHandle = @SlackHandle,
                                                cohortId = @cohortId
                                                
                                            WHERE student.Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@FirstName", student.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@LastName", student.LastName));
                        cmd.Parameters.Add(new SqlParameter("@SlackHandle", student.SlackHandle));
                        cmd.Parameters.Add(new SqlParameter("@cohortId", student.CohortId));
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!StudentsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {


                        cmd.CommandText = @"DELETE FROM StudentExercise WHERE StudentId = @id"; ;

                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        /*
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                        */
                    }

    /*

                    using (SqlCommand cmd = conn.CreateCommand())
                    {


                        cmd.CommandText = @"DELETE FROM Student WHERE Id = @id";

                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                    }

                }
            }
            catch (Exception ex)
            {
                if (!StudentsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool StudentsExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                         SELECT student.Id, FirstName, LastName, SlackHandle, cohortId, name from Student " +
                        "LEFT JOIN Cohort " +
                        "ON student.CohortId = Cohort.Id " +
                        "WHERE student.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    return reader.Read();
                }
            }
        }
    }
     */
}
