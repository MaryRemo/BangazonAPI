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


        /*
      Getting all orders:
      */
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
       Getting one order:
       */
        [Fact]
        public async Task Test_Get_One_Order()
        {

            using (var client = new APIClientProvider().Client)
            {

                var response = await client.GetAsync("/api/order/6");


                string responseBody = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<Order>(responseBody);


                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(3, order.PaymentTypeId);
            }
        }

        /*
         Deleting a student
        */
        [Fact]
        public async Task Test_Delete_One_Order()
        {

            using (var client = new APIClientProvider().Client)
            {

                var deleteResponse = await client.DeleteAsync("/api/order/15");


                string responseBody = await deleteResponse.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<Order>(responseBody);


                Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            }
        }

        /*
         Updating a student
        */
        [Fact]
        public async Task Test_Modify_Order()
        {
            // New last name to change to and test
            int newPaymentId = 3;

            using (var client = new APIClientProvider().Client)
            {
                /*
                    PUT section
                */
                Order modifiedOrder = new Order
                {
                    CustomerId = 2,
                    PaymentTypeId = newPaymentId
                };
                var modifiedOrderAsJSON = JsonConvert.SerializeObject(modifiedOrder);

                var response = await client.PutAsync(
                    "/api/order/11",
                    new StringContent(modifiedOrderAsJSON, Encoding.UTF8, "application/json")
                );
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
                /*
                   GET section
                   Verify that the PUT operation was successful
                 */
                var getOrder = await client.GetAsync("/api/order/11");
                getOrder.EnsureSuccessStatusCode();

                string getOrderBody = await getOrder.Content.ReadAsStringAsync();
                Order newOrder = JsonConvert.DeserializeObject<Order>(getOrderBody);

                Assert.Equal(HttpStatusCode.OK, getOrder.StatusCode);
                Assert.Equal(newPaymentId, newOrder.PaymentTypeId);
            }
        }
    }
}