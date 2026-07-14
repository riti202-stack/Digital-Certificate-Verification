<%@ Page Title="My Requests" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="MyRequests.aspx.cs" Inherits="DigitalCertSystem.MyRequests" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h3><i class="bi bi-clock-history text-primary"></i> My Certificate Requests</h3>
    <div class="card p-3 mt-3">
        <asp:GridView ID="gvMyRequests" runat="server" CssClass="table table-hover table-bordered align-middle"
            AutoGenerateColumns="false" EmptyDataText="You have not submitted any requests yet.">
            <Columns>
                <asp:BoundField DataField="REQUEST_ID"     HeaderText="ID" />
                <asp:BoundField DataField="TYPE_NAME"      HeaderText="Type" />
                <asp:BoundField DataField="TITLE"          HeaderText="Title" />
                <asp:BoundField DataField="REQUESTED_DATE" HeaderText="Requested On" DataFormatString="{0:dd-MMM-yyyy}" />
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <span class='<%# ((DigitalCertSystem.MyRequests)Page).GetStatusBadge(Eval("STATUS").ToString()) %>'>
                            <%# Eval("STATUS") %>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <a href="RequestCertificate.aspx" class="btn btn-primary btn-sm mt-3">
        <i class="bi bi-plus-circle"></i> New Request
    </a>
</asp:Content>