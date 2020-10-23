using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Amo.Lib
{
    public static class IpUtils
    {
        /// <summary>
        /// 可能存在如下格式：X-Forwarded-For: client, proxy1, proxy2
        /// </summary>
        /// <param name="ip">IP字符串</param>
        /// <returns>首个有效IP</returns>
        public static string ValidClientIP(string ip)
        {
            if (ip.Contains(","))
            {
                // 如果存在多个反向代理，获得的IP是一个用逗号分隔的IP集合，取第一个
                // X-Forwarded-For: client 第一个
                string[] ips = ip.Split(new string[1] { "," }, StringSplitOptions.RemoveEmptyEntries);
                var i = 0;
                for (i = 0; i < ips.Length; i++)
                {
                    ips[i] = ips[i].Trim();
                    if (ips[i] != string.Empty)
                    {
                        // 判断是否为内网IP
                        if (IsInnerIp(ips[i]) == false)
                        {
                            IPAddress realIp;
                            if (IPAddress.TryParse(ips[i], out realIp) && ips[i].Split('.').Length == 4)
                            {
                                // 合法IP
                                return ips[i];
                            }

                            return string.Empty;
                        }
                    }
                }

                ip = ips[0]; // 默认获取第一个IP地址
            }

            return ip;
        }

        public static long Ip2Long(string ip)
        {
            byte[] bytes = IPAddress.Parse(ip).GetAddressBytes();
            long ret = 0;
            foreach (var b in bytes)
            {
                ret <<= 8;
                ret |= b;
            }

            return ret;
        }

        /// <summary>
        /// 将IP地址转换为Long型数字
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns>长整型地址</returns>
        public static ulong Ip2Ulong(string ip)
        {
            byte[] bytes = IPAddress.Parse(ip).GetAddressBytes();
            ulong ret = 0;
            foreach (var b in bytes)
            {
                ret <<= 8;
                ret |= b;
            }

            return ret;
        }

        /// <summary>
        /// 判断IP地址是否为内网IP地址
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns>判断结果</returns>
        private static bool IsInnerIp(string ip)
        {
            bool isInnerIp = false;
            ulong ipNum = Ip2Ulong(ip);
            /*
             * 私有IP
             * A类：10.0.0.0-10.255.255.255
             * B类：172.16.0.0-172.31.255.255
             * C类：192.168.0.0-192.168.255.255
             * 当然，还有127这个网段是环回地址
             */
            ulong aBegin = Ip2Ulong("10.0.0.0");
            ulong aEnd = Ip2Ulong("10.255.255.255");
            ulong bBegin = Ip2Ulong("172.16.0.0");
            ulong bEnd = Ip2Ulong("10.31.255.255");
            ulong cBegin = Ip2Ulong("192.168.0.0");
            ulong cEnd = Ip2Ulong("192.168.255.255");
            isInnerIp = IsInner(ipNum, aBegin, aEnd) || IsInner(ipNum, bBegin, bEnd) || IsInner(ipNum, cBegin, cEnd) ||
               ip.Equals("127.0.0.1");
            return isInnerIp;
        }

        /// <summary>
        /// 判断用户IP地址转换为Long型后是否在内网IP地址所在范围
        /// </summary>
        /// <param name="userIp">用户IP</param>
        /// <param name="begin">开始范围</param>
        /// <param name="end">结束范围</param>
        /// <returns>是否属于地址范围</returns>
        private static bool IsInner(ulong userIp, ulong begin, ulong end)
        {
            return (userIp >= begin) && (userIp <= end);
        }
    }
}
