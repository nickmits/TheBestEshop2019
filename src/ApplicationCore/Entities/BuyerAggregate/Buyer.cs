using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Ardalis.GuardClauses;
using System.Collections.Generic;
using System;

namespace Microsoft.eShopWeb.ApplicationCore.Entities.BuyerAggregate
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
