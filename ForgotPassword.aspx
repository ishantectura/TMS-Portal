<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="ForgotPassword.aspx.cs" Inherits="ForgotPassword" %>

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
                                <h3 class="text-center">Forgot Password?</h3>
                                <p>Get your password on Mail !</p>
                                <div class="panel-body">
                                    <fieldset>
                                        <legend>
                                            <div class="form-group">
                                                 <div class="input-group">
                                                    <span class="input-group-addon"><i class="glyphicon glyphicon-envelope color-blue"></i></span>
                                                    <label class="sr-only" for="exampleInputPassword2">Enter User Name/User ID</label>
                                                    <input type="text" name="form-username" class="form-control" id="txtUserName" placeholder="User Name" required="" runat="server" />
                                                </div>
                                                <div class="input-group">
                                                    <span class="input-group-addon"><i class="glyphicon glyphicon-envelope color-blue"></i></span>
                                                    <!--EMAIL ADDRESS-->
                                                    <label class="sr-only" for="exampleInputPassword2">Please Enter your Email ID</label>
                                                    <input type="email" class="form-control" id="txtEmail" placeholder="Registered Email ID" required="" runat="server" />
                                               </div>
                                            </div>
                                            <asp:Label ID="lblmsg" runat="server" CssClass="clsValidation" Text="" Visible="false"></asp:Label>
                                        </legend>
                                        <div class="form-group">
                                            <asp:Button ID="btnOk" CssClass="btn" Text="Submit" runat="server" OnClick="btnOk_Click" />
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
