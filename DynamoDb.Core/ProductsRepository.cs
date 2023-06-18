using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using DynamoDb.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDb.Core
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly AmazonDynamoDBClient _client;
        private readonly DynamoDBContext _context;

        public ProductsRepository()
        {
            _client = new AmazonDynamoDBClient();
            _context = new DynamoDBContext(_client);
        }

        public async Task Add(ProductInputModel entity)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = entity.Name,
                Price = entity.Price,
                Stock = entity.Stock,
                Providers = entity.Providers
            };
            await _context.SaveAsync(product);
        }

        public async Task<ProductViewModel> All(string paginationToken = "")
        {
            var table = _context.GetTargetTable<Product>();
            var scanOps = new ScanOperationConfig();

            if (!string.IsNullOrEmpty(paginationToken))
            {
                scanOps.PaginationToken = paginationToken;
            }

            var results = table.Scan(scanOps);

            List<Document> data = await results.GetNextSetAsync();

            IEnumerable<Product> products = _context.FromDocuments<Product>(data);

            return new ProductViewModel
            {
                PaginationToken = results.PaginationToken,
                Products = products,
                ResultsType = ResultsType.List
            };
        }

        public async Task<IEnumerable<Product>> Find(SearchRequest searchReq)
        {
            var scanConditions = new List<ScanCondition>();
            if (!string.IsNullOrEmpty(searchReq.Name))
                scanConditions.Add(new ScanCondition("Name", ScanOperator.Equal, searchReq.Name));

            return await _context.ScanAsync<Product>(scanConditions, null).GetRemainingAsync();
        }

        public async Task Remove(Guid productId)
        {
            await _context.DeleteAsync<Product>(productId);
        }

        public async Task<Product> Single(Guid productId)
        {
            return await _context.LoadAsync<Product>(productId);
        }

        public async Task Update(Guid productId, ProductInputModel entity)
        {
            var product = await Single(productId);

            product.Name = entity.Name;
            product.Price = entity.Price;
            product.Stock = entity.Stock;

            if(entity.InputType == DynamoDb.Contracts.InputType.AddProvider)
            {
                product.Providers.Add(entity.Providers.First());
            }

            if(entity.InputType == DynamoDb.Contracts.InputType.DeleteProvider)
            {
                product.Providers.Remove(entity.Providers.First());
            }

            await _context.SaveAsync<Product>(product);
        }
    }
}
