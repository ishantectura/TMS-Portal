<%@ Page Title="" Language="C#" MasterPageFile="~/RoleBaseMaster.master" AutoEventWireup="true" CodeFile="LRReadyForUploading.aspx.cs" Inherits="LRReadyForUploading" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <script type="text/javascript">
         function validate() {
             if (document.getElementById("<%=txtFromDate.ClientID%>").textContent == "") {
                 alert("from date Feild can not be blank");
                 document.getElementById("<%=txtFromDate.ClientID%>").focus();
                 return false;
             }
             return true;
         }
    </script>
    <asp:Panel ID="h4Trans" runat="server" Visible="false">
    <h4  style="visibility:visible">LR Ready for Uploading</h4> </asp:Panel>
    <asp:Panel runat="server" ID="h4Third" Visible="false">
    <h4  style="visibility:visible">LR Ready to be Verified</h4> </asp:Panel>
    <br />
    <asp:ScriptManager ID="scriptmgr" runat="server"></asp:ScriptManager>


    <asp:Button ID="btnShowDetails" CssClass="btn" Text="LR Details" runat="server" OnClick="btnShowDetails_Click" Visible="true" />
   <%-- <asp:Button id="testbtn" runat="server"  OnClick="testbtn_Click"/>--%>
    &nbsp;&nbsp;&nbsp;&nbsp;   
   <%-- <asp:Button ID="btnCalculateInvoice" CssClass="btn" Text="Calculate Invoice" runat="server" OnClick="btnCalculateInvoice_Click" />&nbsp;&nbsp;&nbsp;&nbsp;--%>
    <asp:Button ID="btnUpload" CssClass="btn" Text="Upload Documents" runat="server" OnClick="btnUpload_Click" />
     &nbsp;&nbsp;&nbsp;&nbsp;  
    <asp:Button ID="btnDownload" CssClass="btn" runat="server" Text="Download" OnClick="btnDownload_Click" /> &nbsp;&nbsp;&nbsp;&nbsp;
    <div runat="server" style="height: 31px">


    </div>
    <table style="margin-left:10px; border:1px none black; width:80%; padding:20px;" >
        <tr><td colspan="6"></td></tr>
        <tr style="padding:5px 5px 5px 5px">
            <td style="padding:5px" > 
                <asp:Label ID="lblLrnofilter" Text="LRNo" runat="server"></asp:Label>
            </td>
            <td style="padding:5px">
                <asp:TextBox ID="txtFilterLrno" runat="server" OnTextChanged="txtFilterLrno_TextChanged"></asp:TextBox>
            </td>
            <td style="padding:5px">
                <asp:Label ID="lblVehicleno" Text="Truck No" runat="server"></asp:Label>
            </td>
            <td style="padding:5px"><asp:TextBox ID="txtFillterTrcukNo" runat="server" OnTextChanged="txtFillterTrcukNo_TextChanged"></asp:TextBox>
            </td>
            <td style="padding:5px">
                <asp:Label ID="lblTransportercode" Text="Transporter Code" runat="server"></asp:Label>
            </td>
            <td style="padding:5px">
                <asp:DropDownList ID="ddlTransporterNo" runat="server"  OnSelectedIndexChanged="ddlTransporterNo_SelectedIndexChanged"  AutoPostBack="true" Width="150px">
                   
                </asp:DropDownList>

            </td>

            </tr>
        <tr style="padding:5px 5px 5px 5px">
            <td style="padding:5px"><asp:Label ID="lblCity" Text="City" runat="server"></asp:Label></td>
            <td style="padding:5px"><asp:TextBox ID="txtCity" runat="server" OnTextChanged="txtCity_TextChanged"></asp:TextBox></td>
            <td style="padding:5px"><asp:Label runat="server" Text="From date"></asp:Label></td>
            <td style="padding:5px"><asp:TextBox ID="txtFromDate" runat="server" AutoPostBack="true"  /> 
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
                <asp:TextBox ID="txtToDate" runat="server" AutoPostBack="true"  />
                <%--  OnTextChanged="txtToDate_TextChanged"--%>
                            <asp:ImageButton runat="server" ID="ImageButton2" ImageUrl="App_Themes/img/Calendar.png" />
                            <cc2:calendarextender ID="CalendarExtender2" EndDate="<%# DateTime.Now %>" Format="dd-MM-yyyy" runat="server" TargetControlID="txtToDate"
                                PopupButtonID="ImageButton2" />
                <asp:RequiredFieldValidator ID="rqftodate" runat="server" ControlToValidate="txtToDate" ErrorMessage="*"  ForeColor="Red" ValidationGroup="SearchValidationGroup"></asp:RequiredFieldValidator>

            </td>
        </tr>
        <tr>
            <td>
                <asp:Button id="btngo" runat="server" CssClass="btn" Text="Go" ValidationGroup="SearchValidationGroup" OnClick="btngo_Click"/>

            </td>

        </tr>
    </table>
   
     &nbsp;&nbsp;&nbsp;&nbsp;  
    

     

    
    <div runat="server" id="uploadFileDiv" visible="false">
        <asp:FileUpload runat="server" ID="UploadImages" AllowMultiple="true" />
        <asp:Button runat="server" ID="uploadedFile" Text="Upload" OnClick="uploadFile_Click" />
        <%--<asp:Button runat="server" ID="uploadedFile" Text="Upload" OnClick="Submit1_ServerClick" />--%>
        <asp:Label ID="listofuploadedfiles" runat="server" />
    </div>
       
    
    <br />
    <asp:Label ID="lblmsg" runat="server" Text="" Visible="false" />
    <asp:Label ID="lblmsg1" runat="server"  Visible="true" />
    <table style="width:85%;">
         <tr>
             <td style="width:100%;">
                 <asp:Button Text="ExportToExcel" ID="btnExport" OnClick="btnExport_Click" runat="server" style="float:right;" />
             </td>
         </tr>
    </table>
    <div class="clsParentGrid">
        <asp:UpdatePanel ID="updatepnl" runat="server">
            <ContentTemplate>
                <asp:GridView ID="grdPendingLR" runat="server" AutoGenerateColumns="false" PagerSettings-PageButtonCount="10"
                    AllowPaging="True" PageSize="8" PagerSettings-Position="Bottom" PagerSettings-Mode="NumericFirstLast" OnRowDataBound="grdPendingLR_RowDataBound"
                    EmptyDataText="No entries found." CssClass="clsHalfGrid" DataKeyNames="LRNo" OnPageIndexChanging="OnPaging">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="40px">
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" BackColor="#EAEFE8"  />
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelectRow" runat="server" ToolTip="Select" Enabled="true" AutoPostBack="true" OnCheckedChanged="CheckBox1_CheckedChanged"/>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%--edited by vijay--%>

                <asp:TemplateField HeaderText="S.No">
                      <ItemTemplate>
                               <%# Container.DataItemIndex + 1 %>
                      </ItemTemplate>
                </asp:TemplateField>
                         
                        <asp:BoundField DataField="LRNo" HeaderText="LR No"  HeaderStyle-Width="80px" />

                        <%--<asp:HyperLinkField DataTextField="LRNo" DataNavigateUrlFields="LRNo" DataNavigateUrlFormatString="~/LREntry.aspx?LRNo={0}"
                    HeaderText="LR No" />--%>
                        <asp:BoundField DataField="LRDate" HeaderText="LR Date" HeaderStyle-Width="80px" />
                        <%-- <asp:BoundField DataField="UserId" HeaderText="User Id" HeaderStyle-Width="80px" />--%>
                        <asp:BoundField DataField="TransporterCode" HeaderText="Transporter Code" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="TransporterName" HeaderText="Transporter Name" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="Size" HeaderText="Size" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="Destination" HeaderText="Destination" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="TruckNo" HeaderText="TruckNo" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="TransitStatus" HeaderText="Transit Status" HeaderStyle-Width="80px" />

                        <asp:BoundField DataField="ExpectedDateofDelivery" HeaderText="Expected Date of Delivery" HeaderStyle-Width="80px" />
                        

                        <%--<asp:TemplateField HeaderText="Transit Status">
                            <ItemTemplate>--%>
                                <%--<asp:Label ID="lblTransitStatus" runat="server" Text='<%# Eval("TransitStatus") %>' Visible="false" />--%>
                                <%--<asp:DropDownList ID="ddlTransitStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTransitStatus_SelectedIndexChanged">
                                    <asp:ListItem Text="" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="In-Transit" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Reached at destination" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="LR Copy Submitted" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                        <%--    <asp:BoundField DataField="UOM" HeaderText="UOM" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="Freight" HeaderText="Freight" HeaderStyle-Width="80px" />
                <asp:BoundField DataField="UnloadingCharges" HeaderText="Unloading Charges" HeaderStyle-Width="80px" />
                <asp:BoundField DataField="LoadingCharges" HeaderText="Loading Charges" HeaderStyle-Width="80px" />
                <asp:BoundField DataField="Detention" HeaderText="Detention" HeaderStyle-Width="80px" />
                <asp:BoundField DataField="Others" HeaderText="Others" HeaderStyle-Width="80px" />
                <asp:BoundField DataField="Breakage" HeaderText="Breakage" HeaderStyle-Width="80px" />
                <asp:BoundField DataField="Discount" HeaderText="Discount" HeaderStyle-Width="80px" />
                <asp:BoundField DataField="DelayPenalty" HeaderText="Delay Penalty" HeaderStyle-Width="80px" />
                <asp:BoundField DataField="InvoiceValue" HeaderText="Invoice Value" HeaderStyle-Width="80px" />--%>
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
            </ContentTemplate>

        </asp:UpdatePanel>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:GridView ID="grdPendingLRExcel" runat="server" AutoGenerateColumns="false"
                    OnRowDataBound="grdPendingLR_RowDataBound" Visible="false"
                    EmptyDataText="No entries found." CssClass="clsHalfGrid" DataKeyNames="LRNo">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="40px">
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" BackColor="#EAEFE8"  />
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelectRow" runat="server" ToolTip="Select" Enabled="true" AutoPostBack="true" OnCheckedChanged="CheckBox1_CheckedChanged"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                         
                        <asp:BoundField DataField="LRNo" HeaderText="LR No"  HeaderStyle-Width="80px" />

                        <%--<asp:HyperLinkField DataTextField="LRNo" DataNavigateUrlFields="LRNo" DataNavigateUrlFormatString="~/LREntry.aspx?LRNo={0}"
                    HeaderText="LR No" />--%>
                        <asp:BoundField DataField="LRDate" HeaderText="LR Date" HeaderStyle-Width="80px" />
                        <%-- <asp:BoundField DataField="UserId" HeaderText="User Id" HeaderStyle-Width="80px" />--%>
                        <asp:BoundField DataField="TransporterCode" HeaderText="Transporter Code" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="TransporterName" HeaderText="Transporter Name" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="Size" HeaderText="Size" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="Destination" HeaderText="Destination" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="TruckNo" HeaderText="TruckNo" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="TransitStatus" HeaderText="Transit Status" HeaderStyle-Width="80px" />

                        <asp:BoundField DataField="ExpectedDateofDelivery" HeaderText="Expected Date of Delivery" HeaderStyle-Width="80px" />
                        

                        <%--<asp:TemplateField HeaderText="Transit Status">
                            <ItemTemplate>--%>
                                <%--<asp:Label ID="lblTransitStatus" runat="server" Text='<%# Eval("TransitStatus") %>' Visible="false" />--%>
                                <%--<asp:DropDownList ID="ddlTransitStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTransitStatus_SelectedIndexChanged">
                                    <asp:ListItem Text="" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="In-Transit" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Reached at destination" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="LR Copy Submitted" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                        <%--    <asp:BoundField DataField="UOM" HeaderText="UOM" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="Freight" HeaderText="Freight" HeaderStyle-Width="80px" />
                <asp:BoundField DataField="UnloadingCharges" HeaderText="Unloading Charges" HeaderStyle-Width="80px" />
                <asp:BoundField DataField="LoadingCharges" HeaderText="Loading Charges" HeaderStyle-Width="80px" />
                <asp:BoundField DataField="Detention" HeaderText="Detention" HeaderStyle-Width="80px" />
                <asp:BoundField DataField="Others" HeaderText="Others" HeaderStyle-Width="80px" />
                <asp:BoundField DataField="Breakage" HeaderText="Breakage" HeaderStyle-Width="80px" />
                <asp:BoundField DataField="Discount" HeaderText="Discount" HeaderStyle-Width="80px" />
                <asp:BoundField DataField="DelayPenalty" HeaderText="Delay Penalty" HeaderStyle-Width="80px" />
                <asp:BoundField DataField="InvoiceValue" HeaderText="Invoice Value" HeaderStyle-Width="80px" />--%>
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
            </ContentTemplate>

        </asp:UpdatePanel>
    </div>

</asp:Content>

