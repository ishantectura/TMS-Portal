<%@ Page Title="" Language="C#" MasterPageFile="~/RoleBaseMaster.master" AutoEventWireup="true" CodeFile="TrackVehicle.aspx.cs" Inherits="TrackVehicle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h4>Track Vehicle</h4>
    <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
    <asp:Label ID="lblVehicle" runat="server" Text="Vehicle No" ></asp:Label> &nbsp;&nbsp; <asp:TextBox ID="txtVehicleNo" AutoPostBack="true" runat="server" OnTextChanged="txtVehicleNo_TextChanged"></asp:TextBox>
    <asp:Label ID="lblcity" runat="server" Text="city" ></asp:Label> &nbsp;&nbsp; <asp:TextBox ID="txtcity" AutoPostBack="true" runat="server" OnTextChanged="txtcity_TextChanged"></asp:TextBox>
     <table style="width:85%;">
         <tr>
             <td style="width:100%;">
                 <asp:Button Text="ExportToExcel" ID="btnExport" OnClick="btnExport_Click" runat="server" style="float:right;" />
             </td>
         </tr>
    </table>
    <div class="clsParentGrid">
        <asp:GridView ID="grdTrackVehicle" runat="server" AutoGenerateColumns="false" PagerSettings-PageButtonCount="10"
                    AllowPaging="True" PageSize="8" PagerSettings-Position="Bottom" PagerSettings-Mode="NumericFirstLast" 
                    EmptyDataText="No entries found." CssClass="clsHalfGrid" DataKeyNames="LRNo" OnRowDataBound="grdTrackVehicle_RowDataBound" OnPageIndexChanging="grdTrackVehicle_PageIndexChanging">

            <Columns>

                <%--edited by vijay--%>

                <asp:TemplateField HeaderText="S.No">
                      <ItemTemplate>
                               <%# Container.DataItemIndex + 1 %>
                      </ItemTemplate>
                </asp:TemplateField>

                 <asp:BoundField DataField="LRNo" HeaderText="LR No" HeaderStyle-Width="80px" />
                 <asp:BoundField DataField="LRDate" HeaderText="LR Date" HeaderStyle-Width="80px" />
                 <%--<asp:BoundField DataField="Size" HeaderText="Size" HeaderStyle-Width="80px" />--%>
                 <asp:BoundField DataField="Destination" HeaderText="Destination" HeaderStyle-Width="80px" />
                 <asp:BoundField DataField="TruckNo" HeaderText="TruckNo" HeaderStyle-Width="80px" />
                
                
                <asp:BoundField  DataField ="ExpectedDateofDelivery" HeaderText="Expected Date of Delivery" HeaderStyle-Width="80px"/>

                <%--Edited by vijay--%>
                <asp:BoundField  DataField ="StatusChangeDate" HeaderText="Status Change Date" HeaderStyle-Width="80px"/>

                <asp:BoundField  DataField ="StatusDate" HeaderText="Status Date" HeaderStyle-Width="80px"/>
                <asp:TemplateField HeaderText="Transit Status">
                            <ItemTemplate>
                                <asp:Label ID="lblTransitStatus" runat="server" Text='<%# Eval("TransitStatus") %>' Visible="false" />
                                <asp:DropDownList ID="ddlTransitStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTransitStatus_SelectedIndexChanged" dataf>
                                    
                                    <asp:ListItem Text="In-Transit" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Reached at destination" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Unloaded" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Accident" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Hold by Authorities" Value="4"></asp:ListItem>
                                   
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                <asp:TemplateField HeaderText="Remark">
                    <ItemTemplate>
                        <asp:TextBox ID="txtStatusRemark" TextMode="MultiLine" runat="server" Text='<%# (Eval("StatusRemark")) %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

               
                 <%--<asp:TemplateField  >
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkboxSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkboxSelectAll_CheckedChanged" />
                        </HeaderTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelectedRow" runat="server"></asp:CheckBox>
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


         <asp:GridView ID="grdTrackVehicleExcel" runat="server" AutoGenerateColumns="false" visible="false"
                    EmptyDataText="No entries found." CssClass="clsHalfGrid" DataKeyNames="LRNo" OnRowDataBound="grdTrackVehicle_RowDataBoundExcel">

            <Columns>
                 <asp:BoundField DataField="LRNo" HeaderText="LR No" HeaderStyle-Width="80px" />
                 <asp:BoundField DataField="LRDate" HeaderText="LR Date" HeaderStyle-Width="80px" />
                 <%--<asp:BoundField DataField="Size" HeaderText="Size" HeaderStyle-Width="80px" />--%>
                 <asp:BoundField DataField="Destination" HeaderText="Destination" HeaderStyle-Width="80px" />
                 <asp:BoundField DataField="TruckNo" HeaderText="TruckNo" HeaderStyle-Width="80px" />
                
                
                <asp:BoundField  DataField ="ExpectedDateofDelivery" HeaderText="Expected Date of Delivery" HeaderStyle-Width="80px"/>

                <%--Edited by vijay--%>
                <asp:BoundField  DataField ="StatusChangeDate" HeaderText="Status Change Date" HeaderStyle-Width="80px"/>

                <asp:BoundField  DataField ="StatusDate" HeaderText="Status Date" HeaderStyle-Width="80px"/>
                <asp:TemplateField HeaderText="Transit Status">
                            <ItemTemplate>
                                <asp:Label ID="lblTransitStatus" runat="server" Text='<%# Eval("TransitStatus") %>' Visible="false" />
                                <asp:DropDownList ID="ddlTransitStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTransitStatus_SelectedIndexChanged" dataf>
                                    
                                    <asp:ListItem Text="In-Transit" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Reached at destination" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Unloaded" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Accident" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Hold by Authorities" Value="4"></asp:ListItem>
                                   
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                <asp:TemplateField HeaderText="Remark">
                    <ItemTemplate>
                        <asp:TextBox ID="txtStatusRemark" TextMode="MultiLine" runat="server" Text='<%# (Eval("StatusRemark")) %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

               
                 <%--<asp:TemplateField  >
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkboxSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkboxSelectAll_CheckedChanged" />
                        </HeaderTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelectedRow" runat="server"></asp:CheckBox>
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
    <br />
    <asp:Button ID="btnSub" CssClass="btn" Text="Submit" runat="server" OnClick="btnSub_Click" Style="float: right; margin-right: 15%; width: 10%;" />
     <asp:Button ID="btnSubmit" CssClass="btn" Text="Submit" runat="server" OnClick="btnSubmit_Click" Style="float: right; margin-right: 15%; visibility:hidden; width: 10%;" />

    
</asp:Content>

