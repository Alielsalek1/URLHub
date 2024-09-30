using System.Data.SqlTypes;
using URLshortner.Models;

namespace URLshortner.Validators;

public class URLValidator
{
    private bool IsValidURL(string? Url)
    {
        return Url != null && Uri.TryCreate(Url, UriKind.Absolute, out var uriResult) &&
           (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
    private bool IsValidID(int? ID)
    {
        return ID != null && ID >= 1;
    }
    public bool IsValidURL(URL url)
    {
        return url != null && IsValidURL(url.Url) && IsValidID(url.ID);
    }
}
