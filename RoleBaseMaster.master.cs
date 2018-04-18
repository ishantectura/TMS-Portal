using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RoleBaseMaster : System.Web.UI.MasterPage
{

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["UserName"] != null || Session["isLogin"] != null || Session["UserType"] != null)
        {

        }
        else
        {
            Response.Redirect("SignOut.aspx");
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string UserType;

            UserType = Session["UserType"].ToString();
            lblWelcomeUser.Text = "Welcome  " + Session["UserType"].ToString() + " !!";
            //Transporter or CIPL or ThirdParty

            switch (UserType)
            {
                case "CIPL":

                    //  Side Menu item
                    MenuItem menuItemPendingRequestListCIPL = new MenuItem("Pending Request", "Pending Request List ", null, "~/PendingRequest.aspx");
                    MenuItem menuItmRequestPlacedRequestCIPL = new MenuItem("Placed Request", "Placed Request", null, "~/PlacedRequest.aspx");
                    MenuItem menuItmCancelRejectRequestCIPL = new MenuItem("Canceled & Rejected Requests", "Canceled & Rejected Requests", null, "~/CanceledRejectedRequest.aspx");
                   // MenuItem menuItemLRReadyForUploadingCIPL = new MenuItem("LR ready for uploading", "LR ready for uploading", null, "~/LRReadyForUploading.aspx");
                   // MenuItem menuItemLRReadyForInvoiceCIPL = new MenuItem("LR ready  for invoice", "LR ready for invoice", null, "~/LRReadyForInvoice.aspx");
                    MenuItem menuItemInvoicedLRCIPL = new MenuItem("Invoiced LR", "Invoiced LR", null, "~/InvoiceLR.aspx"); 
                   // MenuItem menuItemTransporterLedgerCIPL = new MenuItem("Transporter Ledger", "Transporter Ledger ", null, "~/PendingRequest.aspx");
                    MenuItem menuItemMasterDataCIPL = new MenuItem("Master Data", "Master Data ", null, "~/MasterData.aspx");
                    MenuItem menuItemGateEntryCIPL = new MenuItem("Gate Entry", "Gate Entry ", null, "~/GateEntry.aspx");
                    MenuItem menuItemPendingForGateEntry = new MenuItem("Pending For Gate Entry", "Pending For Gate Entry", null, "~/PendingForGateEntry.aspx");

                    // header menu item 
                    MenuItem menuItemHomeCIPL = new MenuItem("Home", "Home", null, "~/Dashboard.aspx");
                    MenuItem menuItemAgreementdCIPL = new MenuItem("Agreements", "Agreements", null, "~/Agreements.aspx");
                    MenuItem menuItemReportCIPL = new MenuItem("Report", "Report", null, "#");

                    //adding first submenuitem
                     MenuItem submenuitemCIPL1 = new MenuItem();
                    menuItemReportCIPL.ChildItems.Add(submenuitemCIPL1);
                    submenuitemCIPL1.Text = "Payment Outstanding Report";
                    submenuitemCIPL1.Target = "_blank";
                    submenuitemCIPL1.NavigateUrl = "~/Report.aspx?Report=" + "PaymentOutstandingReport";


                    MenuItem submenuitemCIPL11 = new MenuItem();
                    menuItemReportCIPL.ChildItems.Add(submenuitemCIPL11);
                    submenuitemCIPL11.Text = "Pending LR for Processing";
                    submenuitemCIPL11.Target = "_blank";
                    submenuitemCIPL11.NavigateUrl = "~/Report.aspx?Report=" + "PendingLRForProcessing";

                      MenuItem submenuitemCIPL12 = new MenuItem();
                    menuItemReportCIPL.ChildItems.Add(submenuitemCIPL12);
                    submenuitemCIPL12.Text = "Report data dump";
                    submenuitemCIPL12.Target = "_blank";
                    submenuitemCIPL12.NavigateUrl = "~/Report.aspx?Report=" + "ReportDataDump";


                    //adding second submenuitem 
                    MenuItem submenuitemCIPL2 = new MenuItem();
                    menuItemReportCIPL.ChildItems.Add(submenuitemCIPL2);
                    submenuitemCIPL2.Text = "LR Rejection by Reason";
                    submenuitemCIPL2.Target = "_blank";
                    submenuitemCIPL2.NavigateUrl = "~/Report.aspx?Report=" + "LRRejectionByReason";



                    MenuItem submenuitemCIPL3 = new MenuItem();
                    menuItemReportCIPL.ChildItems.Add(submenuitemCIPL3);
                    submenuitemCIPL3.Text = "Pending LR for Payment";
                    submenuitemCIPL3.Target = "_blank";
                    submenuitemCIPL3.NavigateUrl = "~/Report.aspx?Report=" + "PendingLrForPayment";
                    
                    MenuItem submenuitemCIPL4 = new MenuItem();
                    menuItemReportCIPL.ChildItems.Add(submenuitemCIPL4);
                    submenuitemCIPL4.Text = "Carlsberg Payment Adherence Index";
                    submenuitemCIPL4.Target = "_blank";
                    submenuitemCIPL4.NavigateUrl = "~/Report.aspx?Report=" + "CarlsbergPaymentAdherenceIndex";

                    MenuItem submenuitemCIPL5 = new MenuItem();
                    menuItemReportCIPL.ChildItems.Add(submenuitemCIPL5);
                    submenuitemCIPL5.Text = "Vendor Payment Ledger";
                    submenuitemCIPL5.Target = "_blank";
                    submenuitemCIPL5.NavigateUrl = "~/Report.aspx?Report=" + "VendorPaymentLedger";
                 
                    MenuItem submenuitemCIPL6 = new MenuItem();
                    menuItemReportCIPL.ChildItems.Add(submenuitemCIPL6);
                    submenuitemCIPL6.Text = "Vehicle Placement Reliability SLA";
                    submenuitemCIPL6.Target = "_blank";
                    submenuitemCIPL6.NavigateUrl = "~/Report.aspx?Report=" + "VehiclePlacementReliabilitySLA";
                    
                    MenuItem submenuitemCIPL7 = new MenuItem();
                    menuItemReportCIPL.ChildItems.Add(submenuitemCIPL7);
                    submenuitemCIPL7.Text = "Plant Detention Report by transporter";
                    submenuitemCIPL7.Target = "_blank";
                    submenuitemCIPL7.NavigateUrl = "~/Report.aspx?Report=" + "PlantDetentionReportByTransporter";
                   
                    MenuItem submenuitemCIPL8 = new MenuItem();
                    menuItemReportCIPL.ChildItems.Add(submenuitemCIPL8);
                    submenuitemCIPL8.Text = "Transit Time adherence";
                    submenuitemCIPL8.Target = "_blank";
                    submenuitemCIPL8.NavigateUrl = "~/Report.aspx?Report=" + "TransitTimeAdherence";
                   
                    MenuItem submenuitemCIPL9 = new MenuItem();
                    menuItemReportCIPL.ChildItems.Add(submenuitemCIPL9);
                    submenuitemCIPL9.Text = "Form for physical LR Management while sending";
                    submenuitemCIPL9.Target = "_blank";
                    submenuitemCIPL9.NavigateUrl = "~/Report.aspx?Report=" + "FormforPhysicalLRManagementWhileSending";
                    
                    MenuItem submenuitemCIPL10 = new MenuItem();
                    menuItemReportCIPL.ChildItems.Add(submenuitemCIPL10);
                    submenuitemCIPL10.Text = "Form for physical LR Management after receiving from Crown";
                    submenuitemCIPL10.Target = "_blank";
                    submenuitemCIPL10.NavigateUrl = "~/Report.aspx?Report=" + "FormForPhysicalLRManagementAfterReceivingfromCrown";

                   

                    MenuItem menuItemAboutusCIPL = new MenuItem("About Us", "AboutUs", null, "~/AboutUs.aspx");
                    MenuItem menuItemContactusCIPL = new MenuItem("Contact Us", "Contactus", null, "~/ContactUs.aspx");
                    MenuItem menuItemSignoutCIPL = new MenuItem("Sign Out", "Signout", null, "~/SignOut.aspx");

                    // bind side menuitem
                    menuItem.Items.Add(menuItemPendingRequestListCIPL);
                    menuItem.Items.Add(menuItmCancelRejectRequestCIPL);
                    menuItem.Items.Add(menuItemMasterDataCIPL);
                    menuItem.Items.Add(menuItmRequestPlacedRequestCIPL);
                   // menuItem.Items.Add(menuItemLRReadyForUploadingCIPL);
                   // menuItem.Items.Add(menuItemLRReadyForInvoiceCIPL);
                    menuItem.Items.Add(menuItemInvoicedLRCIPL);
                   // menuItem.Items.Add(menuItemTransporterLedgerCIPL);
                    menuItem.Items.Add(menuItemGateEntryCIPL);
                    menuItem.Items.Add(menuItemPendingForGateEntry);


                    // bind header menuitem
                    menuItemHeader.Items.Add(menuItemHomeCIPL);
                    menuItemHeader.Items.Add(menuItemAgreementdCIPL);
                    menuItemHeader.Items.Add(menuItemReportCIPL);
                    menuItemHeader.Items.Add(menuItemAboutusCIPL);
                    menuItemHeader.Items.Add(menuItemContactusCIPL);
                    menuItemHeader.Items.Add(menuItemSignoutCIPL);

                    break;
                case "Transporter":

                    // Side menu Item
                    MenuItem menuItemPendingRequestListTrans = new MenuItem("Pending Request", "Pending Request List ", null, "~/PendingRequest.aspx");
                    MenuItem menuItmRequestReadyforPlacementTrans = new MenuItem("Request Ready for Placement", "Request Ready for Placement", null, "~/ReadyForPlacement.aspx");
                    MenuItem menuItmCancelRejectRequestTrans = new MenuItem("Canceled & Rejected Requests", "Canceled & Rejected Requests", null, "~/CanceledRejectedRequest.aspx");
                    MenuItem menuItmRequestPlacedRequestTrans = new MenuItem("Placed Request", "Placed Request", null, "~/PlacedRequest.aspx");
                    MenuItem menuItemTrackVehicleTrans = new MenuItem("Track Vehicle", "Track Vehicle", null, "~/TrackVehicle.aspx");
                    MenuItem menuItemLRReadyForUploadingTrans = new MenuItem("LR ready for uploading", "LR ready for uploading", null, "~/LRReadyForUploading.aspx");
                    MenuItem menuItemDisputedLRTrans = new MenuItem( "Disputed LR ","Disputed LR", null, "~/DisputedLR.aspx");

                    MenuItem menuItemLRReadyForInvoiceTrans = new MenuItem("LR ready for invoice", "LR ready for invoice", null, "~/LRReadyForInvoice.aspx");
                    MenuItem menuItemInvoicedLRTrans = new MenuItem("Invoiced LR", "Invoiced LR", null, "~/InvoiceLR.aspx"); 
                    MenuItem menuItemTransporterLedgerTrans = new MenuItem("Transporter Ledger", "Transporter Ledger ", null, "~/TransporterLedger.aspx");
                    MenuItem menuItemPendingForGateEntryTrans = new MenuItem("Pending For Gate Entry", "Pending For Gate Entry", null, "~/PendingForGateEntry.aspx");
                   

                    // Header menu item
                    MenuItem menuItemHomeTrans = new MenuItem("Home", "Home", null, "~/Dashboard.aspx");
                    MenuItem menuItemAgreementdTrans = new MenuItem("Agreements", "Agreements", null, "~/Agreements.aspx");
                    MenuItem menuItemReportTrans = new MenuItem("Report", "Report", null, "#");
                    
                    //adding first submenuitem
                     MenuItem submenuitemTransporter11 = new MenuItem();
                    menuItemReportTrans.ChildItems.Add(submenuitemTransporter11);
                    submenuitemTransporter11.Text = "Payment Outstanding Report";
                    submenuitemTransporter11.Target = "_blank";
                    submenuitemTransporter11.NavigateUrl = "~/Report.aspx?Report=" + "PaymentOutstandingReport";


                     MenuItem submenuitemTransporter12 = new MenuItem();
                    menuItemReportTrans.ChildItems.Add(submenuitemTransporter12);
                    submenuitemTransporter12.Text = "Report data dump";
                    submenuitemTransporter12.Target = "_blank";
                    submenuitemTransporter12.NavigateUrl = "~/Report.aspx?Report=" + "ReportDataDump";

                    MenuItem submenuitemTransporter1 = new MenuItem();
                    menuItemReportTrans.ChildItems.Add(submenuitemTransporter1);
                    submenuitemTransporter1.Text = "Pending LR for Processing";
                    submenuitemTransporter1.Target = "_blank";
                    submenuitemTransporter1.NavigateUrl = "~/Report.aspx?Report=" + "PendingLRForProcessing";


                    //adding second submenuitem 
                    MenuItem submenuitemTransporter2 = new MenuItem();
                    menuItemReportTrans.ChildItems.Add(submenuitemTransporter2);
                    submenuitemTransporter2.Text = "LR Rejection by Reason";
                    submenuitemTransporter2.Target = "_blank";
                    submenuitemTransporter2.NavigateUrl = "~/Report.aspx?Report=" + "LRRejectionByReason";



                    MenuItem submenuitemTransporter3 = new MenuItem();
                    menuItemReportTrans.ChildItems.Add(submenuitemTransporter3);
                    submenuitemTransporter3.Text = "Pending LR for Payment";
                    submenuitemTransporter3.Target = "_blank";
                    submenuitemTransporter3.NavigateUrl = "~/Report.aspx?Report=" + "PendingLrForPayment";

                    MenuItem submenuitemTransporter4 = new MenuItem();
                    menuItemReportTrans.ChildItems.Add(submenuitemTransporter4);
                    submenuitemTransporter4.Text = "Carlsberg Payment Adherence Index";
                    submenuitemTransporter4.Target = "_blank";
                    submenuitemTransporter4.NavigateUrl = "~/Report.aspx?Report=" + "CarlsbergPaymentAdherenceIndex";

                    MenuItem submenuitemTransporter5 = new MenuItem();
                    menuItemReportTrans.ChildItems.Add(submenuitemTransporter5);
                    submenuitemTransporter5.Text = "Vendor Payment Ledger";
                    submenuitemTransporter5.Target = "_blank";
                    submenuitemTransporter5.NavigateUrl = "~/Report.aspx?Report=" + "VendorPaymentLedger";

                    MenuItem submenuitemTransporter6 = new MenuItem();
                    menuItemReportTrans.ChildItems.Add(submenuitemTransporter6);
                    submenuitemTransporter6.Text = "Vehicle Placement Reliability SLA";
                    submenuitemTransporter6.Target = "_blank";
                    submenuitemTransporter6.NavigateUrl = "~/Report.aspx?Report=" + "VehiclePlacementReliabilitySLA";

                    MenuItem submenuitemTransporter7 = new MenuItem();
                    menuItemReportTrans.ChildItems.Add(submenuitemTransporter7);
                    submenuitemTransporter7.Text = "Plant Detention Report by transporter";
                    submenuitemTransporter7.Target = "_blank";
                    submenuitemTransporter7.NavigateUrl = "~/Report.aspx?Report=" + "PlantDetentionReportByTransporter";

                    MenuItem submenuitemTransporter8 = new MenuItem();
                    menuItemReportTrans.ChildItems.Add(submenuitemTransporter8);
                    submenuitemTransporter8.Text = "Transit Time adherence";
                    submenuitemTransporter8.Target = "_blank";
                    submenuitemTransporter8.NavigateUrl = "~/Report.aspx?Report=" + "TransitTimeAdherence";

                    MenuItem submenuitemTransporter9 = new MenuItem();
                    menuItemReportTrans.ChildItems.Add(submenuitemTransporter9);
                    submenuitemTransporter9.Text = "Form for physical LR Management while sending";
                    submenuitemTransporter9.Target = "_blank";
                    submenuitemTransporter9.NavigateUrl = "~/Report.aspx?Report=" + "FormforPhysicalLRManagementWhileSending";

                    MenuItem submenuitemTransporter10 = new MenuItem();
                    menuItemReportTrans.ChildItems.Add(submenuitemTransporter10);
                    submenuitemTransporter10.Text = "Form for physical LR Management after receiving from Crown";
                    submenuitemTransporter10.Target = "_blank";
                    submenuitemTransporter10.NavigateUrl = "~/Report.aspx?Report=" + "FormForPhysicalLRManagementAfterReceivingfromCrown";

                    MenuItem menuItemAboutusTrans = new MenuItem("About Us", "AboutUs", null, "~/AboutUs.aspx");
                    MenuItem menuItemContactusTrans = new MenuItem("Contact Us", "Contactus", null, "~/ContactUs.aspx");
                    MenuItem menuItemSignoutTrans = new MenuItem("Sign Out", "Signout", null, "~/SignOut.aspx");

                    // bind side menuitem
                    menuItem.Items.Add(menuItemPendingRequestListTrans);                
                    menuItem.Items.Add(menuItmRequestReadyforPlacementTrans);
                    menuItem.Items.Add(menuItmCancelRejectRequestTrans);
                    menuItem.Items.Add(menuItmRequestPlacedRequestTrans);
                    menuItem.Items.Add(menuItemTrackVehicleTrans);
                    menuItem.Items.Add(menuItemLRReadyForUploadingTrans);
                    menuItem.Items.Add(menuItemDisputedLRTrans);
                    menuItem.Items.Add(menuItemLRReadyForInvoiceTrans);
                    menuItem.Items.Add(menuItemInvoicedLRTrans);
                    menuItem.Items.Add(menuItemTransporterLedgerTrans);
                    menuItem.Items.Add(menuItemPendingForGateEntryTrans);
                    

                    // bind header menuitem
                    menuItemHeader.Items.Add(menuItemHomeTrans);
                    menuItemHeader.Items.Add(menuItemAgreementdTrans);
                    menuItemHeader.Items.Add(menuItemReportTrans);
                    menuItemHeader.Items.Add(menuItemAboutusTrans);
                    menuItemHeader.Items.Add(menuItemContactusTrans);
                    menuItemHeader.Items.Add(menuItemSignoutTrans);
                    break;
                case "Third Party":

                    // Side menu item
                    MenuItem menuItemPendigLRThirdParty = new MenuItem("LR Ready to be verified", "LR Ready to be verified", null, "~/LRReadyForUploading.aspx");
                    //MenuItem menuItemAcceptedLRThirdParty = new MenuItem("Accepted LR", "Accepted LR", null, "~/AcceptedLR.aspx");
                    MenuItem menuItemInvoicedLRThirdParty = new MenuItem("Invoiced LR", "Invoiced LR", null, "~/InvoiceLR.aspx");
                    MenuItem menuItemTransporterLedgerThirdParty = new MenuItem("Transporter Ledger", "Transporter Ledger ", null, "~/TransporterLedger.aspx");
                    //MenuItem menuItemTrackVehicleThirdParty = new MenuItem("Track Vehicle", "Track Vehicle", null, "~/TrackVehicle.aspx");
                    MenuItem menuItemdisputedLrThirdParty = new MenuItem("Disputed Lr", "Disputed Lr", null, "~/DisputedLR.aspx");


                    // Header menu item
                    MenuItem menuItemHomeThirdparty = new MenuItem("Home", "Home", null, "~/Dashboard.aspx");
                    MenuItem menuItemAgreementdThirdparty = new MenuItem("Agreements", "Agreements", null, "~/Agreements.aspx");
                    MenuItem menuItemReportThirdparty = new MenuItem("Report", "Report", null, "#");

                    //adding first submenuitem

                    MenuItem submenuitemthird8 = new MenuItem();
                    menuItemReportThirdparty.ChildItems.Add(submenuitemthird8);
                    submenuitemthird8.Text = "Payment Outstanding Report";
                    submenuitemthird8.Target = "_blank";
                    submenuitemthird8.NavigateUrl = "~/Report.aspx?Report=" + "PaymentOutstandingReport";

                     MenuItem submenuitemthird9 = new MenuItem();
                    menuItemReportThirdparty.ChildItems.Add(submenuitemthird9);
                    submenuitemthird9.Text = "Report data dump";
                    submenuitemthird9.Target = "_blank";
                    submenuitemthird9.NavigateUrl = "~/Report.aspx?Report=" + "ReportDataDump";


                    MenuItem submenuitemthird1 = new MenuItem();
                    menuItemReportThirdparty.ChildItems.Add(submenuitemthird1);
                    submenuitemthird1.Text = "Pending LR for processing";
                    submenuitemthird1.Target = "_blank";
                    submenuitemthird1.NavigateUrl = "~/Report.aspx?Report=" + "FormForPhysicalLRManagementAfterReceivingfromCrown";

                    //adding second submenuitem 
                    MenuItem submenuitemthird2 = new MenuItem();
                    menuItemReportThirdparty.ChildItems.Add(submenuitemthird2);
                    submenuitemthird2.Text = "LR Rejection by Reason";
                   submenuitemthird2.Target = "_blank";
                   submenuitemthird2.NavigateUrl = "~/Report.aspx?Report=" + "LRRejectionbyReason";



                     MenuItem submenuitemthird3 = new MenuItem();
                    menuItemReportThirdparty.ChildItems.Add(submenuitemthird3);
                    submenuitemthird3.Text = "Pending LR for Payment";
                   submenuitemthird3.Target = "_blank";
                   submenuitemthird3.NavigateUrl = "~/Report.aspx?Report=" + "PendingLRforPayment";

                     MenuItem submenuitemthird4 = new MenuItem();
                    menuItemReportThirdparty.ChildItems.Add(submenuitemthird4);
                    submenuitemthird4.Text = "Carlsberg Payment Adherence Index";
                   submenuitemthird4.Target = "_blank";
                   submenuitemthird4.NavigateUrl = "~/Report.aspx?Report=" + "CarlsbergPaymentAdherence Index";

                     MenuItem submenuitemthird5 = new MenuItem();
                    menuItemReportThirdparty.ChildItems.Add(submenuitemthird5);
                    submenuitemthird5.Text = "Vendor Payment Ledger";
                   submenuitemthird5.Target = "_blank";
                   submenuitemthird5.NavigateUrl = "~/Report.aspx?Report=" + "VendorPaymentLedger";

                     MenuItem submenuitemthird6 = new MenuItem();
                    menuItemReportThirdparty.ChildItems.Add(submenuitemthird6);
                    submenuitemthird6.Text = "Vehicle Placement Reliability SLA ";
                   submenuitemthird6.Target = "_blank";
                   submenuitemthird6.NavigateUrl = "~/Report.aspx?Report=" + "VehiclePlacementReliabilitySLA";

                    MenuItem submenuitemthird7 = new MenuItem();
                    menuItemReportThirdparty.ChildItems.Add(submenuitemthird7);
                    submenuitemthird7.Text = "Transit Time adherence";
                   submenuitemthird7.Target = "_blank";
                   submenuitemthird7.NavigateUrl = "~/Report.aspx?Report=" + "TransitTimeAdherence";

                    MenuItem menuItemAboutusThirdparty = new MenuItem("About Us", "AboutUs", null, "~/AboutUs.aspx?key=rd");
                    MenuItem menuItemContactusThirdparty = new MenuItem("Contact Us", "Contactus", null, "~/ContactUs.aspx?key=rd");
                    MenuItem menuItemSignoutThirdparty = new MenuItem("Sign Out", "Signout", null, "~/SignOut.aspx");
                    
                    menuItem.Items.Add(menuItemPendigLRThirdParty);
                    //menuItem.Items.Add(menuItemAcceptedLRThirdParty);
                    menuItem.Items.Add(menuItemInvoicedLRThirdParty);
                    menuItem.Items.Add(menuItemTransporterLedgerThirdParty);
                    //menuItem.Items.Add(menuItemTrackVehicleThirdParty);

                    menuItem.Items.Add(menuItemdisputedLrThirdParty);

                    
                    // bind header menuitem
                    menuItemHeader.Items.Add(menuItemHomeThirdparty);
                    menuItemHeader.Items.Add(menuItemAgreementdThirdparty);
                    menuItemHeader.Items.Add(menuItemReportThirdparty);
                    menuItemHeader.Items.Add(menuItemAboutusThirdparty);
                    menuItemHeader.Items.Add(menuItemContactusThirdparty);
                    menuItemHeader.Items.Add(menuItemSignoutThirdparty);
                    break;


                case "Admin":
                    // Side menu Item
                    MenuItem menuItemPendingRequestListAdmin = new MenuItem("Pending Request", "Pending Request List ", null, "~/PendingRequest.aspx");
                    MenuItem menuItmRequestReadyforPlacementAdmin = new MenuItem("Request Ready for Placement", "Request Ready for Placement", null, "~/ReadyForPlacement.aspx");
                    MenuItem menuItmCancelRejectRequestAdmin = new MenuItem("Canceled & Rejected Requests", "Canceled & Rejected Requests", null, "~/CanceledRejectedRequest.aspx");
                    MenuItem menuItmRequestPlacedRequestAdmin = new MenuItem("Placed Request", "Placed Request", null, "~/PlacedRequest.aspx");
                    MenuItem menuItemTrackVehicleAdmin = new MenuItem("Track Vehicle", "Track Vehicle", null, "~/TrackVehicle.aspx");
                    MenuItem menuItemLRReadyForUploadingAdmin = new MenuItem("LR ready for uploading", "LR ready for uploading", null, "~/LRReadyForUploading.aspx");
                    MenuItem menuItemDisputedLRAdmin = new MenuItem("Disputed LR ", "Disputed LR", null, "~/DisputedLR.aspx");

                    MenuItem menuItemLRReadyForInvoiceAdmin = new MenuItem("LR ready for invoice", "LR ready for invoice", null, "~/LRReadyForInvoice.aspx");
                    MenuItem menuItemInvoicedLRAdmin = new MenuItem("Invoiced LR", "Invoiced LR", null, "~/InvoiceLR.aspx");
                    MenuItem menuItemTransporterLedgerAdmin = new MenuItem("Transporter Ledger", "Transporter Ledger ", null, "~/TransporterLedger.aspx");


                    // Header menu item
                    MenuItem menuItemHomeAdmin = new MenuItem("Home", "Home", null, "~/Dashboard.aspx");
                    MenuItem menuItemAgreementdAdmin = new MenuItem("Agreements", "Agreements", null, "~/Agreements.aspx");
                    MenuItem menuItemReportAdmin = new MenuItem("Report", "Report", null, "#");

                    //adding first submenuitem
                    MenuItem submenuitemAdmin11 = new MenuItem();
                    menuItemReportAdmin.ChildItems.Add(submenuitemAdmin11);
                    submenuitemAdmin11.Text = "Payment Outstanding Report";
                    submenuitemAdmin11.Target = "_blank";
                    submenuitemAdmin11.NavigateUrl = "~/Report.aspx?Report=" + "PaymentOutstandingReport";


                    MenuItem submenuitemAdmin12 = new MenuItem();
                    menuItemReportAdmin.ChildItems.Add(submenuitemAdmin12);
                    submenuitemAdmin12.Text = "Report data dump";
                    submenuitemAdmin12.Target = "_blank";
                    submenuitemAdmin12.NavigateUrl = "~/Report.aspx?Report=" + "ReportDataDump";

                    MenuItem submenuitemAdmin1 = new MenuItem();
                    menuItemReportAdmin.ChildItems.Add(submenuitemAdmin1);
                    submenuitemAdmin1.Text = "Pending LR for Processing";
                    submenuitemAdmin1.Target = "_blank";
                    submenuitemAdmin1.NavigateUrl = "~/Report.aspx?Report=" + "PendingLRForProcessing";


                    //adding second submenuitem 
                    MenuItem submenuitemAdmin2 = new MenuItem();
                    menuItemReportAdmin.ChildItems.Add(submenuitemAdmin2);
                    submenuitemAdmin2.Text = "LR Rejection by Reason";
                    submenuitemAdmin2.Target = "_blank";
                    submenuitemAdmin2.NavigateUrl = "~/Report.aspx?Report=" + "LRRejectionByReason";



                    MenuItem submenuitemAdmin3 = new MenuItem();
                    menuItemReportAdmin.ChildItems.Add(submenuitemAdmin3);
                    submenuitemAdmin3.Text = "Pending LR for Payment";
                    submenuitemAdmin3.Target = "_blank";
                    submenuitemAdmin3.NavigateUrl = "~/Report.aspx?Report=" + "PendingLrForPayment";

                    MenuItem submenuitemAdmin4 = new MenuItem();
                    menuItemReportAdmin.ChildItems.Add(submenuitemAdmin4);
                    submenuitemAdmin4.Text = "Carlsberg Payment Adherence Index";
                    submenuitemAdmin4.Target = "_blank";
                    submenuitemAdmin4.NavigateUrl = "~/Report.aspx?Report=" + "CarlsbergPaymentAdherenceIndex";

                    MenuItem submenuitemAdmin5 = new MenuItem();
                    menuItemReportAdmin.ChildItems.Add(submenuitemAdmin5);
                    submenuitemAdmin5.Text = "Vendor Payment Ledger";
                    submenuitemAdmin5.Target = "_blank";
                    submenuitemAdmin5.NavigateUrl = "~/Report.aspx?Report=" + "VendorPaymentLedger";

                    MenuItem submenuitemAdmin6 = new MenuItem();
                    menuItemReportAdmin.ChildItems.Add(submenuitemAdmin6);
                    submenuitemAdmin6.Text = "Vehicle Placement Reliability SLA";
                    submenuitemAdmin6.Target = "_blank";
                    submenuitemAdmin6.NavigateUrl = "~/Report.aspx?Report=" + "VehiclePlacementReliabilitySLA";

                    MenuItem submenuitemAdmin7 = new MenuItem();
                    menuItemReportAdmin.ChildItems.Add(submenuitemAdmin7);
                    submenuitemAdmin7.Text = "Plant Detention Report by transporter";
                    submenuitemAdmin7.Target = "_blank";
                    submenuitemAdmin7.NavigateUrl = "~/Report.aspx?Report=" + "PlantDetentionReportByTransporter";

                    MenuItem submenuitemAdmin8 = new MenuItem();
                    menuItemReportAdmin.ChildItems.Add(submenuitemAdmin8);
                    submenuitemAdmin8.Text = "Transit Time adherence";
                    submenuitemAdmin8.Target = "_blank";
                    submenuitemAdmin8.NavigateUrl = "~/Report.aspx?Report=" + "TransitTimeAdherence";

                    MenuItem submenuitemAdmin9 = new MenuItem();
                    menuItemReportAdmin.ChildItems.Add(submenuitemAdmin9);
                    submenuitemAdmin9.Text = "Form for physical LR Management while sending";
                    submenuitemAdmin9.Target = "_blank";
                    submenuitemAdmin9.NavigateUrl = "~/Report.aspx?Report=" + "FormforPhysicalLRManagementWhileSending";

                    MenuItem submenuitemAdmin10 = new MenuItem();
                    menuItemReportAdmin.ChildItems.Add(submenuitemAdmin10);
                    submenuitemAdmin10.Text = "Form for physical LR Management after receiving from Crown";
                    submenuitemAdmin10.Target = "_blank";
                    submenuitemAdmin10.NavigateUrl = "~/Report.aspx?Report=" + "FormForPhysicalLRManagementAfterReceivingfromCrown";

                    MenuItem menuItemAboutusAdmin = new MenuItem("About Us", "AboutUs", null, "~/AboutUs.aspx");
                    MenuItem menuItemContactusAdmin = new MenuItem("Contact Us", "Contactus", null, "~/ContactUs.aspx");
                    MenuItem menuItemSignoutAdmin = new MenuItem("Sign Out", "Signout", null, "~/SignOut.aspx");

                    // bind side menuitem
                     menuItem.Items.Add(menuItemPendingRequestListAdmin);
                    menuItem.Items.Add(menuItmRequestReadyforPlacementAdmin);
                    menuItem.Items.Add(menuItmCancelRejectRequestAdmin);
                    menuItem.Items.Add(menuItmRequestPlacedRequestAdmin);
                    menuItem.Items.Add(menuItemTrackVehicleAdmin);
                    menuItem.Items.Add(menuItemLRReadyForUploadingAdmin);
                    menuItem.Items.Add(menuItemDisputedLRAdmin);
                    menuItem.Items.Add(menuItemLRReadyForInvoiceAdmin);
                    menuItem.Items.Add(menuItemInvoicedLRAdmin);
                    //menuItem.Items.Add(menuItemAdminporterLedgerAdmin);
                    menuItem.Items.Add(menuItemTransporterLedgerAdmin);


                    // bind header menuitem
                    menuItemHeader.Items.Add(menuItemHomeAdmin);
                    menuItemHeader.Items.Add(menuItemAgreementdAdmin);
                    menuItemHeader.Items.Add(menuItemReportAdmin);
                    menuItemHeader.Items.Add(menuItemAboutusAdmin);
                    menuItemHeader.Items.Add(menuItemContactusAdmin);
                    menuItemHeader.Items.Add(menuItemSignoutAdmin);
                    break;

            }


        }
    }
}
