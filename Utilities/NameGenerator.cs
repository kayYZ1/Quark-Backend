using System.Linq;
using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Quark_Backend.DAL;
using Quark_Backend.Entities;

namespace Quark_Backend.Utilities;
public static class NameGenerator
{
    private static Random random = new Random();
    public async static Task<string> GenerateUsername(string firstName, string lastName)
    {
        string tempUsername;
        string digitsPart;
        User user;
        string firstNamePart = firstName.Substring(0, Math.Min(3, firstName.Length));
        string lastNamePart = lastName.Substring(0, Math.Min(3, lastName.Length));
        using(var db = new QuarkDbContext())
        {
            do
            {
                digitsPart = random.Next(10000).ToString();//0-9999
                tempUsername = firstNamePart + lastNamePart + digitsPart;
                user = await db.Users.FirstAsync(u => u.Username == tempUsername);
                if(user == null) break;
            } while(true);
        }
        return tempUsername;
    }
    public static string GenerateRandomConversationName()
    {
        string chars = "abcdefghijklmnoprstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}