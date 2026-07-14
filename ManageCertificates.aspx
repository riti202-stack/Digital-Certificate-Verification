<%@ Page Title="Manage Certificates" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ManageCertificates.aspx.cs" Inherits="DigitalCertSystem.ManageCertificates" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h3>Manage Certificates</h3>
    <div class="card p-3 mb-4">
        <div class="row g-2">
            <div class="col-md-2"><asp:TextBox ID="txtCode"      runat="server" CssClass="form-control" placeholder="Certificate Code" /></div>
            <div class="col-md-2"><asp:DropDownList ID="ddlStudent" runat="server" CssClass="form-select" /></div>
            <div class="col-md-2"><asp:DropDownList ID="ddlType"    runat="server" CssClass="form-select" /></div>
            <div class="col-md-2"><asp:TextBox ID="txtTitle"     runat="server" CssClass="form-control" placeholder="Title" /></div>
            <div class="col-md-2"><asp:TextBox ID="txtAuthority" runat="server" CssClass="form-control" placeholder="Authority" /></div>
            <div class="col-md-2">
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select">
                    <asp:ListItem Text="Valid"   Value="Valid" />
                    <asp:ListItem Text="Expired" Value="Expired" />
                    <asp:ListItem Text="Revoked" Value="Revoked" />
                </asp:DropDownList>
            </div>
        </div>
        <div class="row g-2 mt-1">
            <div class="col-md-2">
                <label class="form-label small text-muted">Issue Date</label>
                <asp:TextBox ID="txtIssueDate"  runat="server" CssClass="form-control" TextMode="Date" />
            </div>
            <div class="col-md-2">
                <label class="form-label small text-muted">Expiry Date</label>
                <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="form-control" TextMode="Date" />
            </div>
        </div>
        <div class="mt-2">
            <asp:HiddenField ID="hdnCertId" runat="server" />
            <asp:Button ID="btnSave"  runat="server" Text="Save"  CssClass="btn btn-primary me-1" OnClick="btnSave_Click" />
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-secondary"    OnClick="btnClear_Click" CausesValidation="false" />
        </div>
        <asp:Label ID="lblMsg" runat="server" CssClass="d-block mt-2" />
    </div>

    <asp:GridView ID="gvCertificates" runat="server" CssClass="table table-hover table-bordered" AutoGenerateColumns="false"
        DataKeyNames="CERTIFICATE_ID" OnRowCommand="gvCertificates_RowCommand">
        <Columns>
            <asp:BoundField DataField="CERTIFICATE_CODE" HeaderText="Code" />
            <asp:BoundField DataField="STUDENT_NAME"     HeaderText="Student" />
            <asp:BoundField DataField="TYPE_NAME"        HeaderText="Type" />
            <asp:BoundField DataField="STATUS"           HeaderText="Status" />
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:Button runat="server" Text="Edit"   CommandName="EditRow"   CommandArgument='<%# Eval("CERTIFICATE_ID") %>' CssClass="btn btn-sm btn-warning me-1" />
                    <asp:Button runat="server" Text="Delete" CommandName="DeleteRow" CommandArgument='<%# Eval("CERTIFICATE_ID") %>' CssClass="btn btn-sm btn-danger" OnClientClick="return confirm('Delete this certificate?');" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>