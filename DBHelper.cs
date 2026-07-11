using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace DigitalCertSystem
{
    public static class DBHelper
    {
        private static string ConnStr =>
            ConfigurationManager.ConnectionStrings["OracleDbConn"].ConnectionString;

        public static OracleConnection GetConnection()
        {
            var conn = new OracleConnection(ConnStr);
            conn.Open();
            return conn;
        }

        public static OracleCommand CreateCommand(string procName, OracleConnection conn)
        {
            var cmd = new OracleCommand(procName, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd;
        }
    }
}