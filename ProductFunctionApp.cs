
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System;

namespace RouteWildCard
{
    public static class RouteWildCardDemo
    {
        [FunctionName("ProductsFunctionApp")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "{*path}")] HttpRequest req,
            [Table("products")] CloudTable table,
            ILogger log)
        {            
            if (req.Path == "/products")
            {
                TableContinuationToken token = null;
                var entities = new List<Product>();
                do
                {
                    var queryResult = await table.ExecuteQuerySegmentedAsync<Product>(new TableQuery<Product>(), token);                    
                    entities.AddRange(queryResult.Results);
                    token = queryResult.ContinuationToken;
                } while (token != null);
                if (entities.Count ==0)
                {
                    return new NoContentResult();
                }
                return new JsonResult(entities);

            }
            else if (req.Path == "/addprodcucts")
            {
                Product product;
                try
                {
                     product = await MapJsonToProductAsync(req);
                    
                }
                catch (System.Exception e)
                {
                    log.LogWarning("Bad Request: "+e.Message);
                    return new BadRequestResult();
                }                                                
                TableOperation tableOperation = TableOperation.Insert(product);
                TableResult result = await table.ExecuteAsync(tableOperation);
                return new CreatedResult(table.Uri, result.Result);
            }
            else {
                return new NotFoundResult();
            }                        
        }

        private static async Task<Product> MapJsonToProductAsync(HttpRequest req) {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            Product product = new Product(data.category.ToString(), data.id.ToString())
            {
                Name = data.name,
                Price = Convert.ToDouble(data.price),
                SkuNumber = data.sku
            };
            return product;
        }
    }
}
