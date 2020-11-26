using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Settings;
using MongoDB.Driver;

namespace Catalog.API.Data
{
  public class CatalogContext : ICatalogContext
  {
    public CatalogContext(ICatalogDbSettings dbSettings)
    {
        var client = new MongoClient(dbSettings.ConnectionString);
        var database = client.GetDatabase(dbSettings.DatabaseName);

        Products = database.GetCollection<Product>(dbSettings.CollectionName);
        CatalogContextSeed.Seed(Products);
    }

    public IMongoCollection<Product> Products { get; }
  }
}