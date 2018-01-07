using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RESTService
{
    public class Service1 : IService1
    {
        public static string Authenticate(string username, string password)
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LoftAppConnectionString"].ConnectionString))
            {
                string cmd = "Select password from \"Uporabnik\" where username = @username";

                connection.Open();
                using (SqlCommand command = new SqlCommand(cmd, connection))
                {
                    command.Parameters.Add(new SqlParameter("@username", username));
                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        da.Fill(data);
                    }
                }

                connection.Close();
            }

            if (data.Rows.Count > 0)
            {
                string hashGesloTable = (string)data.Rows[0][0];

                //byte[] hashGeslo = ComputeHash.ComputeHashMD5(password);

                string hashGeslo = MD5Hash(password);

                if (hashGeslo.Equals(hashGesloTable))
                {
                    //
                    Random rnd = new Random();

                    int coo = rnd.Next(1, Int32.MaxValue);

                    return "cookie" + coo;

                }
                return null;

            }

            return null;
        }

        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }



        public List<Items> GetItems(string IDs)
        {
            var ret = new List<Items>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LoftAppConnectionString"].ConnectionString);
            conn.Open();
            string pridobi = "Select ID, Name from \"Items\" where @id = Sho_ID";
            SqlCommand comm = new SqlCommand(pridobi, conn);
            comm.Parameters.AddWithValue("@id", int.Parse(IDs));
            try {
                comm.ExecuteNonQuery();
                using (var command = comm)
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ret.Add(new Items { IDi = reader.GetInt32(0), Ime = reader.GetString(1) });
                            }   
                        }
                        else
                        {
                            throw new FaultException("Izbran Shopping List ne obstaja.");
                        }
                        conn.Close();
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw new Exception("Napaka");
            }

           
        }

        public List<ShoppingList> GetShoppingLists(string ID)
        {
            int ids = 0;
            var ret = new List<ShoppingList>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LoftAppConnectionString"].ConnectionString);
            conn.Open();
            string sql = "Select Sho_ID from \"ShoppingList_Users\" where ID = '" + int.Parse(ID) + "'";
            string sql2 = "Select Name from ShoppingList where ID = @id";
            SqlCommand comm = new SqlCommand(sql, conn);
            SqlCommand comma = new SqlCommand(sql2, conn);
            

            using (var command = comm)
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ids = reader.GetInt32(0);
                            comm.Parameters.AddWithValue("@id", ids);
                            using (var comman = comma)
                            {
                                using (var reader1 = comman.ExecuteReader())
                                {
                                    while (reader1.Read())
                                    {
                                        ret.Add(new ShoppingList { IDs = ids, Ime = reader.GetString(0) });
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new FaultException("Za danega uporabnika ne obstaja noben Shopping List.");
                    }
                    conn.Close();
                }
            }
            return ret;
        }

        public List<User> GetUsers()
        {
            var ret = new List<User>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LoftAppConnectionString"].ConnectionString);
            conn.Open();
            string sql = "SELECT ID, Name, Surname FROM \"User\"";
            SqlCommand comm = new SqlCommand(sql, conn);

            using (var command = comm)
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ret.Add(new User { ID = reader.GetInt32(0), Ime = reader.GetString(1), Priimek = reader.GetString(2) });
                            //ret.Add(new User { ID = reader.GetInt32(0) });

                           
                        }
                        conn.Close();
                    }
                    else
                    {
                        throw new FaultException("Noben uporabnik ne obstaja.");
                    }

                }

                return ret;
            }

        }

        public int Login(string username, string password)
        {
            string cookie = Authenticate(username, password);
            int id = 0;

            if (cookie != null)
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LoftAppConnectionString"].ConnectionString);
                conn.Open();
                string sql = "select ID from \"User\" where username = '@username'";
                SqlCommand comm = new SqlCommand(sql, conn);
                comm.Parameters.AddWithValue("@username", username);
                try
                {
                    comm.ExecuteNonQuery();
                    using (var command = comm)
                    {
                        using (var reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                id = reader.GetInt32(0);
                            }
                        }
                    }
                    return id;
                }
                catch (Exception)
                {
                    throw new Exception("Uporabnik s tem uporabniškim imenom ne obstaja.");
                }
            }
            else
            {
                throw new Exception("Neuspešna prijava.");
            }
        }

        public bool Registration(string username, string password, string Ime, string Priimek, string number, string mail)
        {
            try
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LoftAppConnectionString"].ConnectionString);
                conn.Open();
                string preveriUporabnika = "INSERT INTO \"User\" (Name,Surname, Phonenumber,Mail, Password, Username) VALUES (, @Ime, @Priimek, @number, @Mail, @Geslo, @Username)";
                SqlCommand comm = new SqlCommand(preveriUporabnika, conn);
                comm.Parameters.AddWithValue("@Username", username);
                comm.Parameters.AddWithValue("@Ime", Ime);
                comm.Parameters.AddWithValue("@Priimek", Priimek);
                comm.Parameters.AddWithValue("@Geslo", MD5Hash(password));
                comm.Parameters.AddWithValue("@Mail", mail);
                comm.Parameters.AddWithValue("@number", number);

                comm.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void CreateNewShopingList(string IDu, string ImeSL)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LoftAppConnectionString"].ConnectionString);
            conn.Open();
            int id = 0;
            string sql = "Insert into \"ShoppingList\" (Name) values (@ime)";
            string dobID = "select ID from \"ShoppingList\" where Name = @ime";
            string sql2 = "Insert into \"ShoppingList_Users\" (ID, Sho_ID) values (@id, @IDs)";
            SqlCommand comm = new SqlCommand(sql, conn);
            comm.Parameters.AddWithValue("@ime", ImeSL);
            SqlCommand comma = new SqlCommand(dobID, conn);
            comma.Parameters.AddWithValue("@ime", ImeSL);
            SqlCommand comman = new SqlCommand(sql2, conn);
            

             
            comm.ExecuteNonQuery();
            try
            {
                comm.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw new FaultException("Napaka pri izdelavi Shopping Lista.");
            }
            try
            {
                comma.ExecuteNonQuery();
                using (var command = comma)
                {
                    using (var reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            id = reader.GetInt32(0);
                            comman.Parameters.AddWithValue("id", int.Parse(IDu));
                            comman.Parameters.AddWithValue("@IDs", id);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new FaultException("Napaka pri izdelavi Shopping Lista.");
            }
            try
            {
                comman.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception)
            {
                throw new FaultException("Napaka pri posodobitvi baze.");
            }

        }

        public bool SaveItem(string IDs, string Ime, string Cena, string IDdodal, string IDkupu)
        {
            Cena = "0";
            IDkupu = null;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LoftAppConnectionString"].ConnectionString);
            conn.Open();
            string sql = "Insert into \"ShoppingListItem\" (Name,Price, AddedBy_ID, BoughtBy_ID, Sho_ID) values ( @Ime,@Cena, @IDdodal, @IDkupu, @IDs)";
            SqlCommand comm = new SqlCommand(sql, conn);
            comm.Parameters.AddWithValue("@Ime", Ime);
            comm.Parameters.AddWithValue("@Cena", int.Parse(Cena));
            comm.Parameters.AddWithValue("@IDdodal", IDdodal);
            comm.Parameters.AddWithValue("@IDkupu", IDkupu );
            comm.Parameters.AddWithValue("@IDs", int.Parse(IDs));


            try
            {
                comm.ExecuteNonQuery();
                conn.Close();
            }
            catch (SqlException)
            {
               throw new Exception("Napaka pri izvajanju.");
            }
            return true;
        }

        public void AddNewUserToSL(string IDs, string Mail)
        {
            int id = 0;
            string sql = "Select ID from \"Users\" where Mail = @Mail";
            string sql2 = "Insert into \"ShoppingList_Users\"(ID, Sho_ID) values (@id, @IDs)";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LoftAppConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand comm = new SqlCommand(sql, conn);
            comm.Parameters.AddWithValue("@Mail", Mail);

            SqlCommand comman = new SqlCommand(sql2, conn);
            comman.Parameters.AddWithValue("@IDs", int.Parse(IDs));

            try
            {
                comm.ExecuteNonQuery();
                using (var command = comm)
                {
                    using (var reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            id = reader.GetInt32(0);
                            comman.Parameters.AddWithValue("@id", id);
                        }
                    }
                }
            }
            catch (SqlException)
            {
                throw new Exception("Napaka pri poizvedbi.");
            }
            try
            {
                comman.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                throw new Exception("Napaka pri zapisovanju v bazo.");
            }
            conn.Close();

        }

       
        public void RemoveSL(string ID, string IDs)
        {
            throw new NotImplementedException();
        }

        public void RemoveYouFromSL(string ID, string IDs)
        {
            throw new NotImplementedException();
        }
    }
      
}
     
    
