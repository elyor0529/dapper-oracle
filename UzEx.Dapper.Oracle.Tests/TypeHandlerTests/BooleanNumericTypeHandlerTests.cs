using FluentAssertions;
using Oracle.ManagedDataAccess.Client;
using UzEx.Dapper.Oracle.TypeHandler;
using Xunit;

namespace UzEx.Dapper.Oracle.Tests.TypeHandlerTests
{
    public class BooleanNumericTypeHandlerTests
    {
        [Fact]
        public void ConvertFromTrue()
        {
            var parameter = new OracleParameter();
            var sut = new BooleanNumericTypeHandler();
            sut.SetValue(parameter, true);

            parameter.Value.Should().Be(1);
            parameter.OracleDbType.Should().Be(OracleDbType.Int16);
        }

        [Fact]
        public void ConvertToTrue()
        {
            var sut = new BooleanNumericTypeHandler();
            var result = sut.Parse(1);

            result.Should().Be(true);
        }
    }
}