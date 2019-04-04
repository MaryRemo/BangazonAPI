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
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public DepartmentController(IConfiguration configuration)
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

        // GET: api/Department
        [HttpGet]
        public IActionResult Get(string include, string _filter, int _gt)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (include == "employees")
                    {
                        cmd.CommandText = @"SELECT d.id as DeptmentId, d.[name], d.budget, e.firstname, e.lastname, e.id as EmployeeId
                                            FROM Department d INNER JOIN Employee e ON e.DepartmentId = d.id
                                         WHERE 1 = 1";
                    }
                    else
                    {
                        cmd.CommandText = @"SELECT d.id as DeptmentId, d.[name], d.budget
                                            FROM Department d
                                         WHERE 1 = 1";
                    }

                    if (_filter == "budget")
                    {
                        cmd.CommandText += @" AND 
                                             d.budget > @_gt";
                        cmd.Parameters.Add(new SqlParameter("@_gt", _gt));
                    }

                    SqlDataReader reader = cmd.ExecuteReader();

                    Dictionary<int, Department> departments = new Dictionary<int, Department>();
                    while (reader.Read())
                    {
                        int departmentId = reader.GetInt32(reader.GetOrdinal("DeptmentId"));
                        if (!departments.ContainsKey(departmentId))
                        {
                            Department newDepartment = new Department
                            {
                                Id = departmentId,
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Budget = reader.GetInt32(reader.GetOrdinal("Budget"))
                            };

                            departments.Add(departmentId, newDepartment);
                        }

                        if (include == "employees")
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("EmployeeId")))
                            {
                                Department currentDepartment = departments[departmentId];
                                currentDepartment.employees.Add(
                                    new Employee
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    }
                                );
                            }
                        }
                    }

                    reader.Close();
                    if (departments.Count == 0)
                    {
                        return NoContent();
                    }
                    else
                    {
                        return Ok(departments);
                    }
                }
            }
        }

        // GET: api/Department/5
        [HttpGet("{id}", Name = "GetSingleDepartment")]
        public IActionResult Get(string include, int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (include == "employees")
                    {
                        cmd.CommandText = @"SELECT d.id as DeptmentId, d.[name], d.budget, e.firstname, e.lastname, e.id as EmployeeId
                                            FROM Department d INNER JOIN Employee e ON e.DepartmentId = d.id
                                         WHERE d.id = @id";
                    }
                    else
                    {
                        cmd.CommandText = @"SELECT d.id as DeptmentId, d.[name], d.budget
                                            FROM Department d
                                         WHERE d.id = @id";
                    }
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Dictionary<int, Department> departments = new Dictionary<int, Department>();
                    while (reader.Read())
                    {
                        int departmentId = reader.GetInt32(reader.GetOrdinal("DeptmentId"));
                        if (!departments.ContainsKey(departmentId))
                        {
                            Department newDepartment = new Department
                            {
                                Id = departmentId,
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Budget = reader.GetInt32(reader.GetOrdinal("Budget"))
                            };

                            departments.Add(departmentId, newDepartment);
                        }

                        if (include == "employees")
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("EmployeeId")))
                            {
                                Department currentDepartment = departments[departmentId];
                                currentDepartment.employees.Add(
                                    new Employee
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    }
                                );
                            }
                        }
                    }

                    reader.Close();
                    if (departments.Count == 0)
                    {
                        return NoContent();
                    }
                    else
                    {
                        return Ok(departments);
                    }
                }
            }
        }

        // POST: api/Department
        [HttpPost]
        public ActionResult Post([FromBody] Department newDepartment)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO department (name, budget)
                                             OUTPUT INSERTED.Id
                                             VALUES (@name, @budget)";
                    cmd.Parameters.Add(new SqlParameter("@name", newDepartment.Name));
                    cmd.Parameters.Add(new SqlParameter("@budget", newDepartment.Budget));

                    int newId = (int)cmd.ExecuteScalar();
                    newDepartment.Id = newId;
                    return CreatedAtRoute("GetSingleDepartment", new { id = newId }, newDepartment);
                }
            }
        }

        // PUT: api/Department/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Department department)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE department
                                           SET name = @name,
                                                budget = @budget
                                         WHERE id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@name", department.Name));
                    cmd.Parameters.Add(new SqlParameter("@budget", department.Budget));
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
                    cmd.CommandText = "DELETE FROM department WHERE id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}