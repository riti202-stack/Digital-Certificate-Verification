using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace DigitalCertSystem
{
    public partial class ManageCertificates : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminId"] == null) { Response.Redirect("Login.aspx"); return; }
            if (!IsPostBack) { LoadDropdowns(); LoadCertificates(); }
        }

        private void LoadDropdowns()
        {
            using (var conn = DBHelper.GetConnection())
            {
                using (var cmd = DBHelper.CreateCommand("cert_pkg.get_students_lookup", conn))
                {
                    cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                    BindDropdown(cmd, ddlStudent, "STUDENT_NAME", "STUDENT_ID");
                }
                using (var cmd = DBHelper.CreateCommand("cert_pkg.get_certificate_types", conn))
                {
                    cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                    BindDropdown(cmd, ddlType, "TYPE_NAME", "TYPE_ID");
                }
            }
        }

        private void BindDropdown(OracleCommand cmd, System.Web.UI.WebControls.DropDownList ddl,
            string textField, string valueField)
        {
            using (var adapter = new OracleDataAdapter(cmd))
            {
                var dt = new DataTable();
                adapter.Fill(dt);
                ddl.DataSource = dt;
                ddl.DataTextField = textField;
                ddl.DataValueField = valueField;
                ddl.DataBind();
            }
        }

        private void LoadCertificates()
        {
            using (var conn = DBHelper.GetConnection())
            using (var cmd = DBHelper.CreateCommand("cert_pkg.get_all_certificates", conn))
            {
                cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                using (var adapter = new OracleDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    gvCertificates.DataSource = dt;
                    gvCertificates.DataBind();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DateTime? issueDate = string.IsNullOrEmpty(txtIssueDate.Text) ? (DateTime?)null : DateTime.Parse(txtIssueDate.Text);
            DateTime? expiryDate = string.IsNullOrEmpty(txtExpiryDate.Text) ? (DateTime?)null : DateTime.Parse(txtExpiryDate.Text);

            using (var conn = DBHelper.GetConnection())
            {
                if (string.IsNullOrEmpty(hdnCertId.Value))
                {
                    using (var cmd = DBHelper.CreateCommand("cert_pkg.add_certificate", conn))
                    {
                        cmd.Parameters.Add("p_code", OracleDbType.Varchar2).Value = txtCode.Text.Trim();
                        cmd.Parameters.Add("p_student_id", OracleDbType.Decimal).Value = Convert.ToInt32(ddlStudent.SelectedValue);
                        cmd.Parameters.Add("p_type_id", OracleDbType.Decimal).Value = Convert.ToInt32(ddlType.SelectedValue);
                        cmd.Parameters.Add("p_title", OracleDbType.Varchar2).Value = txtTitle.Text.Trim();
                        cmd.Parameters.Add("p_issue_date", OracleDbType.Date).Value = (object)issueDate ?? DBNull.Value;
                        cmd.Parameters.Add("p_expiry_date", OracleDbType.Date).Value = (object)expiryDate ?? DBNull.Value;
                        cmd.Parameters.Add("p_authority", OracleDbType.Varchar2).Value = txtAuthority.Text.Trim();
                        cmd.Parameters.Add("p_status", OracleDbType.Varchar2).Value = ddlStatus.SelectedValue;
                        cmd.ExecuteNonQuery();
                        lblMsg.CssClass = "text-success d-block mt-2";
                        lblMsg.Text = "Certificate added successfully.";
                    }
                }
                else
                {
                    using (var cmd = DBHelper.CreateCommand("cert_pkg.update_certificate", conn))
                    {
                        cmd.Parameters.Add("p_cert_id", OracleDbType.Decimal).Value = Convert.ToInt32(hdnCertId.Value);
                        cmd.Parameters.Add("p_title", OracleDbType.Varchar2).Value = txtTitle.Text.Trim();
                        cmd.Parameters.Add("p_issue_date", OracleDbType.Date).Value = (object)issueDate ?? DBNull.Value;
                        cmd.Parameters.Add("p_expiry_date", OracleDbType.Date).Value = (object)expiryDate ?? DBNull.Value;
                        cmd.Parameters.Add("p_authority", OracleDbType.Varchar2).Value = txtAuthority.Text.Trim();
                        cmd.Parameters.Add("p_status", OracleDbType.Varchar2).Value = ddlStatus.SelectedValue;
                        cmd.ExecuteNonQuery();
                        lblMsg.CssClass = "text-success d-block mt-2";
                        lblMsg.Text = "Certificate updated successfully.";
                    }
                }
            }
            ClearForm();
            LoadCertificates();
        }

        protected void gvCertificates_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int certId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditRow")
            {
                using (var conn = DBHelper.GetConnection())
                using (var cmd = DBHelper.CreateCommand("cert_pkg.get_certificate_by_id", conn))
                {
                    cmd.Parameters.Add("p_cert_id", OracleDbType.Decimal).Value = certId;
                    cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            hdnCertId.Value = certId.ToString();
                            txtCode.Text = reader["CERTIFICATE_CODE"].ToString();
                            ddlStudent.SelectedValue = reader["STUDENT_ID"].ToString();
                            ddlType.SelectedValue = reader["TYPE_ID"].ToString();
                            txtTitle.Text = reader["TITLE"].ToString();
                            txtIssueDate.Text = reader["ISSUE_DATE"] == DBNull.Value ? "" : Convert.ToDateTime(reader["ISSUE_DATE"]).ToString("yyyy-MM-dd");
                            txtExpiryDate.Text = reader["EXPIRY_DATE"] == DBNull.Value ? "" : Convert.ToDateTime(reader["EXPIRY_DATE"]).ToString("yyyy-MM-dd");
                            txtAuthority.Text = reader["ISSUING_AUTHORITY"].ToString();
                            ddlStatus.SelectedValue = reader["STATUS"].ToString();
                        }
                    }
                }
            }
            else if (e.CommandName == "DeleteRow")
            {
                using (var conn = DBHelper.GetConnection())
                using (var cmd = DBHelper.CreateCommand("cert_pkg.delete_certificate", conn))
                {
                    cmd.Parameters.Add("p_cert_id", OracleDbType.Decimal).Value = certId;
                    cmd.ExecuteNonQuery();
                }
                LoadCertificates();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e) => ClearForm();

        private void ClearForm()
        {
            hdnCertId.Value = "";
            txtCode.Text = txtTitle.Text = txtAuthority.Text = txtIssueDate.Text = txtExpiryDate.Text = "";
        }
    }
}