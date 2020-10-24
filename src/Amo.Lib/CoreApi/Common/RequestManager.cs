using Amo.Lib.CoreApi.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Amo.Lib.CoreApi.Common
{
    public class RequestManager : IRequestManager<HttpContext, LogData>
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

        public LogData GetRequestLog(HttpContext context)
        {
            LogData logEntity = new LogData();

            if (context != null && context.Request != null)
            {
                logEntity.RequestMethod = context.Request.Method.ToLower();
                logEntity.Url = context.Request.Path;
                logEntity.IP = GetClientIP(context);
                logEntity.QueryString = context.Request.QueryString.ToString();
                logEntity.StateCode = context.Response.StatusCode;

                if (context.Request.Body.CanRead && context.Request.Body.CanSeek && context.Request.Body.Length > 0)
                {
                    // context.Request.EnableBuffering();
                    context.Request.Body.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(context.Request.Body))
                    {
                        // logEntity.Body = reader.ReadToEnd();
                        // context.Request.Body.Seek(0, SeekOrigin.Begin);
                        logEntity.Body = reader.ReadToEndAsync().GetAwaiter().GetResult();
                    }
                }

                if (context.Request.Headers != null)
                {
                    logEntity.Site = context.Request.Headers["Site"];
                    logEntity.UserAgent = context.Request.Headers["User-Agent"];
                    logEntity.UrlReferrer = context.Request.Headers["Referer"];
                    logEntity.Guid = context.Request.Headers["guid"];
                    logEntity.LogKey = context.Request.Headers["logkey"];
                    logEntity.Url = context.Request.Headers["currentUrl"];
                }
            }

            return logEntity;
        }
    }
}
