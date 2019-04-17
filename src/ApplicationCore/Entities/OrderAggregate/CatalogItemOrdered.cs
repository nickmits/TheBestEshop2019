using System;

namespace Microsoft.ESportShop.ApplicationCore.Entities.OrderAggregate
{
    public class CatalogItemOrdered 
    {
        public int CatalogItemId { get; private set; }
        public string ProductName { get; private set; }
        public string PictureUri { get; private set; }

        private CatalogItemOrdered() { }
        public CatalogItemOrdered(int catalogItemId, string productName, string pictureUri)
        {
            if ( catalogItemId != CatalogItemId) throw new ArgumentOutOfRangeException();
            CatalogItemId = catalogItemId;
            ProductName = productName ?? throw new ArgumentNullException();
            PictureUri = pictureUri ?? throw new ArgumentNullException();
        }
    }
}
