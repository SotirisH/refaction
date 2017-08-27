using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using Xero.Refactor.WebApi.Modeling.Validators;

namespace Xero.Refactor.WebApi.Modeling
{
    [Validator(typeof(ProductOptionValidator))]
    public class ProductOptionApiModel:ILinkResource
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string RowVersion { get; set; }
        public List<Link> Links { get; set; }
    }
}