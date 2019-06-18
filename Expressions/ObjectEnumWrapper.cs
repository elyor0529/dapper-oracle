using System;
using System.Linq.Expressions;

namespace UzEx.Dapper.Oracle.Expressions
{
    public class ObjectEnumWrapper<TObject, TEnumType> : ObjectWrapper<TObject, TEnumType>
    {

        private readonly string _enumType;
        private readonly string _propertyName;
        private readonly Type _objectType;

        public ObjectEnumWrapper(string enumType, string propertyName, Type objectType) : base(propertyName, objectType)
        {
            _enumType = enumType;
            _propertyName = propertyName;
            _objectType = objectType;
        }

        protected override Func<TObject, TEnumType> CreateGetter()
        {
            var inputVariable = Expression.Parameter(typeof(TObject));
            var converted = Expression.Convert(inputVariable, _objectType);
            var retreiver = Expression.Property(converted, _propertyName);
            var intValue = Expression.Convert(retreiver, typeof(int));
            var returnValue = Expression.Convert(intValue, typeof(TEnumType));

            return Expression.Lambda<Func<TObject, TEnumType>>(returnValue, inputVariable).Compile();
        }

        protected override Action<TObject, TEnumType> CreateSetter()
        {
            var enumType = _objectType.Assembly.GetType($"{_objectType.Namespace}.{_enumType}");
            var inputVariable = Expression.Parameter(typeof(TObject));
            var inputVariable2 = Expression.Parameter(typeof(TEnumType));
            var intValue = Expression.Convert(inputVariable2, typeof(int));
            var enumValue = Expression.Convert(intValue, enumType);
            var convertExpression = Expression.Convert(inputVariable, _objectType);
            var expression = Expression.Assign( Expression.PropertyOrField(convertExpression, _propertyName), enumValue);

            return Expression.Lambda<Action<TObject, TEnumType>>(expression, inputVariable, inputVariable2).Compile();
        }
    }
}


