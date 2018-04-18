<%@ Page Title="" Language="C#" MasterPageFile="~/RoleBaseMaster.master" AutoEventWireup="true" CodeFile="GateEntry.aspx.cs" Inherits="GateEntry" %>

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

        function GetHourValues() {
            var values = "";
            var listBox = document.getElementById("<%= lstHour.ClientID%>");
            for (var i = 0; i < listBox.options.length; i++) {
                if (listBox.options[i].selected) {
                    values = listBox.options[i].innerHTML;
                }
            }
            document.getElementById("<%= txthr.ClientID%>").value = values;
        }


        function GetMinuteValues() {
            var Min_values = "";
            var listBox2 = document.getElementById("<%= lstMin.ClientID%>");
            for (var i = 0; i < listBox2.options.length; i++) {
                if (listBox2.options[i].selected) {
                    Min_values = listBox2.options[i].innerHTML;
                }
            }
            document.getElementById("<%= txtmin.ClientID%>").value = Min_values;
        }

        //   function GetCalenderDate() {
        //   var calendarDate = '<= calGateInDate.SelectedDate %>'
        //    alert(calendarDate);
        //}
        //Added by Jyothi
        Sys.Application.add_load(function () {
            Sys.Extended.UI.ComboBox.prototype._ensureHighlightedIndex = function () {
                // highlight an index according to textbox value
                var textBoxValue = this.get_textBoxControl().value;
                //alert("enter");
                // first, check the current highlighted index
                if (this._highlightedIndex != null && this._highlightedIndex >= 0
                    && this._isExactMatch(this._optionListItems[this._highlightedIndex].text, textBoxValue)) {
                    return;
                }

                // need to find the correct index
                var firstMatch = -1;
                var ensured = false;
                var children = this.get_optionListControl().childNodes;

                for (var i = 0; i < this._optionListItems.length; i++) {
                    var itemText = this._optionListItems[i].text;
                    // alert(itemText);
                    //children[i].style.display = this._isPrefixMatch(itemText, textBoxValue) ? "list-item" : "none";
                    var str = itemText;
                    // alert(str.toLowerCase().indexOf(textBoxValue));
                    if (str.toLowerCase().indexOf(textBoxValue) != -1)
                        children[i].style.display = "list-item";
                    else
                        children[i].style.display = "none";
                    // children[i].style.display = this._isPrefixMatch(itemText, textBoxValue) ? "list-item" : "none";
                    //if (!ensured && this._isExactMatch(itemText, textBoxValue)) {
                    //    this._highlightListItem(i, true);
                    //    ensured = true;
                    //}
                    if (!ensured) {
                        if (str.toLowerCase().indexOf(textBoxValue) != -1) {
                            this._highlightListItem(i, true);
                            ensured = true;
                        }
                    }
                        // if in DropDownList mode, save first match.
                    else if (!ensured && firstMatch < 0 && this._highlightSuggestedItem) {
                        //if (this._isPrefixMatch(itemText, textBoxValue)) {
                        if (str.toLowerCase().indexOf(textBoxValue) != -1) {
                            firstMatch = i;
                        }
                    }
                }

                if (!ensured) {
                    this._highlightListItem(firstMatch, true);
                }
            };
        });

    </script>

    <script language="javascript" type="text/javascript">
        function getSelected(source, eventArgs) {
            if (eventArgs != undefined)
                alert(eventArgs.get_value());
            // document.getElementById('<%=txtVehicle.ClientID %>').value = eventArgs.get_value();
          }
    </script>
    <style type="text/css">
        #ContentPlaceHolder1_lstHour, #ContentPlaceHolder1_lstMin {
            height: 150px;
        }

        .ajax__calendar_container TD {
            font-size: 13px !important;
            margin: 0;
            padding: 0;
        }

        .ajax__calendar_container {
            font-size: 13px !important;
        }
            #ContentPlaceHolder1_ddlVehicle_ddlVehicle_OptionList {
            left: 317px !important;
            top: 110px !important;
            /*height: 0px !important;*/
        }
    </style>
    <h4>Gate In Entry </h4>
    <div style="width: 1000px;">

        <table class="table table-bordered">
            <tbody>
                <tr>

                    <td>Vehicle Last 4 Digits.</td>
                    <td><asp:TextBox runat="server" ID="txtdemo" AutoPostBack="true" OnTextChanged="txtdemo_TextChanged"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Vehicle No.</td>
                    <td>
                        <%--<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>--%>
                        <asp:ScriptManager ID="scriptMgr" runat="server" EnablePageMethods="true"></asp:ScriptManager>
                    <%--    <asp:TextBox ID="txtVehicle1" runat="server"></asp:TextBox>
                        <cc2:AutoCompleteExtender ServiceMethod="GetCompletionList" CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" MinimumPrefixLength="1" runat="server" ServicePath="" ID="aceVehicleNo" TargetControlID="txtVehicle1" OnClientItemSelected="getSelected()"></cc2:AutoCompleteExtender>--%>
                       
                       
                         <%--Modified by jyothi--%>
                      <%--  <asp:DropDownList ID="ddlVehicle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlVehicle_SelectedIndexChanged" ></asp:DropDownList>--%>

                        <%--Modified by jyothi--%>
                        <%-- <asp:TextBox ID="txtVehicle1" runat="server" OnTextChanged="txtVehicle1_TextChanged"></asp:TextBox>--%>
                        <cc2:ComboBox runat="server" ID="ddlVehicle" AutoPostBack="true" DropDownStyle="DropDownList" AutoCompleteMode="None" OnSelectedIndexChanged="ddlVehicle_SelectedIndexChanged"  CaseSensitive="False" CssClass="" ItemInsertLocation="Append"></cc2:ComboBox>
                    </td>
                </tr>
                <tr>
                    <td>Actual Vehicle No.</td>
                    <td>
                        <asp:TextBox ID="txtVehicle" runat="server" AutoPostBack="true" OnTextChanged="txtVehicle_TextChanged1"></asp:TextBox>
                        <asp:Label ID="lblvehicleerr" runat="server" Style="color: red"></asp:Label>

                    </td>
                </tr>
                <tr>
                    <td>Request No.</td>
                    <td>
                        <asp:DropDownList ID="ddlreqno" runat="server" OnSelectedIndexChanged="ddlreqno_SelectedIndexChanged" AutoPostBack="true" Width="153px"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Destination</td>
                    <td>
                        <asp:Label ID="lbldestination" runat="server" AutoPostBack="true" Width="153px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Name</td>
                    <td>
                        <asp:Label ID="lblName" runat="server" AutoPostBack="true" Width="153px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>City</td>
                    <td>
                        <asp:Label ID="lblCity" runat="server" AutoPostBack="true" Width="153px"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td>Transporter Name</td>
                    <td>
                        <asp:Label ID="lblTransporterType" runat="server" Text='<%# Eval("TransporterType") %>' Visible="false" />
                        <asp:Label ID="lblTransporterName" runat="server" Text='<%# Eval("TransporterName") %>' Visible="true" />
                        <%--<asp:DropDownList ID="ddlTransType" runat="server" AutoPostBack="true"  Width="153px"  >
                                <asp:ListItem Text="Market" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Dedicated" Value="1"></asp:ListItem>
                            </asp:DropDownList>--%>
                    </td>
                </tr>

                <tr>
                    <td>Brewery Gate Inward No.</td>
                    <td>
                        <asp:TextBox ID="txtGateInwardNo" runat="server" Text=""></asp:TextBox>
                        &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:RequiredFieldValidator ID="rfvGateInwardNo" runat="server" ValidationGroup="v1" ControlToValidate="txtGateInwardNo" ErrorMessage="Please Enter Gate Inward No." ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>

                </tr>
                <tr>
                    <td>Gate In Date</td>
                    <td>
                        <asp:UpdatePanel ID="uppnl" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtGateInDate" Enabled="false" runat="server" />
                                        </td>
                                        <td>&nbsp;&nbsp;
                                            <asp:ImageButton runat="server" ID="ImageButton2" ImageUrl="App_Themes/img/Calendar.png" />
                                            <cc2:CalendarExtender ID="calGateInDate" Format="dd-MM-yyyy" runat="server" TargetControlID="txtGateInDate"
                                                PopupButtonID="ImageButton2" />
                                            &nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvGateInDate" runat="server" ControlToValidate="txtGateInDate" ErrorMessage="Please Enter Gate In Date." ForeColor="Red"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>Gate In Time</td>
                    <td>
                        <table>
                            <tr>
                                <th>HH</th>
                                <th>MM</th>
                                <th></th>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListBox ID="lstHour" runat="server" CssClass="lstStyle" OnClick="GetHourValues();">
                                        <asp:ListItem Text="00" Value="00"></asp:ListItem>
                                        <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                        <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                        <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                        <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                        <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                        <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                        <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                        <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                        <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                    </asp:ListBox>
                                </td>
                                <td>&nbsp;
                         <asp:ListBox ID="lstMin" runat="server" OnClick="GetMinuteValues();">
                             <asp:ListItem Text="00" Value="00"></asp:ListItem>
                             <asp:ListItem Text="01" Value="01"></asp:ListItem>
                             <asp:ListItem Text="02" Value="02"></asp:ListItem>
                             <asp:ListItem Text="03" Value="03"></asp:ListItem>
                             <asp:ListItem Text="04" Value="04"></asp:ListItem>
                             <asp:ListItem Text="05" Value="05"></asp:ListItem>
                             <asp:ListItem Text="06" Value="06"></asp:ListItem>
                             <asp:ListItem Text="07" Value="07"></asp:ListItem>
                             <asp:ListItem Text="08" Value="08"></asp:ListItem>
                             <asp:ListItem Text="09" Value="09"></asp:ListItem>
                             <asp:ListItem Text="10" Value="10"></asp:ListItem>
                             <asp:ListItem Text="11" Value="11"></asp:ListItem>
                             <asp:ListItem Text="12" Value="12"></asp:ListItem>
                             <asp:ListItem Text="13" Value="13"></asp:ListItem>
                             <asp:ListItem Text="14" Value="14"></asp:ListItem>
                             <asp:ListItem Text="15" Value="15"></asp:ListItem>
                             <asp:ListItem Text="16" Value="16"></asp:ListItem>
                             <asp:ListItem Text="17" Value="17"></asp:ListItem>
                             <asp:ListItem Text="18" Value="18"></asp:ListItem>
                             <asp:ListItem Text="19" Value="19"></asp:ListItem>
                             <asp:ListItem Text="20" Value="20"></asp:ListItem>
                             <asp:ListItem Text="21" Value="21"></asp:ListItem>
                             <asp:ListItem Text="22" Value="22"></asp:ListItem>
                             <asp:ListItem Text="23" Value="23"></asp:ListItem>
                             <asp:ListItem Text="24" Value="24"></asp:ListItem>
                             <asp:ListItem Text="25" Value="25"></asp:ListItem>
                             <asp:ListItem Text="26" Value="26"></asp:ListItem>
                             <asp:ListItem Text="27" Value="27"></asp:ListItem>
                             <asp:ListItem Text="28" Value="28"></asp:ListItem>
                             <asp:ListItem Text="29" Value="29"></asp:ListItem>
                             <asp:ListItem Text="30" Value="30"></asp:ListItem>
                             <asp:ListItem Text="31" Value="31"></asp:ListItem>
                             <asp:ListItem Text="32" Value="32"></asp:ListItem>
                             <asp:ListItem Text="33" Value="33"></asp:ListItem>
                             <asp:ListItem Text="34" Value="34"></asp:ListItem>
                             <asp:ListItem Text="35" Value="35"></asp:ListItem>
                             <asp:ListItem Text="36" Value="36"></asp:ListItem>
                             <asp:ListItem Text="37" Value="37"></asp:ListItem>
                             <asp:ListItem Text="38" Value="38"></asp:ListItem>
                             <asp:ListItem Text="39" Value="39"></asp:ListItem>
                             <asp:ListItem Text="40" Value="40"></asp:ListItem>
                             <asp:ListItem Text="41" Value="41"></asp:ListItem>
                             <asp:ListItem Text="42" Value="42"></asp:ListItem>
                             <asp:ListItem Text="43" Value="43"></asp:ListItem>
                             <asp:ListItem Text="44" Value="44"></asp:ListItem>
                             <asp:ListItem Text="45" Value="45"></asp:ListItem>
                             <asp:ListItem Text="46" Value="46"></asp:ListItem>
                             <asp:ListItem Text="47" Value="47"></asp:ListItem>
                             <asp:ListItem Text="48" Value="48"></asp:ListItem>
                             <asp:ListItem Text="49" Value="49"></asp:ListItem>
                             <asp:ListItem Text="50" Value="50"></asp:ListItem>
                             <asp:ListItem Text="51" Value="51"></asp:ListItem>
                             <asp:ListItem Text="52" Value="52"></asp:ListItem>
                             <asp:ListItem Text="53" Value="53"></asp:ListItem>
                             <asp:ListItem Text="54" Value="54"></asp:ListItem>
                             <asp:ListItem Text="55" Value="55"></asp:ListItem>
                             <asp:ListItem Text="56" Value="56"></asp:ListItem>
                             <asp:ListItem Text="57" Value="57"></asp:ListItem>
                             <asp:ListItem Text="58" Value="58"></asp:ListItem>
                             <asp:ListItem Text="59" Value="59"></asp:ListItem>
                         </asp:ListBox>

                                </td>
                                <td>&nbsp;&nbsp;
                                    <asp:TextBox runat="server" Text="00" ID="txthr" MaxLength="2" Width="25px" Enabled="false"></asp:TextBox>
                                    <b>:</b>
                                    <asp:TextBox runat="server" Text="00" ID="txtmin" MaxLength="2" Width="25px" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>





                    </td>
                </tr>
            </tbody>
        </table>


    </div>
    <br />
    &nbsp; &nbsp;<asp:Label ID="lblerr" runat="server" Style="color: red" Text=""></asp:Label><br />
    <asp:Label ID="lblmsg" Text="" runat="server" ForeColor="green" Visible="false"></asp:Label>
    <asp:Button ID="btnSubmit" CssClass="btn" Text="Receive" runat="server" OnClick="btnSubmit_Click" ValidationGroup="v1" Style="float: right; margin-right: 15%; width: 10%;" />

</asp:Content>

