using System.Collections.Generic;

namespace UzEx.Dapper.Oracle.TypeHandler
{
    internal class DictionaryKeyComparer : IEqualityComparer<DictionaryKey>
    {
        public bool Equals(DictionaryKey x, DictionaryKey y)
        {
            if (x == null || y == null)
                return false;

            return x.ParameterType == y.ParameterType && x.OracleTypeName.Equals(y.OracleTypeName);
        }

        public int GetHashCode(DictionaryKey obj)
        {
            return 17 + obj.ParameterType.GetHashCode() * 23 + obj.OracleTypeName.GetHashCode() * 31;
        }
    }
}