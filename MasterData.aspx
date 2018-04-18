<%@ Page Title="" Language="C#" MasterPageFile="~/RoleBaseMaster.master" AutoEventWireup="true" CodeFile="MasterData.aspx.cs" Inherits="MasterData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    
    <h4>Master Data</h4>
    <table>
        <tr>
        <td> <asp:Label ID="lblMasterTable" runat="server" Text="Master Table :"></asp:Label>
    <asp:DropDownList ID="drpMastertable" runat="server" OnTextChanged="drpMastertable_TextChanged" OnSelectedIndexChanged="drpMastertable_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList><br /></td>
        <td style="text-align:center; padding-left:50px" >
            <asp:Label runat="server" ID="lblSelectCmpny" Text-="Select Company :"></asp:Label>
            <asp:DropDownList runat="server" ID="drpSelectCompany" OnSelectedIndexChanged="drpSelectCompany_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>

        </td>
        </tr>
    </table>
   
    <div style="width: 87%; height: auto; max-height: 450px; max-width: 96%; margin-top: 10px; overflow: auto;">
        
            <table style="margin-top:20px" >
                <tr style="padding:5px">
                    <td style="width:auto;padding:5px">
                        <asp:Label Text="Name  " runat="server" ID="lblName"></asp:Label><asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
                    <td style="width:auto;padding:5px" >
                        <asp:Label Text="Code  " runat="server" ID="lblCode"></asp:Label>
                        <asp:TextBox ID="txtCode" runat="server"></asp:TextBox></td>
                    <td  style="width:auto;padding:5px">
                        <asp:Label Text="City  " runat="server" ID="lblCity"></asp:Label>
                        <asp:TextBox ID="txtCity" runat="server"></asp:TextBox></td>
                    <td style="width:20%;text-align:center">
                        <asp:Button CssClass="btn" Text="Go" Width="90px" ID="btnGo" runat="server" OnClick="btnGo_Click" />
                    </td>
                </tr>
                
            </table>
        <%--Added by jyothi--%>
         <table style="width:85%;">
         <tr>
             <td style="width:100%;">
                 <asp:Button Text="ExportToExcel" ID="btnExport" OnClick="btnExport_Click" runat="server" style="float:right;" />
             </td>
         </tr>
    </table>
        <div class="clsMasterDataGrid">
            <asp:GridView ID="grdMasterTable" runat="server" AutoGenerateColumns="true" PagerSettings-PageButtonCount="10"
                AllowPaging="True" PageSize="20" PagerSettings-Position="Bottom" PagerSettings-Mode="NumericFirstLast" OnPageIndexChanging="grdMasterTable_PageIndexChanging"
                EmptyDataText="No entries found." CssClass="clsHalfGrid" DataKeyNames="">
                <Columns>
                </Columns>
                <AlternatingRowStyle CssClass="gvAlternatingRowStyle" />
                <EditRowStyle CssClass="gvEditRowStyle" />
                <FooterStyle CssClass="gvFooterStyle" />
                <HeaderStyle CssClass="gvHeaderStyle" />
                <PagerSettings Mode="NumericFirstLast"></PagerSettings>
                <PagerStyle CssClass="gvPagerStyle" HorizontalAlign="Left" />
                <RowStyle CssClass="gvRowStyle" />
                <SelectedRowStyle CssClass="gvSelectedRowStyle" />
            </asp:GridView>

             <%--Added by jyothi--%>

              <asp:GridView ID="grdMasterTableExcel" runat="server" AutoGenerateColumns="true" visible="false"
                EmptyDataText="No entries found." CssClass="clsHalfGrid" DataKeyNames="">
                <Columns>
                </Columns>
                <AlternatingRowStyle CssClass="gvAlternatingRowStyle" />
                <EditRowStyle CssClass="gvEditRowStyle" />
                <FooterStyle CssClass="gvFooterStyle" />
                <HeaderStyle CssClass="gvHeaderStyle" />
                <PagerSettings Mode="NumericFirstLast"></PagerSettings>
                <PagerStyle CssClass="gvPagerStyle" HorizontalAlign="Left" />
                <RowStyle CssClass="gvRowStyle" />
                <SelectedRowStyle CssClass="gvSelectedRowStyle" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>

