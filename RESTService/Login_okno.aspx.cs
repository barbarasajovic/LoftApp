using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RESTService
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void login_clicked(object sender, EventArgs e)
        {
            //Service1.Login(uporabnisko.ToString(), geslo.ToString());
            WebRequest request = WebRequest.Create("http://loftapp.azurewebsites.net/Service1.svc/Login/" + uporabnisko.Text + "/" + geslo.Text + "");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();

            dynamic data = JObject.Parse(responseFromServer);
            if (data.ID == -1)
            {
                test.Text = "Napačno uporabniško ime ali geslo.";
            }
            else
            {
                Session["ID"] = data.ID;
                Response.Redirect("ShoppingLists.aspx");
            }
            //test.Text = data.ID;
        }

        protected void Register_click(object sender, EventArgs e)
        {
            Response.Redirect("Register_okno.aspx");
        }
    }
}