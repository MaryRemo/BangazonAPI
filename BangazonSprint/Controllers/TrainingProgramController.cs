using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BangazonSprint.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BangazonSprint.Controllers
{

    /*GET
    POST
    PUT
    DELETE (but only if the start date is in the future)
    User should be able to GET a list, and GET a single item.
    Employees who signed up for a training program should be included in the response
    Should be able to view only programs starting today, or in the future, with the ?completed=false query string parameter.*/

    [Route("api/[controller]")]
    [ApiController]
    public class TrainingProgramController : ControllerBase
    {

        private readonly IConfiguration _config;

        public TrainingProgramController(IConfiguration config)
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

        // GET: api/TrainingProgram
        [HttpGet]
        public List<TrainingProgram> GetTrainingProgram(string completed, string q)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (completed == "false")
                    {
                        cmd.CommandText = $@"Select tp.id, tp.[Name], tp.StartDate, tp.EndDate, tp.MaxAttendees,
                                        et.TrainingProgramId, et.EmployeeId, e.FirstName, e.LastName, d.id AS departmentid, d.Name AS deptName
                                             From TrainingProgram tp
                                            right JOIN EmployeeTraining et ON et.TrainingProgramId = tp.id
                                            left JOIN Employee e ON e.id = et.EmployeeId
                                            join Department d ON d.id = e.DepartmentId
                                            Where EndDate >= GetDate()";
                    }
                    else
                    {
                        cmd.CommandText = $@"Select tp.id, tp.[Name], tp.StartDate, tp.EndDate, tp.MaxAttendees,
                                        et.TrainingProgramId, et.EmployeeId, e.FirstName, e.LastName, e.departmentid, d.id AS departmentid, d.Name AS deptName
                                             From TrainingProgram tp
                                            right JOIN EmployeeTraining et ON et.TrainingProgramId = tp.id
                                            left JOIN Employee e ON e.id = et.EmployeeId
                                            join Department d ON d.id = e.DepartmentId
                                    WHERE 1 = 1";
                    }
                    SqlDataReader reader = cmd.ExecuteReader();

                    Dictionary<int, TrainingProgram> trainingprogram = new Dictionary<int, TrainingProgram>();
                    while (reader.Read())
                    {
                        int trainingprogramid = reader.GetInt32(reader.GetOrdinal("trainingprogramid"));

                        if (!trainingprogram.ContainsKey(trainingprogramid))
                        {
                            TrainingProgram training = new TrainingProgram
                            {
                                Id = trainingprogramid,
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                                EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                                MaxAttendees = reader.GetInt32(reader.GetOrdinal("MaxAttendees")),
                                employeeList = new List<Employee>(),
                            };
                            trainingprogram.Add(trainingprogramid, training);
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("employeeid")))
                        {
                            TrainingProgram currentTrainingProgram = trainingprogram[trainingprogramid];
                            currentTrainingProgram.employeeList.Add(
                            new Employee
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("employeeid")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                DepartmentId = reader.GetInt32(reader.GetOrdinal("departmentid")),
                                department = new Department
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("departmentid")),
                                    Name = reader.GetString(reader.GetOrdinal("deptName"))
                                }

                              }
                               
                            );

                        }

                    }
                            reader.Close();
                            return trainingprogram.Values.ToList();
                        }
                    }
                }
 


        // GET: api/Customer/5
        [HttpGet("{id}", Name = "GetSingleTrainingProgram")]
        public List<TrainingProgram> GetTrainingProgram(int id, string include)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Select tp.id AS trainingprogramid, tp.[Name], tp.StartDate, tp.EndDate, tp.MaxAttendees,
                                        et.TrainingProgramId, et.EmployeeId, e.FirstName, e.LastName, e.departmentid, d.id AS departmentid, d.Name AS deptName
                                             From TrainingProgram tp
                                            right JOIN EmployeeTraining et ON et.TrainingProgramId = tp.id
                                            left JOIN Employee e ON e.id = et.EmployeeId
                                            join Department d ON d.id = e.DepartmentId
                                             WHERE tp.id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Dictionary<int, TrainingProgram> trainingprogram = new Dictionary<int, TrainingProgram>();
                    while (reader.Read())
                    {
                        int trainingprogramid = reader.GetInt32(reader.GetOrdinal("trainingprogramid"));

                        if (!trainingprogram.ContainsKey(trainingprogramid))
                        {
                            TrainingProgram training = new TrainingProgram
                            {
                                Id = trainingprogramid,
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                                EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                                MaxAttendees = reader.GetInt32(reader.GetOrdinal("MaxAttendees")),
                                employeeList = new List<Employee>(),
                            };
                            trainingprogram.Add(trainingprogramid, training);
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("employeeid")))
                        {
                            TrainingProgram currentTrainingProgram = trainingprogram[trainingprogramid];
                            currentTrainingProgram.employeeList.Add(
                            new Employee
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("employeeid")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                DepartmentId = reader.GetInt32(reader.GetOrdinal("departmentid")),
                                department = new Department
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("departmentid")),
                                    Name = reader.GetString(reader.GetOrdinal("deptName"))
                                }

                            }

                            );

                        }

                    }
                    reader.Close();
                    return trainingprogram.Values.ToList();
                }
            }
        }

        // POST: api/Customer
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TrainingProgram newTrainingProgram)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO TrainingProgram (Name, StartDate, EndDate, MaxAttendees)
                                                OUTPUT INSERTED.Id
                                                VALUES (@Name, @StartDate, @EndDate, @MaxAttendees);";
                    cmd.Parameters.Add(new SqlParameter("@Name", newTrainingProgram.Name));
                    cmd.Parameters.Add(new SqlParameter("@StartDate", newTrainingProgram.StartDate));
                    cmd.Parameters.Add(new SqlParameter("@EndDate", newTrainingProgram.EndDate));
                    cmd.Parameters.Add(new SqlParameter("@MaxAttendees", newTrainingProgram.MaxAttendees));

                    int newId = (int)cmd.ExecuteScalar();
                    newTrainingProgram.Id = newId;
                    return CreatedAtRoute("GetSingleTrainingProgram", new { id = newId }, newTrainingProgram);

                }
            }
        }

        // PUT: api/TrainingProgram/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] TrainingProgram training)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE TrainingProgram
                                                SET Name = @Name, 
                                                    StartDate = @StartDate,
                                                    EndDate = @EndDate,
                                                    MaxAttendees = @MaxAttendees
                                                WHERE id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@Name", training.Name));
                    cmd.Parameters.Add(new SqlParameter("@StartDate", training.StartDate));
                    cmd.Parameters.Add(new SqlParameter("@EndDate", training.EndDate));
                    cmd.Parameters.Add(new SqlParameter("@MaxAttendees", training.MaxAttendees));
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
