using System;

namespace UzEx.Dapper.Oracle.TypeHandler
{
    internal class DictionaryKey
    {
        public Type ParameterType { get; set; }
        public string OracleTypeName { get; set; }
    }
}