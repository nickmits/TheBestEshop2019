using Microsoft.ESportShop.ApplicationCore.Interfaces;
using System.Collections.Generic;
using System;

namespace Microsoft.ESportShop.ApplicationCore.Entities.BuyerAggregate
{
    public class Buyer : BaseEntity
    {
        public string IdentityGuid { get; private set; }
        private Buyer() {}

        public Buyer(string identity) : this()
        {
            IdentityGuid = identity ?? throw new ArgumentNullException();
        }
    }
}
