using Microsoft.ESportShop.ApplicationCore.Entities.OrderAggregate;
using System.Threading.Tasks;

namespace Microsoft.ESportShop.ApplicationCore.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrderAsync(int basketId, Address shippingAddress);
    }
}
