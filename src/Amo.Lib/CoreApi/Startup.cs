using Amo.Lib.CoreApi.Common;
using Amo.Lib.Extensions;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;

namespace Amo.Lib.CoreApi
{
    public class Startup
    {
        // api endpoint json: /{ApiRoutePrefix}/{apiName}/swagger.json
        private readonly string apiRoutePrefix;
        private readonly bool enableShowApiSwagger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// 实例化
        /// </summary>
        /// <param name="configuration">appsetting</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            apiRoutePrefix = Configuration.GetValue<string>("Setting:ApiRoutePrefix");
            enableShowApiSwagger = Configuration.GetValue<bool>("Setting:EnableShowApiSwagger");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*
            services.AddHealthChecks()
                .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy());
                */

            if (enableShowApiSwagger)
            {
                // 配置swagger 文档
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc(ApiCommon.ApiName, new OpenApiInfo { Title = "Api Document V1", Version = "v1" });
                    ApiCommon.XmlDocumentes?.ForEach(x =>
                    {
                        c.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, x));
                    });

                    // swagger中控制请求的时候发是否需要在url中增加accesstoken
                    c.OperationFilter<Filters.AddHeaderParameterFilter>();
                    c.CustomSchemaIds(type => type.FullName); // 解决相同类名会报错的问题
                });
            }

            // Api限流组件
            // 添加appsettings.json
            // services.AddOptions();

            // 需要存储速率和ip规则
            services.AddMemoryCache();

            // 加载appsettings.json中的配置项 ，下面三项是加载general,rules
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));

            // services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));

            // 注入计时器和规则
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

            // 添加全局路由前缀和错误捕捉,配置自定义路由前缀
            services.AddMvc(c =>
            {
                c.UseGeneralRoutePrefix(apiRoutePrefix);
                c.Filters.Add(typeof(Filters.GlobalExceptionFilter));
            });

            // services.AddMvc().AddJsonOptions(opt=> { opt.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Populate; });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // 配置跨域支持
            services.AddCors(options => options.AddPolicy(
                ApiCommon.CorsName,
                builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                }));

            // root,先注册httpcontext,给control用
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            // DI注册
            // 所有需要注册的命名空间(组件名)
            ILog log = new Common.Log();
            ServiceManager.RegisterServices(services, ApiCommon.Scopeds, log, ApiCommon.NameSpaces, ApiCommon.Prefixs);
            ServiceManager.BuildServices();

            // services.BuildServiceProvider();
            services.AddControllers().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // app.UseExceptionHandler(builder => builder.Run(context => new ExceptionEvent().ErrorEvent(context)));
            if (enableShowApiSwagger)
            {
                app.UseSwagger();

                /*
                app.UseSwagger(c =>
                {
                    c.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.Components = virtualPath);
                });

                */

                // 注册swagger文档
                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = apiRoutePrefix;
                    c.DocumentTitle = $"{ApiCommon.ApiName} Api Document";

                    c.SwaggerEndpoint($"/{apiRoutePrefix}/{ApiCommon.ApiName}/swagger.json", "Api Document V1");
                    c.DefaultModelsExpandDepth(-1);
                    c.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Example);
                });

                // 配置json路径,/api/{documentName}/swagger.json,给上面SwaggerEndpoint使用的
                app.UseSwagger(c => { c.RouteTemplate = $"/{apiRoutePrefix}/{{documentName}}/swagger.json"; });
            }

            // 获取代理IP
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // 注册跨域配置
            // app.UseMiddleware<CorsMiddleware>();
            app.UseIpRateLimiting();

            app.UseRouting();
            app.UseCors(ApiCommon.CorsName);

            // app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
