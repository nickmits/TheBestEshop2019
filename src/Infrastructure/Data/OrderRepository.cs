using Microsoft.ESportShop.ApplicationCore.Entities.OrderAggregate;
using Microsoft.ESportShop.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Microsoft.ESportShop.Infrastructure.Data
{
    public class OrderRepository : EfRepository<Order>, IOrderRepository
    {
        public OrderRepository(CatalogContext dbContext) : base(dbContext)
        {
        }

        public Task<Order> GetByIdWithItemsAsync(int id)
        {
            return _dbContext.Orders
                .Include(o => o.OrderItems)
                .Include($"{nameof(Order.OrderItems)}." +
                     $"{nameof(OrderItem.ItemOrdered)}")
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
