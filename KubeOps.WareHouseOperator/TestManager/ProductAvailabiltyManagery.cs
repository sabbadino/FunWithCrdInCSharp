using System;
using System.Collections.Generic;
using k8s;
using k8s.Models;
using KubeOps.Operator.Client;
using KubeOps.TestOperator.Entities;
using KubeOps.WareHouse.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace KubeOps.WareHouse.TestManager
{
    public class ProductAvailabilityManager : IManager<ProductAvailabilityEntity>
    {
        static JsonSerializer _camelCaseSerializer = JsonSerializer.Create(
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        private readonly IKubernetesClient _kubernetesClient;
        private readonly ILogger<ProductAvailabilityManager> _logger;
        public const string Namespace = "dev";
        public ProductAvailabilityManager(IKubernetesClient kubernetesClient, ILogger<ProductAvailabilityManager> logger)
        {
            _kubernetesClient = kubernetesClient;
            _logger = logger;
        }

        public void Created(ProductAvailabilityEntity entity)
        {
            _logger.LogDebug(nameof(Created));

           
            
        }

        private readonly TimeSpan _refillTime = new TimeSpan(0, 1, 0);
        private readonly TimeSpan _pollTime = new TimeSpan(0, 0, 10);

        public TimeSpan? Updated(ProductAvailabilityEntity entity)
        {
            _logger.LogDebug(nameof(Updated));
            if ((DateTime.UtcNow - entity.Spec.LastUpdateTimeStamp) < _refillTime)
            {
                entity.Annotations().TryAdd("re-queued","true");
                return _pollTime;
            }

            //search for product info , get it and set it back to its maximum availability 
            var existingProductItemJson = _kubernetesClient.ApiClient.GetNamespacedCustomObject(
                ProductInfoEntity.CrdGroup, ProductInfoEntity.CrdApiVersion, Namespace, ProductInfoEntity.CrdPluralName
                , entity.Metadata.Name) as JObject;
            var productInfo = existingProductItemJson.ToObject<ProductInfoEntity>();
            if (productInfo.Spec.MaximumQuantity != entity.Spec.AvailableQuantity)
            {
                entity.Spec.AvailableQuantity = productInfo.Spec.MaximumQuantity;

                var body = JObject.FromObject(entity, _camelCaseSerializer);

                _kubernetesClient.ApiClient.ReplaceNamespacedCustomObject(body, ProductAvailabilityEntity.CrdGroup,
                    ProductAvailabilityEntity.CrdApiVersion
                    , Namespace, ProductAvailabilityEntity.CrdPluralName, entity.Metadata.Name);
            }
            return null;
        }

        public void StatusModified(ProductAvailabilityEntity entity)
        {
            _logger.LogDebug(nameof(StatusModified));
        }

        public void NotModified(ProductAvailabilityEntity entity)
        {
            _logger.LogDebug(nameof(NotModified));
        }

        public void Deleted(ProductAvailabilityEntity entity)
        {
            _logger.LogDebug(nameof(Deleted));
        }

        public void Finalized(ProductAvailabilityEntity entity)
        {
            _logger.LogDebug(nameof(Finalized));
        }
    }
}
