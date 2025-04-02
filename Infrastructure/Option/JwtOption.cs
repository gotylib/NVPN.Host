
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure.Option
{
    public static class JwtOption
    {
        /// <summary>
        /// Издатель токена.
        /// </summary>
        public const string ISSUER = "NVPN";

        /// <summary>
        /// Потребитель токена
        /// </summary>
        public const string AUDIENCE = "UserNVPN";

        const string KEY = "werfhpwqjpqjwnvpiewvwpejqvnwqpoiuqwhrvowqvnpqiwvbqwepivqwnevpiqewbvpkqwejvbwqbvqwjkvb";

        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));

    }
}
