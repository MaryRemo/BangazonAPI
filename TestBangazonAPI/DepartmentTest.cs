// CREATED BY: AB

using BangazonSprint.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestBangazonAPI;
using Xunit;

namespace BangazonSprintStartUp
{
    public class DepartmentTest
    {
        [Fact]
        public async Task Test_Create_Department()
        {
            using (var client = new APIClientProvider().Client)
            {
                Department department = new Department
                {
                    Name = "Toys",
                    Budget = 300000
                };

                var departmentAsJSON = JsonConvert.SerializeObject(department);

                var response = await client.PostAsync(
                    "/api/department",
                    new StringContent(departmentAsJSON, Encoding.UTF8, "application/json")
                );

                string responseBody = await response.Content.ReadAsStringAsync();

                var newDepartment = JsonConvert.DeserializeObject<Department>(responseBody);

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("Toys", department.Name);
                Assert.Equal(300000, department.Budget);

            }
        }
        [Fact]
        public async Task Test_Get_All_Departments()
        {

            using (var client = new APIClientProvider().Client)
            {

                var response = await client.GetAsync("/api/department");

                string responseBody = await response.Content.ReadAsStringAsync();
                var departmentList = JsonConvert.DeserializeObject<Dictionary<int, Department>>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(departmentList.Count > 0);
            }
        }

        [Fact]
         public async Task Test_Modify_Department()
        {
            string newName = "Bubblegum";

            using (var client = new APIClientProvider().Client)
            {
                Department modifiedDepartment = new Department
                {
                    Id = 1,
                    Name = newName,
                    Budget = 10000
                };
                var modifiedDepartmentAsJSON = JsonConvert.SerializeObject(modifiedDepartment);

                var response = await client.PutAsync(
                    "/api/department/1",
                    new StringContent(modifiedDepartmentAsJSON, Encoding.UTF8, "application/json")
                );
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var getCandy = await client.GetAsync("/api/department/1");
                getCandy.EnsureSuccessStatusCode();

                string getCandyBody = await getCandy.Content.ReadAsStringAsync();
                Dictionary<int, Department> newCandy = JsonConvert.DeserializeObject<Dictionary<int, Department>>(getCandyBody);

                Assert.Equal(HttpStatusCode.OK, getCandy.StatusCode);
                Assert.Equal(newName, newCandy[1].Name);
            }
        }
    }
}