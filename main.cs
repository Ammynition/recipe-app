using System;
using System.Threading;
using System.Net.Http.Formatting;
using System.Security.Cryptography.Xml;
using System.Text.Json;
using System.Text.Json.Nodes;
using MongoDB.Driver;
using MongoDB.Bson;
using Newtonsoft.Json;
using System.Windows.Forms;

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
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("https://localhost", "http://localhost")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });
            });

            
            var app = builder.Build();
            app.UseCors();
            
            int Number = 0;
            var ProductApi = new MyApi();
/*
            // TODO add a GUI frontend, with a name and author field, that sends this POST request
            Form mainForm = new Form();
            Label lblFirst = new Label();
            mainForm.Width = 400;
            mainForm.Height = 400; 
            lblFirst.Text = "Name:";
            lblFirst.Location = new Point(200,200);
            mainForm.Controls.Add(lblFirst);
            Application.Run(mainForm);
*/
            app.MapPost("/recipe", async (string name, string author, string tags) => {
                var insertRecipe = new BsonDocument() {
                    {"Name", name},
                    {"Author", author},
                    {"Tags", tags}
                };
                await db.recipes.InsertOneAsync(insertRecipe);
            });

            // DONE? TODO add a GET recipes endpoint, that lists all the recipes
            app.MapGet("/recipes", async (string filter) => {
                if (filter != "null") {
                    Console.WriteLine(filter.GetType());
                    Console.WriteLine("a" + filter + "c");
                    var bsonFilter = Builders<BsonDocument>.Filter.Eq("Tags", filter);
                    var allRecipes = await db.recipes.Find(bsonFilter).ToListAsync();
                    return allRecipes.ToJson(new MongoDB.Bson.IO.JsonWriterSettings() {OutputMode = MongoDB.Bson.IO.JsonOutputMode.Strict});
                }
                else {
                    Console.WriteLine("No Filter");
                    var allRecipes = await db.recipes.Find(document => true).ToListAsync();
                    return allRecipes.ToJson(new MongoDB.Bson.IO.JsonWriterSettings() {OutputMode = MongoDB.Bson.IO.JsonOutputMode.Strict});
                }
                /*
                //var result = new BsonDocument();
                var results = new JsonArray();
                foreach(var recipe in allRecipes)
                {
                    //result.Merge(recipe);
                    results.Add(JsonDocument.Parse(recipe.ToJson()));
                }
                //var recipeObj = BsonTypeMapper.MapToDotNetValue(result);
                //JsonConvert.SerializeObject(recipeObj);
                return results;
                */
            });
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