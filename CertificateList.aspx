<%@ Page Title="Certificate List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="CertificateList.aspx.cs" Inherits="DigitalCertSystem.CertificateList" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h3>Certificate List</h3>
    <div class="row">
        <div class="col-md-3">
            <div class="card p-3">
                <p class="fw-bold mb-1">Department</p>
                <asp:CheckBoxList ID="cblDepartment" runat="server" CssClass="mb-3" />
                <p class="fw-bold mb-1">Certificate Type</p>
                <asp:CheckBoxList ID="cblType" runat="server" CssClass="mb-3" />
                <p class="fw-bold mb-1">Status</p>
                <asp:CheckBoxList ID="cblStatus" runat="server" CssClass="mb-3">
                    <asp:ListItem Text="Valid"   Value="Valid" />
                    <asp:ListItem Text="Expired" Value="Expired" />
                    <asp:ListItem Text="Revoked" Value="Revoked" />
                </asp:CheckBoxList>
                <p class="fw-bold mb-1">Year</p>
                <asp:CheckBoxList ID="cblYear" runat="server" CssClass="mb-3">
                    <asp:ListItem Text="2026" Value="2026" />
                    <asp:ListItem Text="2025" Value="2025" />
                    <asp:ListItem Text="2024" Value="2024" />
                </asp:CheckBoxList>
                <label class="form-label fw-bold">Institution</label>
                <asp:TextBox ID="txtInstitution" runat="server" CssClass="form-control mb-3" placeholder="e.g. KUET" />
                <label class="form-label fw-bold">Issue Date From</label>
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control mb-2" TextMode="Date" />
                <label class="form-label fw-bold">Issue Date To</label>
                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control mb-3" TextMode="Date" />
                <asp:Button ID="btnApply" runat="server" Text="Apply Filters" CssClass="btn btn-primary w-100 mb-2" OnClick="btnApply_Click" />
                <asp:Button ID="btnReset" runat="server" Text="Reset"         CssClass="btn btn-secondary w-100"  OnClick="btnReset_Click" CausesValidation="false" />
            </div>
        </div>

        <div class="col-md-9">
            <div class="d-flex gap-2 mb-3">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search by certificate code or student name" />
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-outline-primary" OnClick="btnSearch_Click" />
            </div>
            <asp:GridView ID="gvCertificates" runat="server" CssClass="table table-hover table-bordered" AutoGenerateColumns="false"
                EmptyDataText="No certificates found.">
                <Columns>
                    <asp:BoundField DataField="CERTIFICATE_CODE" HeaderText="Code" />
                    <asp:BoundField DataField="STUDENT_NAME"     HeaderText="Student" />
                    <asp:BoundField DataField="DEPARTMENT_NAME"  HeaderText="Department" />
                    <asp:BoundField DataField="TYPE_NAME"        HeaderText="Type" />
                    <asp:BoundField DataField="ISSUING_AUTHORITY" HeaderText="Institution" />
                    <asp:BoundField DataField="ISSUE_DATE"       HeaderText="Issued" DataFormatString="{0:dd-MMM-yyyy}" />
                    <asp:BoundField DataField="STATUS"           HeaderText="Status" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>