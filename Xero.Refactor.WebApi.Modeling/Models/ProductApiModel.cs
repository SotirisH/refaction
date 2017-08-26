using FluentValidation.Attributes;
using System;
using Xero.Refactor.WebApi.Modeling.Validators;

namespace Xero.Refactor.WebApi.Modeling
{
    [Validator(typeof(ProductValidator))]
    public class ProductApiModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string RowVersion { get; set; }

    }
}
