<%@ Page Title="" Language="C#" MasterPageFile="~/RoleBaseMaster.master" AutoEventWireup="true" CodeFile="LREntry.aspx.cs" Inherits="LREntry" EnableViewState="true" %>


<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="MKB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
        function pageLoad() {
            ShowPopup();
            setTimeout(HidePopup, 2000);
        }

        function ShowPopup() {
            $find('modalpopup').show();
            //$get('Button1').click();
        }

        function HidePopup() {
            $find('modalpopup').hide();
            //$get('btnCancel').click();
        }
    </script>


    <style type="text/css">
        input[type=checkbox] {
            width: 18px;
            height: 18px;
        }
        /******************************************************* GATE OUT DATE***************************************************************/
        #ContentPlaceHolder1_txtGateOutDate > #ContentPlaceHolder1_txtGateOutDate_CalendarPanel {
            margin-top: -130px;
            margin-left: -225px;
        }

        #ContentPlaceHolder1_txtGateOutTime > table > tbody > tr > td {
            vertical-align: middle !important;
        }
        /******************************************************* Reached Destination Date****************************************************/
        #ContentPlaceHolder1_txtReachedDestinationDate > #ContentPlaceHolder1_txtReachedDestinationDate_CalendarPanel {
            margin-top: -130px;
            margin-left: -225px;
        }

        #ContentPlaceHolder1_txtReachedDestinationDate > table > tbody > tr > td {
            vertical-align: middle !important;
        }
        /******************************************************* Truck Received Date ********************************************************/
        /*#ContentPlaceHolder1_txtTruckReceivedDate > #ContentPlaceHolder1_txtTruckReceivedDate_CalendarPanel {
            margin-top: -130px;
            margin-left: -225px;
        }*/

        #ContentPlaceHolder1_txtTruckReceivedDate > table > tbody > tr > td {
            vertical-align: middle !important;
        }
        /******************************************************* Truck Release Date *********************************************************/
        #ContentPlaceHolder1_txtTruckReleaseDate > #ContentPlaceHolder1_txtTruckReleaseDate_CalendarPanel {
            margin-top: -130px;
            margin-left: -225px;
        }

        #ContentPlaceHolder1_txtTruckReleaseDate > table > tbody > tr > td {
            vertical-align: middle !important;
        }

        /******************************************************* Gate Out Time *********************************************************/
        input[id="ctl00$ContentPlaceHolder1$txtGateOutTime_txtHour"], input[id="ctl00$ContentPlaceHolder1$txtGateOutTime_txtMinute"], input[id="ctl00$ContentPlaceHolder1$txtGateOutTime_txtAmPm"] {
            height: 30px !important;
        }

            input[id="ctl00$ContentPlaceHolder1$txtGateOutTime_txtHour"] ~ input {
                height: 30px !important;
            }

        img[id="ctl00$ContentPlaceHolder1$txtGateOutTime_imgUp"], img[id="ctl00$ContentPlaceHolder1$txtGateOutTime_imgDown"] {
            height: 12px !important;
        }

        /******************************************************* Reached Destination Time *********************************************************/

        input[id="ctl00$ContentPlaceHolder1$txtReachedDestinationTime_txtHour"], input[id="ctl00$ContentPlaceHolder1$txtReachedDestinationTime_txtMinute"], input[id="ctl00$ContentPlaceHolder1$txtTruckReceivedTime_txtAmPm"] {
            height: 30px !important;
        }

            input[id="ctl00$ContentPlaceHolder1$txtReachedDestinationTime_txtHour"] ~ input {
                height: 30px !important;
            }

        img[id="ctl00$ContentPlaceHolder1$txtReachedDestinationTime_imgUp"], img[id="ctl00$ContentPlaceHolder1$txtReachedDestinationTime_imgDown"] {
            height: 12px !important;
        }

        /******************************************************* Truck Received Time *********************************************************/
        input[id="ctl00$ContentPlaceHolder1$txtTruckReceivedTime_txtHour"], input[id="ctl00$ContentPlaceHolder1$txtTruckReceivedTime_txtMinute"], input[id="ctl00$ContentPlaceHolder1$txtGateOutTime_txtAmPm"] {
            height: 30px !important;
        }

            input[id="ctl00$ContentPlaceHolder1$txtTruckReceivedTime_txtHour"] ~ input {
                height: 30px !important;
            }

        img[id="ctl00$ContentPlaceHolder1$txtTruckReceivedTime_imgUp"], img[id="ctl00$ContentPlaceHolder1$txtTruckReceivedTime_imgDown"] {
            height: 12px !important;
        }

        /******************************************************* Truck Released Time*********************************************************/
        input[id="ctl00$ContentPlaceHolder1$txtTruckReleaseTime_txtHour"], input[id="ctl00$ContentPlaceHolder1$txtTruckReleaseTime_txtMinute"], input[id="ctl00$ContentPlaceHolder1$txtTruckReleaseTime_txtAmPm"] {
            height: 30px !important;
        }

            input[id="ctl00$ContentPlaceHolder1$txtTruckReleaseTime_txtHour"] ~ input {
                height: 30px !important;
            }

        img[id="ctl00$ContentPlaceHolder1$txtTruckReleaseTime_imgUp"], img[id="ctl00$ContentPlaceHolder1$txtTruckReleaseTime_imgDown"] {
            height: 12px !important;
        }


        .modalBackground {
            background-color: Black;
            filter: alpha(opacity=40);
            opacity: 0.4;
        }

        .modalPopup {
            background-color: #FFFFFF;
            width: 300px;
            border: 3px solid black;
        }

            .modalPopup .header {
                background-color: #67A562; /*#2FBDF1 ;*/
                height: 30px;
                color: White;
                line-height: 30px;
                text-align: center;
                font-weight: bold;
            }

            .modalPopup .body {
                min-height: 50px;
                line-height: 30px;
                text-align: center;
                font-weight: bold;
            }

            .modalPopup .footer {
                padding: 3px;
            }

            .modalPopup .yes, .modalPopup .no {
                height: 23px;
                color: White;
                line-height: 23px;
                text-align: center;
                font-weight: bold;
                cursor: pointer;
            }

            .modalPopup .yes {
                background-color: #2FBDF1;
                border: 1px solid #0DA9D0;
            }

            .modalPopup .no {
                background-color: #9F9F9F;
                border: 1px solid #5C5C5C;
            }
    </style>


    <asp:ScriptManager ID="scm" runat="server"></asp:ScriptManager>
    <link href="App_Themes/css/CSS.css" rel="stylesheet" />
    <%-- <script src="App_Themes/js/Extension.min.js"></script>--%>
    <h4>LR Entry Details</h4>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <table class="table table-striped" style="width: 80%; border: 1px; border-color: black;">
                <tbody>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="LR No. :"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblLRNo"></asp:Label></td>

                        <td>
                            <asp:Label ID="Label2" runat="server" Text="LR Date :"></asp:Label></td>
                        <td>
                            <asp:Label runat="server" ID="lblLRDate"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Transporter Code :">  </asp:Label></td>
                        <td>
                            <asp:Label runat="server" ID="lblTransporterCode"></asp:Label></td>

                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Destination :"></asp:Label></td>
                        <td>
                            <asp:Label runat="server" ID="lblDestination"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="Arrival Time :">  </asp:Label></td>
                        <td>
                            <asp:Label runat="server" ID="lblArrivalTime"></asp:Label></td>

                        <td>
                            <asp:Label ID="Label10" runat="server" Text="Departure Time :">  </asp:Label></td>
                        <td>
                            <asp:Label runat="server" ID="lblDepartureTime"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Size :"></asp:Label></td>
                        <td>
                            <asp:Label runat="server" ID="lblSize"></asp:Label></td>

                        <td>
                            <asp:Label ID="Label6" runat="server" Text="Truck No. :"></asp:Label></td>
                        <td>
                            <asp:Label runat="server" ID="lblTruckNo"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="Transporter Remark :"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="txtTransporterRemark" Enabled="false" runat="server" Text="" TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label12" runat="server" Text="Bill Processing Team Remarks. :"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="txtBillProcessingRemark" Enabled="false" runat="server" Text="" TextMode="MultiLine"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label14" runat="server" Text="Gate Out Date :"></asp:Label></td>
                        <td>

                            <asp:TextBox ID="txtGateOutDate" runat="server" />
                            <asp:ImageButton runat="server" ID="ImageButton1" ImageUrl="App_Themes/img/Calendar.png" />
                            <cc2:CalendarExtender ID="CalendarExtender1" EndDate="<%# DateTime.Now %>" Format="dd-MM-yyyy" runat="server" TargetControlID="txtGateOutDate"
                                PopupButtonID="ImageButton1" />

                            <%-- <cc1:DatePicker ID="txtGateOutDate" runat="server" AutoPostBack="true" Width="100px" PaneWidth="150px" Culture="<%# System.Globalization.CultureInfo.InvariantCulture %>">
                                <PaneTableStyle BorderColor="#707070" BorderWidth="1px" BorderStyle="Solid" />
                                <PaneHeaderStyle BackColor="#0099FF" />
                                <TitleStyle ForeColor="White" Font-Bold="true" />
                                <NextPrevMonthStyle ForeColor="White" Font-Bold="true" />
                                <NextPrevYearStyle ForeColor="#E0E0E0" Font-Bold="true" />
                                <DayHeaderStyle BackColor="#E8E8E8" />
                                <TodayStyle BackColor="#FFFFCC" ForeColor="#000000"
                                    Font-Underline="false" BorderColor="#FFCC99" />
                                <AlternateMonthStyle BackColor="#F0F0F0"
                                    ForeColor="#707070" Font-Underline="false" />
                                <MonthStyle BackColor="" ForeColor="#000000" Font-Underline="false" />
                            </cc1:DatePicker>--%>

                        </td>
                        <td>
                            <asp:Label ID="Label13" runat="server" Text="Gate Out Time :"></asp:Label></td>
                        <td>
                            <MKB:TimeSelector ID="txtGateOutTime" runat="server" DisplaySeconds="false" SelectedTimeFormat="Twelve">
                            </MKB:TimeSelector>
                            <%-- Minute="<%# System.DateTime.Now.Minute %>" Hour="<%# System.DateTime.Now.Hour %>" AmPm="<%# DateTime.Today.ToString("tt", System.Globalization.CultureInfo.InvariantCulture) %>"--%>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label15" runat="server" Text="Reached at Destination Date :"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="txtReachedDestinationDate" runat="server" />
                            <asp:ImageButton runat="server" ID="ImageButton2" ImageUrl="App_Themes/img/Calendar.png" />
                            <cc2:CalendarExtender ID="CalendarExtender2" EndDate="<%# DateTime.Now %>" Format="dd-MM-yyyy" runat="server" TargetControlID="txtReachedDestinationDate"
                                PopupButtonID="ImageButton2" />
                        </td>
                        
                        <td>
                            <asp:Label ID="Label16" runat="server" Text="Reached at Destination Time :"></asp:Label></td>
                        <td>
                            <MKB:TimeSelector ID="txtReachedDestinationTime" runat="server" DisplaySeconds="false" Style="height: 30px;"></MKB:TimeSelector>
                        </td>
                    </tr>
                    <tr>
                        <td>

                            <asp:Label ID="Label17" runat="server" Text="Truck Received by Customer Date :"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="txtTruckReceivedDate" runat="server" />
                            <asp:ImageButton runat="server" ID="ImageButton3" ImageUrl="App_Themes/img/Calendar.png" />
                            <cc2:CalendarExtender ID="CalendarExtender3" EndDate="<%# DateTime.Now %>" Format="dd-MM-yyyy" runat="server" TargetControlID="txtTruckReceivedDate"
                                PopupButtonID="ImageButton3" />
                        </td>

                        <td>
                            <asp:Label ID="Label18" runat="server" Text="Truck Received by Customer Time :"></asp:Label></td>
                        <td>
                            <MKB:TimeSelector ID="txtTruckReceivedTime" runat="server" DisplaySeconds="false" Style="height: 30px;"></MKB:TimeSelector>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label19" runat="server" Text="Truck Release by Customer Date :"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="txtTruckReleaseDate" runat="server" />
                            <asp:ImageButton runat="server" ID="ImageButton4" ImageUrl="App_Themes/img/Calendar.png" />
                            <cc2:CalendarExtender ID="CalendarExtender4" EndDate="<%# DateTime.Now %>" Format="dd-MM-yyyy" runat="server" TargetControlID="txtTruckReleaseDate"
                                PopupButtonID="ImageButton4" />
                        </td>
                        <td>
                            <asp:Label ID="Label20" runat="server" Text="Truck Release by Customer Time :"></asp:Label></td>
                        <td>
                            <MKB:TimeSelector ID="txtTruckReleaseTime" runat="server" DisplaySeconds="false" Style="height: 30px;"></MKB:TimeSelector>
                        </td>
                    </tr>


                    <tr>
                        <td>
                            <asp:Label ID="Label7" runat="server" Text="Approval Status :"></asp:Label></td>
                        <td>
                            <asp:DropDownList ID="ddlApprovalStatus" runat="server" Enabled="false">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label11" runat="server" Text="Brewery Gate Outward No. :"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="txtBreweryGateOutwardNo" runat="server"></asp:TextBox></td>
                    </tr>                  

                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <%-- <div style='overflow: scroll; width: 1000px; height: 300px; margin-top: 50px;'>--%>
    <h4>LR Entry</h4>
    <asp:Button ID="btnInvoice" CssClass="btn" Text="INVOICE" runat="server" OnClick="btnInvoice_Click" Style="float: right; margin-right: 15%; width: 10%;" />
    <div class="clsParentGrid">
        <asp:GridView ID="grdLR" runat="server" AutoGenerateColumns="false" PagerSettings-PageButtonCount="10"
            AllowPaging="false" PageSize="4" PagerSettings-Position="Bottom" PagerSettings-Mode="NumericFirstLast"
            EmptyDataText="No entries found." CssClass="clsHalfGrid" DataKeyNames="ID" OnRowDataBound="grdLR_RowDataBound" OnPageIndexChanging="grdLR_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="LineNo" HeaderText="LR Line No" HeaderStyle-Width="80px" />
                <asp:BoundField DataField="Brand" HeaderText="Brand" HeaderStyle-Width="80px" />
                <asp:BoundField DataField="Pack" HeaderText="Pack" HeaderStyle-Width="80px" />
                <asp:BoundField DataField="ItemNo" HeaderText="Item No" HeaderStyle-Width="80px" />
                <asp:BoundField DataField="UOM" HeaderText="UOM" HeaderStyle-Width="80px" />
                <asp:BoundField DataField="TransitLoss" HeaderText="Transit Loss(Amt.)" HeaderStyle-Width="80px" ControlStyle-BackColor="Yellow" />
                <asp:BoundField DataField="AccidentalLoss" HeaderText="Accidental Loss(Amt.)" HeaderStyle-Width="80px" ControlStyle-BackColor="Yellow" />

                <asp:TemplateField HeaderText="Transit Loss(Qty)" ControlStyle-BackColor="Yellow">
                    <ItemTemplate>
                        <asp:TextBox ID="txtTransitLossQty" ReadOnly="true" runat="server" Text='<%# Eval("LastEnteredTransitQty") %>' onkeypress="return isNumberKey(event)"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Accidental Loss(Qty)" ControlStyle-BackColor="Yellow">
                    <ItemTemplate>
                        <asp:TextBox ID="txtAccidentalLossQty" ReadOnly="true" runat="server" Text='<%# Eval("LastEnteredAccidentalQty") %>' onkeypress="return isNumberKey(event)"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Verified by Bill Processing Team" ControlStyle-Width="30px" ControlStyle-Height="30px" ControlStyle-BackColor="#A9D08E">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkVerifiedbyBill" CssClass="chkbox" Enabled="false" runat="server" Checked='<%#(Eval("VerifiedbyBillProcessingTeam").ToString()=="1"? true : false) %>' />
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
    <cc2:ConfirmButtonExtender ID="cbe" runat="server" DisplayModalPopupID="mpe" TargetControlID="btnYes"></cc2:ConfirmButtonExtender>
    <cc2:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlPopup" TargetControlID="btnYes" OkControlID="btnYes"
        CancelControlID="btnNo" BackgroundCssClass="modalBackground">
    </cc2:ModalPopupExtender>
    <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
        <div class="header">
            Confirmation
        </div>
        <div class="body">
            All LR Lines are not verified. Do you still want to submit LR?         
        </div>
        <div class="footer" align="right">
            <asp:Button ID="btnYes" runat="server" Text="Yes" OnClick="btnOK_Click" />
            <asp:Button ID="btnNo" runat="server" Text="No" OnClick="btnCancel_Click" />
        </div>
    </asp:Panel>

    &nbsp; &nbsp;<asp:Label ID="lblerr" runat="server" Text=""></asp:Label><br />
    <asp:Button ID="btnSubmit" CssClass="btn" Text="Submit" runat="server" OnClick="btnSubmit_Click" Style="float: right; margin-right: 15%; width: 10%;" />

</asp:Content>

