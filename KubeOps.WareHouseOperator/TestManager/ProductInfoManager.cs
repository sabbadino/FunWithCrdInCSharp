using System;
using System.Net;
using k8s;
using KubeOps.Operator.Client;
using KubeOps.TestOperator.Entities;
using KubeOps.WareHouse.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace KubeOps.WareHouse.TestManager
{
    public class ProductInfoManager : IManager<ProductInfoEntity>
    {
        static JsonSerializer _camelCaseSerializer = JsonSerializer.Create(
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        private readonly IKubernetesClient _kubernetesClient;
        private readonly ILogger<ProductInfoManager> _logger;
        public const string Namespace = "dev";
        public ProductInfoManager(IKubernetesClient kubernetesClient, ILogger<ProductInfoManager> logger)
        {
            _kubernetesClient = kubernetesClient;
            _logger = logger;
        }

        public void Created(ProductInfoEntity entity)
        {
            _logger.LogDebug(nameof(Created));
            try
            {
                var existingProductAvailability = _kubernetesClient.ApiClient.GetNamespacedCustomObject(
                    ProductAvailabilityEntity.CrdGroup
                    , ProductAvailabilityEntity.CrdApiVersion
                    , Namespace, ProductAvailabilityEntity.CrdPluralName, entity.Metadata.Name);
            }
            catch (HttpOperationException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.NotFound)
                {
                    var x = new ProductAvailabilityEntity
                    {
                        ApiVersion = ProductAvailabilityEntity.CrdGroup + "/" + ProductAvailabilityEntity.CrdApiVersion,
                        Kind = ProductAvailabilityEntity.CrdKind,
                        Metadata = { Name = entity.Metadata.Name },
                        Spec =
                        {
                            LastUpdateTimeStamp = DateTime.UtcNow,
                            AvailableQuantity= entity.Spec.MaximumQuantity
                        }
                    };
                    var body = JObject.FromObject(x, _camelCaseSerializer);
                    _kubernetesClient.ApiClient.CreateNamespacedCustomObject(body, ProductAvailabilityEntity.CrdGroup
                        , ProductAvailabilityEntity.CrdApiVersion
                        , Namespace, ProductAvailabilityEntity.CrdPluralName);

                }
            }
        }

        public TimeSpan? Updated(ProductInfoEntity entity)
        {
            _logger.LogDebug(nameof(Updated));
            return null;
        }

        public void StatusModified(ProductInfoEntity entity)
        {
            _logger.LogDebug(nameof(StatusModified));
        }

        public void NotModified(ProductInfoEntity entity)
        {
            _logger.LogDebug(nameof(NotModified));
        }

        public void Deleted(ProductInfoEntity entity)
        {
            _logger.LogDebug(nameof(Deleted));
        }

        public void Finalized(ProductInfoEntity entity)
        {
            _logger.LogDebug(nameof(Finalized));
        }
    }
}
