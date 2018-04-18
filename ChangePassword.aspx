<%@ Page Title="" Language="C#" EnableViewState="true" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <!--PROVIDED BY MARCELO MARTINS - BOOTLY @ mmartins-->
    <div class="container">
        <div class="row">
            <div class="col-md-4 col-md-offset-4">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <div class="text-center">
                            <h3 class="text-center">Change Password</h3>
                            <p>Change your password here !</p>
                            <div class="panel-body">
                                <fieldset>
                                    <legend>
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="glyphicon glyphicon-envelope color-blue"></i></span>
                                                <!--EMAIL ADDRESS-->
                                                <label class="sr-only" for="exampleInputPassword2">Old Password</label>
                                                <%-- <input type="password" class="form-control" id="txtOldPassword" placeholder="Old Password" required="" runat="server" />--%>
                                                <input type="password" class="form-control" id="txtOldPassword" placeholder="Old Password" required="" runat="server" />

                                            </div>
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="glyphicon glyphicon-envelope color-blue"></i></span>
                                                <label class="sr-only" for="exampleInputPassword2">New Password</label>
                                                <input type="password" class="form-control" id="txtPassword" placeholder="New Password" required="" runat="server" />

                                            </div>
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="glyphicon glyphicon-envelope color-blue"></i></span>
                                                <label class="sr-only" for="exampleInputPassword2">Confirm Password</label>
                                                <input type="password" class="form-control" id="txtConfirmPassword" placeholder="Confirm Password" required="" runat="server" />
                                            </div>
                                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword" ErrorMessage="Password and Confirm password must be same" CssClass="clsValidation"></asp:CompareValidator>
                                        </div>
                                        <asp:Label ID="lblmsg" runat="server" CssClass="clsValidation" Text="" Visible="false"></asp:Label>
                                    </legend>
                                    <div class="form-group">
                                        <asp:Button ID="btnOk" CssClass="btn" Text="Save Changes" runat="server" OnClick="btnOk_Click" />
                                        <br />
                                    </div>
                                </fieldset>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>



</asp:Content>

