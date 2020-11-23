using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

#region OPENTRACING_IMPORTS
using OpenTracing;
using OpenTracing.Contrib.NetCore;
using webapi_sample.OpenTracingSample;
#endregion

#region OPENTELEMETRY_IMPORTS
using OpenTelemetry.Extensions.Hosting;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Exporter.OpenTelemetryProtocol;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Context.Propagation;
#endregion


namespace webapi_sample
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
            
            var serviceName = String.Format(Configuration.GetValue<string>("name", "Service {0}"), new Random().Next());

            Console.WriteLine(String.Format("Service Name: {0}", serviceName));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "webapi_sample", Version = "v1" });
            });

            if (Configuration.GetValue<string>("tracer", "none").ToLower().Equals("jaeger"))
            {
            #region OpenTracing_Usage
            OpenTracing.Util.GlobalTracer.Register(                
                JaegerTracerFactory.Create(
                    serviceName,LoggerFactory.Create(builder => builder.AddConsole()))
                );
            services.AddOpenTracing();
            #endregion
            }

            #region OpenTelemetry_Usage
            if (Configuration.GetValue<string>("tracer", "none").ToLower().Equals("opentelemetry"))
            {
                services.AddOpenTelemetryTracing((builder) =>
                    builder
                    .AddSource("NETCONF2020.MyApplication")
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    //.AddEntityFrameworkCoreInstrumentation()
                    //.AddJaegerExporter(o => o.AgentHost = Configuration.GetValue<string>("otlp-endpoint", "otel-collector"))
                    .AddOtlpExporter(o => o.Endpoint = Configuration.GetValue<string>("otlp-endpoint", "otel-collector:55680"))
                    .AddConsoleExporter()
                    .SetSampler(new AlwaysOnSampler())
                    );
            }
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "webapi_sample v1"));
            }
                
            //app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
