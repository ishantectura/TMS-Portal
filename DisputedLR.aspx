<%@ Page Title="" Language="C#" MasterPageFile="~/RoleBaseMaster.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="DisputedLR.aspx.cs" Inherits="DisputedLR" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h4>Diputed LRs</h4>
     <asp:ScriptManager ID="scriptmgr" runat="server"></asp:ScriptManager>
    

    <asp:Button ID="btnShowDetails" CssClass="btn" Text="Disputed Breakage Details" runat="server" OnClick="btnShowDetails_Click" /> &nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnUpload" CssClass="btn" Text="Upload Documents" runat="server" OnClick="btnUpload_Click" />
     &nbsp;&nbsp;&nbsp;&nbsp;  
     <asp:Button ID="btnDownload" CssClass="btn" runat="server" Text="Download" OnClick="btnDownload_Click" /> &nbsp;&nbsp;&nbsp;&nbsp;
    
     <table style="margin-left:10px; border:1px none black; width:60%; padding:20px;" >
         <tr><td>&nbsp</td></tr>
        <tr><td colspan="6"></td></tr>
        <tr style="padding:5px 5px 5px 5px">
            <td style="padding:5px" > 
                <asp:Label ID="lblLrnofilter" Text="LRNo" runat="server"></asp:Label>
            </td>
            <td style="padding:5px">
                <asp:TextBox ID="txtFilterLrno" runat="server" ></asp:TextBox>
            </td>
            <td style="padding:5px">
                <asp:Label ID="lblVehicleno" Text="Truck No" runat="server"></asp:Label>
            </td>
            <td style="padding:5px"><asp:TextBox ID="txtFillterTrcukNo" runat="server" ></asp:TextBox>
            </td>
            

            </tr>
        <tr style="padding:5px 5px 5px 5px">
            
            <td style="padding:5px"><asp:Label runat="server" Text="From date"></asp:Label></td>
            <td style="padding:5px"><asp:TextBox ID="txtFromDate" runat="server"   /> 
               <%-- OnTextChanged="txtFromDate_TextChanged" --%>
                            <asp:ImageButton runat="server" ID="ImageButton1" ImageUrl="App_Themes/img/Calendar.png" />
                            <cc2:calendarextender ID="CalendarExtender1" EndDate="<%# DateTime.Now %>" Format="dd-MM-yyyy" runat="server" TargetControlID="txtFromDate"
                                PopupButtonID="ImageButton1" />
                <asp:RequiredFieldValidator ID="rqffromdate" runat="server" ControlToValidate="txtFromDate" ErrorMessage="*" ForeColor="Red" ValidationGroup="SearchValidationGroup"></asp:RequiredFieldValidator>

            </td>
            <td style="padding:5px">
                <asp:Label ID="lbltodate" runat="server" Text="To date"> 
                </asp:Label></td>
            <td style="padding:5px">
                <asp:TextBox ID="txtToDate" runat="server"  />
                <%--  OnTextChanged="txtToDate_TextChanged"--%>
                            <asp:ImageButton runat="server" ID="ImageButton2" ImageUrl="App_Themes/img/Calendar.png" />
                            <cc2:calendarextender ID="CalendarExtender2" EndDate="<%# DateTime.Now %>" Format="dd-MM-yyyy" runat="server" TargetControlID="txtToDate"
                                PopupButtonID="ImageButton2" />
                <asp:RequiredFieldValidator ID="rqftodate" runat="server" ControlToValidate="txtToDate" ErrorMessage="*"  ForeColor="Red" ValidationGroup="SearchValidationGroup"></asp:RequiredFieldValidator>

            </td>
        </tr>
        <tr>
            <td>
                <asp:Button id="btngo" runat="server" CssClass="btn" Text="Go"  OnClick="btngo_Click" />

            </td>

        </tr>
         <tr><td>&nbsp</td></tr>
    </table>
    <div runat="server" id="uploadFileDiv" visible="false">
        <asp:FileUpload runat="server" ID="UploadImages" AllowMultiple="true" />
        <asp:Button runat="server" ID="uploadedFile" Text="Upload" OnClick="uploadFile_Click" />
        <%--<asp:Button runat="server" ID="uploadedFile" Text="Upload" OnClick="Submit1_ServerClick" />--%>
        <asp:Label ID="listofuploadedfiles" runat="server" />
    </div>

    <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
    
     <table style="width:85%;">
         <tr>
             <td style="width:100%;">
                 <asp:Button Text="ExportToExcel" ID="btnExport" OnClick="btnExport_Click" runat="server" style="float:right;" />
             </td>
         </tr>
    </table>
     <div class="clsParentGrid">
         <asp:GridView ID="grdDisputedLR" runat="server" AutoGenerateColumns="false" PagerSettings-PageButtonCount="10"
                    AllowPaging="True" PageSize="8" PagerSettings-Position="Bottom" PagerSettings-Mode="NumericFirstLast" 
                    EmptyDataText="No entries found." CssClass="clsHalfGrid" DataKeyNames="LRNo" OnPageIndexChanging="grdDisputedLR_PageIndexChanging">
              <Columns>
                  <asp:TemplateField ItemStyle-Width="40px">
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" BackColor="#EAEFE8"  />
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelectRow" runat="server" ToolTip="Select" Enabled="true" AutoPostBack="true" OnCheckedChanged="chkSelectRow_CheckedChanged"/>
                            </ItemTemplate>
                        </asp:TemplateField>

                  <%--edited by vijay--%>

                <asp:TemplateField HeaderText="S.No" ItemStyle-Width="40px">
                      <ItemTemplate>
                               <%# Container.DataItemIndex + 1 %>
                      </ItemTemplate>
                </asp:TemplateField>

                 <asp:BoundField DataField="LRNo" HeaderText="LR No" HeaderStyle-Width="80px" />
                 <asp:BoundField DataField="LRDate" HeaderText="LR Date" HeaderStyle-Width="80px" />
                 <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" HeaderStyle-Width="80px" />
                 <asp:BoundField DataField="Destination" HeaderText="Destination" HeaderStyle-Width="80px" />
                 <asp:BoundField DataField="Size" HeaderText="Size" HeaderStyle-Width="80px" />
                 <asp:BoundField DataField="TruckNo" HeaderText="TruckNo" HeaderStyle-Width="80px" />
                <%-- <asp:BoundField DataField="TransitStatus" HeaderText="Transit Status" HeaderStyle-Width="80px" />--%>
                        <%-- <asp:BoundField DataField="UserId" HeaderText="User Id" HeaderStyle-Width="80px" />--%>
                        <%--<asp:BoundField DataField="TransporterCode" HeaderText="Transporter Code" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="TransporterName" HeaderText="Transporter Name" HeaderStyle-Width="80px" />--%>
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

         <asp:GridView ID="grdDisputedLRExcel" runat="server" AutoGenerateColumns="false" visible="false"
                    EmptyDataText="No entries found." CssClass="clsHalfGrid" DataKeyNames="LRNo" >
              <Columns>
                  <asp:TemplateField ItemStyle-Width="40px">
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" BackColor="#EAEFE8"  />
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelectRow" runat="server" ToolTip="Select" Enabled="true" AutoPostBack="true" OnCheckedChanged="chkSelectRow_CheckedChanged"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                 <asp:BoundField DataField="LRNo" HeaderText="LR No" HeaderStyle-Width="80px" />
                 <asp:BoundField DataField="LRDate" HeaderText="LR Date" HeaderStyle-Width="80px" />
                 <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" HeaderStyle-Width="80px" />
                 <asp:BoundField DataField="Destination" HeaderText="Destination" HeaderStyle-Width="80px" />
                 <asp:BoundField DataField="Size" HeaderText="Size" HeaderStyle-Width="80px" />
                 <asp:BoundField DataField="TruckNo" HeaderText="TruckNo" HeaderStyle-Width="80px" />
                <%-- <asp:BoundField DataField="TransitStatus" HeaderText="Transit Status" HeaderStyle-Width="80px" />--%>
                        <%-- <asp:BoundField DataField="UserId" HeaderText="User Id" HeaderStyle-Width="80px" />--%>
                        <%--<asp:BoundField DataField="TransporterCode" HeaderText="Transporter Code" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="TransporterName" HeaderText="Transporter Name" HeaderStyle-Width="80px" />--%>
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

