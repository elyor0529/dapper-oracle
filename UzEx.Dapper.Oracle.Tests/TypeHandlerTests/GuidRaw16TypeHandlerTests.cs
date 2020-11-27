using System;
using FluentAssertions;
using Oracle.ManagedDataAccess.Client;
using UzEx.Dapper.Oracle.TypeHandler;
using Xunit;

namespace UzEx.Dapper.Oracle.Tests.TypeHandlerTests
{
    public class GuidRaw16TypeHandlerTests
    {
        [Fact]
        public void ConvertTo()
        {
            var input = Guid.NewGuid();

            var parameter = new OracleParameter();
            var sut = new GuidRaw16TypeHandler();
            sut.SetValue(parameter, input);

            parameter.Value.Should().BeEquivalentTo(input.ToByteArray());
            parameter.OracleDbType.Should().Be(OracleDbType.Raw);
        }

        [Fact]
        public void ConvertFrom()
        {
            var input = Guid.NewGuid();

            var sut = new GuidRaw16TypeHandler();
            var result = sut.Parse(input.ToByteArray());
            result.Should().Be(input);
        }
    }
}