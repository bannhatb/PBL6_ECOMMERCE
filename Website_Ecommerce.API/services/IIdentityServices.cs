namespace Website_Ecommerce.API.services
{
    public interface IIdentityServices
    {
        /// <summary>
        /// Generate token
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        /// <param name="expires"></param>
        /// <returns></returns>
        string GenerateToken(int userId, string username, List<int> roles, int expires);

        /// <summary>
        /// Get MD5
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        string GetMD5(string text);

        /// <summary>Verify MD5 hash
        /// 
        /// </summary>
        /// <param name="inputHash"></param>
        /// <param name="hashVerify"></param>
        /// <returns></returns>
        bool VerifyMD5Hash(string inputHash, string hashVerify);

        /// <summary>
        /// Send password by email
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        string SendingPasswordByEmail(string fromAddress, string toAddress, string password);
    }
}