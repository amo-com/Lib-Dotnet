using System.Collections.Generic;

namespace Amo.Lib.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// 修改指定Key值,顺序和Value不变
        /// </summary>
        /// <typeparam name="TK">Key类型</typeparam>
        /// <typeparam name="TV">Value类型</typeparam>
        /// <param name="attr">字典</param>
        /// <param name="oldKey">旧的Key值</param>
        /// <param name="newKey">新的Key值</param>
        public static void RenameKey<TK, TV>(this Dictionary<TK, TV> attr, TK oldKey, TK newKey)
        {
            if (attr == null || oldKey == null || newKey == null || !attr.ContainsKey(oldKey))
            {
                return;
            }

            // 先copy到Temp字典
            Dictionary<TK, TV> tempAttr = attr.Clone();
            attr.Clear();

            foreach (var key in tempAttr.Keys)
            {
                if (key.Equals(oldKey))
                {
                    attr[newKey] = tempAttr[key];
                    continue;
                }

                attr[key] = tempAttr[key];
            }
        }

        public static Dictionary<TK, TV> Clone<TK, TV>(this Dictionary<TK, TV> attr)
        {
            if (attr == null)
            {
                return null;
            }

            Dictionary<TK, TV> newAttr = new Dictionary<TK, TV>();
            foreach (var key in attr.Keys)
            {
                newAttr[key] = attr[key];
            }

            return newAttr;
        }

        /// <summary>
        /// 在指定Key前后插入新数据
        /// </summary>
        /// <typeparam name="TK">Key类型</typeparam>
        /// <typeparam name="TV">Value类型</typeparam>
        /// <param name="attr">字典</param>
        /// <param name="selectedKey">要搜索deKey</param>
        /// <param name="newKey">新数据的Key</param>
        /// <param name="newValue">新数据的Value</param>
        /// <param name="beforeOrAfter">true:Before;  false:After</param>
        /// <returns>插入成功Or失败</returns>
        public static bool Insert<TK, TV>(this Dictionary<TK, TV> attr, TK selectedKey, TK newKey, TV newValue, bool beforeOrAfter = false)
        {
            if (attr == null || selectedKey == null || newKey == null || !attr.ContainsKey(selectedKey))
            {
                return false;
            }

            // 先copy到Temp字典
            Dictionary<TK, TV> tempAttr = attr.Clone();
            attr.Clear();

            bool isInsert = false;
            foreach (var key in tempAttr.Keys)
            {
                if (key.Equals(selectedKey))
                {
                    if (beforeOrAfter)
                    {
                        attr[newKey] = newValue;
                        attr[key] = tempAttr[key];
                    }
                    else
                    {
                        attr[key] = tempAttr[key];
                        attr[newKey] = newValue;
                    }

                    isInsert = true;
                    continue;
                }

                attr[key] = tempAttr[key];
            }

            return isInsert;
        }

        /// <summary>
        /// 在指定Key前插入新数据
        /// </summary>
        /// <typeparam name="TK">Key类型</typeparam>
        /// <typeparam name="TV">Value类型</typeparam>
        /// <param name="attr">字典</param>
        /// <param name="selectedKey">要搜索deKey</param>
        /// <param name="newKey">新数据的Key</param>
        /// <param name="newValue">新数据的Value</param>
        /// <returns>插入成功Or失败</returns>
        public static bool InsertBefore<TK, TV>(this Dictionary<TK, TV> attr, TK selectedKey, TK newKey, TV newValue)
        {
            return attr.Insert(selectedKey, newKey, newValue, true);
        }

        /// <summary>
        /// 在指定Key后插入新数据
        /// </summary>
        /// <typeparam name="TK">Key类型</typeparam>
        /// <typeparam name="TV">Value类型</typeparam>
        /// <param name="attr">字典</param>
        /// <param name="selectedKey">要搜索deKey</param>
        /// <param name="newKey">新数据的Key</param>
        /// <param name="newValue">新数据的Value</param>
        /// <returns>插入成功Or失败</returns>
        public static bool InsertAfter<TK, TV>(this Dictionary<TK, TV> attr, TK selectedKey, TK newKey, TV newValue)
        {
            return attr.Insert(selectedKey, newKey, newValue, false);
        }

        /// <summary>
        /// 在指定索引位置插入新数据
        /// </summary>
        /// <typeparam name="TK">Key类型</typeparam>
        /// <typeparam name="TV">Value类型</typeparam>
        /// <param name="attr">字典</param>
        /// <param name="index">要插入的索引位置(0-Count)</param>
        /// <param name="newKey">新数据的Key</param>
        /// <param name="newValue">新数据的Value</param>
        /// <returns>插入成功Or失败</returns>
        public static bool Insert<TK, TV>(this Dictionary<TK, TV> attr, int index, TK newKey, TV newValue)
        {
            if (attr == null || newKey == null || index > attr.Count)
            {
                return false;
            }

            // 先copy到Temp字典
            Dictionary<TK, TV> tempAttr = attr.Clone();
            attr.Clear();

            bool isInsert = false;
            int number = 0;
            foreach (var key in tempAttr.Keys)
            {
                if (number++ == index)
                {
                    isInsert = true;
                    attr[newKey] = newValue;
                }

                attr[key] = tempAttr[key];
            }

            if (number++ == index)
            {
                isInsert = true;
                attr[newKey] = newValue;
            }

            return isInsert;
        }
    }
}
