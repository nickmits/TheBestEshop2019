using Microsoft.ESportShop.ApplicationCore.Entities.OrderAggregate;
using System.Threading.Tasks;

namespace Microsoft.ESportShop.ApplicationCore.Interfaces
{

    public interface IOrderRepository : IAsyncRepository<Order>
    {
        Task<Order> GetByIdWithItemsAsync(int id);
    }
}
