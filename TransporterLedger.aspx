<%@ Page Title="" Language="C#" MasterPageFile="~/RoleBaseMaster.master" AutoEventWireup="true" CodeFile="TransporterLedger.aspx.cs" Inherits="TransporterLedger" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <h4>Transporter Ledger</h4>
    <asp:ScriptManager ID="scm" runat="server"></asp:ScriptManager>
    <table style="margin-left:10px; border:1px none black; width:80%; padding:20px;" >
        <tr>
            <td>
                <asp:Label ID="lblTransporterNo" runat="server" Text="Transporter No" Visible="true" />
            </td>
            <td>
                <asp:DropDownList ID="ddlTransporterNo" runat="server"  OnSelectedIndexChanged="ddlTransporterNo_SelectedIndexChanged"  AutoPostBack="true" Width="150px">
                   
                </asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="lblFromPostingDate" runat="server" Text="From Date" Visible="true"  />
            </td>
            <td>
                 <asp:TextBox ID="txtFromDate" runat="server" AutoPostBack="true" /> 
               <%-- OnTextChanged="txtFromDate_TextChanged" --%>
                            <asp:ImageButton runat="server" ID="ImageButton1" ImageUrl="App_Themes/img/Calendar.png" />
                            <cc2:CalendarExtender ID="CalendarExtender1" EndDate="<%# DateTime.Now %>" Format="dd-MM-yyyy" runat="server" TargetControlID="txtFromDate"
                                PopupButtonID="ImageButton1" />
                <asp:RequiredFieldValidator ID="rqffromdate" runat="server" ControlToValidate="txtFromDate" ErrorMessage="*" ForeColor="Red" ValidationGroup="SearchValidationGroup"></asp:RequiredFieldValidator>
            </td>

            <td>
                <asp:Label ID="lblToPostingDate" runat="server" Text="To Date" Visible="true"  />
            </td>
            <td>
                  <asp:TextBox ID="txtToDate" runat="server" AutoPostBack="true"/>
                <%--  OnTextChanged="txtToDate_TextChanged"--%>
                            <asp:ImageButton runat="server" ID="ImageButton2" ImageUrl="App_Themes/img/Calendar.png" />
                            <cc2:CalendarExtender ID="CalendarExtender2" EndDate="<%# DateTime.Now %>" Format="dd-MM-yyyy" runat="server" TargetControlID="txtToDate"
                                PopupButtonID="ImageButton2" />
                <asp:RequiredFieldValidator ID="rqftodate" runat="server" ControlToValidate="txtToDate" ErrorMessage="*"  ForeColor="Red" ValidationGroup="SearchValidationGroup"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:Button id="btngo" runat="server" CssClass="btn" Text="Go" ValidationGroup="SearchValidationGroup" OnClick="btngo_Click"/>

            </td>
        </tr>
    </table>


    <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>

  <%--     <asp:TextBox ID="txtGateOutDate" runat="server" />
                            <asp:ImageButton runat="server" ID="ImageButton1" ImageUrl="App_Themes/img/Calendar.png" />
                            <cc2:CalendarExtender ID="CalendarExtender1" EndDate="<%# DateTime.Now %>" Format="dd-MM-yyyy" runat="server" TargetControlID="txtGateOutDate"
                                PopupButtonID="ImageButton1" />--%>

     <table style="width:85%;">
         <tr>
             <td style="width:100%;">
                 <asp:Button Text="ExportToExcel" ID="btnExport" OnClick="btnExport_Click" runat="server" style="float:right;" />
             </td>
         </tr>
    </table> 
    <div class="clsParentGrid">
        <asp:GridView ID="grdTransporterLedger" runat="server" AutoGenerateColumns="false" PagerSettings-PageButtonCount="10"
            AllowPaging="True" PageSize="10" PagerSettings-Position="Bottom" PagerSettings-Mode="NumericFirstLast"
            EmptyDataText="No entries found." CssClass="clsHalfGrid" DataKeyName="TransporterNo" OnPageIndexChanging="grdTransporterLedger_PageIndexChanging" 
            EnableViewState="true" >
            <Columns>

                 <%--edited by vijay--%>

                <asp:TemplateField HeaderText="S.No" ItemStyle-Width="40px">
                      <ItemTemplate>
                               <%# Container.DataItemIndex + 1 %>
                      </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="TransporterNo" HeaderText="Transporter No" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="Description" HeaderText="Description" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="DocNo" HeaderText="Document No" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="BillInvoiceno" HeaderText="Bill Invoice no" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="DocType" HeaderText="Document type" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="Postingdate" HeaderText="Posting date" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="Amount" HeaderText="Amount" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="RemainingAmount" HeaderText="Remaining Amount" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="DueDate" HeaderText="Due Date" ReadOnly="true" HeaderStyle-Width="100px" />
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

        <asp:GridView ID="grdTransporterLedgerExcel" runat="server" AutoGenerateColumns="false" visible="false"
            EmptyDataText="No entries found." CssClass="clsHalfGrid" DataKeyName="TransporterNo" 
            EnableViewState="true" >
            <Columns>
                <asp:BoundField DataField="TransporterNo" HeaderText="Transporter No" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="Description" HeaderText="Description" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="DocNo" HeaderText="Document No" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="BillInvoiceno" HeaderText="Bill Invoice no" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="DocType" HeaderText="Document type" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="Postingdate" HeaderText="Posting date" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="Amount" HeaderText="Amount" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="RemainingAmount" HeaderText="Remaining Amount" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="DueDate" HeaderText="Due Date" ReadOnly="true" HeaderStyle-Width="100px" />
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

