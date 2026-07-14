<%@ Page Title="Manage Requests" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ManageRequests.aspx.cs" Inherits="DigitalCertSystem.ManageRequests" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h3><i class="bi bi-clipboard-check text-primary"></i> Pending Certificate Requests</h3>
    <asp:Label ID="lblMsg" runat="server" CssClass="d-block mb-2" />
    <div class="card p-3">
        <asp:GridView ID="gvRequests" runat="server" CssClass="table table-hover table-bordered align-middle"
            AutoGenerateColumns="false" DataKeyNames="REQUEST_ID" OnRowCommand="gvRequests_RowCommand"
            EmptyDataText="No pending requests.">
            <Columns>
                <asp:BoundField DataField="STUDENT_NAME"        HeaderText="Student" />
                <asp:BoundField DataField="TYPE_NAME"           HeaderText="Type" />
                <asp:BoundField DataField="TITLE"               HeaderText="Title" />
                <asp:BoundField DataField="ISSUING_AUTHORITY"   HeaderText="Authority" />
                <asp:BoundField DataField="REQUESTED_DATE"      HeaderText="Requested" DataFormatString="{0:dd-MMM-yyyy}" />
                <asp:TemplateField HeaderText="Certificate Code">
                    <ItemTemplate>
                        <asp:TextBox ID="txtCode" runat="server" CssClass="form-control form-control-sm" placeholder="CERT-2026-XXX" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button runat="server" Text="Approve" CommandName="Approve" CommandArgument='<%# Eval("REQUEST_ID") %>' CssClass="btn btn-sm btn-success me-1" />
                        <asp:Button runat="server" Text="Reject"  CommandName="Reject"  CommandArgument='<%# Eval("REQUEST_ID") %>' CssClass="btn btn-sm btn-danger" OnClientClick="return confirm('Reject this request?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>