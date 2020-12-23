Dotnet common

提供三个common库，用于Dotnetcore的开发，
1. [Amo.Lib](./src/Amo.Lib/Lib/readme.md) (netstandard2.0)
提供一些基础支持
    - mssql,mysql的SqlHelper
    - JsonData默认格式封装
    - LogEntity默认格式封装
    - ReadConfig接口
    - BaseProxy异步封装代理
    - Discovery服务发现
    - Setting绑定
    - IOC自定义管理(基于Scoped)
    - Swagger默认配置
    - ...
1. [Amo.Lib.CoreApi](./src/Amo.Lib/CoreApi/readme.md) (netcoreapp3.1)
    dependency on Amo.Lib
2. [Amo.Lib.RestClient](./src/Amo.Lib/RestClient/readme.md) (netstandard2.0)