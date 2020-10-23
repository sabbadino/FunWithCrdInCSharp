using k8s;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using warehouseapi.config;

namespace warehouseapi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var config = KubernetesClientConfiguration.BuildDefaultConfig();
            IKubernetes client = new Kubernetes(config);
            services.AddSingleton(client); // config / settings here
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
            services.Configure<AppSettings>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseKubernetesOperator();
            app.Use((context, next) =>

            {
                //ngnix is configured to rewrite the path to / .. stripping down /ems/MESSAGING/v1/ .. 
                // this breaks the tilde (~) in mvc code that is used to form path to css and javscript
                // it generates ~ is rendered to / if we don't do this
//                context.Request.PathBase = new PathString(BasePath);
#if !DEBUG
                context.Request.PathBase = new PathString(BasePath);
#endif
                return next();
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {

#if !DEBUG
                c.SwaggerEndpoint(BasePath + "/swagger/v1/swagger.json", "My API V1");
#else
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
#endif
            });
        }
        public const string BasePath = "/demoapi";
    }
}
