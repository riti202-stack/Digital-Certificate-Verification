using System;
using Oracle.ManagedDataAccess.Client;

namespace DigitalCertSystem
{
    public partial class StudentLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            using (var conn = DBHelper.GetConnection())
            using (var cmd = new OracleCommand("BEGIN :ret := cert_pkg.login_student(:e, :p); END;", conn))
            {
                cmd.Parameters.Add("ret", OracleDbType.Decimal, System.Data.ParameterDirection.ReturnValue);
                cmd.Parameters.Add("e", OracleDbType.Varchar2, txtEmail.Text.Trim(), System.Data.ParameterDirection.Input);
                cmd.Parameters.Add("p", OracleDbType.Varchar2, txtPassword.Text.Trim(), System.Data.ParameterDirection.Input);
                cmd.ExecuteNonQuery();

                int studentId = Convert.ToInt32(cmd.Parameters["ret"].Value.ToString());
                if (studentId > 0)
                {
                    Session["StudentId"] = studentId;
                    Session["StudentName"] = txtEmail.Text.Trim();
                    Response.Redirect("RequestCertificate.aspx");
                }
                else
                {
                    lblMessage.Text = "Invalid email or password.";
                }
            }
        }
    }
}