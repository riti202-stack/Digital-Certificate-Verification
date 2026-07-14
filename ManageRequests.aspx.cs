using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace DigitalCertSystem
{
    public partial class ManageRequests : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminId"] == null) { Response.Redirect("Login.aspx"); return; }
            if (!IsPostBack) LoadRequests();
        }

        private void LoadRequests()
        {
            using (var conn = DBHelper.GetConnection())
            using (var cmd = DBHelper.CreateCommand("cert_pkg.get_pending_requests", conn))
            {
                cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                using (var adapter = new OracleDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    gvRequests.DataSource = dt;
                    gvRequests.DataBind();
                }
            }
        }

        protected void gvRequests_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int requestId = Convert.ToInt32(e.CommandArgument);
            int adminId = Convert.ToInt32(Session["AdminId"]);

            int rowIndex = ((System.Web.UI.WebControls.GridViewRow)
                ((System.Web.UI.WebControls.Button)e.CommandSource).NamingContainer).RowIndex;

            if (e.CommandName == "Approve")
            {
                var txtCode = (System.Web.UI.WebControls.TextBox)gvRequests.Rows[rowIndex].FindControl("txtCode");
                if (string.IsNullOrWhiteSpace(txtCode.Text))
                {
                    lblMsg.CssClass = "alert alert-warning d-block mb-2";
                    lblMsg.Text = "Please enter a certificate code before approving.";
                    return;
                }

                using (var conn = DBHelper.GetConnection())
                using (var cmd = DBHelper.CreateCommand("cert_pkg.approve_request", conn))
                {
                    cmd.Parameters.Add("p_request_id", OracleDbType.Decimal).Value = requestId;
                    cmd.Parameters.Add("p_admin_id", OracleDbType.Decimal).Value = adminId;
                    cmd.Parameters.Add("p_code", OracleDbType.Varchar2).Value = txtCode.Text.Trim();
                    cmd.ExecuteNonQuery();
                }
                lblMsg.CssClass = "alert alert-success d-block mb-2";
                lblMsg.Text = "Request approved and certificate issued.";
            }
            else if (e.CommandName == "Reject")
            {
                using (var conn = DBHelper.GetConnection())
                using (var cmd = DBHelper.CreateCommand("cert_pkg.reject_request", conn))
                {
                    cmd.Parameters.Add("p_request_id", OracleDbType.Decimal).Value = requestId;
                    cmd.Parameters.Add("p_admin_id", OracleDbType.Decimal).Value = adminId;
                    cmd.ExecuteNonQuery();
                }
                lblMsg.CssClass = "alert alert-danger d-block mb-2";
                lblMsg.Text = "Request rejected.";
            }

            LoadRequests();
        }
    }
}