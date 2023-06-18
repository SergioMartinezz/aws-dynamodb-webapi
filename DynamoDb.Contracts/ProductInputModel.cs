using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDb.Contracts
{
    public class ProductInputModel
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public List<string>? Providers { get; set; }

        // Creando o Actualizando
        public InputType InputType { get; set; }
    }
}
