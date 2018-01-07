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
        [WebGet(UriTemplate = "ShopingLists/{ID}", ResponseFormat = WebMessageFormat.Json)]
        List<ShoppingList> GetShoppingLists(string ID);

        [OperationContract]
        [WebGet(UriTemplate = "Items/{IDs}", ResponseFormat = WebMessageFormat.Json)]
        List<Items> GetItems(string IDs);
        [OperationContract]
        [WebGet(UriTemplate = "Login/{username}/{password}", ResponseFormat = WebMessageFormat.Json)]
        string Login(string username, string password);
        [OperationContract]
        [WebInvoke(UriTemplate = "Registration/{username}/{password}/{Ime}/{Priimek}/{number}/{mail}", ResponseFormat = WebMessageFormat.Json, Method = "PUT")]
        bool Registration(string username, string password, string Ime, string Priimek, string number, string mail);
        [OperationContract]
        [WebInvoke(UriTemplate = "CreateNewShopingList/{IDu}/{ImeSL}", ResponseFormat = WebMessageFormat.Json, Method ="PUT")]
        void CreateNewShopingList(string IDu, string ImeSL);
        [OperationContract]
        [WebInvoke(UriTemplate = "SaveItem/{IDs}/{Ime}/{Cena}/{IDdodal}", ResponseFormat = WebMessageFormat.Json, Method = "PUT")]
        bool SaveItem(string IDs, string Ime, string Cena, string IDdodal, string IDkupu);
        [WebInvoke(UriTemplate = "AddNewUserToSL/{IDs}/{Mail}", ResponseFormat = WebMessageFormat.Json, Method = "PUT")]
        void AddNewUserToSL(string IDs, string Mail);
        [WebInvoke(UriTemplate = "RemoveSL/{ID}/{IDs}", ResponseFormat = WebMessageFormat.Json, Method = "DELETE")]
        void RemoveSL(string ID, string IDs);
        [WebInvoke(UriTemplate = "AddNewUserToSL/{ID}/{IDs}", ResponseFormat = WebMessageFormat.Json, Method = "DELETE")]
        void RemoveYouFromSL(string ID, string IDs);
        
    }


    [DataContract]
    public class Items
    {
        [DataMember]
        public string IDi { get; set; }
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
        public Int32 ID{ get; set; }
        [DataMember]
        public string Ime { get; set; }
        [DataMember]
        public string Priimek { get; set; }
        [DataMember]
        public string Telefon { get; set; }
        [DataMember]
        public string Mail { get; set; }

    }

    [DataContract]
    public class ShoppingList
    {
        [DataMember]
        public string IDs { get; set; }
        [DataMember]
        public string Ime { get; set; }
    }

    [DataContract]
    public class Vmesna
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string IDs { get; set; }
    }

}
