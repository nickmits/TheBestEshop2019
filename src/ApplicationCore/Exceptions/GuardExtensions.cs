using Microsoft.ESportShop.ApplicationCore.Exceptions;
using Microsoft.ESportShop.ApplicationCore.Entities.BasketAggregate;

namespace Microsoft.ESportShop.ApplicationCore.Exceptions
{
    public static class BasketGuards
    {
        public static void NullBasket(int basketId, Basket basket)
        {
            if (basket == null) throw new BasketNotFoundException(basketId);
        }
    }
}