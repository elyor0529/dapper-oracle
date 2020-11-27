using System;
using System.Data;
using UzEx.Dapper.Oracle.Expressions;

namespace UzEx.Dapper.Oracle
{
    internal class OracleParameterExpressions
    {
        public OracleParameterExpressions(Type oracleParameterType)
        {
            if (!oracleParameterType.Namespace.StartsWith("Oracle"))
                throw new NotSupportedException(
                    $"Whoopsies! This library will only work with Oracle types, you are attempting to use type {oracleParameterType.FullName}.");

            OraDbType = new ObjectEnumWrapper<IDbDataParameter, OracleMappingType>("OracleDbType", "OracleDbType",
                oracleParameterType);
            ArrayBindSize = new ObjectWrapper<IDbDataParameter, int[]>("ArrayBindSize", oracleParameterType);
            CollectionType =
                new ObjectEnumWrapper<IDbDataParameter, OracleMappingCollectionType>("OracleCollectionType",
                    "CollectionType", oracleParameterType);
            Status = new ObjectEnumWrapper<IDbDataParameter, OracleParameterMappingStatus>("Status", "Status",
                oracleParameterType);
            IsNullable = new ObjectWrapper<IDbDataParameter, bool>("IsNullable", oracleParameterType);
        }

        public ObjectEnumWrapper<IDbDataParameter, OracleMappingType> OraDbType { get; }

        public ObjectWrapper<IDbDataParameter, int[]> ArrayBindSize { get; }

        public ObjectWrapper<IDbDataParameter, bool> IsNullable { get; }

        public ObjectEnumWrapper<IDbDataParameter, OracleMappingCollectionType> CollectionType { get; }

        public ObjectEnumWrapper<IDbDataParameter, OracleParameterMappingStatus> Status { get; }
    }
}