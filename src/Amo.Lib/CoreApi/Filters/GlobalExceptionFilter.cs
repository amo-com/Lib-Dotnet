using Amo.Lib.Enums;
using Amo.Lib.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net;

namespace Amo.Lib.CoreApi.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILog log;
        private readonly IEnumerable<IExceptionAnalysis> eventTypeAnalyses;
        private readonly IRequestManager requestManager;

        public GlobalExceptionFilter(ILog log)
        {
            this.log = log;
        }

        public GlobalExceptionFilter(ILog log, IRequestManager requestManager)
        {
            this.log = log;
            this.requestManager = requestManager;
        }

        public GlobalExceptionFilter(ILog log, IEnumerable<IExceptionAnalysis> eventTypeAnalyses)
        {
            this.log = log;
            this.eventTypeAnalyses = eventTypeAnalyses;
        }

        public GlobalExceptionFilter(ILog log, IEnumerable<IExceptionAnalysis> eventTypeAnalyses, IRequestManager requestManager)
        {
            this.log = log;
            this.eventTypeAnalyses = eventTypeAnalyses;
            this.requestManager = requestManager;
        }

        public void OnException(ExceptionContext context)
        {
            string message = string.Empty;
            int eventType = (int)EventType.ApiError;
            if (context.Exception != null)
            {
                var ex = context.Exception;
                bool isAnysis = false;
                if (eventTypeAnalyses != null)
                {
                    foreach (var eventTypeAnalysis in eventTypeAnalyses)
                    {
                        (eventType, message, isAnysis) = eventTypeAnalysis.GetEventType(ex);
                        if (isAnysis)
                        {
                            break;
                        }
                    }
                }

                if (!isAnysis)
                {
                    message = ex.Message;
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

            LogEntity logEntity = new LogEntity();

            if (requestManager != null)
            {
                logEntity = requestManager.GetRequestLog(context.HttpContext);
            }

            logEntity.Exception = context.Exception;
            logEntity.EventType = (int)eventType;

            log?.Log(logEntity);
        }
    }
}
