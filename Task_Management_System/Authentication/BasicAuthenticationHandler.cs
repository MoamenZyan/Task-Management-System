using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Project.API.Entities;
using Project.API.Interfaces;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Project.API.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserService _userService;
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IUserService userService) : base(options, logger, encoder, clock)
        {
            _userService = userService;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return Task.FromResult(AuthenticateResult.NoResult());

            if (!AuthenticationHeaderValue.TryParse(Request.Headers["Authorization"], out var authHeader))
                return Task.FromResult(AuthenticateResult.Fail("Unknown scheme"));

            if (!authHeader.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(AuthenticateResult.Fail("Unknown scheme"));

            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter!)).Split(":");
            UserCredentials userCredentials = new UserCredentials
            {
                Email = credentials[0],
                Password = credentials[1]
            };

            var result = _userService.CheckUserCredentials(userCredentials);
            if (!result.Item1)
                return Task.FromResult(AuthenticateResult.NoResult());

            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim (ClaimTypes.NameIdentifier, Convert.ToString(result.Item2))
            }, authHeader.Scheme);

            var prinicpal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(prinicpal, authHeader.Scheme);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
