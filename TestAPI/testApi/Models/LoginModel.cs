using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace testApi.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
