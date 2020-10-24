using Amo.Lib.CoreApi.Models;
using Amo.Lib.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Amo.Lib.CoreApi.Common
{
    public class RequestManager : IRequestManager
    {
        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>ClientIP</returns>
        public static string GetClientIP(HttpContext context)
        {
            // 通过服务端转发的真实客户端IP
            string ip = context.Request.Headers["clientIp"];
            if (string.IsNullOrEmpty(ip) || string.Equals("unknown", ip, StringComparison.OrdinalIgnoreCase))
            {
                ip = context.Request.Headers["x-forwarded-for"];
            }

            if (string.IsNullOrEmpty(ip) || string.Equals("unknown", ip, StringComparison.OrdinalIgnoreCase))
            {
                ip = context.Request.Headers["Proxy-Client-IP"];
            }

            if (string.IsNullOrEmpty(ip) || string.Equals("unknown", ip, StringComparison.OrdinalIgnoreCase))
            {
                ip = context.Request.Headers["WL-Proxy-Client-IP"];
            }

            if (string.IsNullOrEmpty(ip) || string.Equals("unknown", ip, StringComparison.OrdinalIgnoreCase))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }

            ip = IpUtils.ValidClientIP(ip);
            return ip;
        }

        // TLog:LogData, TContext:HttpContext
        public TLog GetRequestLog<TContext, TLog>(TContext context)
            where TLog : LogEntity, new()
        {
            LogData logEntity = new LogData();
            HttpContext httpContext = context as HttpContext;
            if (httpContext != null && httpContext.Request != null)
            {
                logEntity.RequestMethod = httpContext.Request.Method.ToLower();
                logEntity.Url = httpContext.Request.Path;
                logEntity.IP = GetClientIP(httpContext);
                logEntity.QueryString = httpContext.Request.QueryString.ToString();
                logEntity.StateCode = httpContext.Response.StatusCode;

                if (httpContext.Request.Body.CanRead && httpContext.Request.Body.CanSeek && httpContext.Request.Body.Length > 0)
                {
                    // context.Request.EnableBuffering();
                    httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(httpContext.Request.Body))
                    {
                        // logEntity.Body = reader.ReadToEnd();
                        // context.Request.Body.Seek(0, SeekOrigin.Begin);
                        logEntity.Body = reader.ReadToEndAsync().GetAwaiter().GetResult();
                    }
                }

                if (httpContext.Request.Headers != null)
                {
                    logEntity.Site = httpContext.Request.Headers["Site"];
                    logEntity.UserAgent = httpContext.Request.Headers["User-Agent"];
                    logEntity.UrlReferrer = httpContext.Request.Headers["Referer"];
                    logEntity.Guid = httpContext.Request.Headers["guid"];
                    logEntity.LogKey = httpContext.Request.Headers["logkey"];
                    logEntity.Url = httpContext.Request.Headers["currentUrl"];
                }
            }

            // TLog logResult = logEntity is TLog ? logEntity as TLog : new TLog();
            return logEntity as TLog;
        }
    }
}
