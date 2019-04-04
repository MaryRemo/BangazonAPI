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
    public class TrainingProgramTest
    {
        [Fact]
        public async Task Test_Get_All_TrainingPrograms()
        {
            using (var client = new APIClientProvider().Client)
            {
               
                var response = await client.GetAsync("/api/TrainingProgram");


                string responseBody = await response.Content.ReadAsStringAsync();
                var orderList = JsonConvert.DeserializeObject<List<TrainingProgram>>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(orderList.Count > 0);
            }
        }

        [Fact]
        public async Task Test_Create_Training()
        {

            using (var client = new APIClientProvider().Client)
            {
               
                var getOldList = await client.GetAsync("/api/trainingprogram");
                getOldList.EnsureSuccessStatusCode();

                string getOldListBody = await getOldList.Content.ReadAsStringAsync();
                var oldList = JsonConvert.DeserializeObject<List<TrainingProgram>>(getOldListBody);



                TrainingProgram trainingprogram = new TrainingProgram
                {
                    Name = "Learn to Swim",
                    StartDate = new DateTime(2019, 04, 20),
                    EndDate = new DateTime(2019, 04, 24),
                    MaxAttendees = 15
                };

                var modifiedOrderAsJSON = JsonConvert.SerializeObject(trainingprogram);

                var response = await client.PostAsync(
                    "/api/trainingprogram",
                    new StringContent(modifiedOrderAsJSON, Encoding.UTF8, "application/json")
                );

                string responseBody = await response.Content.ReadAsStringAsync();

            
                var getOrder = await client.GetAsync("/api/trainingprogram");
                var newProgram = JsonConvert.DeserializeObject<TrainingProgram>(responseBody);


                Assert.Equal(HttpStatusCode.OK, getOrder.StatusCode);
                Assert.Equal(15, newProgram.MaxAttendees);
            }
        }

        [Fact]
        public async Task Test_Modify_TrainingProgram()
        {

            using (var client = new APIClientProvider().Client)
            {

                int newmax = 15;
                TrainingProgram trainingprogram = new TrainingProgram
                {
                    Name = "How To Sell Cars",
                    StartDate = new DateTime(2020, 02, 14),
                    EndDate = new DateTime(2019, 02, 15),
                    MaxAttendees = newmax
                };

                var modifiedDeptAsJSON = JsonConvert.SerializeObject(trainingprogram);

                var response = await client.PutAsync(
                    "/api/TrainingProgram/1",
                    new StringContent(modifiedDeptAsJSON, Encoding.UTF8, "application/json")
                );
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var getmax = await client.GetAsync("/api/TrainingProgram");
                getmax.EnsureSuccessStatusCode();

                string getBody = await getmax.Content.ReadAsStringAsync();
               var newMaxAttendees = JsonConvert.DeserializeObject<List<TrainingProgram>>(getBody);
                Assert.Equal(HttpStatusCode.OK, getmax.StatusCode);
                Assert.Equal(newmax, newMaxAttendees[0].MaxAttendees);
            }
        }
    }
}
