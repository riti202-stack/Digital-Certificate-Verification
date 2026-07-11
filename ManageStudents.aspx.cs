using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace DigitalCertSystem
{
    public partial class ManageStudents : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminId"] == null) { Response.Redirect("Login.aspx"); return; }
            if (!IsPostBack) { LoadDepartments(); LoadStudents(); }
        }

        private void LoadDepartments()
        {
            using (var conn = DBHelper.GetConnection())
            using (var cmd = DBHelper.CreateCommand("cert_pkg.get_departments", conn))
            {
                cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                using (var adapter = new OracleDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    ddlDepartment.DataSource = dt;
                    ddlDepartment.DataTextField = "DEPARTMENT_NAME";
                    ddlDepartment.DataValueField = "DEPARTMENT_ID";
                    ddlDepartment.DataBind();
                }
            }
        }

        private void LoadStudents()
        {
            using (var conn = DBHelper.GetConnection())
            using (var cmd = DBHelper.CreateCommand("cert_pkg.get_all_students", conn))
            {
                cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                using (var adapter = new OracleDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    gvStudents.DataSource = dt;
                    gvStudents.DataBind();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (var conn = DBHelper.GetConnection())
            {
                if (string.IsNullOrEmpty(hdnStudentId.Value))
                {
                    using (var cmd = DBHelper.CreateCommand("cert_pkg.add_student", conn))
                    {
                        cmd.Parameters.Add("p_name", OracleDbType.Varchar2).Value = txtName.Text.Trim();
                        cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = txtEmail.Text.Trim();
                        cmd.Parameters.Add("p_password", OracleDbType.Varchar2).Value = txtPassword.Text.Trim();
                        cmd.Parameters.Add("p_phone", OracleDbType.Varchar2).Value = txtPhone.Text.Trim();
                        cmd.Parameters.Add("p_dept_id", OracleDbType.Decimal).Value = Convert.ToInt32(ddlDepartment.SelectedValue);
                        cmd.ExecuteNonQuery();
                        lblMsg.CssClass = "text-success d-block mt-2";
                        lblMsg.Text = "Student added successfully.";
                    }
                }
                else
                {
                    using (var cmd = DBHelper.CreateCommand("cert_pkg.update_student", conn))
                    {
                        cmd.Parameters.Add("p_student_id", OracleDbType.Decimal).Value = Convert.ToInt32(hdnStudentId.Value);
                        cmd.Parameters.Add("p_name", OracleDbType.Varchar2).Value = txtName.Text.Trim();
                        cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = txtEmail.Text.Trim();
                        cmd.Parameters.Add("p_phone", OracleDbType.Varchar2).Value = txtPhone.Text.Trim();
                        cmd.Parameters.Add("p_dept_id", OracleDbType.Decimal).Value = Convert.ToInt32(ddlDepartment.SelectedValue);
                        cmd.ExecuteNonQuery();
                        lblMsg.CssClass = "text-success d-block mt-2";
                        lblMsg.Text = "Student updated successfully.";
                    }
                }
            }
            ClearForm();
            LoadStudents();
        }

        protected void gvStudents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int studentId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditRow")
            {
                using (var conn = DBHelper.GetConnection())
                using (var cmd = DBHelper.CreateCommand("cert_pkg.get_student_by_id", conn))
                {
                    cmd.Parameters.Add("p_student_id", OracleDbType.Decimal).Value = studentId;
                    cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            hdnStudentId.Value = studentId.ToString();
                            txtName.Text = reader["STUDENT_NAME"].ToString();
                            txtEmail.Text = reader["EMAIL"].ToString();
                            txtPhone.Text = reader["PHONE"].ToString();
                            ddlDepartment.SelectedValue = reader["DEPARTMENT_ID"].ToString();
                        }
                    }
                }
            }
            else if (e.CommandName == "DeleteRow")
            {
                using (var conn = DBHelper.GetConnection())
                using (var cmd = DBHelper.CreateCommand("cert_pkg.delete_student", conn))
                {
                    cmd.Parameters.Add("p_student_id", OracleDbType.Decimal).Value = studentId;
                    cmd.ExecuteNonQuery();
                }
                LoadStudents();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e) => ClearForm();

        private void ClearForm()
        {
            hdnStudentId.Value = "";
            txtName.Text = txtEmail.Text = txtPhone.Text = txtPassword.Text = "";
        }
    }
}