namespace LibraryApi
{

    public class LibraryAccount
    {
        public int Id { get; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<string> FavoriteCategory { get; set; } = new();

        public LibraryAccount(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public void ChangePasswordInMemory(string newPassword)
        {
            Password = newPassword;
        }

    }
    public class Books
    {
        public string Author { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }
}