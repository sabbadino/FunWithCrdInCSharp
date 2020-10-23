using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using k8s;
using KubeOps.Operator.Entities;
using KubeOps.TestOperator.Entities;
using KubeOps.WareHouse.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using warehouseapi.Dtos;

namespace warehouseapi.Controllers
{


    [Route("api/warehouse")]
    public class WareHouseController : Controller
    {
        public const string Namespace = "dev";
        private readonly IKubernetes _kubernetesClient;

        static JsonSerializer _camelCaseSerializer = JsonSerializer.Create(
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

        public WareHouseController(IKubernetes kubernetesClient)
        {
            _kubernetesClient = kubernetesClient;
        }


        [HttpGet()]
        public List<ProductAvailabilityReadDto> ListToDoItemS()
        {
            var list = _kubernetesClient.ListNamespacedCustomObject(ProductAvailabilityEntity.CrdGroup,
                ProductAvailabilityEntity.CrdApiVersion, Namespace, ProductAvailabilityEntity.CrdPluralName) as JObject;
            var toDoItemList = list.ToObject<EntityList<ProductAvailabilityEntity>>();
            var ret = new List<ProductAvailabilityReadDto>();
            foreach (var item in toDoItemList.Items)


            {
                ret.Add(new ProductAvailabilityReadDto
                {
                    CreationTimeStamp = item.Metadata.CreationTimestamp,
                    Id = item.Metadata.Name,
                    AvailableQuantity = item.Spec.AvailableQuantity,
                    LastUpdateTimeStamp= item.Spec.LastUpdateTimeStamp,
                });


            }

            return ret;
        }

        [HttpPost()]
        public ActionResult<ProductAvailabilityReadDto> AddProductAvailability(ProductAvailabilityDto productAvailabilityDto)
        {
            var x = new ProductAvailabilityEntity
            {
                ApiVersion = ProductAvailabilityEntity.CrdGroup + "/" + ProductAvailabilityEntity.CrdApiVersion,
                Kind = ProductAvailabilityEntity.CrdKind,
                Metadata = { Name = productAvailabilityDto.Id },
                Spec =
            {
                AvailableQuantity = productAvailabilityDto.AvailableQuantity
            }
            };

            var body = JObject.FromObject(x, _camelCaseSerializer);
            var retJson = _kubernetesClient.CreateNamespacedCustomObject(body, ProductAvailabilityEntity.CrdGroup
                , ProductAvailabilityEntity.CrdApiVersion
                , Namespace, ProductAvailabilityEntity.CrdPluralName) as JObject;
            var ret = retJson.ToObject<ProductAvailabilityEntity>();
            return new ProductAvailabilityReadDto { AvailableQuantity = ret.Spec.AvailableQuantity, Id = ret.Metadata.Name, CreationTimeStamp = ret.Metadata.CreationTimestamp };
        }

        [HttpPost("get-from-wareHouse")]
        public ActionResult<ProductAvailabilityReadDto> GetProductFromWareHouse(GetProductCommand getProductCommand)
        {
            var productAvailabilityJson = _kubernetesClient.GetNamespacedCustomObject(
                ProductAvailabilityEntity.CrdGroup,
                ProductAvailabilityEntity.CrdApiVersion, Namespace,
                ProductAvailabilityEntity.CrdPluralName, getProductCommand.ProductId) as JObject;
            var productAvailability = productAvailabilityJson.ToObject<ProductAvailabilityEntity>();

            if (productAvailability.Spec.AvailableQuantity >= getProductCommand.Quantity)
            {
                var x = new ProductAvailabilityEntity
                {
                    ApiVersion = ProductAvailabilityEntity.CrdGroup + "/" + ProductAvailabilityEntity.CrdApiVersion,
                    Kind = ProductAvailabilityEntity.CrdKind,
                    Metadata =
                    {
                        Name = getProductCommand.ProductId,
                        ResourceVersion = productAvailability.Metadata.ResourceVersion
                    },
                    Spec =
                    {
                        LastUpdateTimeStamp = DateTime.UtcNow,
                        AvailableQuantity = productAvailability.Spec.AvailableQuantity - getProductCommand.Quantity
                    }
                };
                var body = JObject.FromObject(x, _camelCaseSerializer);

                productAvailabilityJson = _kubernetesClient.ReplaceNamespacedCustomObject(body, ProductAvailabilityEntity.CrdGroup, ProductAvailabilityEntity.CrdApiVersion
                , Namespace, ProductAvailabilityEntity.CrdPluralName, getProductCommand.ProductId) as JObject;
                productAvailability = productAvailabilityJson.ToObject<ProductAvailabilityEntity>();

                return new ProductAvailabilityReadDto
                {
                    AvailableQuantity = productAvailability.Spec.AvailableQuantity,
                    Id = productAvailability.Metadata.Name
                };
            }

            return BadRequest(new
            {
                Error =
                    $"There are only {productAvailability.Spec.AvailableQuantity} items for product {getProductCommand.ProductId}, {getProductCommand.Quantity} items cannot be requested"
            });
        }


    }
}
