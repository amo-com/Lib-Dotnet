 1. 所有Api都以接口形式定义,Api再对接口进行实现
2. 目前只支持Get和Post两种，且Get都是单个参数，不允许实体，而Post只允许实体，不要单个参数
3. 
```
    [HttpHost("http://192.168.11.114:8080")]
    [HttpRoutePrefix("api/vehicle")]
    public interface IVehicleApi
    {
        [HttpGet("makes")]
        ReturnData GetMakeList(string year, int type);


    }

    public class VehicleApi : IVehicleApi
    {
        private readonly HttpApiFactory factory;
        public VehicleApi(string site)
        {
            factory = HttpApi.Resolve<IVehicleApi>(site);
        }

        public ReturnData GetMakeList(string year, int type)
        {
            MethodBase method = System.Reflection.MethodBase.GetCurrentMethod();
            object[] args = new object[] { year, type };

            return factory.RequestResult<ReturnData>(method as MethodInfo, args);
        }

    }
```
```
HttpApi.Register<IVehicleApi>(Site).ConfigureHttpApiConfig(c => { c.Host = "http://192.168.11.114:8080"; }).Init();
factory = HttpApi.Resolve<IVehicleApi>(site);
```
