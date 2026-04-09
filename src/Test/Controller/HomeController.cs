using Egov.Integrations.MSign.Soap;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;

namespace Test.Controller;

public class HomeController : ControllerBase
{
    private readonly IMSignSoapClient _msignClient;

    public HomeController(IMSignSoapClient msignClient)
    {
        _msignClient = msignClient;
    }

    public async Task<IActionResult> IndexAsync()
    {
        var requestID = await _msignClient.PostSignRequestAsync(new SignRequest
        {
            ShortContentDescription = "Sample HASH signature",
            ContentType = ContentType.Hash,
            Contents = new[]
            {
                new SignContent
                {
                    Content = SHA1.HashData(new byte[] { 0xDE, 0xAD, 0xBE, 0xEF })
                }
            }
        });

        // example Relay State
        var relayState = Guid.NewGuid().ToString();

        return Redirect(_msignClient.BuildRedirectAddress(requestID, Url.Action(nameof(MSignResponse))!, relayState));
    }

    public async Task<IActionResult> MSignResponse(string requestID, string relayState)
    {
        var response = await _msignClient.GetSignResponseAsync(requestID, null);

        if (response.Status == SignStatus.Success)
        {
            return Content(new StringBuilder("<h1>Signature succeeded:</h1><div>")
                .Append($"<h2>Relay State: {relayState}</h2>")
                .Append("<h2>Signature:</h2><div>")
                .Append(HtmlEncoder.Default.Encode(Encoding.UTF8.GetString(response.Results[0].Signature)))
                .Append("</div>").ToString(), "text/html");
        }

        return Content(new StringBuilder("<h1>Signature failed</h1>")
            .Append($"<h2>Status: {response.Status}</h2>")
            .Append($"<h2>Message: {response.Message}</h2>").ToString(), "text/html");
    }
}
