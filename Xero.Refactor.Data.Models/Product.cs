using System;
using System.Collections.Generic;
using Xero.AspNet.Core.Data;

namespace Xero.Refactor.Data.Models
{
    public class Product: EntityBase
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }
        public virtual ICollection<ProductOption> ProductOptions { get; set; }

    }
}
