using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace DigitalCertSystem
{
    public partial class RequestCertificate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["StudentId"] == null) { Response.Redirect("StudentLogin.aspx"); return; }
            if (!IsPostBack) LoadTypes();
        }

        private void LoadTypes()
        {
            using (var conn = DBHelper.GetConnection())
            using (var cmd = DBHelper.CreateCommand("cert_pkg.get_certificate_types", conn))
            {
                cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                using (var adapter = new OracleDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    ddlType.DataSource = dt;
                    ddlType.DataTextField = "TYPE_NAME";
                    ddlType.DataValueField = "TYPE_ID";
                    ddlType.DataBind();
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            using (var conn = DBHelper.GetConnection())
            using (var cmd = DBHelper.CreateCommand("cert_pkg.submit_request", conn))
            {
                cmd.Parameters.Add("p_student_id", OracleDbType.Decimal).Value = Convert.ToInt32(Session["StudentId"]);
                cmd.Parameters.Add("p_type_id", OracleDbType.Decimal).Value = Convert.ToInt32(ddlType.SelectedValue);
                cmd.Parameters.Add("p_title", OracleDbType.Varchar2).Value = txtTitle.Text.Trim();
                cmd.Parameters.Add("p_authority", OracleDbType.Varchar2).Value = txtAuthority.Text.Trim();
                cmd.Parameters.Add("p_pref_date", OracleDbType.Date).Value = string.IsNullOrEmpty(txtIssueDate.Text) ? (object)DBNull.Value : DateTime.Parse(txtIssueDate.Text);
                cmd.Parameters.Add("p_remarks", OracleDbType.Varchar2).Value = txtRemarks.Text.Trim();
                cmd.ExecuteNonQuery();
            }
            lblMsg.CssClass = "alert alert-success d-block mt-2";
            lblMsg.Text = "Request submitted successfully.";
            txtTitle.Text = txtAuthority.Text = txtIssueDate.Text = txtRemarks.Text = "";
        }
    }
}