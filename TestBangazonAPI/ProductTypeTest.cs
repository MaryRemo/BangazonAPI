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

namespace BangazonSprint
{
    public class ProductTypeTest
    {
        [Fact]
        public async Task Test_Create_ProductType()
        {
            using (var client = new APIClientProvider().Client)
            {
                ProductType productType = new ProductType
                {
                    Name = "Toys"
                };

                var productTypeAsJSON = JsonConvert.SerializeObject(productType);

                var response = await client.PostAsync(
                    "/api/producttype",
                    new StringContent(productTypeAsJSON, Encoding.UTF8, "application/json")
                );

                string responseBody = await response.Content.ReadAsStringAsync();

                var newProductType = JsonConvert.DeserializeObject<ProductType>(responseBody);

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("Toys", newProductType.Name);

            }
        }

        [Fact]
        public async Task Test_Get_All_ProductTypes()
        {

            using (var client = new APIClientProvider().Client)
            {

                var response = await client.GetAsync("/api/producttype");

                string responseBody = await response.Content.ReadAsStringAsync();
                var productTypeList = JsonConvert.DeserializeObject<List<ProductType>>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(productTypeList.Count > 0);
            }
        }

        [Fact]
        public async Task Test_Modify_ProductType()
        {
            string newName = "Bouncy Balls";

            using (var client = new APIClientProvider().Client)
            {
                ProductType modifiedProductType = new ProductType
                {
                    Name = newName
                };
                var modifiedProductTypeAsJSON = JsonConvert.SerializeObject(modifiedProductType);

                var response = await client.PutAsync(
                    "/api/producttype/2",
                    new StringContent(modifiedProductTypeAsJSON, Encoding.UTF8, "application/json")
                );
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var getBouncyBalls = await client.GetAsync("/api/producttype/2");
                getBouncyBalls.EnsureSuccessStatusCode();

                string getBouncyBallsBody = await getBouncyBalls.Content.ReadAsStringAsync();
                ProductType newBouncyBall = JsonConvert.DeserializeObject<ProductType>(getBouncyBallsBody);

                Assert.Equal(HttpStatusCode.OK, getBouncyBalls.StatusCode);
                Assert.Equal(newName, newBouncyBall.Name);
            }
        }
    }
}
