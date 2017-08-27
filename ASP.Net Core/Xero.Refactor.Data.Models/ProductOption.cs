using System;
using Xero.AspNet.Core.Modelling;

namespace Xero.Refactor.Data.Models
{
    public class ProductOption : EntityBase
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Navigation Property on Products
        /// </summary>
        public Product Product { get; set; }
    }
}