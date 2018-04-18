<%@ Page Title="" Language="C#" MasterPageFile="~/RoleBaseMaster.master" AutoEventWireup="true" CodeFile="PlacedRequest.aspx.cs" Inherits="PlacedRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <h4>Placed Requests</h4>
    <table style="margin-left:10px; border:1px none black; width:50%; padding:20px;" >
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

        <asp:GridView ID="grdPlacedRequest" runat="server" AutoGenerateColumns="false" PagerSettings-PageButtonCount="10"
            AllowPaging="True" PageSize="8" PagerSettings-Position="Bottom" PagerSettings-Mode="NumericFirstLast"
            EmptyDataText="No entries found." CssClass="clsHalfGrid" DataKeyNames="RequesterId" OnPageIndexChanging="grdPlacedRequest_PageIndexChanging"
            EnableViewState="true" OnRowDataBound="OnRowDataBound" >
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
                <asp:BoundField DataField="OperationalStatus" HeaderText="Status" ReadOnly="true" HeaderStyle-Width="100px" />                
                <asp:BoundField DataField="TransporterType" HeaderText="Transporter Type" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TransporterCode" HeaderText="Transporter Code" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TransporterName" HeaderText="Transporter Name" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="Size" HeaderText="Size" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="ExpectedPlacementDate" ReadOnly="true" HeaderText="Expected Placement Date" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="AcceptanceDate" ReadOnly="true" HeaderText="Date of Acceptance" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TruckNo" ReadOnly="true" HeaderText="Truck No." HeaderStyle-Width="100px" />
                <asp:BoundField DataField="Destination" HeaderText="Destination" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="NoofPermits" HeaderText="NoOfPermits" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="PlacementDateTime" ReadOnly="true" HeaderText="Placement Date Time" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TransporterRemark" ReadOnly="true" HeaderText="Transporter Remark" HeaderStyle-Width="100px" />            
                 <asp:TemplateField HeaderText="Reason Code" HeaderStyle-Width="200px" ItemStyle-Width="150px">
                    <ItemTemplate>                      
                        <asp:Label ID="lblReasonCode" runat="server" Text='<%# Eval("ReasonCode") %>' Visible="true" />
                        <asp:DropDownList ID="ddlReasonCode" runat="server" AutoPostBack="true" Width="130px" OnSelectedIndexChanged="ddlReasonCode_SelectedIndexChanged"></asp:DropDownList>
                        <asp:Label ID="lblmandatoryReasonCode" Text="*" ForeColor="Red" Visible="false" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Cancellation Remark">
                    <ItemTemplate>
                        <asp:TextBox ID="txtCancellationRemark" TextMode="MultiLine" runat="server" Text=''></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                  <asp:TemplateField HeaderText="Select">
                      <HeaderTemplate >
                            <%--Edited by vijay--%>
                            <asp:CheckBox ID="chkboxSelectAll" runat="server" AutoPostBack="true"  OnCheckedChanged="chkboxSelectAll_CheckedChanged"  Text="Select All" color="white" />
                        </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelectedRow" runat="server" />
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

        <asp:GridView ID="grdPlacedRequestExcel" runat="server" AutoGenerateColumns="false" Visible="false"
            EmptyDataText="No entries found." CssClass="clsHalfGrid" DataKeyNames="RequesterId"
            EnableViewState="true" OnRowDataBound="OnRowDataBoundExcel" >
            <Columns>
                <asp:BoundField DataField="RequestNo" HeaderText="Req No" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="RequestDate" HeaderText="Req Date" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="RequesterId" HeaderText="Req Id" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="ResponsibilityCenter" HeaderText="Responsibility Center" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="OperationalStatus" HeaderText="Status" ReadOnly="true" HeaderStyle-Width="100px" />                
                <asp:BoundField DataField="TransporterType" HeaderText="Transporter Type" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TransporterCode" HeaderText="Transporter Code" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TransporterName" HeaderText="Transporter Name" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="Size" HeaderText="Size" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="ExpectedPlacementDate" ReadOnly="true" HeaderText="Expected Placement Date" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="AcceptanceDate" ReadOnly="true" HeaderText="Date of Acceptance" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TruckNo" ReadOnly="true" HeaderText="Truck No." HeaderStyle-Width="100px" />
                <asp:BoundField DataField="Destination" HeaderText="Destination" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="NoofPermits" HeaderText="NoOfPermits" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="PlacementDateTime" ReadOnly="true" HeaderText="Placement Date Time" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TransporterRemark" ReadOnly="true" HeaderText="Transporter Remark" HeaderStyle-Width="100px" />            
                 <asp:TemplateField HeaderText="Reason Code" HeaderStyle-Width="200px" ItemStyle-Width="150px">
                    <ItemTemplate>                      
                        <asp:Label ID="lblReasonCode" runat="server" Text='<%# Eval("ReasonCode") %>' Visible="true" />
                        <asp:DropDownList ID="ddlReasonCode" runat="server" AutoPostBack="true" Width="130px" OnSelectedIndexChanged="ddlReasonCode_SelectedIndexChanged"></asp:DropDownList>
                        <asp:Label ID="lblmandatoryReasonCode" Text="*" ForeColor="Red" Visible="false" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Cancellation Remark">
                    <ItemTemplate>
                        <asp:TextBox ID="txtCancellationRemark" TextMode="MultiLine" runat="server" Text=''></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                  <asp:TemplateField HeaderText="Select">
                      <HeaderTemplate >
                            <%--Edited by vijay--%>
                            <asp:CheckBox ID="chkboxSelectAll" runat="server" AutoPostBack="true"  OnCheckedChanged="chkboxSelectAll_CheckedChanged"  Text="Select All" color="white" />
                        </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelectedRow" runat="server" />
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
      <asp:Label ID="lblErr" runat="server" Text=""></asp:Label>
    <br />
    <asp:Button ID="btnCancelRequest" CssClass="btn" Text="Cancel Request" runat="server" OnClick="btnCancelRequest_Click" Style="float: right; margin-right: 15%; width: 10%;" />
</asp:Content>


