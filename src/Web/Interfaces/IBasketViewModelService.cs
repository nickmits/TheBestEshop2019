using Microsoft.ESportShop.Web.Pages.Basket;
using System.Threading.Tasks;

namespace Microsoft.ESportShop.Web.Interfaces
{
    public interface IBasketViewModelService
    {
        Task<BasketViewModel> GetOrCreateBasketForUser(string userName);
    }
}
