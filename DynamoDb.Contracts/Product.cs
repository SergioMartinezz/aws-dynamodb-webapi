using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;

namespace DynamoDb.Contracts
{
    [DynamoDBTable("Products")]
    public class Product
    {
        [DynamoDBProperty("id")]
        [DynamoDBHashKey]
        public Guid Id { get; set; }
        
        [DynamoDBProperty("Name")]
        public string Name { get; set; }
        
        [DynamoDBProperty("Price")]
        public int Price { get; set; }
        
        [DynamoDBProperty("Stock")]
        public int Stock { get; set; }

        [DynamoDBProperty("Providers")]
        public List<string> Providers { get; set; }
    }
}
