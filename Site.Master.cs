using System;

namespace DigitalCertSystem
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        public string HomeUrl
        {
            get
            {
                if (Session["AdminId"] != null) return "AdminDashboard.aspx";
                if (Session["StudentId"] != null) return "RequestCertificate.aspx";
                return "VerifyCertificate.aspx";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminId"] != null)
            {
                phAdminNav.Visible = true;
                phLoggedIn.Visible = true;
                lblWelcome.Text = "<i class='bi bi-person-circle'></i> " + Session["AdminUsername"];
            }
            else if (Session["StudentId"] != null)
            {
                phStudentNav.Visible = true;
                phLoggedIn.Visible = true;
                lblWelcome.Text = "<i class='bi bi-person-circle'></i> " + Session["StudentName"];
            }
            else
            {
                phLoggedOut.Visible = true;
            }
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("VerifyCertificate.aspx");
        }
    }
}