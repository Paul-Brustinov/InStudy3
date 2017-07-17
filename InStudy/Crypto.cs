/*!
\file
\brief File for Crypto class to generate Hash of password

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace InStudy
{

    /// <summary>
    ///  This is the static class to get hash of password
    /// </summary>
    public static class Crypto
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="password">Password to hash </param>
        /// <returns>Hashed password</returns>
        public static string Hash(string password)
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.Create().
                    ComputeHash(Encoding.UTF8.GetBytes(password))
            );
        }
    }
}