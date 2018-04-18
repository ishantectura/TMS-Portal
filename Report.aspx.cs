using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Net;
using Microsoft.Reporting.WebForms;

public partial class Report : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
           
            string str = Request.QueryString["Report"];
            //string LRNO="" ;
            //if (Request.QueryString["LRNO"].ToString()!="")
            //{
            //    LRNO = Request.QueryString["LRNO"].ToString();
            //}
            ReportViewer1.Width = 1570;
            ReportViewer1.Height = 1000;
            //Parametr to report
            ReportParameter[] reportParameterCollection = new ReportParameter[13];
            try
            {
                ReportViewer1.ProcessingMode = ProcessingMode.Remote;
                IReportServerCredentials irsc = new CustomReportCredentials("INNAV2013NASSerAcc", "IN76^2165b", "TecturaCorp");// e.g.: ("demo-001", "123456789", "ifc") username,password,domainname
                  ReportViewer1.ServerReport.ReportServerCredentials = irsc;
               ReportViewer1.ServerReport.ReportServerUrl = new Uri("http://indel-sqdev12nv/ReportServer");
                //ReportViewer1.ServerReport.ReportServerCredentials = irsc;
                //ReportViewer1.ServerReport.ReportServerUrl = new Uri(" http://ccnhkgdsql15/ReportServer");


                

                switch(str)

                {
                    case "PaymentOutstandingReport":
                        ReportViewer1.ServerReport.ReportPath = "/TMS_Portal_Reports/PaymentOutstandingReport"; //e.g.: /demo-001/test
                        reportParameterCollection[0] = new ReportParameter();
                        reportParameterCollection[0].Name="User";
                        reportParameterCollection[0].Values.Add(Session["UserName"].ToString());
                        ReportViewer1.ServerReport.SetParameters(reportParameterCollection[0]);
                        break;
                    case "PendingLRForProcessing":
                        ReportViewer1.ServerReport.ReportPath = "/TMS_Portal_Reports/PendingLRProcess"; //e.g.: /demo-001/test
                         reportParameterCollection[1] = new ReportParameter();
                        reportParameterCollection[1].Name="User";
                        reportParameterCollection[1].Values.Add(Session["UserName"].ToString());
                        ReportViewer1.ServerReport.SetParameters(reportParameterCollection[1]);
                        break;

                    case "LRRejectionByReason":
                        ReportViewer1.ServerReport.ReportPath = "/TMS_Portal_Reports/LrRejectionByReason"; //e.g.: /demo-001/test
                        reportParameterCollection[2] = new ReportParameter();
                        reportParameterCollection[2].Name="User";
                        reportParameterCollection[2].Values.Add(Session["UserName"].ToString());
                        ReportViewer1.ServerReport.SetParameters(reportParameterCollection[2]);
                        break;
                    case "PendingLrForPayment":
                        ReportViewer1.ServerReport.ReportPath = "/TMS_Portal_Reports/PendingLRforPayment"; //e.g.: /demo-001/test
                        reportParameterCollection[3] = new ReportParameter();
                        reportParameterCollection[3].Name="User";
                        reportParameterCollection[3].Values.Add(Session["UserName"].ToString());
                        ReportViewer1.ServerReport.SetParameters(reportParameterCollection[3]);

                        break;
                    case "CarlsbergPaymentAdherenceIndex":
                        ReportViewer1.ServerReport.ReportPath = "/TMS_Portal_Reports/CarlsbergPaymentAdherenceIndex"; //e.g.: /demo-001/test
                        reportParameterCollection[4] = new ReportParameter();
                        reportParameterCollection[4].Name="User";
                        reportParameterCollection[4].Values.Add(Session["UserName"].ToString());
                        ReportViewer1.ServerReport.SetParameters(reportParameterCollection[4]);

                        break;
                    case "VendorPaymentLedger":
                        ReportViewer1.ServerReport.ReportPath = "/TMS_Portal_Reports/VendorPaymentLedger"; //e.g.: /demo-001/test
                        reportParameterCollection[5] = new ReportParameter();
                        reportParameterCollection[5].Name="User";
                        reportParameterCollection[5].Values.Add(Session["UserName"].ToString());
                        ReportViewer1.ServerReport.SetParameters(reportParameterCollection[5]);

                        break;
                    case "VehiclePlacementReliabilitySLA":
                        ReportViewer1.ServerReport.ReportPath = "/TMS_Portal_Reports/VehiclePlacementReliabilitySLA"; //e.g.: /demo-001/test
                        reportParameterCollection[6] = new ReportParameter();
                        reportParameterCollection[6].Name="User";
                        reportParameterCollection[6].Values.Add(Session["UserName"].ToString());
                        ReportViewer1.ServerReport.SetParameters(reportParameterCollection[6]);

                        break;
                    case "PlantDetentionReportByTransporter":
                        ReportViewer1.ServerReport.ReportPath = "/TMS_Portal_Reports/ReportPlantDetentionByTransporter"; //e.g.: /demo-001/test
                        reportParameterCollection[7] = new ReportParameter();
                        reportParameterCollection[7].Name="User";
                        reportParameterCollection[7].Values.Add(Session["UserName"].ToString());
                        ReportViewer1.ServerReport.SetParameters(reportParameterCollection[7]);
                        break;
                    case "TransitTimeAdherence":
                        ReportViewer1.ServerReport.ReportPath = "/TMS_Portal_Reports/InTransitTimeAdherence"; //e.g.: /demo-001/test
                        reportParameterCollection[8] = new ReportParameter();
                        reportParameterCollection[8].Name="User";
                        reportParameterCollection[8].Values.Add(Session["UserName"].ToString());
                        ReportViewer1.ServerReport.SetParameters(reportParameterCollection[8]);
                        break;
                    case "FormforPhysicalLRManagementWhileSending":
                        ReportViewer1.ServerReport.ReportPath = "/TMS_Portal_Reports/FormForPhysicalLRManagementWhileSending"; //e.g.: /demo-001/test
                        reportParameterCollection[9] = new ReportParameter();
                        reportParameterCollection[9].Name="User";
                        reportParameterCollection[9].Values.Add(Session["UserName"].ToString());
                        ReportViewer1.ServerReport.SetParameters(reportParameterCollection[9]);

                        break;
                    case "FormForPhysicalLRManagementAfterReceivingfromCrown":
                        ReportViewer1.ServerReport.ReportPath = "/TMS_Portal_Reports/FormforphysicalLRManagementAfterReceivingfromCrown"; //e.g.: /demo-001/test
                        reportParameterCollection[10] = new ReportParameter();
                        reportParameterCollection[10].Name="User";
                        reportParameterCollection[10].Values.Add(Session["UserName"].ToString());
                        ReportViewer1.ServerReport.SetParameters(reportParameterCollection[10]);
                        break;

                    case "ReportDataDump":
                        ReportViewer1.ServerReport.ReportPath = "/TMS_Portal_Reports/ReportDataDump"; //e.g.: /demo-001/test
                        reportParameterCollection[11] = new ReportParameter();
                        reportParameterCollection[11].Name = "User";
                        reportParameterCollection[11].Values.Add(Session["UserName"].ToString());
                        ReportViewer1.ServerReport.SetParameters(reportParameterCollection[11]);
                        break;

                    case "TPTInvoiceReport":
                        ReportViewer1.ServerReport.ReportPath = "/TMS_Portal_Reports/TPTInvoiceReport"; //e.g.: /demo-001/test
                        reportParameterCollection[12] = new ReportParameter();
                        reportParameterCollection[12].Name = "LRNo";
                        reportParameterCollection[12].Values.Add(Session["LRNO"].ToString());
                        ReportViewer1.ServerReport.SetParameters(reportParameterCollection[12]);
                        break;

                   

                       
                }


                //ReportViewer1.ServerReport.ReportPath = "/TMS_Report_New/PendingLRforPayment"; //e.g.: /demo-001/test
                ReportViewer1.ServerReport.Refresh();

                //ReportParameter[] reportParameterCollection = new ReportParameter[1];       //Array size describes the number of paramaters.
                //reportParameterCollection[0] = new ReportParameter();
                //reportParameterCollection[0].Name = "";                                 //Give Your Parameter Name
                //reportParameterCollection[0].Values.Add("Seattle");                         //Pass Parametrs's value here.
                //ReportViewer1.ServerReport.SetParameters(reportParameterCollection);
                //ReportViewer1.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

        }
    }
}
public class CustomReportCredentials : IReportServerCredentials
{
    private string _UserName;
    private string _PassWord;
    private string _DomainName;

    public CustomReportCredentials(string UserName, string PassWord, string DomainName)
    {
        _UserName = UserName;
        _PassWord = PassWord;
        _DomainName = DomainName;
    }

    public System.Security.Principal.WindowsIdentity ImpersonationUser
    {
        get { return null; }
    }

    public ICredentials NetworkCredentials
    {
        get { return new NetworkCredential(_UserName, _PassWord, _DomainName); }
    }

    public bool GetFormsCredentials(out Cookie authCookie, out string user,
     out string password, out string authority)
    {
        authCookie = null;
        user = password = authority = null;
        return false;
    }
}