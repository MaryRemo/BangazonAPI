// CREATED BY: AB

using BangazonSprintStartUp.Models;
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
    public class PaymentTypeTest
    {
        [Fact]
        public async Task Test_Create_PaymentType()
        {
            using (var client = new APIClientProvider().Client)
            {
                PaymentType paymentType = new PaymentType
                {
                    Name = "MasterCard",
                    AcctNumber = 555555555,
                    CustomerId = 1
                };

                var paymentTypeAsJSON = JsonConvert.SerializeObject(paymentType);

                var response = await client.PostAsync(
                    "/api/paymenttype",
                    new StringContent(paymentTypeAsJSON, Encoding.UTF8, "application/json")
                );

                string responseBody = await response.Content.ReadAsStringAsync();

                var newPaymentType = JsonConvert.DeserializeObject<PaymentType>(responseBody);

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("MasterCard", newPaymentType.Name);
                Assert.Equal(555555555, newPaymentType.AcctNumber);
                Assert.Equal(1, newPaymentType.CustomerId);

            }
        }

        [Fact]
        public async Task Test_Get_All_PaymentTypes()
        {

            using (var client = new APIClientProvider().Client)
            {

                var response = await client.GetAsync("/api/paymenttype");

                string responseBody = await response.Content.ReadAsStringAsync();
                var paymentTypeList = JsonConvert.DeserializeObject<List<PaymentType>>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(paymentTypeList.Count > 0);
            }
        }

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

                var getPaymentType = await client.GetAsync("/api/paymenttype/1");
                getPaymentType.EnsureSuccessStatusCode();

                string getPaymentTypeBody = await getPaymentType.Content.ReadAsStringAsync();
                PaymentType newPaymentType = JsonConvert.DeserializeObject<PaymentType>(getPaymentTypeBody);

                Assert.Equal(HttpStatusCode.NoContent, getPaymentType.StatusCode);
                Assert.Equal(newName, newPaymentType.Name);
            }
        }

        /*[Fact]
        public async Task Test_Delete_Student()
        {
            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/api/students/?q=Gucci");

                string yeah = await response.Content.ReadAsStringAsync();

                var letsWork = JsonConvert.DeserializeObject<List<Student>>(yeah);

                var studentResponse = await client.DeleteAsync(
                    $"/api/students/{letsWork[0].Id}"
                );
                string responseBody = await studentResponse.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, studentResponse.StatusCode);

                var getStudent = await client.GetAsync($"/api/students/{letsWork[0].Id}");
                getStudent.EnsureSuccessStatusCode();
            }
        }*/
    }
}