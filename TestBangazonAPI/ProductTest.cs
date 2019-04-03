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

/*
    **************HMN: Helpful links and resources************
    * For more information on xUnit, please see Getting Started with xUnit.net: https://xunit.net/
    * For information on xUnit test patterns, see: http://xunitpatterns.com/Parameterized%20Test.html 
    * Newtonsoft.Json Docs (also see the Serialization Guide in this resource): https://www.newtonsoft.com/json/help/html/SerializingJSON.htm
 */

namespace BangazonSprint
{
    public class ProductTest
    {
        //HMN Notes: [Fact] is an xUnit attribute. When we use it like this we say that we are "decorating a method" with the Fact attribute. [Fact] is used to identify a test method (TestCreateProduct() in this case) that takes no arguments. Another attribute of xUnit that we are not using is the [Theory] attribute. [Theory] expects one or more DataAttribute instances to supply the values for a Parameterized Test's method arguments. 

        [Fact]
        public async Task Test_Get_Product()
        {
            using (var client = new APIClientProvider().Client)
            //HMN: APIClientProvider is defined in APIClientProvider.cs in TestBangazonAPI folder at the top level of our application. 

            {
                var response = await client.GetAsync("api/product");
                //HMN: Gets the entire list of objects at the api/product Route. GetAsync is a method provided on HttpClient.

                string responseBody = await response.Content.ReadAsStringAsync();
                //HMN: Stores the content of the Http response as a string

                var productList = JsonConvert.DeserializeObject<List<Product>>(responseBody);
                //HMN: Converts the "stringified" Http response as a "jsonized" object of type Product and stores it in the new variable productList

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                //HMN: HttpStatusCode.Ok is equivalent to Http status 200; this indicates that the requested information is in the response.

                Assert.True(productList.Count > 0);
                //HMN: If there are more than 0 items in the productList variable, something was retrieved from the database.

                //HMN: If time allows, work on a GET test that has specific parameters (i.e., specifically the object's "price" or "description") that need to be evaluated before the test passes, rather than simply ensuring that something came back from the database. Because just getting "anything" (or, getting an object with more than 0 items in it) doesn't seem good enough.
            }
        }

        [Fact]
        public async Task Test_Get_Single_Product()
        {

            using (var client = new APIClientProvider().Client)
            {
                var productGetInitialResponse = await client.GetAsync("api/products");

                string initialResponseBody = await productGetInitialResponse.Content.ReadAsStringAsync();

                var productList = JsonConvert.DeserializeObject<List<Product>>(initialResponseBody);

                Assert.Equal(HttpStatusCode.OK, productGetInitialResponse.StatusCode);

                var productObject = productList[0];

                var response = await client.GetAsync($"api/products/{productObject.Id}");

                string responseBody = await response.Content.ReadAsStringAsync();

                var productReturned = JsonConvert.DeserializeObject<Product>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(productReturned.Id == productObject.Id);
            }
        }

        [Fact]
        public async Task Test_Create_Product()
        {
            using (var client = new APIClientProvider().Client)
            {

                Product product = new Product
                {
                    ProductTypeId = 2,
                    CustomerId = 2,
                    Price = 199,
                    Title = "Kitten",
                    Description = "Soft kitty, warm kitty, little ball of fur; happy kitty, sleepy kitty, purr purr purr.",
                    Quantity = 1
                };

                var productAsJSON = JsonConvert.SerializeObject(product);
                //HMN: "jsonizes" the parameter of SerializedObject(); the parameter is the "product" object created above.

                //HMN: This takes the new, "jsonized" object and posts it to the database:
                var response = await client.PostAsync(
                    "/api/product",
                    new StringContent(productAsJSON, Encoding.UTF8, "application/json")
                );

                //HMN: converts the entire jsonized object to a string and reads it:
                string responseBody = await response.Content.ReadAsStringAsync();

                //HMN: "unjsonizes" the object (productAsJson) and stores it in a new variable, newProduct:
                var newProduct = JsonConvert.DeserializeObject<Product>(responseBody);
                                                                                    
                /*
                         HMN Notes: 
                        Created:
                        HttpStatusCode.Created = 201
                        Equivalent to HTTP Status 201. This indicates that the request resulted in a new resource created before the response was sent

                        response.StatusCode
                        (response is a local variable)
                        Status Code is a property: HttpStatusCode HttpResponseMessage.StatusCode {get; set; }
                        Gets or sets the code of the HTTP response
                         */

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal(2, newProduct.ProductTypeId);
                Assert.Equal(2, newProduct.CustomerId);
                Assert.Equal(199, newProduct.Price);
                Assert.Equal("Kitten", newProduct.Title);
                Assert.Equal("Soft kitty, warm kitty, little ball of fur; happy kitty, sleepy kitty, purr purr purr.", newProduct.Description);
                Assert.Equal(1, newProduct.Quantity);
            }
        }

        //[Fact]
        //public async Task TestGetAllProducts()
        //{

        //    using (var client = new APIClientProvider().Client)
        //    {

        //        var response = await client.GetAsync("/api/product");

        //        string responseBody = await response.Content.ReadAsStringAsync();
        //        var productList = JsonConvert.DeserializeObject<List<Product>>(responseBody);

        //        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //        //NOTE: HMN: How do I know which StatusCode is returned here? 
        //        Assert.True(productList.Count > 0);
        //    }
        //}

        [Fact]
        public async Task Test_Modify_Product()
        {
            int newPrice = 499;

            using (var client = new APIClientProvider().Client)
            {
                Product modifiedProduct = new Product
                {
                    ProductTypeId = 2,
                    CustomerId = 2,
                    Price = newPrice,
                    Title = "Kitten",
                    Description = "Soft kitty, warm kitty, little ball of fur; happy kitty, sleepy kitty, purr purr purr.",
                    Quantity = 1

                };
                var modifiedProductAsJSON = JsonConvert.SerializeObject(modifiedProduct);

                //HMN Notes: Serialization is the process of converting an object into a stream of bytes to store the object or transmit it to memory, a database, or a file.Its main purpose is to save the state of an object in order to be able to recreate it when needed.

                var response = await client.PutAsync(
                    "/api/product/6",
                    new StringContent(modifiedProductAsJSON, Encoding.UTF8, "application/json")
                );
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var getPrice = await client.GetAsync("/api/product/6");
                getPrice.EnsureSuccessStatusCode();

                string getPriceBody = await getPrice.Content.ReadAsStringAsync();
                Product NewPrice = JsonConvert.DeserializeObject<Product>(getPriceBody);

                Assert.Equal(HttpStatusCode.OK, getPrice.StatusCode);
                Assert.Equal(newPrice, NewPrice.Price);
            }
        }
    }
}