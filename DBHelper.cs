using System;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

public static class DBHelper
{
    private static string ConnStr =>
        ConfigurationManager.ConnectionStrings["OracleDbConn"].ConnectionString;

    public static DataTable ExecuteCursorProc(string procName, params OracleParameter[] parameters)
    {
        DataTable dt = new DataTable();
        using (OracleConnection conn = new OracleConnection(ConnStr))
        using (OracleCommand cmd = new OracleCommand(procName, conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (var p in parameters) cmd.Parameters.Add(p);

            conn.Open();
            using (OracleDataAdapter da = new OracleDataAdapter(cmd))
            {
                da.Fill(dt);
            }
        }
        return dt;
    }

    public static void ExecuteNonQueryProc(string procName, params OracleParameter[] parameters)
    {
        using (OracleConnection conn = new OracleConnection(ConnStr))
        using (OracleCommand cmd = new OracleCommand(procName, conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (var p in parameters) cmd.Parameters.Add(p);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    public static DataTable ExecuteCursorProcWithOutParam(
        string procName, string outParamName, OracleDbType outType, int outSize,
        out object outValue, params OracleParameter[] parameters)
    {
        DataTable dt = new DataTable();
        using (OracleConnection conn = new OracleConnection(ConnStr))
        using (OracleCommand cmd = new OracleCommand(procName, conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (var p in parameters) cmd.Parameters.Add(p);

            OracleParameter outParam = new OracleParameter(outParamName, outType, outSize);
            outParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outParam);

            conn.Open();
            using (OracleDataAdapter da = new OracleDataAdapter(cmd))
            {
                da.Fill(dt);
            }
            outValue = outParam.Value;
        }
        return dt;
    }

    public static OracleParameter CursorOutParam(string name = "p_cursor")
    {
        return new OracleParameter(name, OracleDbType.RefCursor, ParameterDirection.Output);
    }

    public static OracleParameter In(string name, OracleDbType type, object value)
    {
        var p = new OracleParameter(name, type);
        p.Value = value ?? DBNull.Value;
        return p;
    }
}