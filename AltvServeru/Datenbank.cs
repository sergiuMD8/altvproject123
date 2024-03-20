using AltV.Net;
using AltV.Net.Resources.Chat.Api;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltvServeru
{
    class Datebank : Server
    {
        public static bool DatabankConnection = false;
        public static MySqlConnection Connection;
        public string Host {  get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public String Datebase { get; set; }

        public Datebank()
        {
            this.Host = "localhost";
            this.Username = "altv";
            this.Password = "12345";
            this.Datebase = "altv";
        }


        public static String GetConnectionString()
        {
            Datebank sql = new Datebank();
            String SQLConnection = $"SERVER = {sql.Host}; DATABASE={sql.Datebase}; UID={sql.Username}; Password={sql.Password}";
            return SQLConnection;
        }


        public static void InitConnection()
        {
            String SQLConnection = GetConnectionString();
            Connection = new MySqlConnection(SQLConnection);
            try
            {
                Connection.Open();
                DatabankConnection = true;
                Alt.Log("MYSQL база данных успешно подключена");
            }
            catch (Exception e) 
            {
                DatabankConnection = false;
                Alt.Log("MYSQL Не удалось подключить базу даннх");
                Alt.Log(e.ToString());
                System.Threading.Thread.Sleep(10000);
                Environment.Exit(0);
            }
        }

        public static bool IsAccountRegistred(string name)
        {
            MySqlCommand command = Connection.CreateCommand();
            command.CommandText = "SELECT * FROM accounts WHERE name=@name LIMIT 1";
            command.Parameters.AddWithValue("name", name);
            using(MySqlDataReader reader = command.ExecuteReader()) 
            {
                if (reader.HasRows) 
                {
                    return true;
                }
            }
            return false;

        }

        public static int NewAccountRegistration(String name, String password)
        {
            string saltedPw = BCrypt.HashPassword(password, BCrypt.GenerateSalt());

            try
            {
                MySqlCommand command = Connection.CreateCommand();
                command.CommandText = "INSERT INTO accounts (password, name) VALUES (@password, @name)";

                command.Parameters.AddWithValue("@password", saltedPw);
                command.Parameters.AddWithValue("@name", name);
                command.ExecuteNonQuery();

                return (int)command.LastInsertedId;
            }
            catch(Exception e)
            {
                Alt.Log("Ошибка при регистрации нового акаунта :" + e.ToString());
                return -1;
            }
        }

        public static void AccountLaden(TPlayer.TPlayer tplayer)
        {
            MySqlCommand command = Connection.CreateCommand();
            command.CommandText = "SELECT * FROM accounts WHERE name=@name LIMIT 1";

            command.Parameters.AddWithValue("@name", tplayer.PlayerName);

            using(MySqlDataReader reader = command.ExecuteReader())
            { if (reader.HasRows)
                {
                    reader.Read();
                    tplayer.PlayerID = reader.GetInt32("id");
                    tplayer.Adminlevel = reader.GetInt16("adminlevel");
                    tplayer.Geld = reader.GetInt32("geld");
                    tplayer.Fraktion = reader.GetInt16("fraktion");
                    tplayer.Rang = reader.GetInt16("rang");
                }
                    
            }
        }

        public static void AccountUpdate(TPlayer.TPlayer tplayer)
        {
            MySqlCommand command = Connection.CreateCommand();
            command.CommandText = "UPDATE accounts SET adminlevel=@adminlevel, geld=@geld, fraktion=@fraktion, rang=@rang WHERE id=@id";

            command.Parameters.AddWithValue("@adminlevel", tplayer.Adminlevel);
            command.Parameters.AddWithValue("@geld", tplayer.Geld);
            command.Parameters.AddWithValue("@fraktion", tplayer.Fraktion);
            command.Parameters.AddWithValue("@rang", tplayer.Rang);
            command.Parameters.AddWithValue("id",tplayer.PlayerID);
            command.ExecuteNonQuery();
        }

        public static bool PasswordCheck(string name, string passwordinput)
        {
            string password = "";
            MySqlCommand command = Connection.CreateCommand();
            command.CommandText = "SELECT password FROM accounts where name=@name Limit 1";
            command.Parameters.AddWithValue("@name", name);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    password = reader.GetString("password");
                }
            }

            if (BCrypt.CheckPassword(passwordinput, password)) return true;
            return false;
        }


    }

    
}
