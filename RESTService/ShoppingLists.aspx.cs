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
    public partial class ShoppingLists : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Session["ID"].ToString();
            WebRequest request = WebRequest.Create("http://loftapp.azurewebsites.net/Service1.svc/ShoppingLists/" + id);
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

            dynamic dar = JArray.Parse(responseFromServer);
            //dynamic data = JObject.Parse(responseFromServer);
            List<ShoppingList> vsi = new List<ShoppingList>();

            vsi.Add(new ShoppingList { IDs = dar[0].ID });
            vsi.Add(new ShoppingList { IDs = dar[1].ID });

            ListBox1.DataSource = vsi;
        }
    }
}