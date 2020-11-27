using System;
using System.Data;

namespace UzEx.Dapper.Oracle.TypeHandler
{
    /// <summary>
    ///     Conversion between <see cref="bool" /> and Oracle VARCHAR2
    /// </summary>
    public class BooleanStringTypeHandler : TypeHandlerBase<bool>
    {
        private readonly StringComparison _compareMode;
        private readonly string _falseString;
        private readonly string _trueString;

        /// <summary>
        ///     Creates an instance of this class
        /// </summary>
        /// <param name="trueValue">BulkMapping value to use in database for a boolean true value</param>
        /// <param name="falseValue">BulkMapping value to use in database for a boolean false value</param>
        /// <param name="comparison"></param>
        public BooleanStringTypeHandler(string trueValue, string falseValue,
            StringComparison comparison = StringComparison.Ordinal)
        {
            _trueString = trueValue;
            _falseString = falseValue;
            _compareMode = comparison;
        }

        public override void SetValue(IDbDataParameter parameter, bool value)
        {
            SetOracleDbTypeOnParameter(parameter, "Varchar2");

            parameter.Value = value ? _trueString : _falseString;
        }

        public override bool Parse(object value)
        {
            if (value is string valuestring)
            {
                if (valuestring.Equals(_trueString, _compareMode))
                    return true;

                if (valuestring.Equals(_falseString, _compareMode))
                    return false;

                throw new NotSupportedException(
                    $"'{valuestring}' was unexpected - expected '{_trueString}' or '{_falseString}'");
            }

            throw new NotSupportedException($"Dont know how to convert a {value.GetType()} to a Boolean");
        }
    }
}