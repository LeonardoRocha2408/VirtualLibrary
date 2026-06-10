using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryShared.DTOs
{
    public class CreateAccountRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
