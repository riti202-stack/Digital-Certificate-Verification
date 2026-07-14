using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace DigitalCertSystem
{
    public partial class AdminDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminId"] == null) { Response.Redirect("Login.aspx"); return; }
            if (!IsPostBack) LoadStats();
        }

        private void LoadStats()
        {
            using (var conn = DBHelper.GetConnection())
            using (var cmd = DBHelper.CreateCommand("cert_pkg.get_dashboard_stats", conn))
            {
                cmd.Parameters.Add("p_total_students", OracleDbType.Decimal, ParameterDirection.Output);
                cmd.Parameters.Add("p_total_certs", OracleDbType.Decimal, ParameterDirection.Output);
                cmd.Parameters.Add("p_expired_certs", OracleDbType.Decimal, ParameterDirection.Output);
                cmd.Parameters.Add("p_pending_requests", OracleDbType.Decimal, ParameterDirection.Output);
                cmd.ExecuteNonQuery();

                lblTotalStudents.Text = cmd.Parameters["p_total_students"].Value.ToString();
                lblTotalCertificates.Text = cmd.Parameters["p_total_certs"].Value.ToString();
                lblExpiredCertificates.Text = cmd.Parameters["p_expired_certs"].Value.ToString();
                lblPendingRequests.Text = cmd.Parameters["p_pending_requests"].Value.ToString();
            }
        }
    }
}