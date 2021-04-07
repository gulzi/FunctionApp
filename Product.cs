
using Microsoft.Azure.Cosmos.Table;

namespace IbizProductsFunctionApp
{
    public class Product: TableEntity
    {                        
        public Product(){}                
        public Product(string category,string sku)
        {
            this.PartitionKey = category;
            this.RowKey = sku;            
        }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
