using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;


namespace RESTService
{
 
    public class Service1 : IService1
    {
        public static string Authenticate(string username, string password)
        {
            string geslo = null;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LoftApp2ConnectionString"].ConnectionString))
            {
                string cmd = "Select Password from \"User\" where Username = @username";
                

                connection.Open();
                /*using (SqlCommand command = new SqlCommand(cmd, connection))
                {
                    command.Parameters.Add(new SqlParameter("@username", username));
                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        da.Fill(data);
                    }
                }*/
                SqlCommand comm = new SqlCommand(cmd, connection);
                comm.Parameters.AddWithValue("@username", username);
                /*try
                {*/
                //comm.ExecuteNonQuery();
                using (var command = comm)
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    geslo = reader.GetString(0);
                                }

                                connection.Close();
                            }
                        }
                    }

                    if (geslo != null)
                    {
                        string hashGesloTable = geslo;

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
               /* }
                catch (Exception)
                {
                    throw new Exception("Napaka.");
                }*/
            }
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
            Items nou = new Items();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LoftApp2ConnectionString"].ConnectionString);
            conn.Open();
            string pridobi = "Select ID, Name, Price, AddedBy_ID, BoughtBy_ID, Sho_ID from \"ShoppingListItem\" where Sho_ID = @id";
            SqlCommand comm = new SqlCommand(pridobi, conn);
            comm.Parameters.AddWithValue("@id", int.Parse(IDs));
            try {
                //comm.ExecuteNonQuery();
                    using (var reader = comm.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                            ret.Add(new Items { IDi = reader.GetInt32(0), Ime = reader.GetString(1), Cena = reader.GetDecimal(2), IDdodal = reader.GetInt32(3), IDs = reader.GetInt32(5) });

                        }
                    }
                        conn.Close();
                    }
                
                return ret;
            }
            catch (SqlException)
            {
                throw new Exception("Napaka");
            }

           
        }

        public List<ShoppingList> GetShoppingLists(string ID)
        {
            int ids = 0;
            //int stevilo = 0;
            List<ShoppingList> ret = new List<ShoppingList>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LoftApp2ConnectionString"].ConnectionString);
            SqlConnection connx = new SqlConnection(ConfigurationManager.ConnectionStrings["LoftApp2ConnectionString"].ConnectionString);
            connx.Open();
            conn.Open();
            string sql = "Select Sho_ID from \"ShoppingList_Users\" where ID = '" + int.Parse(ID) + "'";
            string sql2 = "Select Name from ShoppingList where ID = @id";
            string sql3 = "Select count(*) from ShoppingList where ID =  '"+ int.Parse(ID) + "'";
            SqlCommand comm = new SqlCommand(sql, conn);
            SqlCommand comma = new SqlCommand(sql2, connx);
            /*SqlCommand com = new SqlCommand(sql3, connx);
            using (var command1 = com)
            {
                using (var reader2 = command1.ExecuteReader())
                {
                    if (reader2.HasRows)
                    {

                        while (reader2.Read())
                        {
                            stevilo = reader2.GetInt32(0);
                        }
                    }
                    reader2.Close();
                }
            }
            Object[] idji = new Object[stevilo];*/
            using (var command = comm)
            {
                using ( var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    { 
                        while (reader.Read())
                        {
                            /*reader.GetValues(idji);
                            foreach (var item in idji)
                            {
                                comma.Parameters.AddWithValue("@id", (int) item);
                                ids = (int)item;*/
                            ids = reader.GetInt32(0);
                            comma.Parameters.AddWithValue("@id", ids);
                            using (var comman = comma)
                                {
                                    using (var reader1 = comman.ExecuteReader())
                                    {
                                        comma.Parameters.RemoveAt(0);
                                        if (reader1.Read())
                                            ret.Add(new ShoppingList { IDs = ids, Ime = reader1.GetString(0) });
                                        //}
                                    }
                                    /*comma.Parameters.AddWithValue("@id", ids);
                                    using (var comman = comma)
                                    {
                                        using (var reader1 = comman.ExecuteReader())
                                        {
                                            comma.Parameters.RemoveAt(0);
                                            if (reader1.Read())
                                                ret.Add(new ShoppingList { IDs = ids, Ime = reader1.GetString(0) });

                                        }
                                }*/
                                
                            }
                        }
                    }                                  
                }
            }
            conn.Close();
            return ret;
        }

        public List<User> GetUsers()
        {
            var ret = new List<User>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LoftApp2ConnectionString"].ConnectionString);
            conn.Open();
            string sql = "SELECT ID, Name, Surname, Username, Password , Mail, Phonenumber FROM \"User\"";
            SqlCommand comm = new SqlCommand(sql, conn);

            using (var command = comm)
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ret.Add(new User { ID = reader.GetInt32(0), Ime = reader.GetString(1), Priimek = reader.GetString(2), Username = reader.GetString(3), Password = reader.GetString(4) , Mail = reader.GetString(5), Telefon = reader.GetString(6) });
                            //ret.Add(new User { ID = reader.GetInt32(0) });

                           
                        }
                        
                    }
                    conn.Close();
                }

                return ret;
            }

        }

        public User Login(string username, string password)
        {
            string cookie = Authenticate(username, password);
            User ret = new RESTService.User { ID = -1 };

            if (cookie != null)
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LoftApp2ConnectionString"].ConnectionString);
                conn.Open();
                string sql = "select ID from \"User\" where Username = @username";
                SqlCommand comm = new SqlCommand(sql, conn);
                comm.Parameters.AddWithValue("@username", username);
                try
                {
                    using (var command = comm)
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ret.ID = reader.GetInt32(0);
                            }
                        }
                    }
                    return ret;
                }
                catch (Exception)
                {
                    return ret;
                }
            }
            else
            {
                return ret;
            }
        }

        public User Registration(string username, string password, string Ime, string Priimek, string number, string mail)
        {
            User ret = new RESTService.User { ID = -1, Username = username, Ime = Ime, Priimek = Priimek, Telefon = number, Mail = mail };
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LoftApp2ConnectionString"].ConnectionString);
            conn.Open();
            string sql = "INSERT INTO \"User\" (Name,Surname, Phonenumber,Mail, Password, Username) VALUES (@Ime, @Priimek, @number, @Mail, @Geslo, @Username)";
            string sql2 = "Select ID from \"User\" where Username = @username";
            SqlCommand comm = new SqlCommand(sql, conn);
            /*try
            {*/
                if (checkUsername(username) && checkMail(mail))
                {
                    comm.Parameters.AddWithValue("@Username", username);
                    comm.Parameters.AddWithValue("@Ime", Ime);
                    comm.Parameters.AddWithValue("@Priimek", Priimek);
                    comm.Parameters.AddWithValue("@Geslo", MD5Hash(password));
                    comm.Parameters.AddWithValue("@Mail", mail);
                    comm.Parameters.AddWithValue("@number", number);
                    comm.ExecuteNonQuery();

                    SqlCommand comma = new SqlCommand(sql2, conn);
                    comma.Parameters.AddWithValue("@username", username);
                    using (var reader = comma.ExecuteReader())
                        if (reader.Read())
                            ret.ID = reader.GetInt32(0);
                        else return ret;

                    conn.Close();
                    return ret;

                }
                else
                {
                    return ret;
                }
            
            /*}
            catch (Exception)
            {
                return ret;
            }*/

        }

        private bool checkUsername(string username)
        {
            string sql = "Select Name from \"User\" where Username = @username";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LoftApp2ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand comm = new SqlCommand(sql, conn);
            comm.Parameters.AddWithValue("@username", username);
            using (var reader = comm.ExecuteReader())
            { 
                 if (reader.Read())
                 {
                    if (reader.GetString(0) != null)
                     return false;
                 }
            }
            return true;
        }
        private bool checkMail(string mail)
        {
            string sql = "select Name from \"User\" where Mail = @mail";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LoftApp2ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand comm = new SqlCommand(sql, conn);
            comm.Parameters.AddWithValue("@mail", mail);
            using (var reader = comm.ExecuteReader())
            {
                if (reader.Read())
                {
                    if (reader.GetString(0) != null)
                        return false;
                }
            }
            return true;

        }

        public ShoppingList CreateNewShopingList(string IDu, string ImeSL)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LoftApp2ConnectionString"].ConnectionString);
            conn.Open();
            ShoppingList ret = new ShoppingList { IDs = -1 };
            string sql = "Insert into \"ShoppingList\" (Name) values (@ime)";
            string dobID = "select ID from \"ShoppingList\" where Name = @ime";
            string sql2 = "Insert into \"ShoppingList_Users\" (ID, Sho_ID) values (@id, @IDs)";
            SqlCommand comm = new SqlCommand(sql, conn);
            comm.Parameters.AddWithValue("@ime", ImeSL);
            SqlCommand comma = new SqlCommand(dobID, conn);
            comma.Parameters.AddWithValue("@ime", ImeSL);
            SqlCommand comman = new SqlCommand(sql2, conn);

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
                using (var command = comma)
                {
                    using (var reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            ret.IDs = reader.GetInt32(0);
                            comman.Parameters.AddWithValue("id", int.Parse(IDu));
                            comman.Parameters.AddWithValue("@IDs", ret.IDs);
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
            return ret;

        }

        public Odgovor SaveItem(string IDs, string Ime, string IDdodal)
        {
            Odgovor ret = new Odgovor { vrednost = false };
            int Cena = 0;
            int kupu = 0;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LoftApp2ConnectionString"].ConnectionString);
            conn.Open();
            string sql = "Insert into \"ShoppingListItem\" (Name,Price, AddedBy_ID, BoughtBy_ID ,Sho_ID) values ( @Ime,@Cena, @IDdodal, @IDkupu, @IDs)";
            SqlCommand comm = new SqlCommand(sql, conn);
            comm.Parameters.AddWithValue("@Ime", Ime);
            comm.Parameters.AddWithValue("@Cena", Cena);
            comm.Parameters.AddWithValue("@IDdodal", IDdodal);
            comm.Parameters.AddWithValue("@IDkupu", kupu);
            comm.Parameters.AddWithValue("@IDs", int.Parse(IDs));

            try
            {
                comm.ExecuteNonQuery();
                conn.Close();
            }
            catch (SqlException)
            {
               return ret;
               throw new Exception("Napaka pri izvajanju.");
            }
            ret.vrednost = true;
            return ret;
        }

        public Odgovor AddNewUserToSL(string IDs, string Mail)
        {
            Odgovor ret = new Odgovor { vrednost = false };
            int id = 0;
            string sql = "Select ID from \"User\" where Mail = @Mail";
            string sql2 = "INSERT INTO \"ShoppingList_Users\" (ID, Sho_ID) VALUES (@id, @IDs)";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LoftApp2ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand comm = new SqlCommand(sql, conn);
            comm.Parameters.AddWithValue("@Mail", Mail);

            SqlCommand comman = new SqlCommand(sql2, conn);
            comman.Parameters.AddWithValue("@IDs", int.Parse(IDs));

            try
            {

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
                return ret;
            }
            try
            {
                comman.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                return ret;
            }
            ret.vrednost = true;
            conn.Close();
            return ret;

        }

        public Odgovor BuyItem(string IDi, string IDs, string cena, string IDkupil)
        {
            Odgovor ret = new Odgovor { vrednost = false };
            string sql = "update \"ShoppingListItem\" set Price = @cena, BoughtBy_ID = @idkupu where ID = @id AND Sho_ID = @ids";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LoftApp2ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand comm = new SqlCommand(sql, conn);
            comm.Parameters.AddWithValue("@cena", cena);
            comm.Parameters.AddWithValue("@idkupu", IDkupil);
            comm.Parameters.AddWithValue("@id", IDi);
            comm.Parameters.AddWithValue("@ids", IDs);
            comm.ExecuteNonQuery();
            conn.Close();
            ret.vrednost = true;
            return ret;
        }

        public List<User> GetUsersFromSL(string IDs)
        {
            List<User> ret = new List<User>();
            string sql = "Select ID from ShoppingList_Users where Sho_ID = @ids";
            string sql2 = "Select Name, Surname , Phonenumber, Mail, Username from \"User\" where ID = @id";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LoftApp2ConnectionString"].ConnectionString);
            SqlConnection connx = new SqlConnection(ConfigurationManager.ConnectionStrings["LoftApp2ConnectionString"].ConnectionString);
            conn.Open();
            connx.Open();
            SqlCommand comm = new SqlCommand(sql, conn);
            comm.Parameters.AddWithValue("@ids", IDs);

            using(var reader = comm.ExecuteReader())
            {
                while (reader.Read())
                {
                    User nou = new RESTService.User { ID = reader.GetInt32(0) };
                    SqlCommand comma = new SqlCommand(sql2, connx);
                    comma.Parameters.AddWithValue("@id", nou.ID);
                    using (var reader1 = comma.ExecuteReader())
                    {
                        while (reader1.Read())
                        {
                            nou.Ime = reader1.GetString(0);
                            nou.Priimek = reader1.GetString(1);
                            nou.Telefon = reader1.GetString(2);
                            nou.Mail = reader1.GetString(3);
                            nou.Username = reader1.GetString(4);
                        }
                    }
                    ret.Add(nou);
                    
                }
            }
            return ret;
        }

        public Odgovor Calculate(string IDkupu, string IDs)
        {
            Odgovor ret = new Odgovor { cena = -1 };
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LoftApp2ConnectionString"].ConnectionString);
            conn.Open();
            string sql = "select sum(Price) from ShoppingListItem where BoughtBy_ID = @idkupu and Sho_ID = @ids";
            SqlCommand comm = new SqlCommand(sql, conn);
            comm.Parameters.AddWithValue("@idkupu", IDkupu);
            comm.Parameters.AddWithValue("@ids", IDs);
            using(var reader = comm.ExecuteReader())
            {
                while (reader.Read())
                {
                    ret.cena = reader.GetDecimal(0);
                }
            }

            return ret;

        }
    }
      
}
     
    
