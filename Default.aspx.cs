using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Digital_Certificate_Verification
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnTestConnection_Click(object sender, EventArgs e)
        {
            // 1. Pull connection details from Web.config
            string connStr = ConfigurationManager.ConnectionStrings["OracleDbConn"].ConnectionString;

            using (OracleConnection conn = new OracleConnection(connStr))
            {
                try
                {
                    // 2. Attempt to open a live channel to Oracle
                    conn.Open();

                    // 3. Connection succeeded!
                    lblError.Text = "✅ Database connected successfully! Ready to go.";
                    lblError.ForeColor = System.Drawing.Color.LightGreen;
                    lblError.Visible = true;

                    conn.Close();
                }
                catch (OracleException ex)
                {
                    // 4. Trace the exact Oracle error code (e.g., ORA-12154, ORA-01017)
                    lblError.Text = $"❌ Connection Failed! Oracle Error: {ex.Message} (Code: {ex.Number})";
                    lblError.ForeColor = System.Drawing.Color.Red;
                    lblError.Visible = true;
                }
                catch (Exception ex)
                {
                    // 5. Catch structural C# framework/driver exceptions
                    lblError.Text = $"❌ General Error: {ex.Message}";
                    lblError.ForeColor = System.Drawing.Color.Red;
                    lblError.Visible = true;
                }
            }
        }
    }
}