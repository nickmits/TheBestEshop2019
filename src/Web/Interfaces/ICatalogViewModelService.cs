using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.ESportShop.ApplicationCore.Entities;
using Microsoft.ESportShop.Web.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.ESportShop.Web.Services
{
    public interface ICatalogViewModelService
    {
        Task<CatalogIndexViewModel> GetCatalogItems(int pageIndex, int itemsPage, int? brandId, int? typeId);
        Task<IEnumerable<SelectListItem>> GetBrands();
        Task<IEnumerable<SelectListItem>> GetTypes();
        Task<CatalogItem> AddCatalogItem(CatalogItem Item);
        Task DeleteCatalogItem(int Id);
        Task UpdateCatalogItemPrice(int Id, decimal Price);
        Task UpdateCatalogItem(CatalogItem ItemToUpdate);
        Task<CatalogType> AddCatalogType(string CatalogType);
        Task<CatalogBrand> AddCatalogBrand(string CatalogBrand);
    }
}
