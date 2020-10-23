using Amo.Lib.Enums;
using Amo.Lib.Exceptions;
using System;
using System.Collections.Generic;

namespace Amo.Lib.Impls
{
    public class ExceptionAnalysis : IExceptionAnalysis
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

        public (int enentType, string message, bool isSuccess) GetEventType(Exception ex)
        {
            int def = 0;
            string message = string.Empty;
            if (ex != null)
            {
                message = ex.Message;
                if (ex is CustomizeException customizeException)
                {
                    def = customizeException.EventType;
                }
                else if (ex is System.Data.SqlClient.SqlException sqlException)
                {
                    if (HResultDic.ContainsKey(ex.HResult))
                    {
                        def = (int)HResultDic[ex.HResult];

                        int sqlNumber = sqlException.Number;

                        // 如果是sqlerror问题，详细化
                        if (ex.HResult == -2146232060 && sqlNumber != 0 && SqlNumberDic.ContainsKey(sqlNumber))
                        {
                            def = (int)SqlNumberDic[sqlNumber];
                        }
                    }
                }
            }

            return def == 0 ? ((int)EventType.ApiError, message, false) : (def, message, true);
        }
    }
}
