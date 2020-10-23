using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using k8s.Models;
using KubeOps.Operator.Client;
using KubeOps.Operator.Controller;
using KubeOps.Operator.Rbac;
using KubeOps.Operator.Services;
using KubeOps.TestOperator.Entities;
using KubeOps.WareHouse.Entities;
using KubeOps.WareHouse.Finalizer;
using KubeOps.WareHouse.TestManager;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace KubeOps.WareHouse.Controller
{
    [EntityRbac(typeof(ProductAvailabilityEntity), Verbs = RbacVerb.All)]
    public class ProductAvailabilityController : ResourceControllerBase<ProductAvailabilityEntity>
    {
        private readonly IManager<ProductAvailabilityEntity> _manager;
        private readonly ILogger<ProductAvailabilityController> _logger;

        public ProductAvailabilityController(IManager<ProductAvailabilityEntity> manager
            , IResourceServices<ProductAvailabilityEntity> services, ILogger<ProductAvailabilityController> logger)
            : base(services)
        {
            _manager = manager;
            _logger = logger;
        }

        protected override async Task<TimeSpan?> Created(ProductAvailabilityEntity resource)
        {
            try
            {
                _manager.Created(resource);
                // await AttachFinalizer<TestEntityFinalizer>(resource);


                return await base.Created(resource);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"error in ProductAvailabilityEntityCreated");
                return null;
            }
        }

        protected override async Task<TimeSpan?> Updated(ProductAvailabilityEntity resource)
        {
            try
            {
                var ret = _manager.Updated(resource);
                await base.Updated(resource);
                return ret;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error in ProductInfoEntityCreated");
                return null;
            }
        }

        protected override async Task<TimeSpan?> NotModified(ProductAvailabilityEntity resource)
        {
            if (resource.Annotations().TryGetValue("re-queued", out var value))
            {
                resource.Annotations().Remove("re-queued");
                return _manager.Updated(resource);
            }
            _manager.NotModified(resource);
            return await base.NotModified(resource);
        }

        protected override async Task StatusModified(ProductAvailabilityEntity resource)
        {
            _manager.StatusModified(resource);
            await base.StatusModified(resource);
        }

        protected override async Task Deleted(ProductAvailabilityEntity resource)
        {
            _manager.Deleted(resource);
            await base.Deleted(resource);
        }
    }
}
