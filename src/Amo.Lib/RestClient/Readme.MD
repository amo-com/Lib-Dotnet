 1. 所有Api都以接口形式定义,Api再对接口进行实现
2. 目前只支持Get和Post两种，且Get都是单个参数，不允许实体，而Post只允许实体(并且只能有一个)，不要单个参数
3. 使用示例
    - 接口定义和code实现
    ```
        [HttpHost("http://192.168.11.114:8080")]
        [HttpRoutePrefix("api/vehicle")]
        public interface IVehicleApi
        {
            [HttpGet("makes")]
            Task<ReturnData> GetMakeListAsync([Query]string year, [Query]int type);

            [HttpPost("vehicle/add")]
            Task<bool> AddMakeAsync([Body]VehicleInfo vehicle, [Query]string user);
        }

        public class VehicleApi : IVehicleApi
        {
            private readonly HttpApiFactory factory;
            public VehicleApi(string scoped)
            {
                factory = HttpApi.Resolve<IVehicleApi>(scoped);
            }

            public Task<ReturnData> GetMakeListAsync(string year, int type)
            {
                object[] args = new object[] { year, type };

                return await factory.Request<ReturnData>(args);
            }

            ...
        }
    ```

    - 配置对应服务信息
    ```
    HttpApi.Register<IVehicleApi>("ABC").ConfigureHttpApiConfig(c =>
        {
            c.Host = "http://192.168.11.114:8080";
            c.HttpClient.Timeout = TimeSpan.FromSeconds(5);
        }).Init();
    ```
    HttpApiConfig可以在这里配置(在Init()之前配置)
    PollyPolicy同理,也在使用前配置,建议放在startup中服务启动时配置一次
    配置好参数后调用Init()构建内部信息
      - HttpApiConfig是一些配置信息,如果HttpClient，Site等信息
      - PollyPolicy是一些策略,接口上有重复的定义时会覆盖Factory，即接口的>Factory，如果重试，熔断等
      - 支持注册中心的动态调度,每次请求时动态获取依赖服务，配置方式如下，实例化好Factory后,重新配置Factory下每个接口的Uri参数(httpclient获取服务的Url)，Url=[Host]+[RoutePrefix]+[Route]，故配置方式如下(注册中心替换的是Host,接口对应的Route不变,ApiRoute跟每个接口绑定的)
        ```
                private static void HandleUri(this HttpApiFactory factory, ILoadBalancer loadBalancer, ServiceDto service)
            {
                if (service != null && service.EnableRegister)
                {
                    factory.GetApiActionDescriptors().ForEach(q =>
                    {
                        q.Uri = () => Extensions.GetUri(loadBalancer.ResolveServiceInstance(service.Name).Result, service.RoutePrefix, q.Route);
                    });
                }
            }
        - Policy的依赖是在注册中心上层，故在Policy中配置的重试策略会使得每次都是独立的，会独立调用注册服务的负载策略，获取服务节点，避免重试都在同一节点
        - Init()是注册Factory实例，故Init调用之后对Factory的修改不再生效，HttpApiConfig，PollyPolicy等信息都应在Init之前
        ```

   - Policy配置
       - Factory配置
        需要几个配几个,未配置的或无效的配置会被过滤掉
        ```
        catalogFactory.ConfigurePolicy(c =>
                    {
                        c.RetryPolicy = new RetryPolicy()
                        {
                            RetryCount = 3,
                            SleepDuration = 500,
                        };
                        c.BreakerPolicy ...
                    });
        ```
       - 接口上自定义配置
        同理可以支持多个配置,同类型配置只能有一个,如果Retry只能配一次
        ```
        [Retry(3, 50)]
        [Timeout(8000)]
        [PolicyInterceptor]
        Task<ReturnData> GetMakeListAsync([Query]string year, [Query]int type);
        ```
        - 每个接口的Policies=ApiPolicies+FactoryPolicies，重复的会覆盖掉(Api>Factory),并且Policy的默认顺序权重如下，小的在内部，如Breaker是4，Retry是3，则先Retry后Breaker
          - Breaker: 4
          - FallBack: 1
          - Retry: 3
          - Timeout: 5
