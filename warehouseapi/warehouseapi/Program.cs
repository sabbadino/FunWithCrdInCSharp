using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
//using KubeOps.Operator;

namespace warehouseapi
{
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    BuildWebHost(args).Run();
        //}

        //public static IWebHost BuildWebHost(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>()
        //        .Build();


        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run(); //.RunOperator(args);//;
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
