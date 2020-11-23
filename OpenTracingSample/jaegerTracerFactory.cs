#define CONFIG_DRIVEN



using Jaeger;
using Jaeger.Senders;
using Jaeger.Senders.Thrift;
using Jaeger.Samplers;
using OpenTracing;
using Microsoft.Extensions.Logging;

namespace webapi_sample.OpenTracingSample
{

    public class JaegerTracerFactory
    {
        public static ITracer Create(string serviceName, ILoggerFactory loggerFactory)
        {

            Configuration.SenderConfiguration.DefaultSenderResolver = new SenderResolver(loggerFactory)
                .RegisterSenderFactory<ThriftSenderFactory>();

#if CONFIG_DRIVEN      
            try{
                if (System.String.IsNullOrEmpty(System.Environment.GetEnvironmentVariable("JAEGER_SERVICE_NAME")))
                    System.Environment.SetEnvironmentVariable("JAEGER_SERVICE_NAME",serviceName);
                return (ITracer) Configuration.FromEnv(loggerFactory).GetTracer();
            }
            catch(System.Exception err)
            {
                System.Console.WriteLine(err.ToString());
                return OpenTracing.Noop.NoopTracerFactory.Create();
            }
#else            
            var samplerConfiguration = new Configuration.SamplerConfiguration(loggerFactory)
                .WithType(ConstSampler.Type)       
                .WithParam(1);

            var senderConfiguration = new Configuration.SenderConfiguration(loggerFactory);



            var reporterConfiguration = new Configuration.ReporterConfiguration(loggerFactory)
                .WithSender(senderConfiguration)
                .WithLogSpans(true);

            return (Tracer)new Configuration(serviceName, loggerFactory)
                .WithSampler(samplerConfiguration)
                .WithReporter(reporterConfiguration)        
                .GetTracer();
#endif            
        }
    }

}
