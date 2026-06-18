using MySql.Data.MySqlClient;
namespace LibraryApi.Methods
{
    public static class BookServices
    {
        //PESQUISA LIVROS
        public static List<string>? SearchBook(string searchByBookName)
        {
            Console.WriteLine("Entrou no método");
            List<string>? booksToShow = new();
            using (MySqlConnection connection = Database.GetConnection())
            {
                connection.Open();

                Console.WriteLine(searchByBookName);
                string query = $"SELECT * FROM books WHERE name_book LIKE @searchByBookName";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@searchByBookName", $"%{searchByBookName}%");

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string bookName = reader["name_book"].ToString()!;
                        booksToShow.Add(bookName);
                    }
                    Console.WriteLine($"{booksToShow.Count}");
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
