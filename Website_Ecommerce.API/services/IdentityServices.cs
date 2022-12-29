using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;

namespace Website_Ecommerce.API.services
{
    public class IdentityServices : IIdentityServices
    {
        private readonly Audience _audience;
        public IdentityServices(
            IOptions<Audience> options)
        {
            _audience = options.Value ?? throw new ArgumentException(nameof(options.Value));
        }

        /// <summary>
        /// Generate token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        /// <param name="roles"></param>
        /// <param name="expires"></param>
        /// <returns></returns>
        public string GenerateToken(int userId, string username, List<int> roles, int expires)
        {
            var now = DateTime.UtcNow;
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(ClaimTypes.Name, userId.ToString()),
                new Claim(ClaimTypes.Role, JsonConvert.SerializeObject(roles),
                JsonClaimValueTypes.JsonArray)
            };

            var signningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_audience.Secret));

            var jwt = new JwtSecurityToken(
                issuer: _audience.Issuer,
                audience: _audience.Name,
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromSeconds(expires)),
                signingCredentials: new SigningCredentials(signningKey,
                SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        /// <summary>
        /// Get MD5
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string GetMD5(string text)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                md5.ComputeHash(Encoding.ASCII.GetBytes(text));
                byte[] result = md5.Hash;
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                {
                    str.Append(result[i].ToString("x2"));
                }
                return str.ToString();
            }
        }

        /// <summary>
        /// Verify MD5 hash
        /// </summary>
        /// <param name="inputHash"></param>
        /// <param name="hashVerify"></param>
        /// <returns></returns>
        public bool VerifyMD5Hash(string inputHash, string hashVerify)
        {
            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(inputHash, hashVerify))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private static Random s_random = new Random();
        /// <summary>
        /// Random string
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomString(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[s_random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Send password by email
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string SendingPasswordByEmail(string fromAddress, string toAddress, string password)
        {
            using SmtpClient email = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential(fromAddress, password)
            };

            string code = RandomString(6);

            string subject = "Forget password";
            string body = $"{code} la mat khau moi cua ban. Reset vao luc {DateTime.Now}";
            try
            {
                email.Send(fromAddress, toAddress, subject, body);
            }
            catch (SmtpException e)
            {
                Console.WriteLine(e.Message);
            }

            return code;
        }
    }
};