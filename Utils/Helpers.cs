using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;
using URLshortner.Dtos;
using URLshortner.Exceptions;
using URLshortner.Models;

namespace URLshortner.Utils;

public static class Helpers
{
    public static bool IsValidRefreshToken(RefreshToken token)
    {
        if (token == null)
        {
            throw new NotFoundException("no refresh token found");
        }
        if (token.IsRevoked)
        {
            throw new UnAuthorizedException("refresh token is revoked");
        }
        if (token.Expires < DateTime.UtcNow)
        {
            throw new UnAuthorizedException("refresh token has been expired");
        }
        return true;
    }

    public static bool IsValidUser(User curUser, string password)
    {
        if (curUser == null) return false;

        var passwordHasher = new PasswordHasher<User>();
        var isValidPassword = passwordHasher.VerifyHashedPassword(curUser, curUser.Password, password);

        return isValidPassword != PasswordVerificationResult.Failed;
    }

    public static string ShortenUrl(string url, ulong additioner)
    {
        using var sha256 = SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(url));

        ulong hashValue = BitConverter.ToUInt64(hashBytes, 0);
        return ToBase62(hashValue + additioner);
    }

    private static string ToBase62(ulong value)
    {
        var Base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        var sb = new StringBuilder();
        while (value > 0)
        {
            sb.Insert(0, Base62Chars[(int)(value % 62)]);
            value /= 62;
        }
        return sb.ToString();
    }
}