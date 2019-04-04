// CREATED BY: HMN

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


namespace BangazonSprint
{
    public class ComputerTest
    {

        [Fact]
        public async Task Test_Get_Computer()
        {
            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("api/computer");
                string responseBody = await response.Content.ReadAsStringAsync();
                var computerList = JsonConvert.DeserializeObject<List<Product>>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(computerList.Count > 0);
            }
        }

        [Fact]
        //public async Task Test_Get_Single_Computer()
        //{
        //    using (var client = new APIClientProvider().Client)
        //    {
        //        var computerGetInitialResponse = await client.GetAsync("api/computer");

        //        string initialResponseBody = await computerGetInitialResponse.Content.ReadAsStringAsync();

        //        var computerList = JsonConvert.DeserializeObject<List<Product>>(initialResponseBody);

        //        Assert.Equal(HttpStatusCode.OK, computerGetInitialResponse.StatusCode);

        //        var computerObject = computerList[0];

        //        //var response = await client.GetAsync($"api/computer/{computerObject.Id}");
        //        var response = await client.GetAsync($"api/computer/1");


        //        string responseBody = await response.Content.ReadAsStringAsync();

        //        var computerReturned = JsonConvert.DeserializeObject<Computer>(responseBody);

        //        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //        Assert.True(computerReturned.Id == computerObject.Id);
        //    }
        //}



        public async Task Test_Get_Computer_By_Id()
        {
            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("api/computer/1");

                string responseBody = await response.Content.ReadAsStringAsync();

                var computer = JsonConvert.DeserializeObject<Computer>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.NotNull(computer.Make);
            }
        }
  



    [Fact]
        public async Task Test_Create_Computer()
        {
            using (var client = new APIClientProvider().Client)
            {
                Computer computer = new Computer
                {
                    PurchaseDate = new DateTime(1999, 01, 01),
                    DecomissionDate = new DateTime(2002, 05, 05),
                    Make = "Macbook Pro",
                    Manufacturer = "Apple"
                };

                var computerAsJSON = JsonConvert.SerializeObject(computer);
                var response = await client.PostAsync(
                    "/api/computer",
                    new StringContent(computerAsJSON, Encoding.UTF8, "application/json")
                );

                string responseBody = await response.Content.ReadAsStringAsync();

                var newComputer = JsonConvert.DeserializeObject<Computer>(responseBody);

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal(computer.PurchaseDate, newComputer.PurchaseDate);
                Assert.Equal(computer.DecomissionDate, newComputer.DecomissionDate);
                Assert.Equal(computer.Make, newComputer.Make);
                Assert.Equal(computer.Manufacturer, newComputer.Manufacturer);
            }
        }

        [Fact]
        public async Task Test_Edit_Computer()
        {

            using (var client = new APIClientProvider().Client)
            {

                string newMake = "XPS 13";
                Computer computer = new Computer
                {
                    PurchaseDate = new DateTime(1999, 2, 16),
                    DecomissionDate = new DateTime(1994, 5, 26),
                    Make = newMake,
                    Manufacturer = "Dell"
                };

                var modifiedComputerAsJSON = JsonConvert.SerializeObject(computer);

                var response = await client.PutAsync(
                    "/api/computer/1",
                    new StringContent(modifiedComputerAsJSON, Encoding.UTF8, "application/json")
                );
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var getmax = await client.GetAsync("/api/Computer");
                getmax.EnsureSuccessStatusCode();

                string getBody = await getmax.Content.ReadAsStringAsync();
                var newMaxAttendees = JsonConvert.DeserializeObject<List<Computer>>(getBody);
                Assert.Equal(HttpStatusCode.OK, getmax.StatusCode);
                Assert.Equal(newMake, newMaxAttendees[0].Make);
            }
        }
    



[Fact]
        public async Task Test_Delete_Computer()
        {
            using (var client = new APIClientProvider().Client)
            {
                var computerGetOriginalComputer = await client.GetAsync("api/product");
                string computerGetResponseBody = await computerGetOriginalComputer.Content.ReadAsStringAsync();
                var computerList = JsonConvert.DeserializeObject<List<Computer>>(computerGetResponseBody);

                Assert.Equal(HttpStatusCode.OK, computerGetOriginalComputer.StatusCode);

                int deleteLastComputerObject = computerList.Count - 1;
                var computerObject = computerList[deleteLastComputerObject];

                var response = await client.DeleteAsync($"api/product/{computerObject.Id}");
                string responseBody = await response.Content.ReadAsStringAsync();

                var getComputer = await client.GetAsync($"api/product/{computerObject.Id}");
                getComputer.EnsureSuccessStatusCode();

                string getComputerBody = await getComputer.Content.ReadAsStringAsync();
                Computer newComputer = JsonConvert.DeserializeObject<Computer>(getComputerBody);

                Assert.Equal(HttpStatusCode.NoContent, getComputer.StatusCode);
            }
        }
    }
}