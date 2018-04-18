<%@ Page Title="" Language="C#" MasterPageFile="~/RoleBaseMaster.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="CanceledRejectedRequest.aspx.cs" Inherits="CanceledRejectedRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <h4>Canceled and Rejected Requests</h4>
    <table style="margin-left:10px; border:1px none black; width:60%; padding:20px;" >
        <tr>
            <td>
                <asp:Label ID="lblOperationalStatus" runat="server" Text="Operational Status" Visible="true" />
            </td>
            <td>
                <asp:DropDownList ID="ddlOperationalStatus" runat="server"   Width="150px">
                    <asp:ListItem Text="" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Canceled" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Rejected" Value="2"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="lblTransporterResponse" runat="server" Text="Transporter Response" Visible="true"  />
            </td>
            <td>
                <asp:DropDownList ID="ddlTransporterResponse" runat="server"    Width="150px">
                    <asp:ListItem Text="" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Accepted" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Rejected" Value="2"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr><td>&nbsp</td></tr>
        <tr>
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
                 <asp:Button ID="btnGo" runat="server" AutoPostBack="true"   Text="Go" OnClick="btnGo_Click"   ></asp:Button>
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
        <asp:GridView ID="grdCancelRejectRequest" runat="server" AutoGenerateColumns="false" PagerSettings-PageButtonCount="10"
            AllowPaging="True" PageSize="8" PagerSettings-Position="Bottom" PagerSettings-Mode="NumericFirstLast"
            EmptyDataText="No entries found." CssClass="clsHalfGrid" DataKeyNames="RequesterId" OnPageIndexChanging="grdCancelRejectRequest_PageIndexChanging"
            EnableViewState="true">
            <Columns>

                <%--edited by vijay--%>

                <asp:TemplateField HeaderText="S.No">
                      <ItemTemplate>
                               <%# Container.DataItemIndex + 1 %>
                      </ItemTemplate>
                </asp:TemplateField>


                <asp:BoundField DataField="RequestNo" HeaderText="Req No" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="RequestDate" HeaderText="Req Date" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="RequesterId" HeaderText="Req Id" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="ResponsibilityCenter" HeaderText="Responsibility Center" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="Size" HeaderText="Size" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="ExpectedPlacementDate" ReadOnly="true" HeaderText="Expected Placement Date" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="AcceptanceDate" ReadOnly="true" HeaderText="Date of Acceptance" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TruckNo" ReadOnly="true" HeaderText="Truck No." HeaderStyle-Width="100px" />
                <asp:BoundField DataField="Destination" HeaderText="Destination" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="NoofPermits" HeaderText="NoOfPermits" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="PlacementDateTime" ReadOnly="true" HeaderText="Placement Date Time" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="OperationalStatus" HeaderText="Status" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TransporterResponse" ReadOnly="true" HeaderText="Transporter Response" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TransporterType" HeaderText="Transporter Type" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TransporterCode" HeaderText="Transporter Code" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TransporterName" HeaderText="Transporter Name" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TransporterRemark" ReadOnly="true" HeaderText="Transporter Remark" HeaderStyle-Width="100px" />
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

        <asp:GridView ID="grdCancelRejectRequestExcel" runat="server" AutoGenerateColumns="false" Visible="false"
            EmptyDataText="No entries found." CssClass="clsHalfGrid" DataKeyNames="RequesterId"
            EnableViewState="true">
            <Columns>
                <asp:BoundField DataField="RequestNo" HeaderText="Req No" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="RequestDate" HeaderText="Req Date" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="RequesterId" HeaderText="Req Id" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="ResponsibilityCenter" HeaderText="Responsibility Center" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="Size" HeaderText="Size" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="ExpectedPlacementDate" ReadOnly="true" HeaderText="Expected Placement Date" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="AcceptanceDate" ReadOnly="true" HeaderText="Date of Acceptance" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TruckNo" ReadOnly="true" HeaderText="Truck No." HeaderStyle-Width="100px" />
                <asp:BoundField DataField="Destination" HeaderText="Destination" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="NoofPermits" HeaderText="NoOfPermits" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="PlacementDateTime" ReadOnly="true" HeaderText="Placement Date Time" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="OperationalStatus" HeaderText="Status" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TransporterResponse" ReadOnly="true" HeaderText="Transporter Response" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TransporterType" HeaderText="Transporter Type" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TransporterCode" HeaderText="Transporter Code" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TransporterName" HeaderText="Transporter Name" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TransporterRemark" ReadOnly="true" HeaderText="Transporter Remark" HeaderStyle-Width="100px" />
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


