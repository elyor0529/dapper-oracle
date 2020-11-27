using System;
using System.Linq.Expressions;

namespace UzEx.Dapper.Oracle.Expressions
{
    public class ObjectWrapper<TObject, TValue>
    {
        private readonly Type _objectType;
        private readonly string _propertyName;
        private Func<TObject, TValue> _getter;
        private Action<TObject, TValue> _setter;

        public ObjectWrapper(string propertyName, Type objectType)
        {
            _propertyName = propertyName;
            _objectType = objectType;
        }

        public void SetValue(TObject command, TValue value)
        {
            if (_setter == null) 
                _setter = CreateSetter();

            _setter(command, value);
        }

        public TValue GetValue(TObject obj)
        {
            if (_getter == null)
                _getter = CreateGetter();

            return _getter(obj);
        }

        protected virtual Func<TObject, TValue> CreateGetter()
        {
            var inputVariable = Expression.Parameter(typeof(TObject));

            if (typeof(TObject) != _objectType)
            {
                var converted = Expression.Convert(inputVariable, _objectType);
                var retreiver = Expression.Property(converted, _objectType.GetProperty(_propertyName));

                return Expression.Lambda<Func<TObject, TValue>>(retreiver, inputVariable).Compile();
            }
            else
            {
                var retreiver = Expression.Property(inputVariable, _objectType.GetProperty(_propertyName));

                return Expression.Lambda<Func<TObject, TValue>>(retreiver, inputVariable).Compile();
            }
        }

        protected virtual Action<TObject, TValue> CreateSetter()
        {
            var inputVariable = Expression.Parameter(typeof(TObject));
            var inputVariable2 = Expression.Parameter(typeof(TValue));
            var convertExpression = Expression.Convert(inputVariable, _objectType);
            var expression = Expression.Assign(Expression.PropertyOrField(convertExpression, _propertyName),
                inputVariable2);

            return Expression.Lambda<Action<TObject, TValue>>(expression, inputVariable, inputVariable2).Compile();
        }
    }
}