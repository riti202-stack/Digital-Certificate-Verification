<%@ Page Title="Manage Students" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ManageStudents.aspx.cs" Inherits="DigitalCertSystem.ManageStudents" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h3>Manage Students</h3>
    <div class="card p-3 mb-4">
        <div class="row g-2">
            <div class="col-md-3"><asp:TextBox ID="txtName"     runat="server" CssClass="form-control" placeholder="Full name" /></div>
            <div class="col-md-3"><asp:TextBox ID="txtEmail"    runat="server" CssClass="form-control" placeholder="Email" /></div>
            <div class="col-md-2"><asp:TextBox ID="txtPhone"    runat="server" CssClass="form-control" placeholder="Phone" /></div>
            <div class="col-md-2"><asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-select" /></div>
            <div class="col-md-2"><asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="Password" /></div>
        </div>
        <div class="mt-2">
            <asp:HiddenField ID="hdnStudentId" runat="server" />
            <asp:Button ID="btnSave"  runat="server" Text="Save"  CssClass="btn btn-primary me-1" OnClick="btnSave_Click" />
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-secondary"    OnClick="btnClear_Click" CausesValidation="false" />
        </div>
        <asp:Label ID="lblMsg" runat="server" CssClass="d-block mt-2" />
    </div>

    <asp:GridView ID="gvStudents" runat="server" CssClass="table table-hover table-bordered" AutoGenerateColumns="false"
        DataKeyNames="STUDENT_ID" OnRowCommand="gvStudents_RowCommand">
        <Columns>
            <asp:BoundField DataField="STUDENT_ID"      HeaderText="ID" />
            <asp:BoundField DataField="STUDENT_NAME"    HeaderText="Name" />
            <asp:BoundField DataField="EMAIL"           HeaderText="Email" />
            <asp:BoundField DataField="PHONE"           HeaderText="Phone" />
            <asp:BoundField DataField="DEPARTMENT_NAME" HeaderText="Department" />
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:Button runat="server" Text="Edit"   CommandName="EditRow"   CommandArgument='<%# Eval("STUDENT_ID") %>' CssClass="btn btn-sm btn-warning me-1" />
                    <asp:Button runat="server" Text="Delete" CommandName="DeleteRow" CommandArgument='<%# Eval("STUDENT_ID") %>' CssClass="btn btn-sm btn-danger" OnClientClick="return confirm('Delete this student?');" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>