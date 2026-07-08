<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Digital_Certificate_Verification._Default" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="p-8">
        <asp:Button ID="btnTestConnection" runat="server" Text="⚡ Test DB Connection" OnClick="btnTestConnection_Click" CssClass="bg-blue-600 px-4 py-2 rounded text-white" />
        <asp:Label ID="lblError" runat="server" CssClass="block text-red-500 text-sm mt-4" Visible="false"></asp:Label>
    </div>

</asp:Content>