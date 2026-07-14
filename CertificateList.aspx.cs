using System;
using System.Data;
using System.Text;
using Oracle.ManagedDataAccess.Client;

namespace DigitalCertSystem
{
    public partial class CertificateList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminId"] == null) { Response.Redirect("Login.aspx"); return; }
            if (!IsPostBack) { LoadFilterOptions(); LoadAll(); }
        }

        private void LoadFilterOptions()
        {
            using (var conn = DBHelper.GetConnection())
            {
                using (var cmd = DBHelper.CreateCommand("cert_pkg.get_departments", conn))
                {
                    cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                    BindCheckList(cmd, cblDepartment, "DEPARTMENT_NAME", "DEPARTMENT_ID");
                }
                using (var cmd = DBHelper.CreateCommand("cert_pkg.get_certificate_types", conn))
                {
                    cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                    BindCheckList(cmd, cblType, "TYPE_NAME", "TYPE_ID");
                }
            }
        }

        private void BindCheckList(OracleCommand cmd, System.Web.UI.WebControls.CheckBoxList cbl,
            string textField, string valueField)
        {
            using (var adapter = new OracleDataAdapter(cmd))
            {
                var dt = new DataTable();
                adapter.Fill(dt);
                cbl.DataSource = dt;
                cbl.DataTextField = textField;
                cbl.DataValueField = valueField;
                cbl.DataBind();
            }
        }

        private void LoadAll()
        {
            using (var conn = DBHelper.GetConnection())
            using (var cmd = DBHelper.CreateCommand("cert_pkg.filter_certificates", conn))
            {
                cmd.Parameters.Add("p_dept_ids", OracleDbType.Varchar2).Value = DBNull.Value;
                cmd.Parameters.Add("p_type_ids", OracleDbType.Varchar2).Value = DBNull.Value;
                cmd.Parameters.Add("p_statuses", OracleDbType.Varchar2).Value = DBNull.Value;
                cmd.Parameters.Add("p_institution", OracleDbType.Varchar2).Value = DBNull.Value;
                cmd.Parameters.Add("p_year", OracleDbType.Varchar2).Value = DBNull.Value;
                cmd.Parameters.Add("p_from_date", OracleDbType.Date).Value = DBNull.Value;
                cmd.Parameters.Add("p_to_date", OracleDbType.Date).Value = DBNull.Value;
                cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                BindGrid(cmd);
            }
        }

        private string GetCheckedValues(System.Web.UI.WebControls.CheckBoxList cbl)
        {
            var sb = new StringBuilder();
            foreach (System.Web.UI.WebControls.ListItem item in cbl.Items)
                if (item.Selected) sb.Append(item.Value).Append(",");
            if (sb.Length > 0) sb.Length--;
            return sb.Length == 0 ? null : sb.ToString();
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            using (var conn = DBHelper.GetConnection())
            using (var cmd = DBHelper.CreateCommand("cert_pkg.filter_certificates", conn))
            {
                cmd.Parameters.Add("p_dept_ids", OracleDbType.Varchar2).Value = (object)GetCheckedValues(cblDepartment) ?? DBNull.Value;
                cmd.Parameters.Add("p_type_ids", OracleDbType.Varchar2).Value = (object)GetCheckedValues(cblType) ?? DBNull.Value;
                cmd.Parameters.Add("p_statuses", OracleDbType.Varchar2).Value = (object)GetCheckedValues(cblStatus) ?? DBNull.Value;
                cmd.Parameters.Add("p_institution", OracleDbType.Varchar2).Value = string.IsNullOrWhiteSpace(txtInstitution.Text) ? (object)DBNull.Value : txtInstitution.Text.Trim();
                cmd.Parameters.Add("p_year", OracleDbType.Varchar2).Value = (object)GetCheckedValues(cblYear) ?? DBNull.Value;
                cmd.Parameters.Add("p_from_date", OracleDbType.Date).Value = string.IsNullOrEmpty(txtFromDate.Text) ? (object)DBNull.Value : DateTime.Parse(txtFromDate.Text);
                cmd.Parameters.Add("p_to_date", OracleDbType.Date).Value = string.IsNullOrEmpty(txtToDate.Text) ? (object)DBNull.Value : DateTime.Parse(txtToDate.Text);
                cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                BindGrid(cmd);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            using (var conn = DBHelper.GetConnection())
            using (var cmd = DBHelper.CreateCommand("cert_pkg.search_certificates", conn))
            {
                cmd.Parameters.Add("p_keyword", OracleDbType.Varchar2).Value = txtSearch.Text.Trim();
                cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                BindGrid(cmd);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            foreach (System.Web.UI.WebControls.ListItem i in cblDepartment.Items) i.Selected = false;
            foreach (System.Web.UI.WebControls.ListItem i in cblType.Items) i.Selected = false;
            foreach (System.Web.UI.WebControls.ListItem i in cblStatus.Items) i.Selected = false;
            foreach (System.Web.UI.WebControls.ListItem i in cblYear.Items) i.Selected = false;
            txtInstitution.Text = txtFromDate.Text = txtToDate.Text = txtSearch.Text = "";
            LoadAll();
        }

        private void BindGrid(OracleCommand cmd)
        {
            using (var adapter = new OracleDataAdapter(cmd))
            {
                var dt = new DataTable();
                adapter.Fill(dt);
                gvCertificates.DataSource = dt;
                gvCertificates.DataBind();
            }
        }
    }
}