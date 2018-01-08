using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RESTService
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        [WebGet(UriTemplate = "Users", ResponseFormat = WebMessageFormat.Json)]
        List<User> GetUsers();

        [OperationContract]
        [WebGet(UriTemplate = "ShoppingLists/{ID}", ResponseFormat = WebMessageFormat.Json)]
        List<ShoppingList> GetShoppingLists(string ID);

        [OperationContract]
        [WebGet(UriTemplate = "Items/{IDs}", ResponseFormat = WebMessageFormat.Json)]
        List<Items> GetItems(string IDs);
        [OperationContract]
        [WebGet(UriTemplate = "Login/{username}/{password}", ResponseFormat = WebMessageFormat.Json)]
        int Login(string username, string password);
        [OperationContract]
        [WebGet(UriTemplate = "Registration/{username}/{password}/{Ime}/{Priimek}/{number}/{mail}", ResponseFormat = WebMessageFormat.Json)]
        bool Registration(string username, string password, string Ime, string Priimek, string number, string mail);
        [OperationContract]
        [WebGet(UriTemplate = "CreateNewShoppingList/{IDu}/{ImeSL}", ResponseFormat = WebMessageFormat.Json)]
        int CreateNewShopingList(string IDu, string ImeSL);
        [OperationContract]
        [WebGet(UriTemplate = "SaveItem/{IDs}/{Ime}//{IDdodal}/", ResponseFormat = WebMessageFormat.Json)]
        bool SaveItem(string IDs, string Ime, string IDdodal);
        [OperationContract]
        [WebGet(UriTemplate = "AddNewUserToSL/{IDs}/{Mail}", ResponseFormat = WebMessageFormat.Json)]
        void AddNewUserToSL(string IDs, string Mail);
        
        
    }


    [DataContract]
    public class Items
    {
        [DataMember]
        public int IDi { get; set; }
        [DataMember]
        public string Ime { get; set; }
        [DataMember]
        public string Cena { get; set; }
        [DataMember]
        public string ključKdoDodal { get; set; }
        [DataMember]
        public string KljučKdoKupil { get; set; }
        [DataMember]
        public string IDs { get; set; }
    }

    [DataContract]
    public class User
    {
        [DataMember]
        public int ID{ get; set; }
        [DataMember]
        public string Ime { get; set; }
        [DataMember]
        public string Priimek { get; set; }
        [DataMember]
        public string Telefon { get; set; }
        [DataMember]
        public string Mail { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }

    }

    [DataContract]
    public class ShoppingList
    {
        [DataMember]
        public int IDs { get; set; }
        [DataMember]
        public string Ime { get; set; }
    }

    [DataContract]
    public class Vmesna
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int IDs { get; set; }
    }

}
