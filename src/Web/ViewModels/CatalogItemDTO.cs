using Microsoft.AspNetCore.Http;

namespace Microsoft.eShopWeb.Web.ViewModels
{
    public class CatalogItemDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile Picture { get; set; }
        public string CatalogType { get; set; }
        public string CatalogBrand { get; set; }
    }
}
