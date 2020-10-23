using System;
using System.Threading.Tasks;
using KubeOps.Operator.Client;
using KubeOps.Operator.Controller;
using KubeOps.Operator.Rbac;
using KubeOps.Operator.Services;
using KubeOps.WareHouse.Entities;
using KubeOps.WareHouse.Finalizer;
using KubeOps.WareHouse.TestManager;
using Microsoft.Extensions.Logging;

namespace KubeOps.WareHouse.Controller
{
    [EntityRbac(typeof(ProductInfoEntity), Verbs = RbacVerb.All)]
    public class ProductController : ResourceControllerBase<ProductInfoEntity>
    {
        private readonly IManager<ProductInfoEntity> _manager;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IManager<ProductInfoEntity> manager
            , IResourceServices<ProductInfoEntity> services, ILogger<ProductController> logger)
            : base(services)
        {
            _manager = manager;
            _logger = logger;
        }

        protected override async Task<TimeSpan?> Created(ProductInfoEntity resource)
        {
            try
            {
                _manager.Created(resource);
                await AttachFinalizer<TestEntityFinalizer>(resource);
                return await base.Created(resource);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error in ProductInfoEntityCreated");
                return null;
            }
        }

        protected override async Task<TimeSpan?> Updated(ProductInfoEntity resource)
        {
            _manager.Updated(resource);
            return await base.Updated(resource);
        }

        protected override async Task<TimeSpan?> NotModified(ProductInfoEntity resource)
        {
            _manager.NotModified(resource);
            return await base.NotModified(resource);
        }

        protected override async Task StatusModified(ProductInfoEntity resource)
        {
            _manager.StatusModified(resource);
            await base.StatusModified(resource);
        }

        protected override async Task Deleted(ProductInfoEntity resource)
        {
            _manager.Deleted(resource);
            await base.Deleted(resource);
        }
    }
}
