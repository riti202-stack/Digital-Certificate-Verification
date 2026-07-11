using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;




namespace DigitalCertSystem
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            using (var conn = DBHelper.GetConnection())
            using (var cmd = new OracleCommand("BEGIN :ret := cert_pkg.login_admin(:u, :p); END;", conn))
            {
                cmd.Parameters.Add("ret", OracleDbType.Decimal, System.Data.ParameterDirection.ReturnValue);
                cmd.Parameters.Add("u", OracleDbType.Varchar2, txtUsername.Text.Trim(), System.Data.ParameterDirection.Input);
                cmd.Parameters.Add("p", OracleDbType.Varchar2, txtPassword.Text.Trim(), System.Data.ParameterDirection.Input);
                cmd.ExecuteNonQuery();

                int adminId = Convert.ToInt32(cmd.Parameters["ret"].Value.ToString());
                if (adminId > 0)
                {
                    Session["AdminId"] = adminId;
                    Session["AdminUsername"] = txtUsername.Text.Trim();
                    Response.Redirect("AdminDashboard.aspx");
                }
                else
                {
                    lblMessage.Text = "Invalid username or password.";
                }
            }
        }
    }
}

