using Amo.Lib.Enums;
using Amo.Lib.Exceptions;
using Amo.Lib.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Amo.Lib.CoreApi.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILog log;

        public GlobalExceptionFilter(ILog log)
        {
            this.log = log;
        }

        public void OnException(ExceptionContext context)
        {
            string message = string.Empty;
            EventType eventType = EventType.ApiError;
            if (context.Exception != null)
            {
                var ex = context.Exception;
                message = ex.Message;
                if (ex is CustomizeException)
                {
                    var cex = (CustomizeException)ex;
                    eventType = cex.EventType;
                }
                else if (ex is System.Data.SqlClient.SqlException sqlex)
                {
                    int sqlNumber = sqlex.Number;
                    eventType = Common.EventTypeAnalys.GetEventType(ex, sqlNumber);
                }
            }

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.HttpContext.Response.ContentType = "application/json";
            context.Result = new JsonResult(
                new JsonData<Exception>()
                {
                    Code = (int)eventType,
                    Message = message,
                });

            LogEntity logEntity = Common.RequestManager.GetRequestLog(context.HttpContext);
            logEntity.Exception = context.Exception;
            logEntity.EventType = (int)eventType;

            log?.Log(logEntity);
        }
    }
}
