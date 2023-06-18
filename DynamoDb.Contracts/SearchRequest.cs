using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDb.Contracts
{
    public class SearchRequest
    {
        // Sólo queremos buscar por Nombre
        public string Name { get; set; }
    }
}
