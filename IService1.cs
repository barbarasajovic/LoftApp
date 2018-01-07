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
        /*[WebGet(UriTemplate = "ShopingLists/{ID}", ResponseFormat = WebMessageFormat.Json)]
        int GetShoppingLists(string ID);*/

        [OperationContract]
        [WebGet(UriTemplate = "Items/{IDs}", ResponseFormat = WebMessageFormat.Json)]
        List<Items> GetItems(string IDs);
        [OperationContract]
        [WebGet(UriTemplate = "Login/{username}/{password}", ResponseFormat = WebMessageFormat.Json)]
        string Login(string username, string password);
        [OperationContract]
        [WebGet(UriTemplate = "Registration/{username}/{password}/{Ime}/{Priimek}/{number}/{mail}", ResponseFormat = WebMessageFormat.Json)]
        bool Registration(string username, string password, string Ime, string Priimek, string number, string mail);
        [OperationContract]
        [WebGet(UriTemplate = "CreateNewShopingList/{IDu}/{ImeSL}", ResponseFormat = WebMessageFormat.Json)]
        void CreateNewShopingList(string IDu, string ImeSL);
        [OperationContract]
        [WebGet(UriTemplate = "SaveItem/{IDs}/{Ime}/{Cena}/{IDdodal}", ResponseFormat = WebMessageFormat.Json)]
        void SaveItem(string IDs, string Ime, string Cena, string IDdodal, string IDkupu);
        [OperationContract]
        [WebGet(UriTemplate = "AddNewUserToSL/{IDs}/{Mail}", ResponseFormat = WebMessageFormat.Json)]
        void AddNewUserToSL(string IDs, string Mail);
        [OperationContract]
        [WebGet(UriTemplate = "RemoveSL/{ID}/{IDs}", ResponseFormat = WebMessageFormat.Json)]
        void RemoveSL(string ID, string IDs);
        [OperationContract]
        [WebGet(UriTemplate = "Buy/{ID}/{IDs}/{IDi}/{cena}", ResponseFormat = WebMessageFormat.Json)]
        void Buy(string ID, string IDs, string IDi, string cena);


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
        public int IDs { get; set; }
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
