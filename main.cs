using System;
using System.Threading;
using System.Net.Http.Formatting;
using System.Security.Cryptography.Xml;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MyApp
{
    class Program
    {
       
        static void Main(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();
            int Number = 0;
            var ProductApi = new MyApi();
            // TODO: cache api lookup for products, separate into function
            app.MapGet("/product", async () => {
                
                JsonElement productArray = await ProductApi.GetProductArray();
                JsonElement product = productArray[0];
                return product.ToString();
            });
            app.MapGet("/cheaper-than", async (int price) => {

                JsonElement productArray = await ProductApi.GetProductArray();
                var result = new JsonArray();
                foreach(var product in productArray.EnumerateArray()) {
                    var productPrice = product.GetProperty("price").GetInt64();
                    if (productPrice < price){
                        result.Add(product);
                    }
                }
                
                return result.ToString();
            });
            app.MapGet("/number", async () => $"Number: {Number}");
            app.MapPut("/number", async (int number) => {
                await Task.Delay(10000);
                Number = number;
                return $"Number: {Number}";
            });

            app.Run();
        }

       
    }
}