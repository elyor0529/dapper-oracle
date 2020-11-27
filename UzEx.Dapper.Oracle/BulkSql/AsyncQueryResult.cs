namespace UzEx.Dapper.Oracle.BulkSql
{
    public class AsyncQueryResult
    {
        /// <summary>
        ///     Return value from Execute statement, returns the number of rows affected.
        /// </summary>
        public int ExecuteResult { get; set; }

        /// <summary>
        ///     Returns the Dynamic parameters used in query.
        /// </summary>
        public OracleDynamicParameters Parameters { get; set; }
    }
}