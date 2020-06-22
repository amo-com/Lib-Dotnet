using MySql.Data.MySqlClient;

namespace Amo.Lib.DataBase
{
    public class MySqlDbTypeExtension
    {
        public static MySqlDbType Int = MySqlDbType.Int32;
        public static MySqlDbType NVarChar = MySqlDbType.VarChar;
        public static MySqlDbType Money = MySqlDbType.Decimal;
        public static MySqlDbType SmallInt = MySqlDbType.Int16;
        public static MySqlDbType Char = MySqlDbType.VarChar;
        public static MySqlDbType Image = MySqlDbType.MediumBlob;
        public static MySqlDbType UniqueIdentifier = MySqlDbType.Guid;
        public static MySqlDbType NChar = MySqlDbType.VarChar;
        public static MySqlDbType VarChar = MySqlDbType.VarChar;
        public static MySqlDbType Bit = MySqlDbType.Bit;
        public static MySqlDbType DateTime = MySqlDbType.DateTime;
        public static MySqlDbType Decimal = MySqlDbType.Decimal;
        public static MySqlDbType Float = MySqlDbType.Float;
        public static MySqlDbType VarBinary = MySqlDbType.VarBinary;
        public static MySqlDbType Text = MySqlDbType.Text;
    }
}
