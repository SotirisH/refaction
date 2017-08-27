using System;
using Xero.Refactor.Services;

namespace Xero.Refactor.Test.Common
{
    public static class FixturesDto
    {
        /// <summary>
        /// Builds one Product
        /// </summary>
        /// <returns></returns>
        public static ProductDto ProductsBuildOne()
        {
            var faker = new Bogus.Faker();
            var prod = new ProductDto();
            prod.Name = faker.Commerce.Product();
            prod.Price = decimal.Parse(faker.Commerce.Price());
            prod.DeliveryPrice = decimal.Parse(faker.Commerce.Price());
            prod.Description = faker.Commerce.ProductName();

            return prod;
        }
        /// <summary>
        /// Builds a mock of a ProductOptionDto
        /// </summary>
        /// <param name="productId">The id of the related product. If it not specified then a new random Guid will be created and assigned to this mock</param>
        /// <returns></returns>
        public static ProductOptionDto ProductOptionBuildOne(Guid? productId)
        {
            if (productId.GetValueOrDefault() == Guid.Empty)
            {
                productId = Guid.NewGuid();
            }
            var faker = new Bogus.Faker();
            var prod = new ProductOptionDto();
            prod.Name = faker.Commerce.Product();
            prod.Description = faker.Commerce.ProductMaterial();

            return prod;
        }

    }
}
