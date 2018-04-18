<%@ Page Title="" Language="C#" MasterPageFile="~/RoleBaseMaster.master" AutoEventWireup="true" CodeFile="FreightInvoice.aspx.cs" Inherits="FreightInvoice" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function isNumberDecimal(el, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            var number = el.value.split('.');
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            //just one dot (thanks ddlab)
            if (number.length > 1 && charCode == 46) {
                return false;
            }
            //get the carat position
            var caratPos = getSelectionStart(el);
            var dotPos = el.value.indexOf(".");
            if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
                return false;
            }
            return true;
        }
        function getSelectionStart(o) {
            if (o.createTextRange) {
                var r = document.selection.createRange().duplicate()
                r.moveEnd('character', o.value.length)
                if (r.text == '') return o.value.length
                return o.value.lastIndexOf(r.text)
            } else return o.selectionStart
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
    <br />
    <asp:ScriptManager ID="scm" runat="server"></asp:ScriptManager>
    <h4>Invoice Details</h4>
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
                    <asp:Label ID="Label11" runat="server" Text="Special Discount Code :"></asp:Label></td>
                <td>
                    <asp:DropDownList ID="ddlSpecialDiscount" runat="server" OnSelectedIndexChanged="ddlSpecialDiscount_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="Label12" runat="server" Visible="false" Text="Delay Penalty :"></asp:Label></td>
                <td>
                    <asp:DropDownList ID="ddlDelayPenalty" runat="server" OnSelectedIndexChanged="ddlDelayPenalty_SelectedIndexChanged" Visible="false" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label9" runat="server" Visible="false" Text="Transporter Remarks(Invoice) :"></asp:Label></td>
                <td>
                    <asp:TextBox ID="txttransporterRemark" runat="server" Enabled="false" Text="" TextMode="MultiLine" Visible="false"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="Label10" runat="server" Visible="false" Text="Bill processing Team Remark(Invoice) :"></asp:Label></td>
                <td>
                    <asp:TextBox ID="txtBillProcessTeamRemarks" Visible="false" runat="server" Enabled="false" Text="" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label7" runat="server" Visible="false" Text="Approval Status :"></asp:Label></td>
                <td>
                    <asp:DropDownList ID="ddlApprovalStatus" runat="server" Visible="false">
                    </asp:DropDownList>
                </td>
                <td></td>
                <td>
                    <asp:DropDownList ID="ddlRejectionReason" runat="server" Visible="false" Enabled="false" OnSelectedIndexChanged="ddlRejectionReason_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:Label ID="lblmandatoryReasonCode" Text="Please Select Rejection Reason !" ForeColor="Red" Visible="false" runat="server"></asp:Label>
                </td>
                </tr>


        </tbody>
    </table>
    <br />
    <div class="clsParentGrid">
        <asp:GridView ID="grdInvoice" runat="server" AutoGenerateColumns="false" PagerSettings-PageButtonCount="10"
            AllowPaging="false" PageSize="20" PagerSettings-Position="Bottom" PagerSettings-Mode="NumericFirstLast"
            EmptyDataText="No entries found - Transporter has not logged-In yet to submit entries." CssClass="clsHalfGrid" DataKeyNames="ID" OnRowDataBound="grdInvoice_RowDataBound">
            <Columns>
                <asp:BoundField DataField="Head" HeaderText="Head" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="Amount" HeaderText="Amount" HeaderStyle-Width="80px" />
                <asp:TemplateField HeaderText="Amount Confirmed (Trans)" ControlStyle-BackColor="Yellow">
                    <ItemTemplate>
                        <asp:TextBox ID="txtAmountConfirmed" runat="server" Enabled="false" Text='<%# (Eval("LastEnteredAmtByTransporter")) %>' onkeypress="return isNumberDecimal(this, event)"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                 <%--<asp:TemplateField HeaderText="Amount Confirmed (3rd Party)" ControlStyle-BackColor="#A9D08E">
                    <ItemTemplate>
                        <asp:TextBox ID="txtAmountConfirmedThirdParty" runat="server" Text='<%# (Eval("AmountConfirmedByThirdParty")) %>' onkeypress="return isNumberDecimal(this, event)"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Accept/ Reject (Bill Processing Team)" ControlStyle-Width="30px" ControlStyle-Height="30px" ControlStyle-BackColor="#A9D08E">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkVerifiedbyBill" CssClass="chkbox" Enabled="false" runat="server" Checked='<%#(Eval("VerifiedbyBillProcessingTeam").ToString()=="1"? true : false)%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="Transporter Remarks" ControlStyle-BackColor="Yellow">
                    <ItemTemplate>
                        <asp:TextBox ID="txtTransporterRemarks" runat="server" Text='<%# (Eval("TransporterRemarks")) %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>--%>
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


    <cc2:ConfirmButtonExtender ID="cbe" runat="server" DisplayModalPopupID="mpe" TargetControlID="btnYes"></cc2:ConfirmButtonExtender>
    <cc2:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlPopup" TargetControlID="btnYes" OkControlID="btnYes"
        CancelControlID="btnNo" BackgroundCssClass="modalBackground">
    </cc2:ModalPopupExtender>
    <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
        <div class="header">
            Confirmation
        </div>
        <div class="body">
            All Lines are not verified.<br />
            This will leads Invoice in to Dispute state. Do you still want to submit entries ?        
        </div>
        <div class="footer" align="right">
            <asp:Button ID="btnYes" runat="server" Text="Yes" OnClick="btnOK_Click" />
            <asp:Button ID="btnNo" runat="server" Text="No" OnClick="btnCancel_Click" />
        </div>
    </asp:Panel>
    <br />
    &nbsp; &nbsp;<asp:Label ID="lblerr" runat="server" Text=""></asp:Label><br />
    <br />
    <asp:Button ID="btnUpdate" CssClass="btn" Text="Generate Invoice" runat="server" OnClick="btnUpdate_Click" Style="float: right; margin-right: 15%; width: 10%;" />
</asp:Content>

