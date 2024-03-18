using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using FluentAssertions;
using Oracle.ManagedDataAccess.Client;
using Xunit;

namespace UzEx.Dapper.Oracle.Tests
{
    internal class TestableOracleDynamicParameters : OracleDynamicParameters
    {
        public void AddParam(IDbCommand command)
        {
            AddParameters(command, null);
        }
    }

    public class OracleDynamicParameterTests
    {
        private readonly TestableOracleDynamicParameters _testObject = new TestableOracleDynamicParameters();

        public static IEnumerable<object[]> OracleDataFixture
        {
            get { yield return new object[] {new OracleCommand(), new OracleManagedParameterRetretreiver()}; }
        }

        public static IEnumerable<object[]> OracleCommandFixture
        {
            get { yield return new object[] {new OracleCommand()}; }
        }

        [Theory]
        [MemberData(nameof(OracleDataFixture))]
        public void SetOracleParameter(IDbCommand cmd, IOracleParameterRetretreiver retreiver)
        {
            _testObject.Add("Foo", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.ReturnValue);
            _testObject.AddParam(cmd);
            var param = retreiver.GetParameter(cmd.Parameters[0]);
            param.OracleDbType.Should().Be("RefCursor");
            param.ParameterName.Should().Be("Foo");
        }

        [Theory]
        [MemberData(nameof(OracleDataFixture))]
        public void SetOracleParameterCommand(IDbCommand cmd, IOracleParameterRetretreiver retreiver)
        {
            _testObject.Add("Foo", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.ReturnValue);

            _testObject.AddParam(cmd);
            var param = retreiver.GetParameter(cmd.Parameters[0]);
            param.ParameterName.Should().Be("Foo");
            param.OracleDbType.Should().Be("RefCursor");
        }

        [Theory]
        [MemberData(nameof(OracleDataFixture))]
        public void SetOracleParameterCommandPlSqlAssociativeArray(IDbCommand cmd,
            IOracleParameterRetretreiver retreiver)
        {
            _testObject.Add("Foo", collectionType: OracleMappingCollectionType.PLSQLAssociativeArray);

            _testObject.AddParam(cmd);
            var param = retreiver.GetParameter(cmd.Parameters[0]);
            param.CollectionType.Should().Be("PLSQLAssociativeArray");
        }

        [Theory]
        [MemberData(nameof(OracleDataFixture))]
        public void SetAllProperties(IDbCommand cmd, IOracleParameterRetretreiver retreiver)
        {
            _testObject.Add("Foo", "Bar", OracleMappingType.Varchar2, ParameterDirection.Input, 42, true, 0, 0,
                "MySource", DataRowVersion.Original);

            _testObject.AddParam(cmd);
            var param = retreiver.GetParameter(cmd.Parameters[0]);
            param.ParameterName.Should().Be("Foo");
            param.Value.Should().Be("Bar");
            param.OracleDbType.Should().Be("Varchar2");
            param.Direction.Should().Be(ParameterDirection.Input);
            param.Size.Should().Be(42);
            param.IsNullable.Should().Be(true);
            param.Scale.Should().Be(0);
            param.Precision.Should().Be(0);
            param.SourceColumn.Should().Be("MySource");
            param.SourceVersion.Should().Be(DataRowVersion.Original);
        }

        [Theory]
        [MemberData(nameof(OracleCommandFixture))]
        public void SetBindByNameFalse(IDbCommand cmd)
        {
            _testObject.BindByName = true;
            _testObject.Add("Foo", "Bar");
            _testObject.AddParam(cmd);

            var value = (bool) cmd.GetType().GetProperty("BindByName").GetValue(cmd);
            value.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(OracleCommandFixture))]
        public void SetBindByNameNotSet(IDbCommand cmd)
        {
            _testObject.BindByName = false;
            _testObject.Add("Foo", "Bar");
            _testObject.AddParam(cmd);

            var value = (bool) cmd.GetType().GetProperty("BindByName").GetValue(cmd);
            value.Should().BeFalse();
        }

        [Fact]
        public void WrongTypeConnectionWillThrowException()
        {
            var cmd = new SqlCommand();
            _testObject.Add("Foo", "Bar");
            Action act = () => _testObject.AddParam(cmd);
            act.Should().Throw<NotSupportedException>();
        }

        [Theory]
        [MemberData(nameof(OracleDataFixture))]
        public void ArrayBindSizeSet_ValueIsSet(IDbCommand cmd, IOracleParameterRetretreiver retreiver)
        {
            var bindSizeArray = Enumerable.Range(0, 20).ToArray();
            _testObject.Add("Foo", "Bar", arrayBindSize: bindSizeArray);

            _testObject.AddParam(cmd);

            var param = retreiver.GetParameter(cmd.Parameters[0]);
            param.ParameterName.Should().Be("Foo");
            param.Value.Should().Be("Bar");
            param.ArrayBindSize.Should().BeSameAs(bindSizeArray);
        }

        [Theory]
        [MemberData(nameof(OracleCommandFixture))]
        public void GetParameter_DoesNotReturnNull(IDbCommand cmd)
        {
            var bindSizeArray = Enumerable.Range(0, 20).ToArray();
            _testObject.Add("Foo", "Bar", arrayBindSize: bindSizeArray);

            _testObject.AddParam(cmd);

            var param = _testObject.GetParameter("Foo");
            param.Name.Should().Be("Foo");
            param.Value.Should().Be("Bar");
            param.ArrayBindSize.Should().BeSameAs(bindSizeArray);
        }

        [Fact]
        public void GetParameterValue()
        {
            _testObject.Add("Foo", "Bar", OracleMappingType.Varchar2);

            _testObject.Get<string>("Foo").Should().Be("Bar");
        }

        [Fact]
        public void GetParameter()
        {
            _testObject.Add("Foo", "Bar", OracleMappingType.Varchar2);

            var param = _testObject.GetParameter("Foo");

            param.Should().NotBeNull();
            param.Name.Should().Be("Foo");
            param.Value.Should().Be("Bar");
            param.DbType.Should().Be(OracleMappingType.Varchar2);
        }
    }
}