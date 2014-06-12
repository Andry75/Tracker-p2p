<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Topic.aspx.cs" Inherits="Tracker.Forum.Topic" %>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Forums" runat="server">
    <div Align = "left">
<asp:TreeView ID="TreeView1" runat="server" Width="763px" ImageSet="Contacts" 
        NodeIndent="10">
    <HoverNodeStyle Font-Underline="False" />
    <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" 
        HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
    <ParentNodeStyle Font-Bold="True" ForeColor="#5555DD" />
    <SelectedNodeStyle Font-Underline="True" HorizontalPadding="0px" 
        VerticalPadding="0px" />
        <Nodes>
        </Nodes>
</asp:TreeView>
</div>
</asp:Content>


