using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDb.Contracts
{
    public interface IProductsRepository
    {
        Task<Product> Single(Guid productId);
        Task<ProductViewModel> All(string paginationToken = "");
        Task<IEnumerable<Product>> Find(SearchRequest searchReq);
        Task Add(ProductInputModel entity);
        Task Update(Guid productId, ProductInputModel entity);
        Task Remove(Guid productId);
    }
}
