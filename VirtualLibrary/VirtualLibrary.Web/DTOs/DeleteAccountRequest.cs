using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualLibrary.Web.DTOs
{
    public class DeleteRequest
    {
        public string Username { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
