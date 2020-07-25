using Amo.Lib.Enums;
using Amo.Lib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amo.Lib.CoreApi.Common
{
    public class EventTypeAnalys
    {
        private static readonly Dictionary<int, EventType> HResultDic = new Dictionary<int, EventType>
        {
            { -2147467261, EventType.NullPointerException },
            { -2146233086, EventType.IndexOutOfBoundsException },
            { -2146232060, EventType.DataBaseError },
            { -2147467262, EventType.InvalidCastException },
        };

        private static readonly Dictionary<int, EventType> SqlNumberDic = new Dictionary<int, EventType>
        {
            { -2, EventType.DatabaseTimeout },
            { 207, EventType.SQLInvalid },
        };

        public static EventType GetEventType(Exception ex, int sqlNumber)
        {
            EventType def = EventType.ApiError;
            if (ex != null)
            {
                if (ex is CustomizeException)
                {
                    def = ((CustomizeException)ex).EventType;
                }
                else if (HResultDic.ContainsKey(ex.HResult))
                {
                    def = HResultDic[ex.HResult];

                    // 如果是sqlerror问题，详细化
                    if (ex.HResult == -2146232060 && sqlNumber != 0 && SqlNumberDic.ContainsKey(sqlNumber))
                    {
                        def = SqlNumberDic[sqlNumber];
                    }
                }
            }

            return def;
        }
    }
}
