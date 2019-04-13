using Ardalis.GuardClauses;
using System;

namespace Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate
{
    /// <summary>
    /// Represents a snapshot of the item that was ordered. If catalog item details change, details of
    /// the item that was part of a completed order should not change.
    /// </summary>
    public class CatalogItemOrdered // ValueObject
    {
        public int CatalogItemId { get; private set; }
        public string ProductName { get; private set; }
        public string PictureUri { get; private set; }

        private CatalogItemOrdered()
        {

        }
        public CatalogItemOrdered(int catalogItemId, string productName, string pictureUri)
        {
            if (catalogItemId == null)
            Guard.Against.OutOfRange(catalogItemId, nameof(catalogItemId), 1, int.MaxValue);
            if (productName == null) { throw new ArgumentNullException();}
            if (pictureUri == null) { throw new ArgumentNullException(); }

            CatalogItemId = catalogItemId;
            ProductName = productName;
            PictureUri = pictureUri;
        }
    }
}
