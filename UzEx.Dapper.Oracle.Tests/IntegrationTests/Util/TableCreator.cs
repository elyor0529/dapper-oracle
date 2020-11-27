using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;

namespace UzEx.Dapper.Oracle.Tests.IntegrationTests.Util
{
    public class TableCreator
    {
        public static void Create(IDbConnection connection, string tableName, IEnumerable<TableColumn> columns)
        {
            var sql = @"BEGIN
EXECUTE IMMEDIATE 'DROP TABLE {tableName}';
    EXCEPTION
         WHEN OTHERS THEN
                IF SQLCODE != -942 THEN
                     RAISE;
                END IF;
    END;".Replace("{tableName}", tableName);

            connection.Execute(sql.Replace("\r\n", "\n"));

            var sb = new StringBuilder();

            var list = columns.ToArray();

            sb.AppendLine($"CREATE TABLE {tableName}(");

            sb.AppendLine(string.Join(",\n", columns));

            if (list.Any(l => l.PrimaryKey))
                sb.AppendLine(
                    $",CONSTRAINT pk_{tableName} PRIMARY KEY ({string.Join(",", list.Where(l => l.PrimaryKey).Select(c => c.Name))}))");

            connection.Execute(sb.ToString().Replace("\r\n", "\n"));
        }
    }

    public class NumberColumn : TableColumn
    {
        public override string ToString()
        {
            return $"{Name} NUMBER{GetSize()} {GetNullable(Nullable)}";
        }
    }

    public class TableColumn
    {
        public string Name { get; set; }
        public OracleMappingType DataType { get; set; }
        public int Size { get; set; }

        public bool Nullable { get; set; }
        public bool PrimaryKey { get; set; }

        public override string ToString()
        {
            return $"{Name} {DataType.ToString()}{GetSize()} {GetNullable(Nullable)}";
        }

        protected string GetSize()
        {
            if (Size == 0) return string.Empty;

            return $"({Size})";
        }

        protected static string GetNullable(bool nullable)
        {
            return nullable ? string.Empty : "NOT NULL";
        }
    }
}