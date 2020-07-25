using Amo.Lib.Attributes;
using Amo.Lib.Enums;
using Amo.Lib.Extensions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amo.Lib.CoreApi.Filters
{
    public class AddHeaderParameterFilter : IOperationFilter
    {
        /// <summary>
        /// 添加Header头信息
        /// </summary>
        /// <param name="operation">Operation</param>
        /// <param name="context">Context</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // var attrs = context.ApiDescription.ActionDescriptor.AttributeRouteInfo;
            // 先判断是否是匿名访问,
            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor != null)
            {
                // var controlAttributes = descriptor.ControllerTypeInfo.GetCustomAttributes(inherit: true);
                // var actionAttributes = descriptor.MethodInfo.GetCustomAttributes(inherit: true);
                // bool isAnonymous = actionAttributes.Any(a => a is AllowAnonymousAttribute);
                var controlNeedHeader = descriptor.ControllerTypeInfo.GetAttribute<NeedHeaderAttribute>(true);
                var actionNeedHeader = descriptor.MethodInfo.GetAttribute<NeedHeaderAttribute>(true);

                // 优先用Action的标记,Action没有标记时才采用Control的
                NeedHeaderType needType = actionNeedHeader != null ? actionNeedHeader.Level : controlNeedHeader != null ? controlNeedHeader.Level : NeedHeaderType.None;

                // 非匿名的方法,链接中添加accesstoken值
                // if (!isAnonymous)
                if (needType != NeedHeaderType.None)
                {
                    if (operation.Parameters == null)
                    {
                        operation.Parameters = new List<OpenApiParameter>();
                    }

                    OpenApiParameter HeadInfo(string name, string type, string desc, bool required) =>
                        new OpenApiParameter()
                        {
                            Name = name,
                            In = ParameterLocation.Header, // query header body path formData
                            Schema = new OpenApiSchema()
                            {
                                Type = type,
                            },
                            Description = desc,
                            Required = required // 是否必选
                        };
                    if (needType.IsBelong(NeedHeaderType.All, NeedHeaderType.Necessary))
                    {
                        ApiCommon.ParameterInfos?.FindAll(p => p.Required)?.ForEach(p =>
                        {
                            operation.Parameters.Add(HeadInfo(p.Name, p.Type, p.Desc, p.Required));
                        });
                    }

                    if (needType.IsBelong(NeedHeaderType.All, NeedHeaderType.Optional))
                    {
                        ApiCommon.ParameterInfos?.FindAll(p => !p.Required)?.ForEach(p =>
                        {
                            operation.Parameters.Add(HeadInfo(p.Name, p.Type, p.Desc, p.Required));
                        });
                    }

                    // operation.Parameters.Add(HeadInfo("logkey", "string", "Log串联用的唯一Key", false));
                }
            }
        }
    }
}
