using Carsties.Shared.Extensions.Logger;
using Carsties.Shared.Helpers;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Entities;

namespace SearchService.Data;

public class MongoDb
{
    public static async Task InitConnection(ILogger logger, IConfiguration configuration, IWebHostEnvironment env)
    {
        await DB.InitAsync("SearchDb", MongoClientSettings
            .FromConnectionString(configuration.GetConnectionString("MongoDbConnection")));

        await DB.Index<Item>()
            .Key(x => x.Make, KeyType.Text)
            .Key(x => x.Model, KeyType.Text)
            .Key(x => x.Color, KeyType.Text)
            .CreateAsync();

        var documentCount = await DB.CountAsync<Item>();

        if (documentCount == 0)
        {
            logger.Here().Debug("No documents found - attempt to seed");
            var path = Path.Combine(env.ContentRootPath, "App_Data/SeedData");
            var items = FileReader<Item>.SeederFileReader("SampleItem", path);
            await DB.SaveAsync(items);
        }
    }
}
