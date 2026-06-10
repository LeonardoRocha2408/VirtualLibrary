using MySql.Data.MySqlClient;
namespace LibraryApi.Methods
{
    public static class BookServices
    {
        //PESQUISA LIVROS
        public static List<string>? SearchBook(string searchByBookName)
        {
            List<string>? booksToShow = null;
            using (MySqlConnection connection = Database.GetConnection())
            {
                connection.Open();

                string query = $"SELECT * FROM books WHERE name_book = @searchByBookName";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@searchByBookName", searchByBookName);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        Console.WriteLine("Livro não encontrado");
                    }
                    else
                    {
                        string? toCompare = reader["name_book"].ToString();
                        if (toCompare!.Contains(searchByBookName, StringComparison.OrdinalIgnoreCase))
                        {
                            booksToShow = new List<string>();
                            booksToShow.Add(toCompare);
                        }
                    }
                }
            }
            return booksToShow;
        }


        // PUBLICA LIVROS
        public static void PublishYourBook(string authorName, string title, string bookCategory)
        {
            using (MySqlConnection connection = Database.GetConnection())
            {
                connection.Open();

                string querySelect = "SELECT * FROM books WHERE name_book = @title";

                MySqlCommand commandSelect = new MySqlCommand(querySelect, connection);
                commandSelect.Parameters.AddWithValue("@title", title);
                MySqlDataReader reader = commandSelect.ExecuteReader();

                if (reader.Read())
                {
                    Console.WriteLine("Esse livro já existe");
                }
                else
                {
                    reader.Close();
                    string query = $"INSERT INTO books (name_book, author_name, category) VALUES (@title, @authorName, @bookCategory)";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@title", title);
                    command.Parameters.AddWithValue("@authorName", authorName);
                    command.Parameters.AddWithValue("@bookCategory", bookCategory);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Livro adicionado!");
                }
            }
        }
    }
}
