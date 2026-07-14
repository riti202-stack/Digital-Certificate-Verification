<%@ Page Title="Request Certificate" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="RequestCertificate.aspx.cs" Inherits="DigitalCertSystem.RequestCertificate" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="mx-auto" style="max-width:600px;">
        <h3><i class="bi bi-send text-primary"></i> Request a Certificate</h3>
        <div class="card p-4 mt-3">
            <div class="mb-3">
                <label class="form-label">Certificate Type</label>
                <asp:DropDownList ID="ddlType" runat="server" CssClass="form-select" />
            </div>
            <div class="mb-3">
                <label class="form-label">Title / Course Name</label>
                <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" placeholder="e.g. Web Programming Lab" />
            </div>
            <div class="mb-3">
                <label class="form-label">Issuing Authority</label>
                <asp:TextBox ID="txtAuthority" runat="server" CssClass="form-control" placeholder="e.g. KUET" />
            </div>
            <div class="mb-3">
                <label class="form-label">Preferred Issue Date</label>
                <asp:TextBox ID="txtIssueDate" runat="server" CssClass="form-control" TextMode="Date" />
            </div>
            <div class="mb-3">
                <label class="form-label">Remarks</label>
                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"
                    placeholder="Why are you requesting this certificate?" />
            </div>
            <asp:Button ID="btnSubmit" runat="server" Text="Submit Request" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
            <asp:Label ID="lblMsg" runat="server" CssClass="d-block mt-2" />
        </div>
        <div class="mt-3">
            <a href="MyRequests.aspx" class="btn btn-outline-secondary btn-sm">
                <i class="bi bi-clock-history"></i> View my requests
            </a>
        </div>
    </div>
</asp:Content>