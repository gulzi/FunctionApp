
using Microsoft.Azure.Cosmos.Table;

namespace IbizProductsFunctionApp
{
    public class Product: TableEntity
    {                        
        public Product(){}                
        public Product(string category,string id)
        {
            this.PartitionKey = category;
            this.RowKey = id;            
        }
        public string Name { get; set; }
        public string SkuNumber { get; set; }
        public double Price { get; set; }
    }
}
