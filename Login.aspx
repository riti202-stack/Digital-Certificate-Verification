<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Digital_Certificate_Verification.Login" %>

 <asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
      <link rel="stylesheet" type="text/css" href="style/site.css" />

 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="login-wrapper">
            <div class="login-card">
                <h1 class="brand">CertVerify</h1>
                <p class="subtitle">Digital Certificate Verification System</p>

                <asp:Label ID="lblError" runat="server" CssClass="error-text" Visible="false"></asp:Label>

                <asp:Label runat="server" AssociatedControlID="txtEmail" CssClass="form-label">Email</asp:Label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-input" placeholder="you@institution.edu"></asp:TextBox>

                <asp:Label runat="server" AssociatedControlID="txtPassword" CssClass="form-label">Password</asp:Label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-input" TextMode="Password" placeholder="********"></asp:TextBox>

                <asp:Button ID="btnLogin" runat="server" Text="Sign in" CssClass="btn-primary btn-block" OnClick="btnLogin_Click" />
            </div>
        </div>
    


            </asp:Content>