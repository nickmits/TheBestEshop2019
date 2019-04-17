using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.ESportShop.ApplicationCore.Entities;
using Microsoft.ESportShop.ApplicationCore.Interfaces;
using Microsoft.ESportShop.ApplicationCore.Specifications;
using Microsoft.ESportShop.Web.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.ESportShop.Web.Services
{
    /// <summary>
    /// This is a UI-specific service so belongs in UI project. It does not contain any business logic and works
    /// with UI-specific types (view models and SelectListItem types).
    /// </summary>
    public class CatalogViewModelService : ICatalogViewModelService
    {
        private readonly ILogger<CatalogViewModelService> _logger;
        private readonly IAsyncRepository<CatalogItem> _itemRepository;
        private readonly IAsyncRepository<CatalogBrand> _brandRepository;
        private readonly IAsyncRepository<CatalogType> _typeRepository;
        private readonly IUriComposer _uriComposer;

        public CatalogViewModelService(
            ILoggerFactory loggerFactory,
            IAsyncRepository<CatalogItem> itemRepository,
            IAsyncRepository<CatalogBrand> brandRepository,
            IAsyncRepository<CatalogType> typeRepository,
            IUriComposer uriComposer)
        {
            _logger = loggerFactory.CreateLogger<CatalogViewModelService>();
            _itemRepository = itemRepository;
            _brandRepository = brandRepository;
            _typeRepository = typeRepository;
            _uriComposer = uriComposer;
        }

        public async Task<CatalogIndexViewModel> GetCatalogItems(int pageIndex, int itemsPage, int? brandId, int? typeId)
        {
            _logger.LogInformation("GetCatalogItems called.");

            var filterSpecification = new CatalogFilterSpecification(brandId, typeId);
            var filterPaginatedSpecification =
                new CatalogFilterPaginatedSpecification(itemsPage * pageIndex, itemsPage, brandId, typeId);

            // the implementation below using ForEach and Count. We need a List.
            var itemsOnPage = await _itemRepository.ListAsync(filterPaginatedSpecification);
            var totalItems = await _itemRepository.CountAsync(filterSpecification);

            foreach (var itemOnPage in itemsOnPage)
            {
                itemOnPage.PictureUri = _uriComposer.ComposePicUri(itemOnPage.PictureUri);
            }

            var vm = new CatalogIndexViewModel()
            {
                CatalogItems = itemsOnPage.Select(i => new CatalogItemViewModel()
                {
                    Id = i.Id,
                    Name = i.Name,
                    PictureUri = i.PictureUri,
                    Price = i.Price
                }),
                Brands = await GetBrands(),
                Types = await GetTypes(),
                BrandFilterApplied = brandId ?? 0,
                TypesFilterApplied = typeId ?? 0,
                PaginationInfo = new PaginationInfoViewModel()
                {
                    ActualPage = pageIndex,
                    ItemsPerPage = itemsOnPage.Count,
                    TotalItems = totalItems,
                    TotalPages = int.Parse(Math.Ceiling(((decimal)totalItems / itemsPage)).ToString())
                }
            };

            vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
            vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";

            return vm;
        }

        public async Task<IEnumerable<SelectListItem>> GetBrands()
        {
            _logger.LogInformation("GetBrands called.");
            var brands = await _brandRepository.ListAllAsync();

            var items = new List<SelectListItem>
            {
                new SelectListItem() { Value = null, Text = "All", Selected = true }
            };
            foreach (CatalogBrand brand in brands)
            {
                items.Add(new SelectListItem() { Value = brand.Id.ToString(), Text = brand.Brand });
            }

            return items;
        }

        public async Task<IEnumerable<SelectListItem>> GetTypes()
        {
            _logger.LogInformation("GetTypes called.");
            var types = await _typeRepository.ListAllAsync();
            var items = new List<SelectListItem>
            {
                new SelectListItem() { Value = null, Text = "All", Selected = true }
            };
            foreach (CatalogType type in types)
            {
                items.Add(new SelectListItem() { Value = type.Id.ToString(), Text = type.Type });
            }

            return items;
        }

        public async Task<CatalogType> AddCatalogType(string CatalogType)
        {
            if (await _typeRepository.AnyAsync(ctype => ctype.Type == CatalogType))
            {
                return await _typeRepository.FirstOrDefault(ctype => ctype.Type == CatalogType);
            }
            return await _typeRepository.AddAsync(new CatalogType
            {
                Type = CatalogType
            });
        }

        public async Task<CatalogBrand> AddCatalogBrand(string CatalogBrand)
        {
            if (await _brandRepository.AnyAsync(ctype => ctype.Brand == CatalogBrand))
            {
                return await _brandRepository.FirstOrDefault(ctype => ctype.Brand == CatalogBrand);
            }
            return await _brandRepository.AddAsync(new CatalogBrand
            {
                Brand = CatalogBrand
            });
        }

        public async Task<CatalogItem> AddCatalogItem(CatalogItem ItemToAdd)
        {
            return await _itemRepository.AddAsync(ItemToAdd);
        }

        public async Task DeleteCatalogItem(int Id)
        {
            var ItemToDelete = await _itemRepository.GetByIdAsync(Id);
            await _itemRepository.DeleteAsync(ItemToDelete);
        }

        public async Task UpdateCatalogItemPrice(int Id, decimal Price)
        {
            var ItemToUpdate = await _itemRepository.GetByIdAsync(Id);
            ItemToUpdate.Price = Price;
            await _itemRepository.UpdateAsync(ItemToUpdate);
        }

        public async Task UpdateCatalogItem(CatalogItem ItemToUpdate)
        {
            await _itemRepository.UpdateAsync(ItemToUpdate);
        }
    }
}