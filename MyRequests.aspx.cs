using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace DigitalCertSystem
{
    public partial class MyRequests : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["StudentId"] == null) { Response.Redirect("StudentLogin.aspx"); return; }
            if (!IsPostBack) LoadRequests();
        }

        private void LoadRequests()
        {
            using (var conn = DBHelper.GetConnection())
            using (var cmd = DBHelper.CreateCommand("cert_pkg.get_student_requests", conn))
            {
                cmd.Parameters.Add("p_student_id", OracleDbType.Decimal).Value = Convert.ToInt32(Session["StudentId"]);
                cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                using (var adapter = new OracleDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    gvMyRequests.DataSource = dt;
                    gvMyRequests.DataBind();
                }
            }
        }

        public string GetStatusBadge(string status)
        {
            if (status == "Approved") return "badge bg-success";
            if (status == "Rejected") return "badge bg-danger";
            return "badge bg-warning text-dark";
        }
    }
}