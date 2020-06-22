using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Amo.Lib
{
    public class XmlHandler
    {
        /// <summary>
        /// 全局变量，以字典的结构 存储网站所有的XML文档
        /// </summary>
        public static Dictionary<string, object> XmlDictionary = new Dictionary<string, object>();

        /// <summary>
        /// 获取网站所有的XML文档，存储到全局字典中
        /// </summary>
        /// <param name="xmlFilePath">Xml文件夹</param>
        public static void LoadXmlDictionary(string xmlFilePath)
        {
            Dictionary<string, object> xmlDic = new Dictionary<string, object>();

            try
            {
                // 判断该文件夹是否存在
                if (Directory.Exists(xmlFilePath))
                {
                    XmlDocument xmlDoc = new XmlDocument();

                    // 获取所有的XML文档
                    string[] xmls = Directory.GetFiles(xmlFilePath, "*.xml");

                    // 遍历所有的XML文档
                    foreach (string xml in xmls)
                    {
                        xmlDoc.Load(xml);

                        // 获取文件名称，并作为第一级的key
                        string fileKey = Path.GetFileNameWithoutExtension(xml);

                        // 获取根节点下的所有子节点
                        XmlNodeList xmlNodeList = xmlDoc.SelectNodes("/Root");

                        // 将子节点再转换成一个字典，作为第一级的value
                        Dictionary<string, object> fileValue = AddNodesInfomation(xmlNodeList);

                        if (!xmlDic.Keys.Contains(fileKey))
                        {
                            // 添加key value pair
                            xmlDic.Add(fileKey, fileValue);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            XmlDictionary = xmlDic;
        }

        /// <summary>
        /// 获取所有满足条件的节点
        /// </summary>
        /// <param name="fileName">xml文件名</param>
        /// <param name="xPath">XPath，写法类似于磁盘目录，例如"//root/httpwebsite/honda"，必须以根节点开始</param>
        /// <returns>所有满足条件的节点</returns>
        public static XmlNodeList LoadXmlNodeList(string fileName, string xPath)
        {
            XmlNodeList xmlNodeList = null;

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                string xmlFilePath = fileName + ".xml";
                if (File.Exists(xmlFilePath))
                {
                    xmlDoc.Load(xmlFilePath);

                    xmlNodeList = xmlDoc.SelectNodes(xPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return xmlNodeList;
        }

        /// <summary>
        /// 获取第一个满足条件的节点
        /// </summary>
        /// <param name="fileName">xml文件名</param>
        /// <param name="xPath">XPath，写法类似于磁盘目录，例如"//root/httpwebsite/honda"，必须以根节点开始</param>
        /// <returns>第一个满足条件的节点</returns>
        public static XmlNode LoadXmlSingleNode(string fileName, string xPath)
        {
            XmlNode xmlNode = null;

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                string xmlFilePath = fileName + ".xml";
                if (File.Exists(xmlFilePath))
                {
                    xmlDoc.Load(xmlFilePath);

                    xmlNode = xmlDoc.SelectSingleNode(xPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return xmlNode;
        }

        /// <summary>
        /// 获取第一个满足条件的节点值
        /// </summary>
        /// <param name="fileName">xml文件名</param>
        /// <param name="xPath">XPath，写法类似于磁盘目录，例如"//root/httpwebsite/honda"，必须以根节点开始</param>
        /// <returns>第一个满足条件的节点值</returns>
        public static string LoadXmlSingleNodeText(string fileName, string xPath)
        {
            string text = string.Empty;

            try
            {
                XmlNode xmlNode = LoadXmlSingleNode(fileName, xPath);

                if (xmlNode != null)
                {
                    text = xmlNode.InnerText;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return text;
        }

        /// <summary>
        /// 获取第一个满足条件的节点的属性值
        /// </summary>
        /// <param name="fileName">xml文件名</param>
        /// <param name="xPath">XPath，写法类似于磁盘目录，例如"//root/httpwebsite/honda"，必须以根节点开始</param>
        /// <param name="attributeName">属性名称</param>
        /// <returns>第一个满足条件的节点的属性值</returns>
        public static string LoadXmlSingleNodeAttribute(string fileName, string xPath, string attributeName)
        {
            string attribute = string.Empty;

            try
            {
                XmlNode xmlNode = LoadXmlSingleNode(fileName, xPath);

                if (xmlNode != null)
                {
                    attribute = xmlNode.Attributes[attributeName].InnerText;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return attribute;
        }

        /// <summary>
        /// 将子节点集合转换成一个字典，其中key是某个子节点的名称
        /// 若该子节点没有有孩子节点，那么value是该子节点的值。否则，递归调用该函数，将孩子节点再转换成一个字典，value就是这个字典。
        /// </summary>
        /// <param name="xmlNodeList">某个节点下的所有子节点</param>
        /// <returns>子节点的字典</returns>
        public static Dictionary<string, object> AddNodesInfomation(XmlNodeList xmlNodeList)
        {
            Dictionary<string, object> xmlDic = new Dictionary<string, object>();

            try
            {
                // 遍历节点集合中的所有节点
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    // 节点名称
                    string nodeName = xmlNode.Name;

                    // 剔除注释（注释也被当做一个节点，节点名称是#comment，节点值是注释的信息）
                    if (nodeName != "#comment")
                    {
                        string nodeKey = nodeName;

                        // 检查该节点是否还有子节点，若有子节点，则该节点的value是子节点的字典。
                        // HasChildNodes函数并不能完全保证，即使<tag>abc</tag>这样的节点也认为有子节点，且子节点的名称为"#text"或"#cdata-section"，所以还要再加两个条件判断
                        if (xmlNode.HasChildNodes && xmlNode.FirstChild.Name != "#text" && xmlNode.FirstChild.Name != "#cdata-section")
                        {
                            XmlNodeList childNodeList = xmlNode.ChildNodes;
                            Dictionary<string, object> nodeValue = AddNodesInfomation(childNodeList);
                            if (!xmlDic.Keys.Contains(nodeKey))
                            {
                                xmlDic.Add(nodeKey, nodeValue);
                            }
                        }

                        // 否则，该节点的value是节点的值
                        else
                        {
                            string nodeValue = xmlNode.InnerText;
                            if (!xmlDic.Keys.Contains(nodeKey))
                            {
                                xmlDic.Add(nodeKey, nodeValue);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return xmlDic;
        }

        /// <summary>
        /// 根据提供的路径访问全局字典中的某个节点
        /// </summary>
        /// <param name="path">访问路径，书写格式为"FileName/Root/node1/node2/node3"，例如"WebMaster/Root/template/Footer"</param>
        /// <returns>返回一个对象格式，具体使用何种变量，可在调用时决定</returns>
        public static object Select(string path)
        {
            object myObj = null;

            try
            {
                if (!string.IsNullOrEmpty(path))
                {
                    // 初始赋值为整个字典
                    myObj = XmlDictionary;

                    // 将传入的路径拆分，每个路径都是字典中的一个key
                    string[] paths = path.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    // 依次访问每个key
                    foreach (string p in paths)
                    {
                        // 判断下一级对象是不是字典，若是字典，则判断字典中是否包含下一级的key
                        // 若包含key，则继续查询，否则直接跳出循环，返回一个null
                        if (myObj.GetType().Equals(typeof(Dictionary<string, object>)) && ((Dictionary<string, object>)myObj).Keys.Contains(p))
                        {
                            myObj = ((Dictionary<string, object>)myObj)[p];
                        }
                        else
                        {
                            myObj = null;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return myObj;
        }

        /// <summary>
        /// 根据提供的路径访问全局字典中的某个节点
        /// </summary>
        /// <param name="path">访问路径，书写格式为"FileName/Root/node1/node2/node3"，例如"WebMaster/Root/template/Footer"</param>
        /// <returns>返回一个字符串</returns>
        public static string SelectText(string path)
        {
            string str = string.Empty;

            str = Utils.GetString(Select(path));

            return str;
        }
    }
}
