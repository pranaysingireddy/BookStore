using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BookStore.Services.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BookStore.Services.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        //public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
        //    UrlEncoder encoder,ISystemClock clock) :base(options,logger,encoder,clock)
        //{


        //}

        private BookStoreContext _context;

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, BookStoreContext context)
            : base(options, logger, encoder, clock)
        {
            _context = context;

        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {

                var authorizationHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

                if (authorizationHeader == null)
                {
                    return AuthenticateResult.Fail("Invalid Username or Password");
                }

                var encoededHeader = Convert.FromBase64String(authorizationHeader.Parameter);

                string[] credentials = Encoding.UTF8.GetString(encoededHeader).Split(new[] { ':' }, 2);

                var emailAddress = credentials[0];
                var password = credentials[1];


                var user = _context.User.Where(a => a.EmailAddress == emailAddress && a.Password == password).FirstOrDefault();


                if (user == null)
                    return AuthenticateResult.Fail("Invalid Username or Password");

                //var claims = new[] {
                //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                //    new Claim(ClaimTypes.Name, user.Username),
                //};

                var claims = new[] { new Claim(ClaimTypes.Name, user.EmailAddress) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);

            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail("Invalid Username or Password");
            }

        }
    }
}
