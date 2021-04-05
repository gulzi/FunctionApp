
using Microsoft.WindowsAzure.Storage.Table;

namespace FunctionAppIbiz
{
    public class Product: TableEntity
    {                        
        public Product(){}                
        public Product(string type,string sku)
        {
            this.PartitionKey = type;
            this.RowKey = sku;            
        }
        public string Name { get; set; }
    }
}
