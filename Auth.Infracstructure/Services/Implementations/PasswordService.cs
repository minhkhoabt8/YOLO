using Auth.Infrastructure.Services.Interfaces;
using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace Auth.Infrastructure.Services.Implementations
{
    public class PasswordService : IPasswordService
    {
        private readonly int _degreeOfParallelism = 1;
        private readonly int _memorySize = 65536;
        private readonly int _iterations = 4;

        public PasswordService()
        {
        }

        /// <summary>
        /// Validate Password
        /// </summary>
        /// <param name="enteredPassword"></param>
        /// <param name="storedSalt"></param>
        /// <param name="storedHash"></param>
        /// <returns></returns>
        public bool ValidatePassword(string enteredPassword, string storedSalt, string storedHash)
        {
            using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(enteredPassword)))
            {
                argon2.Salt = Encoding.UTF8.GetBytes(storedSalt);
                argon2.DegreeOfParallelism = _degreeOfParallelism; 
                argon2.MemorySize = _memorySize;     
                argon2.Iterations = _iterations;         

                byte[] computedHash = argon2.GetBytes(32); // 32-byte hash

                // Compare computed hash with stored hash
                string computedHashString = BitConverter.ToString(computedHash).Replace("-", "").ToLower();
                return computedHashString.Equals(storedHash);
            }
        }

        /// <summary>
        /// This method return hashed password with fomat: [salt]-[hashedPassword]
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public string GenerateHashPassword(string password)
        {
            
            string salt = GenerateSalt();

            using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)))
            {
                argon2.Salt = Encoding.UTF8.GetBytes(salt);
                argon2.DegreeOfParallelism = _degreeOfParallelism;
                argon2.MemorySize = _memorySize;
                argon2.Iterations = _iterations;

                byte[] hash = argon2.GetBytes(32);

                return string.Concat(salt, "-", BitConverter.ToString(hash).Replace("-", "").ToLower());
            }
        }

        /// <summary>
        /// Generate a random salt.
        /// </summary>
        /// <param name="length">The length of the salt in bytes.</param>
        /// <returns>A string representation of the generated salt.</returns>
        public string GenerateSalt(int length = 16)
        {
            byte[] saltBytes = new byte[length];

            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(saltBytes);
            }

            string salt = BitConverter.ToString(saltBytes).Replace("-", "").ToLower();

            return salt;
        }
    }
}
