using MySql.Data.MySqlClient;
using System.Linq;
using System.Windows.Input;
namespace LibraryApi.Methods
{
    public static class Services
    {

        //CRIA A CONTA
        public static bool CreateAccount(LibraryAccount User)
        {
            
            using (MySqlConnection connection = Database.GetConnection())
            {
                connection.Open();

                string querySelect = $"SELECT * FROM users WHERE username = @username";

                MySqlCommand commandSelect = new MySqlCommand(querySelect, connection);
                commandSelect.Parameters.AddWithValue("@username", User.Username);

                using (MySqlDataReader reader = commandSelect.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        Console.WriteLine($"{User.Username} já existe");
                        return true;
                    }
                }
                string query = $"INSERT INTO users (username, password) VALUES (@username, @password)";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", User.Username);
                command.Parameters.AddWithValue("@password", User.Password);

                command.ExecuteNonQuery();
                return false;
            }
        }



        //LOGA A CONTA
        public static LibraryAccount? Login(LibraryAccount? User)
        {
            List<string> favoriteCategory = new List<string>();

            using (MySqlConnection connection = Database.GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM users WHERE username = @name AND password = @password";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", User?.Username);
                command.Parameters.AddWithValue("@password", User?.Password);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string userFromDb = reader["username"].ToString() ?? "";
                        string passwordFromDb = reader["password"].ToString() ?? "";
                        favoriteCategory.Add(reader["favorite_category"].ToString() ?? "");

                        User = new LibraryAccount(userFromDb, passwordFromDb)
                        {
                        FavoriteCategory = favoriteCategory
                        };
                        Console.Write(User.Username);
                        reader.Close();
                        return User;
                    }
                }
            }
            return null;
        }

        // MUDA SENHA 
        public static bool ChangePassword(string username, string password, string newPassword)
        {
            LibraryAccount userToChangePassword = new LibraryAccount(username, password);
            var account = Login(userToChangePassword);

            if (account == null)
            {
                return false;
            }

            using (MySqlConnection connection = Database.GetConnection())
            {
                connection.Open();

                string query = "UPDATE users SET password = @newPassword WHERE username = @username AND password = @oldPassword";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", account.Username);
                    command.Parameters.AddWithValue("@oldPassword", account.Password);
                    command.Parameters.AddWithValue("@newPassword", newPassword);

                    int changedLines = command.ExecuteNonQuery();

                    return changedLines > 0;
                }
            }

        }
        //ADICIONA O GÊNERO FAVORITO NO PERFIL
        public static void AddFavoriteGender(LibraryAccount? User, string categorySelected)
        {
            var account = Login(User);
                
            using (MySqlConnection connection = Database.GetConnection())
            {
                connection.Open();
                string querySelect = "SELECT id WHERE username = @username AND password = @password";
                MySqlCommand commandSelect = new MySqlCommand(querySelect, connection);
                commandSelect.Parameters.AddWithValue("@username", account!.Username);
                commandSelect.Parameters.AddWithValue("@password", account!.Password);
                MySqlDataReader reader = commandSelect.ExecuteReader();

                int id = Convert.ToInt32(reader["id"]);
                reader.Close();



                if (categorySelected != null)
                {
                    string query = $"UPDATE users SET favorite_category = @categorySelected WHERE id = @id";

                    MySqlCommand command = new MySqlCommand(query, connection);

                    account.FavoriteCategory.Add(categorySelected);

                    command.Parameters.AddWithValue("@categorySelected", account.FavoriteCategory);
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }


        // APAGA CONTAS
        public static void DeleteAccount(string nameToDelete, string passwordToDelete)
        {
            using (MySqlConnection connection = Database.GetConnection())
            {
                connection.Open();

                string querySelect = $"SELECT * FROM users WHERE username = @nameToDelete AND password = @passwordToDelete";

                MySqlCommand commandSelect = new MySqlCommand(querySelect, connection);
                commandSelect.Parameters.AddWithValue("@nameToDelete", nameToDelete);
                commandSelect.Parameters.AddWithValue("@passwordToDelete", passwordToDelete);
                MySqlDataReader reader = commandSelect.ExecuteReader();

                if (!reader.Read())
                {
                    Console.WriteLine("Dados não encontrados ou incorretos, tente novamente");
                }
                else
                {
                    int id = Convert.ToInt32(reader["id"]);
                    reader.Close();
                    string query = $"DELETE FROM users WHERE id = @idToDelete";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@idToDelete", id);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Conta deletada!");
                }
            }
        }
    }
}
