<%@ Page Title="Verify Certificate" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="VerifyCertificate.aspx.cs" Inherits="DigitalCertSystem.VerifyCertificate" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="mx-auto" style="max-width:480px;">
        <h3 class="text-center mb-3"><i class="bi bi-shield-check text-success"></i> Verify a Certificate</h3>
        <div class="card p-4">
            <label class="form-label">Certificate Code</label>
            <asp:TextBox ID="txtCode" runat="server" CssClass="form-control mb-2" placeholder="CERT-2026-001" />
            <label class="form-label">Your Name</label>
            <asp:TextBox ID="txtVerifierName" runat="server" CssClass="form-control mb-2" placeholder="Optional" />
            <label class="form-label">Your Email</label>
            <asp:TextBox ID="txtVerifierEmail" runat="server" CssClass="form-control mb-3" placeholder="Optional" />
            <asp:Button ID="btnVerify" runat="server" Text="Verify Certificate" CssClass="btn btn-success w-100" OnClick="btnVerify_Click" />
        </div>

        <asp:Panel ID="pnlResult" runat="server" Visible="false" CssClass="card p-4 mt-3">
            <asp:Label ID="lblResultHeader" runat="server" CssClass="fw-bold d-block fs-5 mb-3" />
            <table class="table table-borderless table-sm mb-0">
                <tr><td class="text-muted">Student</td><td><asp:Label ID="lblStudent"   runat="server" /></td></tr>
                <tr><td class="text-muted">Type</td><td><asp:Label ID="lblType"         runat="server" /></td></tr>
                <tr><td class="text-muted">Department</td><td><asp:Label ID="lblDepartment" runat="server" /></td></tr>
                <tr><td class="text-muted">Institution</td><td><asp:Label ID="lblAuthority" runat="server" /></td></tr>
                <tr><td class="text-muted">Issued</td><td><asp:Label ID="lblIssueDate"  runat="server" /></td></tr>
            </table>
        </asp:Panel>
        <asp:Label ID="lblNotFound" runat="server" Visible="false"
            CssClass="alert alert-danger d-block mt-3 text-center"
            Text="No certificate found with that code." />
    </div>
</asp:Content>