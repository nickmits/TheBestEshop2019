using Microsoft.ESportShop.ApplicationCore.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.ESportShop.ApplicationCore.Specifications;
using Microsoft.ESportShop.ApplicationCore.Entities;
using System.Linq;
using Microsoft.ESportShop.ApplicationCore.Entities.BasketAggregate;
using System;

namespace Microsoft.ESportShop.ApplicationCore.Services
{
    public class BasketService : IBasketService
    {
        private readonly IAsyncRepository<Basket> _basketRepository;
        private readonly IAsyncRepository<BasketItem> _basketItemRepository;
        private readonly IUriComposer _uriComposer;
        private readonly IAppLogger<BasketService> _logger;

        public BasketService(IAsyncRepository<Basket> basketRepository,
            IUriComposer uriComposer,
            IAppLogger<BasketService> logger,
            IAsyncRepository<BasketItem> basketItemRepository)
        {
            _basketRepository = basketRepository;
            _uriComposer = uriComposer;
            _logger = logger;
            _basketItemRepository = basketItemRepository;
        }

        public async Task AddItemToBasket(int basketId, int catalogItemId, decimal price, int quantity)
        {
            var basket = await _basketRepository.GetByIdAsync(basketId);

            basket.AddItem(catalogItemId, price, quantity);

            await _basketRepository.UpdateAsync(basket);
        }

        public async Task DeleteBasketAsync(int basketId)
        {
            var basket = await _basketRepository.GetByIdAsync(basketId);

            foreach (var item in basket.Items.ToList())
            {
                await _basketItemRepository.DeleteAsync(item);
            }

            await _basketRepository.DeleteAsync(basket);
        }

        public async Task<int> GetBasketItemCountAsync(string userName)
        {
            if (userName == null) throw new ArgumentNullException("this username doesntexist");
            var basketSpec = new BasketWithItemsSpecification(userName);
            var basket = (await _basketRepository.ListAsync(basketSpec)).FirstOrDefault();
            if (basket == null)
            {
                _logger.LogInformation($"No basket found for {userName}");
                return 0;
            }
            int count = basket.Items.Sum(i => i.Quantity);
            _logger.LogInformation($"Basket for {userName} has {count} items.");
            return count;
        }

        public async Task SetQuantities(int basketId, Dictionary<string, int> quantities)
        {
            if (quantities == null)  throw new ArgumentNullException("Quantities are null"); 

            var basket = await _basketRepository.GetByIdAsync(basketId);

            if (basket == null) throw new ArgumentException("Basket is null");
            
            foreach (var item in basket.Items)
            {
                if (quantities.TryGetValue(item.Id.ToString(), out var quantity))
                {
                    _logger.LogInformation($"Updating quantity of item ID:{item.Id} to {quantity}.");
                    item.Quantity = quantity;
                }
            }
            await _basketRepository.UpdateAsync(basket);
        }

        public async Task TransferBasketAsync(string anonymousId, string userName)
        {
            if (anonymousId == null) throw new ArgumentNullException();
            var basketSpec = new BasketWithItemsSpecification(anonymousId);
            var basket = (await _basketRepository.ListAsync(basketSpec)).FirstOrDefault();
            if (basket == null) return;
            basket.BuyerId = userName ?? throw new ArgumentNullException();
            await _basketRepository.UpdateAsync(basket);
        }
    }
}
