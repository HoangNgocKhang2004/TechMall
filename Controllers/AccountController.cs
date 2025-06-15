using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Auth.OAuth2.Responses;
using System.Threading;
using System.Web.Mvc;

public class AccountController : Controller
{
    private static readonly string clientId = System.Configuration.ConfigurationManager.AppSettings["GoogleClientId"];
    private static readonly string clientSecret = System.Configuration.ConfigurationManager.AppSettings["GoogleClientSecret"];
    private static readonly string redirectUri = System.Configuration.ConfigurationManager.AppSettings["GoogleRedirectUri"];

    public ActionResult LoginWithGoogle()
    {
        var provider = new AuthorizationCodeMvcApp(this, new AppFlowMetadata());
        return new RedirectResult(provider.Flow.CreateAuthorizationCodeRequest(redirectUri).Build().AbsoluteUri);
    }

    public async System.Threading.Tasks.Task<ActionResult> GoogleCallback()
    {
        var provider = new AuthorizationCodeMvcApp(this, new AppFlowMetadata());
        var authResult = await provider.AuthorizeAsync(CancellationToken.None);

        if (authResult.Credential != null)
        {
            // User logged in successfully
            // Get user info here, e.g., email, name
            return RedirectToAction("Index", "Home");
        }

        return RedirectToAction("Login", "Account");
    }

    private class AppFlowMetadata : FlowMetadata
    {
        public override IAuthorizationCodeFlow Flow { get; } = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            },
            Scopes = new[] { "email", "profile" }
        });

        public override string GetUserId(Controller controller)
        {
            return "user";
        }
    }
}
