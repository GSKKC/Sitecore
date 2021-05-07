using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Analytics;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.Configuration;

namespace Coreblimey.Utils
{
    public partial class CreateContact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCreateContact_Click(object sender, EventArgs e)
        {
            using (XConnectClient client = SitecoreXConnectClientConfiguration.GetClient())
                {
                    //Not including disposable, requires extra ref.

                    var source = "CreateContactUtil";
                    if (Tracker.Current == null)
                    {
                        Tracker.StartTracking();
                    }
                    if (Tracker.Current != null)
                    {
                        var id = Tracker.Current.Contact.ContactId.ToString("N");
                        // Identify the user
                        Tracker.Current.Session.IdentifyAs(source, id);
                        // Add some interaction Points
                        Tracker.Current.Interaction.Value = int.Parse(txtEngagementPoints.Text);

                        // Update Personal Details
                        var personalInfo = Tracker.Current.Contact.GetFacet<Sitecore.Analytics.Model.Entities.IContactPersonalInfo>("Personal");
                        personalInfo.FirstName = txtFirstname.Text;
                        personalInfo.Surname = txtSurname.Text;
                        client.Submit();
                    }
                }                

            phCreated.Visible = true;
            phForm.Visible = false;
        }


        protected void btnEndSession_Click(object sender, EventArgs e)
        {
            // Abandon Session and flush to Mongo
            Session.Abandon();

            phSessionCleared.Visible = true;
            phForm.Visible = false;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            phForm.Visible = true;
            phSessionCleared.Visible = false;
            phCreated.Visible = false;
        }
    }
}