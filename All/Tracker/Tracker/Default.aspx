<%@ Page Title="Tracker" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Tracker._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="Forums">
        <div Align = "left">
                    <asp:TreeView ID="TreeView1" runat="server" Width="919px" ImageSet="Faq" PopulateOnDemand ="true">
                    <HoverNodeStyle Font-Underline="True" ForeColor="Purple" />
                    <NodeStyle Font-Names="Tahoma" Font-Size="8pt" ForeColor="DarkBlue" 
                        HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                    <ParentNodeStyle Font-Bold="False" />
                    <SelectedNodeStyle Font-Underline="True" HorizontalPadding="0px" 
                        VerticalPadding="0px" />
                        <Nodes >
                        
                        </Nodes>
                  
                </asp:TreeView>
        </div>
                  
</asp:Content>

