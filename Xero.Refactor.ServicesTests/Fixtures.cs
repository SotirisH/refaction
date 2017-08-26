using Xero.Refactor.Services;

namespace Xero.Refactor.ServicesTests
{
    internal static class Fixtures
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


    }
}
