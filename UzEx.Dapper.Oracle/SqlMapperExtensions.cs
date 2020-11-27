using System;
using System.Collections.Generic;
using Dapper;

namespace UzEx.Dapper.Oracle
{
    public static class OracleTypeMapper
    {
        private static readonly Dictionary<Type, SqlMapper.ITypeHandler> Handlers = new Dictionary<Type, SqlMapper.ITypeHandler>();

        public static void AddTypeHandler(Type type, SqlMapper.ITypeHandler handler)
        {
            Handlers[type] = handler;

            SqlMapper.AddTypeHandler(type, handler);
        }

        public static void AddTypeHandler<T>(SqlMapper.ITypeHandler handler)
        {
            Handlers[typeof(T)] = handler;

            SqlMapper.AddTypeHandler(typeof(T), handler);
        }

        public static bool HasTypeHandler(Type type, out SqlMapper.ITypeHandler handler)
        {
            if (Handlers.ContainsKey(type))
            {
                handler = Handlers[type];
                return true;
            }

            handler = null;
            return false;
        }
    }
}