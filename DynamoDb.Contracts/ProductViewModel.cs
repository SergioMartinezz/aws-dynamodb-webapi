using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDb.Contracts
{
    public class ProductViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public ResultsType ResultsType { get; set; }
        
        // Paginación
        public string PaginationToken { get; set; }
    }
}
