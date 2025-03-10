﻿using System.Collections.Generic;
using System.Data;
using System.Linq;
using FluentAssertions;
using UzEx.Dapper.Oracle.Expressions;
using Xunit;
using Managed = Oracle.ManagedDataAccess.Client;

namespace UzEx.Dapper.Oracle.Tests.Expressions
{
    public class ParameterBaseTests
    {
        public static IEnumerable<object[]> OracleDataFixture
        {
            get
            {
                yield return new object[] {new Managed.OracleCommand()}; 
            }
        }
    }

    public class ObjectEnumWrapperTests : ParameterBaseTests
    {
        [Theory]
        [MemberData(nameof(OracleDataFixture))]
        public void BasicTest(IDbCommand cmd)
        {
            var param = cmd.CreateParameter();

            var setter =
                new ObjectEnumWrapper<IDbDataParameter, OracleMappingType>("OracleDbType", "OracleDbType",
                    param.GetType());
            setter.SetValue(param, OracleMappingType.Date);
            var value = setter.GetValue(param);
            value.ToString().Should().Be("Date");
        }
    }

    public class ObjectWrapperTests : ParameterBaseTests
    {
        [Theory]
        [MemberData(nameof(OracleDataFixture))]
        public void Test(IDbCommand cmd)
        {
            var param = cmd.CreateParameter();
            var wrapper = new ObjectWrapper<IDbDataParameter, int>("Size", param.GetType());
            wrapper.SetValue(param, 100);
            var result = wrapper.GetValue(param);
            result.Should().Be(100);
        }

        [Fact]
        public void SetPropertyOnClass()
        {
            var expected = Enumerable.Range(0, 10).ToArray();
            var input = new TestClass {PropertyWithGetterAndSetter = expected};
            var testClass = new ObjectWrapper<TestClass, int[]>("PropertyWithGetterAndSetter", typeof(TestClass));

            var actual = testClass.GetValue(input);
            actual.Should().BeSameAs(expected);
        }

        [Fact]
        public void SetPropertyOnInterface()
        {
            var expected = Enumerable.Range(0, 10).ToArray();
            var input = new TestClass {PropertyWithGetterAndSetter = expected};
            var testClass = new ObjectWrapper<ITestClass, int[]>("PropertyWithGetterAndSetter", typeof(TestClass));

            var actual = testClass.GetValue(input);
            actual.Should().BeSameAs(expected);
        }

        private class TestClass : ITestClass
        {
            public int[] PropertyWithGetterAndSetter { get; set; }

            public bool PropertyWithGetterOnly { get; }
        }

        private interface ITestClass
        {
        }
    }
}