using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace apiserver
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "apiserver", Version = "v1" });
            });

            
            var jobTracker = new JobTracker();
            var jobQueue = new JobQueue(jobTracker);
            var worker = new Worker(jobQueue, GetWorkerJobCompletionRate());
            worker.Start();

            services.AddSingleton(jobQueue);
            services.AddSingleton(jobTracker);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "apiserver v1"));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static double GetWorkerJobCompletionRate()
        {
            var envVar = Environment.GetEnvironmentVariable("WORKER_JOB_COMPLETION_RATE_PER_SECOND");

            var rate = double.TryParse(envVar, out var parsedRate)
                ? parsedRate
                : 1.0;

            Console.WriteLine($"Worker will complete {rate} jobs per second");

            return rate;
        }
    }
}
