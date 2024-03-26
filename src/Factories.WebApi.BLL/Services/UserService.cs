using Factories.WebApi.BLL.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Security.Cryptography;

namespace Factories.WebApi.BLL.Services
{
    public class UserService(UsersDbContext usersDb)
    {
        private readonly UsersDbContext usersDb = usersDb ?? throw new ArgumentNullException(nameof(usersDb));
        public async Task<IdentityUser?> AuthenticateAsync(string username, string password)
        {
            var user = await usersDb.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user != null)
            {
                // Проверяем соответствие пароля
                var passwordHasher = new PasswordHasher<IdentityUser>();
                var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

                if (result == PasswordVerificationResult.Success)
                {
                    // Возвращаем пользователя, если пароли совпадают
                    return user;
                }
            }

            return null;
        }

        public async Task<bool> RegisterAsync(string username, string password)
        {
           
            var passwordHasher = new PasswordHasher<IdentityUser>();
            var hashPassword = passwordHasher.HashPassword(new IdentityUser() { UserName = username }, password);


            IdentityUser newUser = new()
            {
                UserName = username,
                PasswordHash = hashPassword,
                Email = username,
            };

            // Добавление пользователя в базу данных
            await usersDb.Users.AddAsync(newUser);
            await usersDb.SaveChangesAsync();

            return true;
        }


        //  для генерации случайной соли
        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[16]; // Размер соли в байтах
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        //  для хеширования пароля с использованием соли
        private string HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(20); // Длина хеша 20 байтов (160 бит)
                byte[] hashWithSalt = new byte[36]; // Длина хеша пароля с солью в байтах

                // Скопируем соль в первые 16 байтов хеша с солью
                Array.Copy(salt, 0, hashWithSalt, 0, 16);

                // Скопируем хеш пароля в оставшиеся 20 байтов хеша с солью
                Array.Copy(hash, 0, hashWithSalt, 16, 20);

                // Преобразуем массив байтов в строку Base64 для хранения в базе данных
                return Convert.ToBase64String(hashWithSalt);
            }
        }
    }
}
