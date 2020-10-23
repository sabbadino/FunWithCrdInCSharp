using System.Collections.Generic;
using k8s;
using KubeOps.Operator.Entities;
using KubeOps.WareHouse.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using warehouseapi.Dtos;

namespace warehouseapi.Controllers
{
   

    [Route("api/products")]
    public class ProductController : Controller
    {
        public const string Namespace = "dev";
        private readonly IKubernetes _kubernetesClient;
        static JsonSerializer _camelCaseSerializer = JsonSerializer.Create(
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        public ProductController(IKubernetes kubernetesClient)
        {
            _kubernetesClient = kubernetesClient;
        }


        [HttpGet()]
        public List<ProductInfoReadDto> ListProducts()
        {
            var list = _kubernetesClient.ListNamespacedCustomObject(ProductInfoEntity.CrdGroup, ProductInfoEntity.CrdApiVersion, Namespace, ProductInfoEntity.CrdPluralName) as JObject;
            var toDoItemList = list.ToObject<EntityList<ProductInfoEntity>>();
            var ret = new List<ProductInfoReadDto>();
            foreach (var item in toDoItemList.Items)


            {
                ret.Add(new ProductInfoReadDto
                {
                    CreationTimeStamp = item.Metadata.CreationTimestamp,
                    Id = item.Metadata.Name,
                    Description = item.Spec.Description,
                    MaximumQuantity = item.Spec.MaximumQuantity,
                    MinimumQuantity = item.Spec.MinimumQuantity
                });


            }
            return ret;
        }



        [HttpPost()]
        public ProductInfoReadDto AddProduct([FromBody] ProductInfoDto productInfoDto)
        {

            var x = new ProductInfoEntity
            {
                ApiVersion = ProductInfoEntity.CrdGroup + "/" + ProductInfoEntity.CrdApiVersion,
                Kind = ProductInfoEntity.CrdKind,
                Metadata = {Name = productInfoDto.Id},
                Spec =
                {
                    MaximumQuantity = productInfoDto.MaximumQuantity,
                    MinimumQuantity = productInfoDto.MinimumQuantity,
                    Description = productInfoDto.Description
                }
            };
            var body = JObject.FromObject(x,_camelCaseSerializer);
            //kubectl.exe get todospecs.experiments.sabba
            //kubectl.exe get todospecs.experiments.sabba--all - namespaces

            var list = _kubernetesClient.CreateNamespacedCustomObject(body, ProductInfoEntity.CrdGroup , ProductInfoEntity.CrdApiVersion
                , Namespace, ProductInfoEntity.CrdPluralName) as JObject;
        
     
            return new ProductInfoReadDto { Description = productInfoDto.Description,MinimumQuantity = productInfoDto.MinimumQuantity, MaximumQuantity= productInfoDto.MaximumQuantity, Id=productInfoDto.Id};
        }


        [HttpPut()]
        public ProductInfoReadDto UpdateProduct(ProductInfoDto productInfoDto)
        {
            var existingProductItem = _kubernetesClient.GetNamespacedCustomObject(ProductInfoEntity.CrdGroup, ProductInfoEntity.CrdApiVersion, Namespace, ProductInfoEntity.CrdPluralName, productInfoDto.Id) as JObject;

            var x = new ProductInfoEntity
            {
                ApiVersion = ProductInfoEntity.CrdGroup + "/" + ProductInfoEntity.CrdApiVersion,
                Kind = ProductInfoEntity.CrdKind,
                Metadata =
                {
                    Name = productInfoDto.Id,
                    ResourceVersion = existingProductItem["metadata"]["resourceVersion"].ToString()
                },
                Spec =
                {
                    MinimumQuantity = productInfoDto.MinimumQuantity,
                    MaximumQuantity = productInfoDto.MaximumQuantity,
                    Description = productInfoDto.Description
                }
            };


            var body = JObject.FromObject(x,_camelCaseSerializer);
            //kubectl.exe get todospecs.experiments.sabba
            //kubectl.exe get todospecs.experiments.sabba--all - namespaces
            //var patch = new JsonPatchDocument<ToDoItemSpec>();
            //patch.Replace(e => e.Spec.What , productInfoDto.What);
            //patch.Replace(e => e.Spec.When, productInfoDto.When);
            ////var body = JObject.FromObject(patch);
            //var list = _kubernetesClient.ApiClient.PatchNamespacedCustomObject(new V1Patch(patch), "experiments.sabba", "v1", "dev", "todoitemspecs", productInfoDto.Id) as JObject;

            var list = _kubernetesClient.ReplaceNamespacedCustomObject(body, ProductInfoEntity.CrdGroup, ProductInfoEntity.CrdApiVersion
                , Namespace, ProductInfoEntity.CrdPluralName, productInfoDto.Id) as JObject;

            

            return new ProductInfoReadDto { Description = productInfoDto.Description,MinimumQuantity= productInfoDto.MinimumQuantity, MaximumQuantity= productInfoDto.MaximumQuantity, Id = productInfoDto.Id };
        }

        [HttpDelete()]
        public void DeleteProduct(string Id)
        {

            var list = _kubernetesClient.DeleteNamespacedCustomObject(ProductInfoEntity.CrdGroup , ProductInfoEntity.CrdApiVersion
                , Namespace, ProductInfoEntity.CrdPluralName, Id) as JObject;

            
        }
    }
}
