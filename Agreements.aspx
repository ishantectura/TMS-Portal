<%@ Page Language="C#" MasterPageFile="~/RoleBaseMaster.master" AutoEventWireup="true" CodeFile="Agreements.aspx.cs" Inherits="Agreements" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h4>Agreements</h4>
    <table runat="server" visible="false" id="uploadAgreement" >
        <tr><td style="width:250px">
            Agreement Year
            </td>
            <td>
                <asp:DropDownList ID ="ddlYear" Width="100px" runat="server" >
                </asp:DropDownList>
            </td>
        </tr>
        <tr><td>&nbsp</td></tr>
        <tr><td>Transporter</td>
            <td><div class="PopupBody" style="height:200px; width:400px; border: solid; border-width:1px; overflow-x:auto;"  >
                    <asp:CheckBoxList AutoPostBack="false"  ID="cblTransporters" runat="server"></asp:CheckBoxList>
                </div></td>
        </tr>

        <tr><td>&nbsp</td></tr>
        <tr><td style="width:250px; ">
            Select Document
            </td>
            <td>
               <asp:FileUpload runat="server"  ID="flUploadAgreement" AllowMultiple="false" />
            </td>
        </tr>
         <tr><td>&nbsp</td></tr>
        <tr>
            <td>&nbsp</td>
            <td><asp:Button ID="btnUpload" Width="100px" runat="server" Text="Upload" OnClick="uploadFile_Click" /></td>
        </tr>
        
        <tr><td>&nbsp</td><td> <asp:Label ID="lblmsg" runat="server" Text="" Visible="false" /></td></tr>
        <tr><td>&nbsp</td></tr>
    </table>

       <div id="parentgrid" style="width:670px;height:320px;overflow:auto">
       <asp:GridView Width="650" ID="GridView2" runat="server" BorderStyle="Solid" RowStyle-BorderStyle="Solid" RowStyle-BorderWidth="1px" BorderWidth="1px" AutoGenerateColumns="False"  ShowHeaderWhenEmpty="true"  EmptyDataText="No Records to Display."
                    PagerSettings-PageButtonCount="10" OnPageIndexChanging="GridView2_PageIndexChanging"
            AllowPaging="True" PageSize="50" PagerSettings-Position="Bottom" PagerSettings-Mode="NumericFirstLast"
            CellPadding="4" ForeColor="#333333" GridLines="None"  OnRowCommand="GridView2_RowCommand"
           >
                      <HeaderStyle BackColor="#4a7542" Font-Bold="True" ForeColor="White" />
           <RowStyle HorizontalAlign="Center" />
             <Columns>

                 <asp:TemplateField   HeaderText="Agreement Year">
               <ItemTemplate >
                   <asp:Label ID="Label6"   runat="server" Text='<%# Eval("Year") %>' ></asp:Label>
               </ItemTemplate>
               </asp:TemplateField>

                  <asp:TemplateField   HeaderText="Transporter Name">
               <ItemTemplate >
                   <asp:Label ID="lblName"   runat="server" Text='<%# Eval("Name") %>' ></asp:Label>
               </ItemTemplate>
               </asp:TemplateField>

                   <asp:TemplateField   HeaderText="Transporter Code">
               <ItemTemplate >
                   <asp:Label ID="lblUserName"   runat="server" Text='<%# Eval("Username") %>' ></asp:Label>
               </ItemTemplate>
               </asp:TemplateField>

                 <asp:TemplateField Visible="false" HeaderText="Path">
               <ItemTemplate >
                   <asp:Label ID="Label66" runat="server" Text='<%# Eval("Path") %>' ></asp:Label>
               </ItemTemplate>
               </asp:TemplateField>

                <asp:TemplateField HeaderText="Download Here">
               <ItemTemplate >
                   <asp:Button ID="btnDownload" CommandName="Download" CommandArgument='<%# Eval("Path") %>' runat="server" Text="Download" />
               </ItemTemplate>
               </asp:TemplateField>
                

               

                    

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
</asp:Content>
