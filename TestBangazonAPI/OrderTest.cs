// CREATED BY: HM

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
    public class OrderTest
    {
        [Fact]
        public async Task Test_Create_Order()
        {
            using (var client = new APIClientProvider().Client)
            {
                Order newOrder = new Order
                {
                    CustomerId = 2,
                    PaymentTypeId = 4 
                };

                var orderAsJSON = JsonConvert.SerializeObject(newOrder);

                var response = await client.PostAsync(
                    "/api/order",
                    new StringContent(orderAsJSON, Encoding.UTF8, "application/json")
                );

                string responseBody = await response.Content.ReadAsStringAsync();

                var newOrderEqual = JsonConvert.DeserializeObject<Order>(responseBody);

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal(2, newOrderEqual.CustomerId);
                Assert.Equal(4, newOrderEqual.PaymentTypeId);
                

            }
        }


        
        [Fact]
        public async Task Test_Get_All_Orders()
        {

            using (var client = new APIClientProvider().Client)
            {

                var response = await client.GetAsync("/api/order");

                string responseBody = await response.Content.ReadAsStringAsync();
                var orderList = JsonConvert.DeserializeObject<List<Order>>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(orderList.Count > 0);
            }
        }

        /*

        [Fact]
        public async Task Test_Modify_PaymentType()
        {
            string newName = "Black Card";

            using (var client = new APIClientProvider().Client)
            {
                PaymentType modifiedPaymentType = new PaymentType
                {
                    Name = newName,
                    AcctNumber = 555555555,
                    CustomerId = 1
                };
                var modifiedPaymentTypeAsJSON = JsonConvert.SerializeObject(modifiedPaymentType);

                var response = await client.PutAsync(
                    "/api/paymenttype/2",
                    new StringContent(modifiedPaymentTypeAsJSON, Encoding.UTF8, "application/json")
                );
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var getBlackCard = await client.GetAsync("/api/paymenttype/2");
                getBlackCard.EnsureSuccessStatusCode();

                string getBlackCardBody = await getBlackCard.Content.ReadAsStringAsync();
                PaymentType newBlackCard = JsonConvert.DeserializeObject<PaymentType>(getBlackCardBody);

                Assert.Equal(HttpStatusCode.OK, getBlackCard.StatusCode);
                Assert.Equal(newName, newBlackCard.Name);
            }
        }

    */
    }
}