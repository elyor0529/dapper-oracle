using System;
using System.Collections.Concurrent;
using System.Data;

namespace UzEx.Dapper.Oracle
{
    internal static class OracleMethodHelper
    {
        private static readonly ConcurrentDictionary<Type, OracleParameterExpressions> CachedOracleTypes =
            new ConcurrentDictionary<Type, OracleParameterExpressions>();

        private static readonly ConcurrentDictionary<Type, CommandExpressions> CachedOracleCommandProperties =
            new ConcurrentDictionary<Type, CommandExpressions>();

        public static void SetArrayBindCount(IDbCommand command, int arrayBindCount)
        {
            Get(command).ArrayBindCount.SetValue(command, arrayBindCount);
        }

        public static void SetInitialLobFetchSize(IDbCommand command, int arrayBindCount)
        {
            Get(command).InitialLOBFetchSize.SetValue(command, arrayBindCount);
        }

        public static void SetBindByName(IDbCommand command, bool bindByName)
        {
            Get(command).BindByName.SetValue(command, bindByName);
        }

        public static void SetOracleParameters(IDbDataParameter parameter,
            OracleParameterInfo oracleParameterInfo)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            var method = CachedOracleTypes.GetOrAdd(parameter.GetType(), GetOracleProperties);

            if (oracleParameterInfo.DbType.HasValue)
                method.OraDbType.SetValue(parameter, oracleParameterInfo.DbType.Value);

            if (oracleParameterInfo.IsNullable.HasValue)
                method.IsNullable.SetValue(parameter, oracleParameterInfo.IsNullable.Value);

            if (oracleParameterInfo.Scale.HasValue) parameter.Scale = oracleParameterInfo.Scale.Value;

            if (oracleParameterInfo.Precision.HasValue) parameter.Precision = oracleParameterInfo.Precision.Value;

            parameter.SourceVersion = oracleParameterInfo.SourceVersion;


            if (oracleParameterInfo.SourceColumn != null) parameter.SourceColumn = oracleParameterInfo.SourceColumn;

            if (oracleParameterInfo.CollectionType != OracleMappingCollectionType.None)
                method.CollectionType.SetValue(parameter, oracleParameterInfo.CollectionType);

            if (oracleParameterInfo.ArrayBindSize != null)
                method.ArrayBindSize.SetValue(parameter, oracleParameterInfo.ArrayBindSize);
        }

        internal static OracleParameterInfo GetParameterInfo(IDbDataParameter parameter)
        {
            var method = GetOracleProperties(parameter.GetType());
            var paramInfo = new OracleParameterInfo
            {
                Name = parameter.ParameterName,
                SourceVersion = parameter.SourceVersion,
                Precision = parameter.Precision,
                Size = parameter.Size,
                DbType = method.OraDbType.GetValue(parameter),
                ArrayBindSize = method.ArrayBindSize.GetValue(parameter),
                CollectionType = method.CollectionType.GetValue(parameter),
                ParameterDirection = parameter.Direction,
                IsNullable = parameter.IsNullable,
                Scale = parameter.Scale,
                SourceColumn = parameter.SourceColumn,
                Status = method.Status.GetValue(parameter),
                Value = parameter.Value
            };


            return paramInfo;
        }

        private static OracleParameterExpressions GetOracleProperties(Type type)
        {
            return new OracleParameterExpressions(type);
        }

        private static CommandExpressions Get(IDbCommand command)
        {
            return CachedOracleCommandProperties.GetOrAdd(command.GetType(),type => new CommandExpressions(type));
        }

    }
}