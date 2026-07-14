<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AdminDashboard.aspx.cs" Inherits="DigitalCertSystem.AdminDashboard" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="mb-4">Admin Dashboard</h3>
    <div class="row g-3">
        <div class="col-md-3">
            <div class="card p-3 text-center">
                <i class="bi bi-people fs-2 text-primary"></i>
                <h4 class="mt-2"><asp:Label ID="lblTotalStudents" runat="server" /></h4>
                <small class="text-muted">Total Students</small>
            </div>
        </div>
        <div class="col-md-3">
            <div class="card p-3 text-center">
                <i class="bi bi-patch-check fs-2 text-success"></i>
                <h4 class="mt-2"><asp:Label ID="lblTotalCertificates" runat="server" /></h4>
                <small class="text-muted">Total Certificates</small>
            </div>
        </div>
        <div class="col-md-3">
            <div class="card p-3 text-center">
                <i class="bi bi-exclamation-triangle fs-2 text-danger"></i>
                <h4 class="mt-2"><asp:Label ID="lblExpiredCertificates" runat="server" /></h4>
                <small class="text-muted">Expired</small>
            </div>
        </div>
        <div class="col-md-3">
            <div class="card p-3 text-center">
                <i class="bi bi-clipboard-check fs-2 text-warning"></i>
                <h4 class="mt-2"><asp:Label ID="lblPendingRequests" runat="server" /></h4>
                <small class="text-muted">Pending Requests</small>
            </div>
        </div>
    </div>
</asp:Content>