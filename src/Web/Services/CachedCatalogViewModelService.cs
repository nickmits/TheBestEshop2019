﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.ESportShop.Web.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using System;
using Microsoft.ESportShop.ApplicationCore.Entities;

namespace Microsoft.ESportShop.Web.Services
{
    public class CachedCatalogViewModelService : ICatalogViewModelService
    {
        private readonly IMemoryCache _cache;
        private readonly CatalogViewModelService _catalogViewModelService;
        private static readonly string _brandsKey = "brands";
        private static readonly string _typesKey = "types";
        private static readonly string _itemsKeyTemplate = "items-{0}-{1}-{2}-{3}";
        private static readonly TimeSpan _defaultCacheDuration = TimeSpan.FromSeconds(30);

        public CachedCatalogViewModelService(IMemoryCache cache,
            CatalogViewModelService catalogViewModelService)
        {
            _cache = cache;
            _catalogViewModelService = catalogViewModelService;
        }

        public async Task<IEnumerable<SelectListItem>> GetBrands()
        {
            return await _cache.GetOrCreateAsync(_brandsKey, async entry =>
                    {
                        entry.SlidingExpiration = _defaultCacheDuration;
                        return await _catalogViewModelService.GetBrands();
                    });
        }

        public async Task<CatalogIndexViewModel> GetCatalogItems(int pageIndex, int itemsPage, int? brandId, int? typeId)
        {
            string cacheKey = String.Format(_itemsKeyTemplate, pageIndex, itemsPage, brandId, typeId);
            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.SlidingExpiration = _defaultCacheDuration;
                return await _catalogViewModelService.GetCatalogItems(pageIndex, itemsPage, brandId, typeId);
            });
        }

        public async Task<IEnumerable<SelectListItem>> GetTypes()
        {
            return await _cache.GetOrCreateAsync(_typesKey, async entry =>
            {
                entry.SlidingExpiration = _defaultCacheDuration;
                return await _catalogViewModelService.GetTypes();
            });
        }

        public Task AddCatalogItem()
        {
            throw new NotImplementedException();
        }

        public async Task DeleteCatalogItem(int Id)
        {
           await _catalogViewModelService.DeleteCatalogItem(Id);
        }

        public async Task UpdateCatalogItemPrice(int Id, decimal Price)
        {
            await _catalogViewModelService.UpdateCatalogItemPrice(Id, Price);
        }

        public Task<CatalogItem> AddCatalogItem(CatalogItem Item)
        {
            throw new NotImplementedException();
        }

        public Task<CatalogType> AddCatalogType(string CatalogType)
        {
            throw new NotImplementedException();
        }

        public Task<CatalogBrand> AddCatalogBrand(string CatalogBrand)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCatalogItem(CatalogItem ItemToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
