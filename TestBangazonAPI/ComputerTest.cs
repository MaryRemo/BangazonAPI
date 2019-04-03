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
        public async Task Test_Get_Single_Computer()
        {
            using (var client = new APIClientProvider().Client)
            {
                var computerGetInitialResponse = await client.GetAsync("api/computer");

                string initialResponseBody = await computerGetInitialResponse.Content.ReadAsStringAsync();

                var computerList = JsonConvert.DeserializeObject<List<Product>>(initialResponseBody);

                Assert.Equal(HttpStatusCode.OK, computerGetInitialResponse.StatusCode);

                var computerObject = computerList[0];

                var response = await client.GetAsync($"api/computer/{computerObject.Id}");

                string responseBody = await response.Content.ReadAsStringAsync();

                var productReturned = JsonConvert.DeserializeObject<Computer>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(computerReturned.Id == computerObject.Id);
            }
        }

        [Fact]
        public async Task Test_Create_Product()
        {
            using (var client = new APIClientProvider().Client)
            {

                Computer computer = new Computer
                {
                    PurchaseDate = 01012001,
                    DecomissionDate = 01012011,
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
                Assert.Equal(01012001, newComputer.PurchaseDate);
                Assert.Equal(01012011, newComputer.DecomissionDate);
                Assert.Equal("Macbook Pro", newComputer.Make);
                Assert.Equal("Apple", newComputer.Manufacturer);
            }
        }

        [Fact]
        public async Task Test_Modify_Make()
        {
            string newMake = "iMac Pro";

            using (var client = new APIClientProvider().Client)
            {
                var computerGetAgain = await client.GetAsync("api/computer");
                string computerGetResponseBody = await computerGetAgain.Content.ReadAsStringAsync();

                var computerList = JsonConvert.DeserializeObject<List<Computer>>(computerGetResponseBody);
                Assert.Equal(HttpStatusCode.OK, computerGetAgain.StatusCode);

                var computerObject = computerList[0];
                var originalComputerMake = JsonConvert.SerializeObject(computerObject.Make);
                computerObjectMake = "iMac Pro";

                var editedComputerAsJson = JsonConvert.SerializeObject(computerObject);
                var response = await client.PutAsync($"api/computer/{computerObject.Id}", new StringContent(editedComputerAsJson, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var getProduct = await client.GetAsync($"/api/product/{productObject.Id}");
                getProduct.EnsureSuccessStatusCode();

                string getProductBody = await getProduct.Content.ReadAsStringAsync();
                Product newProduct = JsonConvert.DeserializeObject<Product>(getProductBody);

                Assert.Equal(HttpStatusCode.OK, getProduct.StatusCode);
                Assert.Equal(newDescription, newProduct.Description);

                newProduct.Description = originalProductDescription;
                var returnEditedProductToOriginalProduct = JsonConvert.SerializeObject(newProduct);

                var putEditedProductToOriginalProduct = await client.PutAsync($"api/product/{newProduct.Id}",
                new StringContent(returnEditedProductToOriginalProduct, Encoding.UTF8, "application/json"));
                string originalProductObject = await response.Content.ReadAsStringAsync();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Delete_Product()
        {
            using (var client = new APIClientProvider().Client)
            {
                var productGetInitialResponse = await client.GetAsync("api/product");
                string initialResponseBody = await productGetInitialResponse.Content.ReadAsStringAsync();
                var productList = JsonConvert.DeserializeObject<List<Product>>(initialResponseBody);

                Assert.Equal(HttpStatusCode.OK, productGetInitialResponse.StatusCode);

                int deleteLastProductObject = productList.Count - 1;
                var productObject = productList[deleteLastProductObject];

                var response = await client.DeleteAsync($"api/product/{productObject.Id}");
                string responseBody = await response.Content.ReadAsStringAsync();

                var getProduct = await client.GetAsync($"api/product/{productObject.Id}");
                getProduct.EnsureSuccessStatusCode();

                string getProductBody = await getProduct.Content.ReadAsStringAsync();
                Product newProduct = JsonConvert.DeserializeObject<Product>(getProductBody);

                Assert.Equal(HttpStatusCode.NoContent, getProduct.StatusCode);
            }
        }
    }
}