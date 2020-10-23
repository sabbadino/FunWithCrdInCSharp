﻿using System.Threading.Tasks;
using KubeOps.Operator;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace KubeOps.WareHouse
{
    public static class Program
    {
        public static Task<int> Main(string[] args) => CreateHostBuilder(args).Build().RunOperator(args);

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}