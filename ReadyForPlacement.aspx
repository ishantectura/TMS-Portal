<%@ Page Title="" Language="C#" MasterPageFile="~/RoleBaseMaster.master" AutoEventWireup="true" CodeFile="ReadyForPlacement.aspx.cs" Inherits="ReadyForPlacement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="scriptMgr" runat="server"></asp:ScriptManager>
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }

        function isNumberChar(evt) {
            var keyCode = (evt.which) ? evt.which : evt.keyCode
            if ((keyCode < 65 || keyCode > 90) && (keyCode < 97 || keyCode > 123) && keyCode != 32)
                return false;
            return true;
        }

    </script>
    <h4>Ready for Placement Requests</h4>
     <table style="margin-left:10px; border:1px none black; width:65%; padding:20px;" >
         <tr>
             <td>
                 <asp:Label ID="lblcity" runat="server" Text="City" Visible="true"></asp:Label>
             </td>
             <td>
                 <asp:TextBox ID="txtcity" runat="server" ></asp:TextBox>
             </td>
             <td>
                 <asp:Label ID="lblrequestno" runat="server" Text="Request no." Visible="true"></asp:Label>
             </td>
             <td>
                 <asp:TextBox ID="txtrequestno" runat="server"   ></asp:TextBox>
             </td>

             <td>
                 <asp:Label ID="lbltransportercode" runat="server" Text="Transporter Code" Visible="true"></asp:Label>
             </td>
             <td>
                 <asp:TextBox ID="txttransportercode" runat="server"   ></asp:TextBox>
             </td>
             <td>
                 <asp:Button ID="btnGo" runat="server" AutoPostBack="true"   Text="Go" OnClick="btnGo_Click"  ></asp:Button>
             </td>
         </tr>

     </table>
      <table style="width:85%;">
         <tr>
             <td style="width:100%;">
                 <asp:Button Text="ExportToExcel" ID="btnExport" OnClick="btnExport_Click" runat="server" style="float:right;" />
             </td>
         </tr>
    </table>
     <div class="clsParentGrid">   
         <asp:Label ID="lblerrVehicle"  runat="server"  Style="color: red"  ></asp:Label>
                
        <asp:GridView ID="grdReadyForPlacement" runat="server" AutoGenerateColumns="false" PagerSettings-PageButtonCount="10"
            AllowPaging="True" PageSize="8" PagerSettings-Position="Bottom" PagerSettings-Mode="NumericFirstLast"
            EmptyDataText="No entries found." CssClass="clsHalfGrid" DataKeyNames="RequesterId" OnPageIndexChanging="OnPaging"
            EnableViewState="true" >
            <Columns>

                <%--edited by vijay--%>

                <asp:TemplateField HeaderText="S.No">
                      <ItemTemplate>
                               <%# Container.DataItemIndex + 1 %>
                      </ItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblReq" runat="server" Text='<%# (Eval("RequestNo")) %>' ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="RequestNo" HeaderText="Req No" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="RequestDate" HeaderText="Req Date" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="RequesterId" HeaderText="Req Id"  ReadOnly ="true" Visible="false" HeaderStyle-Width="100px" />
                <%--<asp:BoundField DataField="ResponsibilityCenter" HeaderText="Responsibility Center" ReadOnly="true" HeaderStyle-Width="100px" />--%>
                <%--<asp:BoundField DataField="TransporterType" HeaderText="Transporter Type" ReadOnly="true" HeaderStyle-Width="100px" />--%>
                <%--<asp:BoundField DataField="TransporterCode" HeaderText="Transporter Code" ReadOnly="true" HeaderStyle-Width="100px" />--%>
                <asp:BoundField DataField="TransporterName" HeaderText="Transporter Name" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ReadOnly="true" HeaderStyle-Width="100px" />
                <%--<asp:BoundField DataField="TransporterPhone" HeaderText="Transporter Phone" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TransporterEMail" HeaderText="Transporter Email" ReadOnly="true" HeaderStyle-Width="100px" />--%>
                 
                <%-- <asp:BoundField DataField="BreweryName" HeaderText="Brewery Name" ReadOnly="true" HeaderStyle-Width="100px" />--%>
                 <asp:BoundField DataField="City" HeaderText="City" ReadOnly="true" HeaderStyle-Width="100px" />

                <asp:BoundField DataField="Size" HeaderText="Size" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="Destination" HeaderText="Destination" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="NoOfPermits" HeaderText="NoOfPermits" ReadOnly="true" HeaderStyle-Width="100px" />              
                 <%--<asp:BoundField DataField="TransporterResponse" ReadOnly="true" HeaderText="Transporter Response" HeaderStyle-Width="100px" />    --%>
                <%--<asp:BoundField DataField="ReasonCode" ReadOnly="true" HeaderText="Reason Code" HeaderStyle-Width="100px" />             --%>
                <%--<asp:BoundField DataField="ExpectedPlacementDate" ReadOnly="true" HeaderText="Expected Placement Date" HeaderStyle-Width="100px" />--%>
                 <%--<asp:ImageButton runat="server" ID="ImageButton2" ImageUrl="App_Themes/img/Calendar.png" />
                                            <cc2:CalendarExtender ID="calGateInDate" Format="dd-MM-yyyy" runat="server" TargetControlID="txtGateInDate"
                                                PopupButtonID="ImageButton2" /> --%>   
                
                <asp:TemplateField HeaderText="Expected Placement Date"  >
                   <%-- <HeaderStyle Width="200px" />--%>
                    <%--<ItemStyle Width="200px" />--%>
                    <ItemTemplate >
                        <asp:TextBox ID="txtExpectedPlacementDate" runat="server" Text='<%# (Eval("ExpectedPlacementDate")) %>'  onkeypress="return isNumberKey(event)"></asp:TextBox>
                        <asp:ImageButton runat="server" ID="ImageButton2" ImageUrl="App_Themes/img/Calendar.png" />
                                            <cc2:CalendarExtender ID="calGateInDate" Format="dd-MM-yyyy" runat="server" TargetControlID="txtExpectedPlacementDate"
                                                PopupButtonID="ImageButton2" />
                    </ItemTemplate>
                </asp:TemplateField>  

                <asp:TemplateField HeaderText="Transporter type">
                    <ItemTemplate>
                        <asp:Label ID="lblTransporterType" runat="server" Text='<%# Eval("TransporterType") %>' Visible="true" />
                        <%--<asp:DropDownList ID="ddlTransporterType" runat="server">
                            <asp:ListItem Text="" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Market" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Dedicated" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Canceled" Value="3"></asp:ListItem>
                        </asp:DropDownList>--%>
                        
                    </ItemTemplate>
                </asp:TemplateField>
               
                
                 <asp:BoundField DataField="PermitExpiryDate" HeaderText="Permit Expiry Date" ReadOnly="true" HeaderStyle-Width="100px" />          
                                                         
               
                 <asp:TemplateField HeaderText="Truck No.">
                    <ItemTemplate>
                        <asp:TextBox ID="txtTruckNo" runat="server" Text='<%# (Eval("TruckNo")) %>'  OnTextChanged="txtTruckNo_TextChanged"   AutoPostBack="true" ></asp:TextBox>
                        <asp:Label ID="lbltrucknovalidation" runat="server" Visible="false" ></asp:Label>
                        
                        
                    </ItemTemplate>


                     
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Driver Mobile">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDriverMobNo" runat="server" Text='<%# (Eval("DriverMobileNo")) %>'  onkeypress="return isNumberKey(event)"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                <asp:TemplateField HeaderText="Driver Name">
                    <ItemTemplate>
                        <asp:TextBox ID="txtDriverName" runat="server" Text='<%# (Eval("DriverName")) %>' onkeypress="return isNumberChar(event)"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                 <%-- <asp:TemplateField HeaderText="Transporter Remarks">
                    <ItemTemplate>
                        <asp:TextBox ID="txtTransporterRemarks" runat="server" Text='<%# (Eval("TransporterRemark")) %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Ready to Place">
                    <HeaderTemplate>
                            <asp:CheckBox ID="chkboxSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkboxSelectAll_CheckedChanged"  Text="Select All"/>
                        </HeaderTemplate>
                    
                     <ItemTemplate>
                        <asp:CheckBox ID="chkPlaced" runat="server" Checked="false"   Enabled="false"  />
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

         <br />

         <asp:GridView ID="grdReadyForPlacementExcel" runat="server" AutoGenerateColumns="false" Visible="false"
            EmptyDataText="No entries found." CssClass="clsHalfGrid" DataKeyNames="RequesterId"
            EnableViewState="true" >
            <Columns>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblReq" runat="server" Text='<%# (Eval("RequestNo")) %>' ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="RequestNo" HeaderText="Req No" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="RequestDate" HeaderText="Req Date" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="RequesterId" HeaderText="Req Id"  ReadOnly ="true" Visible="false" HeaderStyle-Width="100px" />
                <%--<asp:BoundField DataField="ResponsibilityCenter" HeaderText="Responsibility Center" ReadOnly="true" HeaderStyle-Width="100px" />--%>
                <%--<asp:BoundField DataField="TransporterType" HeaderText="Transporter Type" ReadOnly="true" HeaderStyle-Width="100px" />--%>
                <%--<asp:BoundField DataField="TransporterCode" HeaderText="Transporter Code" ReadOnly="true" HeaderStyle-Width="100px" />--%>
                <asp:BoundField DataField="TransporterName" HeaderText="Transporter Name" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ReadOnly="true" HeaderStyle-Width="100px" />
                <%--<asp:BoundField DataField="TransporterPhone" HeaderText="Transporter Phone" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TransporterEMail" HeaderText="Transporter Email" ReadOnly="true" HeaderStyle-Width="100px" />--%>
                 
                <%-- <asp:BoundField DataField="BreweryName" HeaderText="Brewery Name" ReadOnly="true" HeaderStyle-Width="100px" />--%>
                 <asp:BoundField DataField="City" HeaderText="City" ReadOnly="true" HeaderStyle-Width="100px" />

                <asp:BoundField DataField="Size" HeaderText="Size" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="Destination" HeaderText="Destination" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="NoOfPermits" HeaderText="NoOfPermits" ReadOnly="true" HeaderStyle-Width="100px" />              
                 <%--<asp:BoundField DataField="TransporterResponse" ReadOnly="true" HeaderText="Transporter Response" HeaderStyle-Width="100px" />    --%>
                <%--<asp:BoundField DataField="ReasonCode" ReadOnly="true" HeaderText="Reason Code" HeaderStyle-Width="100px" />             --%>
                <%--<asp:BoundField DataField="ExpectedPlacementDate" ReadOnly="true" HeaderText="Expected Placement Date" HeaderStyle-Width="100px" />--%>
                 <%--<asp:ImageButton runat="server" ID="ImageButton2" ImageUrl="App_Themes/img/Calendar.png" />
                                            <cc2:CalendarExtender ID="calGateInDate" Format="dd-MM-yyyy" runat="server" TargetControlID="txtGateInDate"
                                                PopupButtonID="ImageButton2" /> --%>   
                
                <asp:TemplateField HeaderText="Expected Placement Date"  >
                   <%-- <HeaderStyle Width="200px" />--%>
                    <%--<ItemStyle Width="200px" />--%>
                    <ItemTemplate >
                        <asp:TextBox ID="txtExpectedPlacementDate" runat="server" Text='<%# (Eval("ExpectedPlacementDate")) %>'  onkeypress="return isNumberKey(event)"></asp:TextBox>
                    <%--    <asp:ImageButton runat="server" ID="ImageButton2" ImageUrl="App_Themes/img/Calendar.png" />
                                            <cc2:CalendarExtender ID="calGateInDate" Format="dd-MM-yyyy" runat="server" TargetControlID="txtExpectedPlacementDate"
                                                PopupButtonID="ImageButton2" />--%>
                    </ItemTemplate>
                </asp:TemplateField>  

                <asp:TemplateField HeaderText="Transporter type">
                    <ItemTemplate>
                        <asp:Label ID="lblTransporterType" runat="server" Text='<%# Eval("TransporterType") %>' Visible="true" />
                        <%--<asp:DropDownList ID="ddlTransporterType" runat="server">
                            <asp:ListItem Text="" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Market" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Dedicated" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Canceled" Value="3"></asp:ListItem>
                        </asp:DropDownList>--%>
                        
                    </ItemTemplate>
                </asp:TemplateField>
               
                
                 <asp:BoundField DataField="PermitExpiryDate" HeaderText="Permit Expiry Date" ReadOnly="true" HeaderStyle-Width="100px" />          
                                                         
               
                 <asp:TemplateField HeaderText="Truck No.">
                    <ItemTemplate>
                        <asp:TextBox ID="txtTruckNo" runat="server" Text='<%# (Eval("TruckNo")) %>'  OnTextChanged="txtTruckNo_TextChanged"   AutoPostBack="true" ></asp:TextBox>
                        <asp:Label ID="lbltrucknovalidation" runat="server" Visible="false" ></asp:Label>
                        
                        
                    </ItemTemplate>


                     
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Driver Mobile">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDriverMobNo" runat="server" Text='<%# (Eval("DriverMobileNo")) %>'  onkeypress="return isNumberKey(event)"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                <asp:TemplateField HeaderText="Driver Name">
                    <ItemTemplate>
                        <asp:TextBox ID="txtDriverName" runat="server" Text='<%# (Eval("DriverName")) %>' onkeypress="return isNumberChar(event)"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                 <%-- <asp:TemplateField HeaderText="Transporter Remarks">
                    <ItemTemplate>
                        <asp:TextBox ID="txtTransporterRemarks" runat="server" Text='<%# (Eval("TransporterRemark")) %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Ready to Place">
                    <HeaderTemplate>
                            <asp:CheckBox ID="chkboxSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkboxSelectAll_CheckedChanged"  Text="Select All"/>
                        </HeaderTemplate>
                    
                     <ItemTemplate>
                        <asp:CheckBox ID="chkPlaced" runat="server" Checked="false"   Enabled="false"  />
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
    <br />
     <asp:Button ID="btnSubmit" CssClass="btn" Text="Submit" runat="server" OnClick="btnSubmit_Click"  Style="float: right; margin-right: 15%; width: 10%;"  />
</asp:Content>

