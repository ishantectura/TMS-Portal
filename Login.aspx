<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <title>TMS Portal</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="" />
    <meta name="author" content="http://bootstraptaste.com" />
    <!-- css -->
    <link href="App_Themes/css/bootstrap.min.css" rel="stylesheet" />
    <link href="App_Themes/css/flexslider.css" rel="stylesheet" />
    <link href="App_Themes/css/style.css" rel="stylesheet" />
    <!-- Theme skin -->
    <link href="App_Themes/skins/default.css" rel="stylesheet" />

    <!-- HTML5 shim, for IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
      <script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
    <![endif]-->
    <style type="text/css">
        #rdCmpName > tbody > tr > td {
            padding-right: 16px;
            text-decoration: none;
            font-family: 'Open Sans', Arial, sans-serif;
        }

            #rdCmpName > tbody > tr > td > label {
                font-weight: normal !important;
                padding: 2px;
                color: #656565;
                font-size: 14px;
                font-weight: 300;
            }
    </style>

</head>
<body>

    <div id="wrapper">
        <!-- start header -->
        <header>

            <div class="navbar navbar-default navbar-static-top">
                <div class="container">
                    <div class="navbar-header">
                        <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                            <span class="icon-bar"></span>
                            <span class="icon-bar"></span>
                            <span class="icon-bar"></span>
                        </button>
                        <a class="navbar-brand" href="index.html">
                            <img src="App_Themes/img/logo.png" />
                        </a>
                    </div>
                    <div class="navbar-collapse collapse ">
                        <ul class="nav navbar-nav">
                            <li class="active"><a href="Login.aspx">Home</a></li>
                            <%--  <li class="dropdown">
                                <a href="#" class="dropdown-toggle " data-toggle="dropdown" data-hover="dropdown" data-delay="0" data-close-others="false">Blogs<b class=" icon-angle-down"></b></a>
                                <ul class="dropdown-menu">
                                    <li><a href="AboutUs.aspx">Menu1</a></li>
                                    <li><a href="components.html">Menu2</a></li>
                                    <li><a href="pricingbox.html">Menu3 box</a></li>
                                </ul>
                            </li>--%>
                            <li><a href="AboutUs.aspx?key=ol">About Us</a></li>
                            <li><a href="ContactUs.aspx?key=ol">Contact</a></li>
                            <li><a href="SignOut.aspx">Sign Out</a></li>
                        </ul>
                    </div>
                    <div class="col-lg-12" style="width: 60%; float: right; border: 5px;">
                        <div class="solidline">
                        </div>
                    </div>
                </div>
            </div>
        </header>
        <!-- end header -->
        <section id="featured">
            <!-- start slider -->
            <div class="container">
                <div class="row">
                    <div class="col-lg-12">
                        <!-- Slider -->
                        <div id="main-slider" class="flexslider">
                            <ul class="slides">
                                <li>
                                    <img src="App_Themes/img/slides/11.jpg" alt="" />
                                </li>
                                <li>
                                    <img src="App_Themes/img/slides/22.jpg" alt="" />
                                </li>
                                <li>
                                    <img src="App_Themes/img/slides/33.jpg" alt="" />
                                </li>
                            </ul>
                        </div>
                        <!-- end slider -->
                    </div>
                </div>
            </div>
        </section>
        <br />
        <!-- Model Strats Here -->

        <!-- Button trigger modal -->
        -
        <!-- Button trigger modal -->
    </div>
    <div class="top-content">
        <div class="inner-bg">
            <div class="container">
                <div class="row">
                    <div class="col-sm-5">
                        <div class="box">
                            <div class="box aligncenter">
                                <div class="form-box">
                                    <img src="App_Themes/img/slides/carlsbergGreen.jpg" style="margin-left: 5%; resize: both;" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-1 middle-border"></div>
                    <div class="col-sm-1"></div>
                    <div class="col-sm-5">
                        <div class="form-box" style="margin-top: -15px;">
                            <div class="form-top">
                                <asp:Label ID="lblmsg" runat="server" Style="color: red;" Text=""></asp:Label>
                                <div class="form-top-left">
                                    <h3>Login to our site</h3>
                                    <p>Enter username and password to log on:</p>
                                </div>
                             <%--   <div class="form-top-right">
                                    <i class="fa fa-key"></i>
                                </div>--%>
                            </div>
                            <form role="form" accept-charset="UTF-8" id="Formlogin" class="login-form" runat="server">
                                <div class="form-bottom">

                                    <div class="form-group">
                                        <label class="sr-only" for="form-username">User ID</label>
                                        <input type="text" name="form-username" placeholder="User ID" class="form-control" id="txtUserName" runat="server" required="">
                                        <%--<asp:RequiredFieldValidator ID="rfvUserName" runat="server" ControlToValidate="txtUserName" ErrorMessage="Please Enter User Name" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                                    </div>
                                    <div class="form-group">
                                        <label class="sr-only" for="exampleInputPassword2">Password</label>
                                        <input type="password" class="form-control" id="txtPassword" placeholder="Password" required="" runat="server">
                                        <div class="help-block text-right">
                                            <%-- <a href="#" style="color: #428bca;">Forget the password ?</a>--%>
                                            <asp:LinkButton runat="server" Style="color: #428bca; cursor: default;" ID="btnForgot" Text="Forget the password ?" OnClick="btnForgot_Click"></asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group" style="margin-bottom:-10px; margin-top:0px;">
                                    <%--<label class="radio-inline">
                                        <input type="radio" name="optradio">CIPL</label>
                                    <label class="radio-inline">
                                        <input type="radio" name="optradio">Transporter</label>
                                    <label class="radio-inline">
                                        <input type="radio" name="optradio">Third Party</label>--%>
                                 <%--   <asp:RadioButtonList ID="rdCmpName" runat="server" RepeatDirection="Horizontal" CssClass="rd">
                                        <asp:ListItem>CIPL</asp:ListItem>
                                        <asp:ListItem>Transporter</asp:ListItem>
                                        <asp:ListItem>Third Party</asp:ListItem>
                                    </asp:RadioButtonList>--%>
                                   <%-- <asp:RequiredFieldValidator ID="rfvUserType" runat="server" ControlToValidate="rdCmpName" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                </div>
                                <asp:Button ID="btnLogin" CssClass="btn" Text="Login" runat="server" OnClick="btnLogin_Click" />
                                <asp:CheckBox ID="chkPersistCookie" runat="server" AutoPostBack="false" Checked="true" Visible="false" />
                                <br />
                                <br />
                                <asp:Label ID="lblError" runat="server" Style="color: red;" Text=""></asp:Label>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="jumbotron">
        <p>
            www.carlsberggroup.com is operated by Carlsberg Breweries A/S, a company registered in Denmark with registered address Ny Carlsberg Vej 100, 1799 Copenhagen V, Denmark, (‘Carlsberg’, ‘we’ or ‘us’).  If you experience problems with the site or would like to comment on it, please feel free to contact us. 
                Any personal information made available by users shall be treated and protected in accordance with Carlsberg’s Privacy Policy.
                This legal notice is governed by Danish law and any dispute arising out of or in connection with this legal notice which cannot be resolved amicably shall be dealt with exclusively by the Danish courts.
                This site is provided on as “as is” and “as available” basis.  Carlsberg cannot guarantee that this site will operate continuously, without interruptions or be error free.  Carlsberg uses reasonable endeavours to ensure that site content is accurate, but makes no representation or warranty that any content is complete or up-to-date. No content is intended to amount to advice on which reliance should be placed.
        </p>
    </div>

    <section id="Section1">
        <!-- divider -->
        <div class="row">
            <div class="col-lg-12" style="width: 80%; margin-left: 10%;">
                <div class="solidline">
                </div>
            </div>
        </div>
    </section>

    <footer>
        <div class="container">
            <div class="row">
                <div class="col-lg-3">
                    <div class="widget">
                        <h4 class="widgetheading">Get in touch with us</h4>
                        <address>
                            <strong>Carlsberg Breweries A/S</strong><br>
                            100 Ny Carlsberg Vej<br>
                            1799 Copenhagen V, Denmark
                        </address>
                        <p>
                            <i class="icon-phone"></i>Phone: (+45) 3327 3300
                                <br>
                            <i class="icon-envelope-alt"></i>contact@carlsberg.com
                        </p>
                    </div>
                </div>
                <div class="col-lg-3">
                    <div class="widget">
                        <h5 class="widgetheading">Pages</h5>
                        <ul class="link-list">
                            <li><a href="Login.aspx">Home</a></li>
                            <li><a href="AboutUs.aspx?key=ol">About Us</a></li>
                            <li><a href="ContactUs.aspx?key=ol">Contact</a></li>
                            <li><a href="SignOut.aspx">Sign Out</a></li>                           
                        </ul>
                    </div>
                </div>
                <div class="col-lg-5">
                    <div class="widget">
                        <h5 class="widgetheading">Latest posts</h5>
                        <ul class="link-list">
                            <li>
                                <u><a href="http://www.carlsberggroup.com/media/news/Pages/MARCELDESAILLYLEADSCARLSBERGFANREVOLUTIONFORUEFAEURO2016%E2%84%A2.aspx">Marcel Desailly Leads Carlsberg Fan Revolution for UEFA EURO 2016™
                                        <br />
                                </a></u>17/05/2016 11:00
                            </li>
                            <li>
                                <u><a href="http://www.carlsberggroup.com/media/news/Pages/CarlsbergPoursFurtherSupportBehindLiverpoolFC.aspx">Carlsberg Pours Further Support Behind Liverpool FC
                                        <br />
                                </a></u>23/03/2016 15:00
                            </li>
                            <li><u><a href="http://www.carlsberggroup.com/media/news/Pages/IfCarlsbergDidChocolateBarsBeerBrandCreatesPop-UpBarMadefromRealChocolate.aspx">If Carlsberg Did Chocolate Bars: Carlsberg Creates Pop-Up Bar Made from Real Chocolate<br />
                            </a></u>23/03/2016 14:00
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <div id="sub-footer">
            <div class="container">
                <div class="row">
                    <div class="col-lg-6">
                        <div class="copyright">
                            <p>
                                <span>&copy; Tectura Corporation 2016 All right reserved. </span><a href="http://bootstraptaste.com/">TMS Web Portal</a> by Tectura India
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </footer>
    </div>
            <a href="#" class="scrollup"><i class="fa fa-angle-up active"></i></a>
    <!--
    ================================================== -->
    <!-- Placed at the end of the document so the pages load faster -->
    <script src="App_Themes/js/jquery.js"></script>
    <script src="App_Themes/js/jquery.easing.1.3.js"></script>
    <script src="App_Themes/js/bootstrap.min.js"></script>
    <script src="App_Themes/js/jquery.fancybox.pack.js"></script>
    <script src="App_Themes/js/jquery.fancybox-media.js"></script>
    <script src="App_Themes/js/google-code-prettify/prettify.js"></script>
    <script src="App_Themes/js/portfolio/jquery.quicksand.js"></script>
    <script src="App_Themes/js/portfolio/setting.js"></script>
    <script src="App_Themes/js/jquery.flexslider.js"></script>
    <script src="App_Themes/js/animate.js"></script>
    <script src="App_Themes/js/custom.js"></script>
</body>
</html>
