<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="ContactUs.aspx.cs" Inherits="ContactUs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="col-lg-12">
     <h3>Contact Us at below Address:</h3>     
            <div class="form-top-right">
        <i class="fa fa-envelope"></i>
    </div>  
            <div class="row">
        <div class="col-lg-3">
            <div class="widget">
                <h4 class="widgetheading">Main Address</h4>
                <address>
                    <strong>Carlsberg Breweries A/S</strong><br>
                    100 Ny Carlsberg Vej<br>
                    1799 Copenhagen V, Denmark
                </address>
                <p>
                    <i class="icon-phone"></i>Phone: (+45) 3327 3300
                                <br/>
                    <i class="icon-envelope-alt"></i>contact@carlsberg.com
                </p>
            </div>
        </div>
    </div>
            <section id="Section1">
        <!-- divider -->
        <div class="row">
            <div class="col-lg-12" style="width: 40%";>
                <div class="solidline">
                </div>
            </div>
        </div>
    </section>
    </div>

      <div class="col-lg-12">          
            <div class="form-top-right">
                 <i class="fa fa-briefcase"></i>
            </div>
            <div class="row"> 
                 <div class="col-lg-8">
                <div class="widget">
                    <h4 class="widgetheading">Press</h4>
                    <address>
                        <p>If you represent the media - print, online, radio or tv - please address enquiries concerning Carlsberg Group to :<br />
                        </p>
                        <strong>Director, International and Danish Media:</strong> <br />
                        Kasper Elbjørn<br />
                    </address>
                    <p>
                        <i class="icon-phone"></i>Tel +45 4179 1216<br />
                        <i class="icon-envelope-alt"></i>Kasper.Elbjorn@carlsberg.com
                    </p>
                </div>
                     </div>
        </div>
    </div>
</asp:Content>

