using BangazonSprint.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestBangazonAPI
{
    public class EmployeeTest
    {
        [Fact]
        public async Task Test_Create_Employee()
        {
            using (var client = new APIClientProvider().Client)
            {
                Employee employee = new Employee
                {
                    FirstName = "Gee",
                    LastName = "Blade",
                    IsSupervisor = false,
                    DepartmentId = 1

                };

                var employeeAsJSON = JsonConvert.SerializeObject(employee);

                var response = await client.PostAsync(
                    "/api/employee",
                    new StringContent(employeeAsJSON, Encoding.UTF8, "application/json")
                );

                string responseBody = await response.Content.ReadAsStringAsync();

                var newEmployee = JsonConvert.DeserializeObject<Employee>(responseBody);

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("Gee", newEmployee.FirstName);
                Assert.Equal("Blade", newEmployee.LastName);
                Assert.False(newEmployee.IsSupervisor);
                Assert.Equal(1, newEmployee.DepartmentId);
              

            }
        }

        [Fact]
        public async Task Test_Get_All_Employees()
        {

            using (var client = new APIClientProvider().Client)
            {

                var response = await client.GetAsync("/api/employee");

                string responseBody = await response.Content.ReadAsStringAsync();
                var employeeList = JsonConvert.DeserializeObject<List<Employee>>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(employeeList.Count > 0);
            }
        }

        [Fact]
        public async Task Test_Modify_Customer()
        {
            string newName = "Remo";

            using (var client = new APIClientProvider().Client)
            {
                Employee modifiedEmployee = new Employee
                {
                    FirstName = "Gee",
                    LastName = newName,
                    IsSupervisor = false,
                    DepartmentId = 1
                };
                var modifiedEmployeeAsJSON = JsonConvert.SerializeObject(modifiedEmployee);

                var response = await client.PutAsync(
                    "/api/employee/3",
                    new StringContent(modifiedEmployeeAsJSON, Encoding.UTF8, "application/json")
                );
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var getRemo = await client.GetAsync("/api/employee/3");
                getRemo.EnsureSuccessStatusCode();

                string getRemoBody = await getRemo.Content.ReadAsStringAsync();
                Customer newLastName = JsonConvert.DeserializeObject<Customer>(getRemoBody);

                Assert.Equal(HttpStatusCode.OK, getRemo.StatusCode);
                Assert.Equal(newName, newLastName.LastName);
            }
        }
    }
}
