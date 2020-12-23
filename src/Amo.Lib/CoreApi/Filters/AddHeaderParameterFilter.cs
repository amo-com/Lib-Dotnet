using Amo.Lib.Attributes;
using Amo.Lib.Enums;
using Amo.Lib.Extensions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

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
            AddApiParameter(operation, context);
        }

        private void AddApiParameter(OpenApiOperation operation, OperationFilterContext context)
        {
            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor != null)
            {
                var controlApiParameters = descriptor.ControllerTypeInfo.GetAttributes<ApiParameterAttribute>(true)?.ToList();
                var methodApiParameters = descriptor.MethodInfo.GetAttributes<ApiParameterAttribute>(true)?.ToList();
                var methodApiParameterNames = descriptor.MethodInfo.GetAttribute<ApiParameterNamesAttribute>(true);

                List<ApiParameterAttribute> apiParameters = new List<ApiParameterAttribute>();
                apiParameters.AddRange(methodApiParameters);

                // 继承Controller的
                List<string> names = methodApiParameterNames?.Names?.ToList();
                if (controlApiParameters != null && controlApiParameters.Count > 0)
                {
                    List<ApiParameterAttribute> necessoryApiParameters = controlApiParameters.FindAll(q => q.NeedType == ApiParameterNeed.Necessary);
                    apiParameters.AddRange(necessoryApiParameters);

                    if (names != null && names.Count > 0)
                    {
                        List<ApiParameterAttribute> inheritApiParameters = controlApiParameters.FindAll(q => q.NeedType == ApiParameterNeed.Optional && names.Contains(q.Name));
                        apiParameters.AddRange(inheritApiParameters);
                    }
                }

                apiParameters = apiParameters.Distinct().ToList();

                if (apiParameters != null && apiParameters.Count > 0)
                {
                    if (operation.Parameters == null)
                    {
                        operation.Parameters = new List<OpenApiParameter>();
                    }

                    apiParameters.ForEach(q =>
                    {
                        operation.Parameters.Add(new OpenApiParameter()
                        {
                            Name = q.Name,
                            In = GetLocation(q.Location), // query header body path formData
                            Schema = new OpenApiSchema()
                            {
                                Type = q.Type,
                            },
                            Description = q.Desc,
                            Required = q.Required // 是否必选
                        });
                    });
                }
            }
        }

        private ParameterLocation GetLocation(ApiParameterLocation apiParameterLocation)
        {
            ParameterLocation location = ParameterLocation.Query;
            switch (apiParameterLocation)
            {
                case ApiParameterLocation.Cookie: location = ParameterLocation.Cookie; break;
                case ApiParameterLocation.Path: location = ParameterLocation.Path; break;
                case ApiParameterLocation.Header: location = ParameterLocation.Header; break;
                case ApiParameterLocation.Query: location = ParameterLocation.Query; break;
                default: break;
            }

            return location;
        }
    }
}
