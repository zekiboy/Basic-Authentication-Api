using System;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using testApi.Interfaces;
namespace testApi.Handlers
{
	public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
		private readonly IUserService _userService;

		private string? _failReason;

		public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,

			ILoggerFactory logger,
            UrlEncoder encoder,
			ISystemClock clock,
			IUserService userService

            ) : base(options, logger, encoder, clock)
						{
							_userService = userService;
						}


        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string authorizationHeader = Request.Headers["Authorization"];

            if (authorizationHeader == null || !authorizationHeader.StartsWith("Basic"))
            {
                _failReason = "Authorization header not found.";
                return await Task.FromResult(AuthenticateResult.Fail("Authorization header not found."));
            }

            string encodedUsernamePassword = authorizationHeader.Substring("Basic ".Length).Trim();
            byte[] decodedUsernamePassword = Convert.FromBase64String(encodedUsernamePassword);
            string usernamePassword = Encoding.UTF8.GetString(decodedUsernamePassword);
            string[] parts = usernamePassword.Split(':', 2);

            string username = parts[0];
            string password = parts[1];

            bool isValid = _userService.Login(username, password) != null;

            if (!isValid)
            {
                _failReason = "Invalid username or password.";
                return await Task.FromResult(AuthenticateResult.Fail("Invalid username or password."));
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(ClaimTypes.Name, username)
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return await Task.FromResult(AuthenticateResult.Success(ticket));
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            await base.HandleChallengeAsync(properties);
            if (Response.StatusCode == StatusCodes.Status401Unauthorized &&
                !string.IsNullOrWhiteSpace(_failReason))
            {
                Response.Headers.Add("WWW-Authenticate", _failReason);
                Response.ContentType = "application/json";
                await WriteProblemDetailsAsync(_failReason);
            }
        }

        private Task WriteProblemDetailsAsync(string detail)
        {
            var problemDetails = new ProblemDetails { Detail = detail, Status = Context.Response.StatusCode };
            var result = new ObjectResult(problemDetails)
            {
                ContentTypes = new MediaTypeCollection(),
                StatusCode = problemDetails.Status,
                DeclaredType = problemDetails.GetType(),
            };
            var executor = Context.RequestServices.GetRequiredService<IActionResultExecutor<ObjectResult>>();
            var routeData = Context.GetRouteData() ?? new RouteData();
            var actionContext = new ActionContext(Context, routeData, new ActionDescriptor());
            return executor.ExecuteAsync(actionContext, result);
        }


    }
}

