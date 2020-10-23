using System.Threading.Tasks;
using KubeOps.Operator.Client;
using KubeOps.Operator.Finalizer;
using KubeOps.WareHouse.Entities;
using KubeOps.WareHouse.TestManager;
using Microsoft.Extensions.Logging;

namespace KubeOps.WareHouse.Finalizer
{
    public class TestEntityFinalizer : ResourceFinalizerBase<ProductInfoEntity>
    {
        private readonly IManager<ProductInfoEntity> _manager;

        public TestEntityFinalizer(IManager<ProductInfoEntity> manager, IKubernetesClient client, ILogger<ResourceFinalizerBase<ProductInfoEntity>> logger) 
            : base(logger, client)
        {
            _manager = manager;
        }

        public override Task Finalize(ProductInfoEntity resource)
        {
            _manager.Finalized(resource);
            return Task.CompletedTask;
        }
    }
}
