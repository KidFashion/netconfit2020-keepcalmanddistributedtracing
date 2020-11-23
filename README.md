# Keep Calm and Distributed Tracing

## Architecture

Sample is composed by 5 containers:

1. **first-service**: Invoked externally, invokes a second, intermediate, service. 
2. **second-service**: Invoked by first-service, invokes a third, backend, service. 
3. **third-service**: Invoked by second-service, retrieve data using EFCore (from SQLite local file for simplicity). 
4. **otel-collector**: Collecting all traces via OTPL receiver and/or Jaeger receiver and forwarding them to Jaeger backend and ApplicationInsight endpoint (needs an real instrumentation key to work). 
5. **jaeger-all-in-one**: Mainly used as dashboard through the Jaeger UI. 

## Configure

C# code is instrumented to send traces using Jaeger (OpenTracing) or OpenTelemetry, configuration of each one of the three services is driven by  `.env` file (FIRST_APPCONFIG, SECOND_APPCONFIG and THIRD_APPCONFIG).
When using Jaeger, the actual backend invoked by C# can be OpenTelemetry Collector (on Jaeger Receiver) or actual Jaeger Collector, this configuration is also driven by  `.env` file (JAEGER_EP setting).

## Run

```shell
docker-compose up
```

## Test

1. Point your browser to http://localhost/Users
2. Check that Jaeger UI has received traces browsing http://localhost:16686
