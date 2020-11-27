using Dapper;
using FluentAssertions;
using UzEx.Dapper.Oracle.Tests.IntegrationTests.Util;
using UzEx.Dapper.Oracle.TypeHandler;
using Xunit;

namespace UzEx.Dapper.Oracle.Tests.IntegrationTests
{
    [Collection("OracleDocker")]
    public class BooleanStringTypeMapperTests
    {
        public BooleanStringTypeMapperTests(DatabaseFixture fixture)
        {
            Fixture = fixture;

            var columns = new[]
            {
                new NumberColumn {Name = "Id", DataType = OracleMappingType.Int32, PrimaryKey = true},
                new TableColumn {Name = "BooleanValue", DataType = OracleMappingType.Char, Size = 1}
            };
            OracleTypeMapper.AddTypeHandler(typeof(bool), new BooleanStringTypeHandler("Y", "N"));
            TableCreator.Create(Fixture.Connection, "BoolCharTypeMappingTest", columns);
        }

        private DatabaseFixture Fixture { get; }

        private int InsertValue(int id, bool booleanValue)
        {
            var sql = "INSERT INTO BoolCharTypeMappingTest(Id,BooleanValue) VALUES(:ID,:BOOL)";

            var parameters = new OracleDynamicParameters();
            parameters.Add("ID", id);
            parameters.Add("BOOL", booleanValue);
            return Fixture.Connection.Execute(sql, parameters);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void TestInsertMapping()
        {
            var count = InsertValue(1, true);
            count.Should().Be(1);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void TestSelectMappingFalse()
        {
            InsertValue(10, false);
            var sql = "SELECT * FROM BoolCharTypeMappingTest WHERE ID=10";
            var actual = Fixture.Connection.QuerySingle<BoolTestMapping>(sql);
            actual.BooleanValue.Should().BeFalse();
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void TestSelectMappingTrue()
        {
            InsertValue(11, true);
            var sql = "SELECT * FROM BoolCharTypeMappingTest WHERE ID=11";
            var actual = Fixture.Connection.QuerySingle<BoolTestMapping>(sql);
            actual.BooleanValue.Should().BeTrue();
        }

        internal class BoolTestMapping
        {
            public int Id { get; set; }
            public bool BooleanValue { get; set; }
        }
    }
}