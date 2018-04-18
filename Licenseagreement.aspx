<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="Licenseagreement.aspx.cs" Inherits="Licenseagreement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="col-lg-12">
        <div class="jumbotron">
            <h4>Terms and conditions</h4>
            <p><strong>Carlsberg believes that our beer brands are there to offer consumers refreshment and social enjoyment. They should be consumed responsibly by adults and misuse should be avoided. INTRODUCTIONUse of the Site is on the terms contained in this document (Terms of Use). If you do not agree to these Terms of Use, or if you are not of a legal drinking age according to the laws of your country or state, please stop using the Site immediately.</strong></p>
            <br />
            <p>
                <asp:CheckBox ID="chklicense" runat="server" Checked="false" OnCheckedChanged="chklicense_CheckedChanged" AutoPostBack="true" /> &nbsp; &nbsp;I agree term & conditions.<br />
            </p>
        </div>
    </div>

    <br />
    &nbsp; &nbsp;<asp:Label ID="lblerr" runat="server" Text=""></asp:Label><br />
</asp:Content>

