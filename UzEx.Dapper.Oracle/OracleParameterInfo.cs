using System.Data;

namespace UzEx.Dapper.Oracle
{
    /// <summary>
    ///     Class used to pass parameter information to and from the OracleCommand object
    /// </summary>
    public class OracleParameterInfo
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public ParameterDirection ParameterDirection { get; set; }

        public OracleMappingType? DbType { get; set; }

        public int? Size { get; set; }

        public bool? IsNullable { get; set; }

        public byte? Precision { get; set; }

        public byte? Scale { get; set; }

        public string SourceColumn { get; set; } = string.Empty;

        public DataRowVersion SourceVersion { get; set; }

        public OracleMappingCollectionType CollectionType { get; set; } = OracleMappingCollectionType.None;

        public int[] ArrayBindSize { get; set; }

        public OracleParameterMappingStatus Status { get; set; }

        public IDbDataParameter AttachedParam { get; set; }
    }
}