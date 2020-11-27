using System.Data;

namespace UzEx.Dapper.Oracle.TypeHandler
{
    /// <summary>
    ///     Conversion between <see cref="bool" /> in .net and NUMBER(1) oracle data type.
    ///     Numeric value 0 is false, any other value equals true.
    /// </summary>
    public class BooleanNumericTypeHandler : TypeHandlerBase<bool>
    {
        public override void SetValue(IDbDataParameter parameter, bool value)
        {
            SetOracleDbTypeOnParameter(parameter, "Int16");

            parameter.Value = value ? 1 : 0;
        }

        public override bool Parse(object value)
        {
            return (int) value != 0;
        }
    }
}