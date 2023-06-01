using System;
using System.Threading;
using System.Net.Http.Formatting;
using System.Security.Cryptography.Xml;
using System.Text.Json;
using System.Text.Json.Nodes;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MyApp
{
    class Program
    {
       
        static void Main(string[] args)
        {
            var db = new Database();
            /*
            await db.recipes.InsertOneAsync(new MongoDB.Bson.BsonDocument{
                {"Author","Jim"},
                {"Name", "Tiramisu"},
                {"Tags", new BsonArray{"italian", "dessert"}},
                {"Rating", 50}
            });
            */
            /*
            BsonDocument filter = new BsonDocument(){
                {"Tags", "italian"}
            };
            var italianRecipes = await db.recipes.Find(filter).ToListAsync();   
            foreach(var recipe in italianRecipes){
                Console.WriteLine(recipe.ToString());
            }
            */
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();
            int Number = 0;
            var ProductApi = new MyApi();

            // TODO add a GUI frontend, with a name and author field, that sends this POST request
            app.MapPost("/recipes", async (string name, string author) => {
                var insertRecipe = new BsonDocument() {
                    {"Name", name},
                    {"Author", author}
                };
                await db.recipes.InsertOneAsync(insertRecipe);
            });

            // TODO add a GET recipes endpoint, that lists all the recipes
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
            app.MapGet("/number", () => $"Number: {Number}");
            app.MapPut("/number", async (int number) => {
                await Task.Delay(10000);
                Number = number;
                return $"Number: {Number}";
            });

            app.Run();
        }

       
    }
}