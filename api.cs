using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace MyApp {
    class MyApi
    {
        const string API_URL = "https://dummyjson.com/";
        MemoryCache cache;
        
        public MyApi() {
            cache = new MemoryCache(
                new MemoryCacheOptions { }
            );

        }

        async Task<JsonElement> GetEndpoint(string endpoint) {
            if(cache.TryGetValue(endpoint, out JsonElement property)){
                return property;
            }
            using var client = new HttpClient();
            using HttpResponseMessage response = await client.GetAsync(API_URL+endpoint);
            using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsByteArrayAsync());
            var cacheEntryOptions = new MemoryCacheEntryOptions{
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
            };

            cache.Set(endpoint, document.RootElement.Clone(), cacheEntryOptions);
            
            return document.RootElement.Clone();
        }
        public async Task<JsonElement> GetProductArray() {
            JsonElement root = await GetEndpoint("products");
            return root.GetProperty("products");
        }

        
    }
}