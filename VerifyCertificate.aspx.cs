using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace DigitalCertSystem
{
    public partial class VerifyCertificate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            using (var conn = DBHelper.GetConnection())
            using (var cmd = DBHelper.CreateCommand("cert_pkg.verify_certificate", conn))
            {
                cmd.Parameters.Add("p_code", OracleDbType.Varchar2).Value = txtCode.Text.Trim();
                cmd.Parameters.Add("p_verifier_name", OracleDbType.Varchar2).Value = string.IsNullOrWhiteSpace(txtVerifierName.Text) ? (object)DBNull.Value : txtVerifierName.Text.Trim();
                cmd.Parameters.Add("p_verifier_email", OracleDbType.Varchar2).Value = string.IsNullOrWhiteSpace(txtVerifierEmail.Text) ? (object)DBNull.Value : txtVerifierEmail.Text.Trim();
                cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        pnlResult.Visible = true;
                        lblNotFound.Visible = false;

                        string status = reader["EFFECTIVE_STATUS"].ToString();
                        if (status == "Valid")
                        {
                            pnlResult.CssClass = "card p-4 mt-3 border-success";
                            lblResultHeader.Text = "✔ Certificate Valid";
                            lblResultHeader.CssClass = "fw-bold d-block fs-5 mb-3 text-success";
                        }
                        else
                        {
                            pnlResult.CssClass = "card p-4 mt-3 border-danger";
                            lblResultHeader.Text = "✘ Certificate " + status;
                            lblResultHeader.CssClass = "fw-bold d-block fs-5 mb-3 text-danger";
                        }

                        lblStudent.Text = reader["STUDENT_NAME"].ToString();
                        lblType.Text = reader["TYPE_NAME"].ToString();
                        lblDepartment.Text = reader["DEPARTMENT_NAME"] == DBNull.Value ? "N/A" : reader["DEPARTMENT_NAME"].ToString();
                        lblAuthority.Text = reader["ISSUING_AUTHORITY"].ToString();
                        lblIssueDate.Text = reader["ISSUE_DATE"] == DBNull.Value ? "N/A" : Convert.ToDateTime(reader["ISSUE_DATE"]).ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        pnlResult.Visible = false;
                        lblNotFound.Visible = true;
                    }
                }
            }
        }
    }
}