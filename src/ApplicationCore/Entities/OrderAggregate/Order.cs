using Microsoft.ESportShop.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;

namespace Microsoft.ESportShop.ApplicationCore.Entities.OrderAggregate
{
    public class Order : BaseEntity
    {
        public Order() {}
        public Order(string buyerId, Address shipToAddress, List<OrderItem> items)
        {
            BuyerId = buyerId ?? throw new ArgumentNullException();
            ShipToAddress = shipToAddress ?? throw new ArgumentNullException();
            _orderItems = items ?? throw new ArgumentNullException();
        }
        public string BuyerId { get; private set; }

        public DateTimeOffset OrderDate { get; private set; } = DateTimeOffset.Now;
        public Address ShipToAddress { get; private set; }

        private readonly List<OrderItem> _orderItems = new List<OrderItem>();
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();    
        public decimal Total()
        {
            var total = 0m;
            foreach (var item in _orderItems)
            {
                total += item.UnitPrice * item.Units;
            }
            return total;
        }
    }
}
