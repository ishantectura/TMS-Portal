<%@ Page Title="" Language="C#" MasterPageFile="~/RoleBaseMaster.master" AutoEventWireup="true" CodeFile="LRReadyForUploadingDetails.aspx.cs" Inherits="LRReadyForUploadingDetails" %>

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
            //ShowPopup();
            //setTimeout(HidePopup, 2000);
        }

        //function ShowPopup() {
        //    $find('modalpopup').show();
        //    //$get('Button1').click();
        //}

        //function HidePopup() {
        //    $find('modalpopup').hide();
        //    //$get('btnCancel').click();
        //}
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
    <asp:Panel ID="h4Trans" runat="server" Visible="false">
        <h4 style="visibility: visible">LR Ready for Uploading</h4>
    </asp:Panel>
    <asp:Panel runat="server" ID="h4Third" Visible="false">
        <h4 style="visibility: visible">LR Ready to be Verified</h4>
    </asp:Panel>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <table class="table table-striped" style="width: 85%; border: 1px; border-color: black;">
                <tbody>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="LR No. :"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblLRNo"></asp:Label></td>
                        <td></td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="LR Date :"></asp:Label></td>
                        <td>
                            <asp:Label runat="server" ID="lblLRDate"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label21" runat="server" Text="From"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblFrom"></asp:Label></td>
                        <td></td>
                        <td>
                            <asp:Label ID="Label23" runat="server" Text="To"></asp:Label></td>
                        <td>
                            <asp:Label runat="server" ID="lblTo"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Transpoter Type :"></asp:Label></td>
                        <td >
                            <asp:Label ID="lblTransporterType" runat="server"></asp:Label></td>
                        <td></td>
                        <td><asp:Label   runat="server" Text="Distance :"></asp:Label></td>
                        <td> <asp:Label ID="lbldistance" runat="server"></asp:Label></td>
                    </tr>

                  
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Transporter Code :">  </asp:Label></td>
                        <td>
                            <asp:Label runat="server" ID="lblTransporterCode"></asp:Label></td>
                        <td></td>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Transporter Name"></asp:Label></td>
                        <td>
                            <asp:Label runat="server" ID="lblTransportName"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label16" runat="server" Text="Truck No ">  </asp:Label></td>
                        <td>
                            <asp:Label runat="server" ID="lblTruckNo"></asp:Label></td>
                        <td></td>
                        <td>
                            <asp:Label ID="Label25" runat="server" Text="Truck Size "></asp:Label></td>
                        <td>
                            <asp:Label runat="server" ID="lblSize"></asp:Label></td>
                    </tr>

                     <%--Edited by vijay--%>
                    <tr>
                        <td>
                            <asp:Label ID="Label18" runat="server" Text="Expected Delivery Date :">  </asp:Label>

                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblExpDate"></asp:Label>

                        </td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>


                    <%--<tr>
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="Arrival Time :">  </asp:Label></td>
                        <td>
                            <asp:Label runat="server" ID="lblArrivalTime"></asp:Label></td>

                        <td>
                            <asp:Label ID="Label10" runat="server" Text="Departure Time :">  </asp:Label></td>
                        <td>
                            <asp:Label runat="server" ID="lblDepartureTime"></asp:Label></td>
                    </tr>--%>
                    <%--<tr>
                        

                        <td>
                            <asp:Label ID="Label6" runat="server" Text="Truck No. :"></asp:Label></td>
                        <td>
                            <asp:Label runat="server" ID="lblTruckNo"></asp:Label></td>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text=" Truck Size :"></asp:Label></td>
                        <td>
                            <asp:Label runat="server" ID="lblSize"></asp:Label></td>
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
                    </tr>--%>

                    <tr>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Detention(+)" Font-Bold="true"></asp:Label></td>
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="Date" Font-Bold="true"></asp:Label></td>
                        <td></td>
                        <td>
                            <asp:Label ID="Label7" runat="server" Text="Time" Font-Bold="true"></asp:Label></td>
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="Detention Days" Font-Bold="true"></asp:Label></td>
                    </tr>

                    <tr>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="Gate In :"></asp:Label><asp:RequiredFieldValidator runat="server" ID="req1" ControlToValidate="txtGateInDate" ErrorMessage="Please fill" ForeColor="Red" ValidationGroup="btnsubmitgrp" /></td>
                        <td>

                            <asp:TextBox ID="txtGateInDate" Width="95px" Font-Bold="false" runat="server"   />
                            <asp:ImageButton runat="server" ID="ImageButton5" ImageUrl="App_Themes/img/Calendar.png" />
                            <cc2:CalendarExtender ID="CalendarExtender5" EndDate="<%# DateTime.Now %>" Format="dd-MM-yyyy" runat="server" TargetControlID="txtGateInDate"
                                PopupButtonID="ImageButton5" Enabled="false" />



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
                        <td></td>
                        <td style="text-align: center; vertical-align: middle">




                            <MKB:TimeSelector ID="TimeSelector1" runat="server" DisplaySeconds="false" Style="height: 60px;">
                            </MKB:TimeSelector>

                            <%--<MKB:TimeSelector ID="txtGateInTime" runat="server" DisplaySeconds="false" Style="height: 30px;" >
                            </MKB:TimeSelector>--%>
                            <%--<MKB:TimeSelector ID="TimeSelector1" runat="server" DisplaySeconds="false" Style="height: 30px;"></MKB:TimeSelector>--%>

                            <%-- Minute="<%# System.DateTime.Now.Minute %>" Hour="<%# System.DateTime.Now.Hour %>" AmPm="<%# DateTime.Today.ToString("tt", System.Globalization.CultureInfo.InvariantCulture) %>"--%>

                        </td>
                        <td style="text-align: center; vertical-align: middle"></td>
                    </tr>

                    <tr>
                        <td>
                            <asp:Label ID="Label14" runat="server" Text="Gate Out :"></asp:Label><asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtGateOutDate" ErrorMessage="Please fill" ForeColor="Red" ValidationGroup="btnsubmitgrp" /></td>
                        <td>

                            <asp:TextBox ID="txtGateOutDate"  Width="95px" runat="server" />
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
                        <td></td>
                        <td>
                            <MKB:TimeSelector ID="TimeSelector3" runat="server" DisplaySeconds="false" Style="height: 30px;" Visible="false">
                            </MKB:TimeSelector>
                            <MKB:TimeSelector ID="TimeSelector2" runat="server" DisplaySeconds="false">
                            </MKB:TimeSelector>
                            <%--<MKB:TimeSelector ID="txtGateOutTime" runat="server" DisplaySeconds="false"  Style="height: 30px;">
                            </MKB:TimeSelector>--%>
                            <%--<MKB:TimeSelector ID="TimeSelector1" runat="server" DisplaySeconds="false" Style="height: 30px;"></MKB:TimeSelector>--%>

                        </td>
                        <td style="text-align: center; vertical-align: middle">
                            <asp:Label ID="lblDetGateInGateOut" runat="server"></asp:Label></td>
                    </tr>
                    <%-- <tr>
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
                    </tr><tr>--%>
                    <td>

                        <asp:Label ID="Label17" runat="server" Text="Gate Out No :"></asp:Label><asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtBreweryGateOutwardNo" ErrorMessage="Please fill" ForeColor="Red" ValidationGroup="btnsubmitgrp" /></td>
                    <td></td>
                    <td>
                        <asp:TextBox ID="txtBreweryGateOutwardNo" runat="server"></asp:TextBox></td>
                    </td><td></td>
                    <td></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label19" runat="server" Text="Truck reported Date :"></asp:Label><asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtTruckReportedDate" ErrorMessage="Please fill" ForeColor="Red" ValidationGroup="btnsubmitgrp" />

                        </td>
                        <td>
                            <asp:TextBox ID="txtTruckReportedDate"  Width="95px" runat="server" />
                            <asp:ImageButton runat="server" ID="ImageButton4" ImageUrl="App_Themes/img/Calendar.png" />
                            <cc2:CalendarExtender ID="CalendarExtender4" EndDate="<%# DateTime.Now %>" Format="dd-MM-yyyy" runat="server" TargetControlID="txtTruckReportedDate"
                                PopupButtonID="ImageButton4" />

                        </td>
                        <td></td>
                        <td>
                            <MKB:TimeSelector ID="txtTruckReportedTime" runat="server" DisplaySeconds="false"></MKB:TimeSelector>

                        </td>
                        <td style="vertical-align: middle; text-align: center">

                            <asp:Label ID="lblDetTruckRepTruckUn" runat="server"></asp:Label>

                        </td>

                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label12" runat="server" Text="Truck Unloaded Date :"></asp:Label><asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="txtTruckUnloadDate" ErrorMessage="Please fill" ForeColor="Red" ValidationGroup="btnsubmitgrp" /></td>
                        <td>
                            <asp:TextBox ID="txtTruckUnloadDate"  Width="95px" runat="server" />
                            <asp:ImageButton runat="server" ID="ImageButton2" ImageUrl="App_Themes/img/Calendar.png" />
                            <cc2:CalendarExtender ID="CalendarExtender2" EndDate="<%# DateTime.Now %>" Format="dd-MM-yyyy" runat="server" TargetControlID="txtTruckUnloadDate"
                                PopupButtonID="ImageButton2" />

                        </td>
                        <td></td>
                        <td>
                            <MKB:TimeSelector ID="txtTruckUnloadedTime" runat="server" DisplaySeconds="false"></MKB:TimeSelector>
                        </td>
                        <td></td>
                    </tr>



                    <tr>
                        <td>
                            <asp:Label ID="Label10" runat="server" Font-Bold="true" Text="Approval Status "></asp:Label></td>
                        <td></td>
                        <td>
                            <asp:Label ID="Label27" runat="server" Font-Bold="true" Text="Expense Details "></asp:Label></td>
                        <td>
                            <asp:Label ID="Label28" runat="server" Font-Bold="true" Text="Amount "></asp:Label></td>
                        <td>
                            <asp:Label ID="Label29" runat="server" Font-Bold="true" Text="Verified"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlApprovalStatus" runat="server" Enabled="false">
                            </asp:DropDownList></td>
                        <td></td>
                        <td>
                            <asp:Label runat="server" ID="lblFreight" Text="Freight(+)"></asp:Label></td>
                        <td>
                            <asp:HiddenField ID="IDFreight" runat="server" Visible="false" />
                            <asp:Label runat="server" ID="lblFreightvalue"></asp:Label></td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkfrght" /></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td>
                            <asp:Label ID="lblloadingcharrges" runat="server" Text="Loading Charges(+)"></asp:Label></td>
                        <td>
                            <asp:HiddenField ID="IDLoadingCharge" runat="server" Visible="false" />
                            <asp:TextBox runat="server" ID="txtLoadingCharge"></asp:TextBox></td>
                        <td>
                            <asp:CheckBox ID="chkLoadingCharge" runat="server" AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td>
                            <asp:Label ID="lblUnloadingcharges" runat="server" Text="Unloading Charges(+)"></asp:Label></td>
                        <td>
                            <asp:HiddenField ID="IDUnLoadingCharge" runat="server" Visible="false" />
                            <asp:TextBox ID="txtUnLoadingCharge" runat="server"></asp:TextBox></td>
                        <td>
                            <asp:CheckBox ID="chkUnLoadingCharge" runat="server" AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td>
                            <asp:Label ID="lblOthercharges" Text="Other Charges(+)" runat="server"></asp:Label></td>
                        <td>
                            <asp:HiddenField ID="IDothers" runat="server" Visible="false" />
                            <asp:TextBox ID="txtOthercharges" runat="server"></asp:TextBox></td>
                        <td>
                            <asp:CheckBox ID="chkOtherCharges" runat="server" AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td>
                            <asp:Label ID="lbldetentioncharges" Text="Detention Charges(+)" runat="server"></asp:Label></td>
                        <td>
                            <asp:HiddenField ID="IdDetCharge" runat="server" Visible="false" />
                            <asp:TextBox ID="txtDetentionCharges" runat="server"></asp:TextBox></td>
                        <td>
                            <asp:CheckBox ID="chkDetentionCharges" runat="server" AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>

                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <%-- <div style='overflow: scroll; width: 1000px; height: 300px; margin-top: 50px;'>--%>

    <%--<asp:Button ID="btnInvoice" CssClass="btn" Text="INVOICE" runat="server" OnClick="btnInvoice_Click" Style="float: right; margin-right: 15%; width: 10%;" />--%>
    <div class="clsParentGrid"  >
        <%--  <asp:GridView ID="grdLR" runat="server" AutoGenerateColumns="false" PagerSettings-PageButtonCount="50"
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


        </asp:GridView>--%>

        <asp:Label runat="server" Text="Deductions:-"></asp:Label><br />
        <asp:Label runat="server" Text="TRLS Details(-)"></asp:Label>
        <asp:GridView ID="grdLR" runat="server" AutoGenerateColumns="false" PagerSettings-PageButtonCount="10"
            AllowPaging="false" PageSize="4" PagerSettings-Position="Bottom" PagerSettings-Mode="NumericFirstLast"
            EmptyDataText="No entries found."  DataKeyNames="ID" OnRowDataBound="grdLR_RowDataBound" OnPageIndexChanging="grdLR_PageIndexChanging" >
            <Columns>
                <%--<asp:BoundField DataField="LineNo" HeaderText="LR Line No" HeaderStyle-Width="80px" />--%>

                <%--Edited By Vijay--%>
                <asp:BoundField DataField="ItemNo" HeaderText="Item No" HeaderStyle-Width="80px" />
                <asp:BoundField DataField="Description" HeaderText="Item Description" HeaderStyle-Width="80px" />
                <asp:BoundField DataField="UOM" HeaderText="UOM" HeaderStyle-Width="80px" />
                <asp:BoundField DataField="TransitLoss"  HeaderText="Transit Loss(Amt.)" HeaderStyle-Width="80px" ControlStyle-BackColor="Yellow" />
                <asp:BoundField DataField="AccidentalLoss" HeaderText="Accidental Loss(Amt.)" HeaderStyle-Width="80px" ControlStyle-BackColor="Yellow" />
                 <%--Edited By Vijay--%>
                <asp:BoundField DataField="Quantity" HeaderText="Cases" HeaderStyle-Width="80px" />

               
               
                <asp:BoundField DataField="ChargableQty" HeaderText="Chargeable (Qty)" HeaderStyle-Width="30px" ControlStyle-BackColor="Yellow" />
                 <asp:TemplateField HeaderText="Transit Loss(Qty)/Leakage/Breakage/Shortage" ControlStyle-BackColor="Yellow">
                     <ItemTemplate>
                        <asp:TextBox ID="txtTransitLossQty" ReadOnly="true" runat="server" Text='<%# Eval("LastEnteredTransitQty") %>' onkeypress="return isNumberKey(event)"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <%--Edited By Vijay--%>

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
    

    
    <div class="clsParentGrid"></div>
    <div class="clsParentGrid" style="border: medium">
        <%-- <table style="width:100%;border:thin" > 
            <tr>
            <td style="text-align:left;width:50%;padding-left:10px"><asp:Label runat="server" Text="Delay delivery deduction(-)" Font-Bold="true"></asp:Label></td>
            <td style="text-align:right;width:50%;padding-right:50px"><asp:Label runat="server" Text="5000" ></asp:Label></td>
            </tr>
        </table>--%>
        <table style="width: 100%">
            <tr>
                <td style="text-align: left; width: 50%; padding-left: 10px">
                    <asp:Label ID="Label13" runat="server" Text="Delay delivery deduction(-)" Font-Bold="true"></asp:Label></td>
                <td style="text-align: right; width: 40%; padding-right: 50px">
                    <asp:Label ID="lblDelaydeliverydays" runat="server"></asp:Label>
                </td>
                <td style="text-align: right; width: 40%; padding-right: 50px">
                    <asp:TextBox ID="lbldelayPenalty" runat="server" ReadOnly="true"></asp:TextBox><asp:HiddenField ID="iDdelaypenalty" runat="server" />
                </td>
                <td style="text-align: right; width: 10%">
                    <asp:CheckBox ID="chkDelayPenalty" runat="server" AutoPostBack="true" />
                </td>


            </tr>

        </table>

    </div>
    <div class="clsParentGrid"></div>
    <div class="clsParentGrid">

        <table style="width: 100%">
            <tr>

                <td style="text-align: left; width: 50%; padding-left: 10px">
                    <asp:Label ID="Label15" runat="server" Text="Billable Amount" Font-Bold="true"></asp:Label>

                </td>
                <td style="text-align: right; width: 40%; padding-right: 50px">
                    <asp:Label ID="lblBillableamnt" runat="server"></asp:Label><asp:HiddenField ID="IDTotalInvoiceValue" runat="server" />
                </td>
                <td style="text-align: right; width: 10%"></td>


            </tr>



        </table>
    </div>
    <div class="clsParentGrid">

        <asp:HiddenField ID="IDTransitLoss" runat="server" />
        <asp:HiddenField ID="IDAccidentalLosss" runat="server" />

        <asp:HiddenField ID="IDDiscount" runat="server" />
        <asp:HiddenField ID="lblTransitLossValue" runat="server" />
        <asp:HiddenField ID="lblAccidentalloss" runat="server" />
    </div>
    <div class="clsParentGrid">

        <table style="width: 100%">
            <tr>
                <td style="width: 100%">
                    <asp:Label Text="Transporter Remark" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 80%; text-align: left; padding: 0px 0px 0px 0px; margin-left: 40px;">
                    <asp:TextBox ID="txtTransporterRemark" Width="100%" Height="60px" runat="server"></asp:TextBox>
                    <%--Edited By Vijay--%>
                    <asp:RequiredFieldValidator Enabled="false" ErrorMessage="Please fill" ControlToValidate="txtTransporterRemark" runat="server" ForeColor="Red" ID="rfvTransRemarks" ValidationGroup="btnsubmitgrp" />
                    <%--<asp:RequiredFieldValidator ErrorMessage="Please fill" ControlToValidate="txtTransporterRemark" runat="server" ForeColor="Red" ID="rfvTransRemarks" ValidationGroup="btnDispute" />--%>
                </td>
                <td></td>
            </tr>
            <tr>
                <td style="width: 80%; text-align: left; padding: 0px 0px 0px 0px">
                    <asp:Label ID="Label11" Text="BPT Remark" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 80%; text-align: left; padding: 0px 0px 0px 0px">
                    <asp:TextBox ID="txtBillProcessingRemark" Width="100%" Height="60px" runat="server"></asp:TextBox>
                    <%--Edited By Vijay--%>
                    <asp:RequiredFieldValidator Enabled="false" ErrorMessage="Please fill" ControlToValidate="txtBillProcessingRemark" runat="server" ForeColor="Red" ID="rfvBillRemarks" ValidationGroup="btnsubmitgrp" />
                    <%--<asp:RequiredFieldValidator ErrorMessage="Please fill" ControlToValidate="txtBillProcessingRemark" runat="server" ForeColor="Red" ID="rfvBillRemarks" ValidationGroup="btnDispute" />--%>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <%--<cc2:ConfirmButtonExtender ID="cbe" runat="server" DisplayModalPopupID="mpe" TargetControlID="btnYes"></cc2:ConfirmButtonExtender>
    <cc2:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlPopup" TargetControlID="btnYes" OkControlID="btnYes"
        CancelControlID="btnNo" BackgroundCssClass="modalBackground">
    </cc2:ModalPopupExtender>--%>
    <%--<asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
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
    </asp:Panel>--%>

    &nbsp; &nbsp;<asp:Label ID="lblerr" runat="server" Text=""></asp:Label><br />
    <asp:Button Text="Dispute" CssClass="btn" ID="btnDispute" runat="server" Style="float: right; margin-right: 25%; width: 10%;" OnClick="btnDispute_Click" ValidationGroup="btnsubmitgrp"/> &nbsp; &nbsp;
    <asp:Button ID="btncalculate" CssClass="btn" Text="Calculate" runat="server" OnClick="btncalculate_Click" ValidationGroup="btnsubmitgrp" Style="float: right; margin-right: 15%; width: 10%;" />&nbsp; &nbsp;
    <asp:Button ID="btnSubmit" CssClass="btn" Text="Submit" runat="server" OnClick="btnSubmit_Click" ValidationGroup="btnsubmitgrp" Style="float: right; margin-right: 15%; width: 10%;" />


</asp:Content>



