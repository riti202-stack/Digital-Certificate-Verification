<%@ Page Title="Student Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="StudentLogin.aspx.cs" Inherits="DigitalCertSystem.StudentLogin" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-center">
        <div class="card p-4" style="width:380px;">
            <h4 class="text-center mb-3"><i class="bi bi-person-circle text-success"></i> Student Login</h4>
            <div class="mb-3">
                <label class="form-label">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="your@email.com" />
            </div>
            <div class="mb-3">
                <label class="form-label">Password</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter password" />
            </div>
            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-success w-100" OnClick="btnLogin_Click" />
            <asp:Label ID="lblMessage" runat="server" CssClass="text-danger d-block mt-2 text-center" />
            <hr />
            <a href="Login.aspx" class="d-block text-center small">Admin login →</a>
        </div>
    </div>
</asp:Content>