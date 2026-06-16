using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualLibrary.Web.DTOs
{
    public class ChangePasswordRequest
    {
        public string Username { get; init; } = string.Empty; 
        public string Password { get; set; } = string.Empty;
        public string NewPassword {  get; set; } = string.Empty;
    }
}
