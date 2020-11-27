using System;
using Oracle.ManagedDataAccess.Client;

namespace UzEx.Dapper.Oracle.Demo
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var conn=new OracleConnection();
            var com = conn.CreateCommand();
            
            com.Parameters.Add(new OracleDynamicParameters());

            Console.WriteLine("Done!");
        }
    }
}
