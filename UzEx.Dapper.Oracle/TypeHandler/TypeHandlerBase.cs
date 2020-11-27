using System;
using System.Collections.Concurrent;
using System.Data;
using System.Linq.Expressions;
using Dapper;

namespace UzEx.Dapper.Oracle.TypeHandler
{
    public abstract class TypeHandlerBase<T> : SqlMapper.TypeHandler<T>
    {
        private static readonly ConcurrentDictionary<DictionaryKey, Action<IDbDataParameter>> OracleDbTypeProperty =
            new ConcurrentDictionary<DictionaryKey, Action<IDbDataParameter>>(new DictionaryKeyComparer());

        protected void SetOracleDbTypeOnParameter(IDbDataParameter parameter, string oracleTypeName, int? length = null)
        {
            var setter = OracleDbTypeProperty.GetOrAdd(new DictionaryKey
            {
                ParameterType = parameter.GetType(),
                OracleTypeName = oracleTypeName
            }, CreateSetTypeAction);

            setter(parameter);

            if (length.HasValue) parameter.Size = length.Value;
        }

        private static Action<IDbDataParameter> CreateSetTypeAction(DictionaryKey key)
        {
            var enumType = key.ParameterType.Assembly.GetType($"{key.ParameterType.Namespace}.OracleDbType");
            var enumValue = Enum.Parse(enumType, key.OracleTypeName);
            var inputVariable = Expression.Parameter(typeof(IDbDataParameter));
            var convertExpression = Expression.Convert(inputVariable, key.ParameterType);
            var expression = Expression.Assign(Expression.PropertyOrField(convertExpression, "OracleDbType"),
                Expression.Constant(enumValue));

            return Expression.Lambda<Action<IDbDataParameter>>(expression, inputVariable).Compile();
        }
    }
}