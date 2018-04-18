<%@ Page Title="" Language="C#" MasterPageFile="~/RoleBaseMaster.master" AutoEventWireup="true" CodeFile="PendingForGateEntry.aspx.cs" Inherits="PendingForGateEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h4>Pending For Gate Entry</h4>
     <table style="margin-left:10px; border:1px none black; width:80%; padding:20px;" >
         <tr>

              <td>
                 <asp:Label ID="lblReqNo" runat="server" Text="Req No." Visible="true"></asp:Label>
             </td>
             <td>
                 <asp:TextBox ID="txtReqNo" runat="server" AutoPostBack="true"  OnTextChanged="txtReqNo_TextChanged"></asp:TextBox>
             </td>

             <td>
                 <asp:Label ID="lblcity" runat="server" Text="City" Visible="true"></asp:Label>
             </td>
             <td>
                 <asp:TextBox ID="txtcity" runat="server" AutoPostBack="true"  OnTextChanged="txtcity_TextChanged"></asp:TextBox>
             </td>
         </tr>

     </table>
    <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
      <table style="width:85%;">
         <tr>
             <td style="width:100%;">
                 <asp:Button Text="ExportToExcel" ID="btnExport" OnClick="btnExport_Click" runat="server" style="float:right;" />
             </td>
         </tr>
    </table> 
    <div class="clsParentGrid">
        <asp:GridView ID="grdPendingRequest" runat="server" AutoGenerateColumns="false" PagerSettings-PageButtonCount="10"
            AllowPaging="True" PageSize="8" PagerSettings-Position="Bottom" PagerSettings-Mode="NumericFirstLast"
            EmptyDataText="No entries found." CssClass="clsHalfGrid" DataKeyNames="RequesterId" OnPageIndexChanging="OnPaging"
            OnRowDataBound="OnRowDataBound" EnableViewState="true" OnRowCommand="OnRowCommand">
            <Columns>

                 <%--edited by vijay--%>

                <asp:TemplateField HeaderText="S.No" ItemStyle-Width="40px">
                      <ItemTemplate>
                               <%# Container.DataItemIndex + 1 %>
                      </ItemTemplate>
                </asp:TemplateField>


                <asp:BoundField DataField="RequestNo" HeaderText="Req No" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="RequestDate" HeaderText="Req Date" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="RequesterId" HeaderText="Req Id" Visible="false" ReadOnly="true" HeaderStyle-Width="100px" />
                <%--<asp:BoundField DataField="TransporterType" HeaderText="Transporter Type" ReadOnly="true" HeaderStyle-Width="100px" />--%>
               <%-- <asp:BoundField DataField="TransporterCode" HeaderText="Transporter Code" ReadOnly="true" HeaderStyle-Width="100px" />--%>
                <asp:BoundField DataField="TransporterName" HeaderText="Transporter Name" ReadOnly="true" HeaderStyle-Width="100px" />
                <%--<asp:BoundField DataField="BreweryName" HeaderText="Brewery Name" ReadOnly="true" HeaderStyle-Width="100px" />--%>
                <asp:BoundField DataField="City" HeaderText="City" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="DriverName" HeaderText="Driver Name" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="DriverMobileNo" HeaderText="Driver Mobile No." ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TruckNo" HeaderText="Vehicle No." ReadOnly="true" HeaderStyle-Width="100px" />

                <%--<asp:BoundField DataField="TransporterPhone" HeaderText="Transporter Phone" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="Transporter" HeaderText="Transporter " ReadOnly="true" HeaderStyle-Width="100px" />--%>
                <asp:BoundField DataField="Size" HeaderText="Size" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="Destination" HeaderText="Destination" ReadOnly="true" HeaderStyle-Width="100px" />
               
                <asp:BoundField DataField="ExpectedPlacementDate" ReadOnly="true" HeaderText="Expected Placement Date" HeaderStyle-Width="100px" />
               
                 <asp:BoundField DataField="PermitExpiryDate" ReadOnly="true" HeaderText=" Permit Expiry Date" HeaderStyle-Width="100px" />
           
                  <asp:BoundField DataField="TransporterType" ReadOnly="true" HeaderText="Transporter Type" HeaderStyle-Width="100px" />
                    <asp:BoundField DataField="TransporterResponse" ReadOnly="true" HeaderText="Transporter response" HeaderStyle-Width="100px" />
             


                <asp:ButtonField CommandName="CancelRequest" Text="Cancel" ButtonType="Button" />

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

         <asp:GridView ID="grdPendingRequestExcel" runat="server" AutoGenerateColumns="false" visible="false"
            EmptyDataText="No entries found." CssClass="clsHalfGrid" DataKeyNames="RequesterId" 
            OnRowDataBound="OnRowDataBoundExcel" EnableViewState="true">
            <Columns>
                <asp:BoundField DataField="RequestNo" HeaderText="Req No" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="RequestDate" HeaderText="Req Date" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="RequesterId" HeaderText="Req Id" Visible="false" ReadOnly="true" HeaderStyle-Width="100px" />
                <%--<asp:BoundField DataField="TransporterType" HeaderText="Transporter Type" ReadOnly="true" HeaderStyle-Width="100px" />--%>
               <%-- <asp:BoundField DataField="TransporterCode" HeaderText="Transporter Code" ReadOnly="true" HeaderStyle-Width="100px" />--%>
                <asp:BoundField DataField="TransporterName" HeaderText="Transporter Name" ReadOnly="true" HeaderStyle-Width="100px" />
                <%--<asp:BoundField DataField="BreweryName" HeaderText="Brewery Name" ReadOnly="true" HeaderStyle-Width="100px" />--%>
                <asp:BoundField DataField="City" HeaderText="City" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="DriverName" HeaderText="Driver Name" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="DriverMobileNo" HeaderText="Driver Mobile No." ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TruckNo" HeaderText="Vehicle No." ReadOnly="true" HeaderStyle-Width="100px" />

                <%--<asp:BoundField DataField="TransporterPhone" HeaderText="Transporter Phone" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="Transporter" HeaderText="Transporter " ReadOnly="true" HeaderStyle-Width="100px" />--%>
                <asp:BoundField DataField="Size" HeaderText="Size" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="Destination" HeaderText="Destination" ReadOnly="true" HeaderStyle-Width="100px" />
               
                <asp:BoundField DataField="ExpectedPlacementDate" ReadOnly="true" HeaderText="Expected Placement Date" HeaderStyle-Width="100px" />
               
                 <asp:BoundField DataField="PermitExpiryDate" ReadOnly="true" HeaderText=" Permit Expiry Date" HeaderStyle-Width="100px" />
           
                  <asp:BoundField DataField="TransporterType" ReadOnly="true" HeaderText="Transporter Type" HeaderStyle-Width="100px" />
                    <asp:BoundField DataField="TransporterResponse" ReadOnly="true" HeaderText="Transporter response" HeaderStyle-Width="100px" />
             


                <asp:ButtonField CommandName="CancelRequest" Text="Cancel" ButtonType="Button" />

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
        <asp:Label ID="lblpermit" runat="server" Text="" Visible="false" Style="font-weight: bold; font-size: 18px; color: black;"></asp:Label>

        <asp:GridView ID="grdRequestPermit" runat="server" AutoGenerateColumns="false" PagerSettings-PageButtonCount="10"
            AllowPaging="True" PageSize="5" PagerSettings-Position="Bottom" PagerSettings-Mode="NumericFirstLast"
            EmptyDataText="No entries found." CssClass="clsHalfGrid" EnableViewState="true">
            <Columns>
                <asp:BoundField DataField="CompanyName" HeaderText="Company Name" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="RequestNo_" HeaderText="Req No" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="ResponsibilityCenter" HeaderText="Responsibility Center" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="Destination" HeaderText="Destination" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="DestinationPostCode" HeaderText="Destination PostCode" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="DestinationCity" HeaderText="Destination City" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="LineNo_" HeaderText="Line No" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="PermitNo_" HeaderText="Permit No" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="PermitDescription" HeaderText="Permit Description" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="PermitValidity" HeaderText="Permit Validity" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="DocumentType" HeaderText="Document Type" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="DocumentNo_" HeaderText="Document No" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="PermitSerialNumber" HeaderText="Permit Serial Number" ReadOnly="true" HeaderStyle-Width="100px" />
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


        <%--<asp:GridView ID="grdRequestPermit" runat="server" AutoGenerateColumns="true" PagerSettings-PageButtonCount="50"
            AllowPaging="True" PageSize="20" PagerSettings-Position="Bottom" PagerSettings-Mode="NumericFirstLast"
            EmptyDataText="No entries found." CssClass="clsHalfGrid" DataKeyNames="RequestNo_">
            <Columns>
              <%--  <asp:BoundField DataField="CompanyName" HeaderText="Company Name" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="RequestNo_" HeaderText="Req No" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="ResponsibilityCenter" HeaderText="Responsibility Center" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="Destination" HeaderText="Destination" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="DestinationPostCode" HeaderText="Destination PostCode" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="DestinationCity" HeaderText="Destination City" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="LineNo_" HeaderText="Line No" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="PermitNo_" HeaderText="Permit No" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="PermitDescription" HeaderText="Permit Description" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="PermitValidity" HeaderText="Permit Validity" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="DocumentType" HeaderText="Document Type" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="DocumentNo_" HeaderText="Document No" ReadOnly="true" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="PermitSerialNumber" HeaderText="Permit Serial Number" ReadOnly="true" HeaderStyle-Width="100px" />
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
    </div>
    <asp:Label ID="lblErr" runat="server" Text="" Style="color: red;"></asp:Label>
    <br />
    <asp:Button ID="btnSubmit" CssClass="btn" Text="Submit" runat="server" OnClick="btnSubmit_Click" Style="float: right; display:none; margin-right: 15%; width: 10%;" />
</asp:Content>
