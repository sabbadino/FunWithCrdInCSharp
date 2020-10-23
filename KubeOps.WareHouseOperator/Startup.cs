using KubeOps.Operator;
using KubeOps.TestOperator.Entities;
using KubeOps.WareHouse.Controller;
using KubeOps.WareHouse.Entities;
using KubeOps.WareHouse.Finalizer;
using KubeOps.WareHouse.TestManager;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace KubeOps.WareHouse
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddKubernetesOperator()
                .AddFinalizer<TestEntityFinalizer>()
                .AddController<ProductController>().AddController<ProductAvailabilityController>();

            services.AddTransient<IManager<ProductInfoEntity>, ProductInfoManager>();
            services.AddTransient<IManager<ProductAvailabilityEntity>, ProductAvailabilityManager>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseKubernetesOperator();
        }
    }
}
