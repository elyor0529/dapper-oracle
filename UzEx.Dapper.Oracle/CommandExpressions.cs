using System;
using System.Data;
using UzEx.Dapper.Oracle.Expressions;

namespace UzEx.Dapper.Oracle
{
    internal   class CommandExpressions
    {
        public CommandExpressions(Type commandType)
        {
            BindByName = new ObjectWrapper<IDbCommand, bool>("BindByName", commandType);
            InitialLOBFetchSize = new ObjectWrapper<IDbCommand, int>("InitialLOBFetchSize", commandType);
            ArrayBindCount = new ObjectWrapper<IDbCommand, int>("ArrayBindCount", commandType);
        }

        public ObjectWrapper<IDbCommand, bool> BindByName { get; }
        public ObjectWrapper<IDbCommand, int> InitialLOBFetchSize { get; }
        public ObjectWrapper<IDbCommand, int> ArrayBindCount { get; }
    }
}