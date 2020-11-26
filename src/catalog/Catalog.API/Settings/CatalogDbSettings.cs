namespace Catalog.API.Settings
{
  public class CatalogDbSettings : ICatalogDbSettings
  {
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string CollectionName { get; set; }
  }
}