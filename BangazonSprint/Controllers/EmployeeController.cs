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
    public class EmployeeController : ControllerBase

    /*Verbs to be supported
    GET
    POST
    PUT
    User should be able to GET a list, and GET a single item.
    The employee's department name should be included in the employee representation
    A representation of the computer that the employee is currently using should be included in the employee representation
    Testing Criteria
    Write a testing class and test methods that validate the GET single, GET all, POST, and PUT operations work as expected.*/

    {
        private readonly IConfiguration configuration;

        public EmployeeController(IConfiguration configuration)
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
        public IActionResult GetAllEmployees()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Select e.id AS employeeid, e.FirstName, e.LastName, d.id AS departmentid, d.[Name] AS [Name]
                                        From Employee e
                                        JOIN Department d ON e.DepartmentId = d.Id
                                        Where 1= 1;";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Employee> employees = new List<Employee>();
                    while (reader.Read())
                    {
                        Employee employee = new Employee
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("employeeid")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DepartmentId = reader.GetInt32(reader.GetOrdinal("departmentid")),
                            department = new Department
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("departmentid")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            }
                        };

                        employees.Add(employee);
                    }

                    reader.Close();
                    if (employees.Count == 0)
                    {
                        return NoContent();
                    }
                    else
                    {
                        return Ok(employees);
                    }
                }
            }
        }

        // GET: api/PaymentType/5
        [HttpGet("{id}", Name = "GetSingleEmployee")]
        public Employee Get(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Select e.id AS employeeid, e.FirstName, e.LastName, d.id AS departmentid, d.[Name] AS [Name]
                                        From Employee e
                                        JOIN Department d ON e.DepartmentId = d.Id
                                        WHERE e.id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Employee employee = null;
                    if (reader.Read())
                    {
                        employee = new Employee
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("employeeid")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DepartmentId = reader.GetInt32(reader.GetOrdinal("departmentid")),
                            department = new Department
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("departmentid")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            }
                        };
                    }

                    reader.Close();
                    return employee;
                }
            }
        }

        // POST: api/Employee
        [HttpPost]
        public IActionResult Post([FromBody] Employee newEmployee)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Employee (FirstName, LastName, IsSupervisor, DepartmentId)
                                        OUTPUT INSERTED.Id
                                        VALUES (@FirstName, @LastName, @IsSupervisor, @DepartmentId);";
                    cmd.Parameters.Add(new SqlParameter("@FirstName", newEmployee.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@LastName", newEmployee.LastName));
                    cmd.Parameters.Add(new SqlParameter("@IsSupervisor", newEmployee.IsSupervisor));
                    cmd.Parameters.Add(new SqlParameter("@DepartmentId", newEmployee.DepartmentId));

                    int newId = (int)cmd.ExecuteScalar();
                    newEmployee.Id = newId;
                    return CreatedAtRoute("GetSingleCustomer", new { id = newId }, newEmployee);

                }
            }
        }

        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Employee employee)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Employee
                                        SET FirstName = @FirstName, 
                                            LastName = @LastName,
                                            IsSupervisor = @IsSupervisor,
                                            DepartmentId = @DepartmentId
                                        WHERE id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@FirstName", employee.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@LastName", employee.LastName));
                    cmd.Parameters.Add(new SqlParameter("@IsSupervisor", employee.IsSupervisor));
                    cmd.Parameters.Add(new SqlParameter("@DepartmentId", employee.DepartmentId));
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}