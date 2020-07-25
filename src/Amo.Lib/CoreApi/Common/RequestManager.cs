using Amo.Lib.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Amo.Lib.CoreApi.Common
{
    public class RequestManager
    {
        /// <summary>
        /// 获取Request下的IP，Url，UserAgent等信息
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>LogEntity</returns>
        public static LogEntity GetRequestLog(HttpContext context)
        {
            LogEntity logEntity = new LogEntity();

            if (context != null && context.Request != null)
            {
                logEntity.RequestMethod = context.Request.Method.ToLower();
                logEntity.Url = context.Request.Path;
                logEntity.IP = GetClientIP(context);
                logEntity.QueryString = context.Request.QueryString.ToString();
                logEntity.StateCode = context.Response.StatusCode;

                if (context.Request.Body.CanRead && context.Request.Body.CanSeek && context.Request.Body.Length > 0)
                {
                    context.Request.EnableBuffering();
                    context.Request.Body.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(context.Request.Body))
                    {
                        logEntity.Body = reader.ReadToEnd();
                        context.Request.Body.Seek(0, SeekOrigin.Begin);
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

            ip = Utils.ValidClientIP(ip);
            return ip;
        }

        /// <summary>
        /// 获取前一级Url
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>UrlReferer</returns>
        public static string GetUrlReferer(HttpContext context)
        {
            return context.Request.Headers["Referer"];
        }

        public static string GetCurrentUrl(HttpContext context)
        {
            return context.Request.Headers["currentUrl"];
        }

        public static string GetUserAgent(HttpContext context)
        {
            return context.Request.Headers["User-Agent"];
        }

        public static string GetHost(HttpContext context)
        {
            string host = context.Request.Headers["Host"];
            if (string.IsNullOrEmpty(host))
            {
                host = context.Request.Host.Value;
            }

            return host;
        }

        public static Guid GetGuid(HttpContext context)
        {
            string strGuid = Utils.GetString(context.Request.Headers["guid"]).Trim();
            bool isSuccess = Guid.TryParse(strGuid, out Guid userGuid);

            if (!isSuccess)
            {
                userGuid = Guid.NewGuid();
            }

            return userGuid;
        }
    }
}
