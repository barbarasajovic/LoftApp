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
    public partial class Register_okno : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Register_clicked(object sender, EventArgs e)
        {
            WebRequest request = WebRequest.Create("http://loftapp.azurewebsites.net/Service1.svc/Register/" + uporabnisko.Text + "/" + geslo.Text + "/"+ ime.Text +"/"+ priimk.Text + "/"+ stevilka.Text+ "/" + mail.Text);
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
                error.Text = "Napaka pri registraciji. Uporabnik s tem uporabniškim imenom ali s tem geslom že obstaja.";
            }
            else
            Response.Redirect("Login_okno.aspx");
        }

        protected void nazaj_prijava(object sender, EventArgs e)
        {
            Response.Redirect("Login_okno.aspx");
        }
    }
}