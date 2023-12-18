using System.Linq;
using System.Collections;
using System.Linq.Expressions;

namespace Quark_Backend.Utilities;
public static class NameGenerator
{
    private static Random random = new Random();
    public static string GenerateUsername(string firstName, string lastName)
    {
        string firstPart = firstName.Substring(0, Math.Min(3, firstName.Length));
        string lastPart = lastName.Substring(0, Math.Min(3, lastName.Length));
        return firstPart + lastPart;
    }
    public static string GenerateRandomConversationName()
    {
        string chars = "abcdefghijklmnoprstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}