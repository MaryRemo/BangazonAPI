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
    public class CustomerTest
    {
        [Fact]
        public async Task Test_Create_Customer()
        {
            using (var client = new APIClientProvider().Client)
            {
                Customer customer = new Customer
                {
                    FirstName = "Mary",
                    LastName = "Massie"
                };

                var customerAsJSON = JsonConvert.SerializeObject(customer);

                var response = await client.PostAsync(
                    "/api/paymenttype",
                    new StringContent(customerAsJSON, Encoding.UTF8, "application/json")
                );

                string responseBody = await response.Content.ReadAsStringAsync();

                var newCustomer = JsonConvert.DeserializeObject<Customer>(responseBody);

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("Mary", newCustomer.FirstName);
                Assert.Equal("Massie", newCustomer.LastName);
            }
        }

        [Fact]
        public async Task Test_Get_All_Customers()
        {

            using (var client = new APIClientProvider().Client)
            {

                var response = await client.GetAsync("/api/paymenttype");

                string responseBody = await response.Content.ReadAsStringAsync();
                var customerList = JsonConvert.DeserializeObject<List<Customer>>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(customerList.Count > 0);
            }
        }

        [Fact]
        public async Task Test_Modify_Customer()
        {
            string newName = "Remo";

            using (var client = new APIClientProvider().Client)
            {
                Customer modifiedCustomer = new Customer
                {
                    FirstName = "Mary",
                    LastName = newName
                };
                var modifiedCustomerAsJSON = JsonConvert.SerializeObject(modifiedCustomer);

                var response = await client.PutAsync(
                    "/api/customer/1",
                    new StringContent(modifiedCustomerAsJSON, Encoding.UTF8, "application/json")
                );
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var getRemo = await client.GetAsync("/api/customer/2");
                getBlackCard.EnsureSuccessStatusCode();

                string getRemoBody = await getRemo.Content.ReadAsStringAsync();
                Customer newLastName = JsonConvert.DeserializeObject<Customer>(getRemoBody);

                Assert.Equal(HttpStatusCode.OK, getRemo.StatusCode);
                Assert.Equal(newName, newRemo.LastName);
            }
        }
    }
}
