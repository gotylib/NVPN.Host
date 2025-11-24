

using DAL;
using Infrastructure.Dto;
using Infrastructure.Option;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class UserControlService(IMapper mapper, VpnDbContext context, ILogger<UserControlService> logger) : IUserControlService
    {
        public async Task<ResultModel<string, Exception>> AddUser(UserModel userModel)
        {
            try
            {
                var validate = await ValidateUser(userModel);
                if (!validate.IsSuccessful)
                {
                    return ResultModel<string,Exception>.CreateFailedResult(validate.Value);
                }
                
                var hashPassword = HashPassword(userModel.Password);
                
                var user = mapper.Map<User>(userModel);
                
                user.HashPassword = hashPassword.Item1;
                user.Salt = hashPassword.Item2;
                
                context.Users.Add(user);
                await context.SaveChangesAsync();
                
                return ResultModel<string,Exception>.CreateSuccessfulResult();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return ResultModel<string, Exception>.CreateFailedResult(ex);
            }
        }

        public async Task<ResultModel<string, Exception>> UpdateUser(UserModel userModel)
        {
            try
            {
                // Проверяем существование пользователя
                var existingUser = await context.Users.FindAsync(userModel.Id);
                if (existingUser == null)
                {
                    return ResultModel<string, Exception>.CreateFailedResult("User not found");
                }

                // Валидация данных
                var validateResult = await ValidateUserForUpdate(userModel, existingUser);
                if (!validateResult.IsSuccessful)
                {
                    return ResultModel<string, Exception>.CreateFailedResult(validateResult.Value);
                }

                // Обновляем поля пользователя
                existingUser.Username = userModel.Username;
                existingUser.Email = userModel.Email;
                existingUser.VpnLinks = userModel.VpnLinks ?? existingUser.VpnLinks;
                existingUser.ServerName = userModel.ServerName ?? existingUser.ServerName;

                // Если пароль изменился - перехешируем его
                if (!string.IsNullOrWhiteSpace(userModel.Password))
                {
                    var hashPassword = HashPassword(userModel.Password);
                    existingUser.HashPassword = hashPassword.Item1;
                    existingUser.Salt = hashPassword.Item2;
                }

                // Если email изменился - проверка уникальности уже выполнена в ValidateUserForUpdate
                context.Users.Update(existingUser);
                await context.SaveChangesAsync();
                
                return ResultModel<string, Exception>.CreateSuccessfulResult();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return ResultModel<string, Exception>.CreateFailedResult(ex);
            }
        }

        public async Task<ResultModel<bool, Exception>> DeleteUser(int id)
        {
            try
            {
                var user = await context.Users.FindAsync(id);
                if (user == null)
                {
                    return ResultModel<bool, Exception>.CreateSuccessfulResult();
                }
                context.Users.Remove(user);
                
                return ResultModel<bool,Exception>.CreateSuccessfulResult();
                
            }catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return ResultModel<bool, Exception>.CreateFailedResult(ex);
            }
        }

        public async Task<ResultModel<List<UserModel>?, Exception>> GetUsers()
        {
            try
            {
                var users = await context.Users.ToListAsync();
                var userModels = mapper.Map<List<UserModel>>(users);
                return ResultModel<List<UserModel>?, Exception>.CreateSuccessfulResult(userModels);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return ResultModel<List<UserModel>?, Exception>.CreateFailedResult(ex);
            }
        }

        /// <summary>
        /// Возвращает IQueryable для работы с OData.
        /// OData middleware автоматически применит параметры запроса ($filter, $orderby, $select и т.д.)
        /// к этому IQueryable перед выполнением запроса к базе данных.
        /// </summary>
        public IQueryable<User> GetUsersQueryable()
        {
            return context.Users;
        }

        public async Task<ResultModel<UserModel?, Exception>> GetUserById(int id)
        {
            try
            {
                var user = await context.Users.FindAsync(id);
                if (user != null)
                {
                    return ResultModel<UserModel?, Exception>.CreateSuccessfulResult(null);
                }

                var userModel = mapper.Map<UserModel>(user);

                return ResultModel<UserModel?, Exception>.CreateSuccessfulResult(userModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return ResultModel<UserModel?, Exception>.CreateFailedResult(ex);
            }
            
        }

        private async Task<ResultModel<string, Exception>> ValidateUser(UserModel user)
        {
            //Проверка валидности Email.
            if (!ValidateEmail(user))
            {
                return ResultModel<string, Exception>.CreateFailedResult("Email is invalid");
            }

            if (!ValidatePassword(user))
            {
                return ResultModel<string, Exception>.CreateFailedResult("Password is invalid");
            }
            
            // Проверка уникальности email.
            var result = await context.Users.Where(u => u.Email == user.Email).FirstOrDefaultAsync();
            if (result != null)
            {
                return ResultModel<string, Exception>.CreateFailedResult("Email is already in use");
            }
            
            return ResultModel<string, Exception>.CreateSuccessfulResult();
        }

        private static bool ValidateEmail(UserModel user)
        {
            return user.Email is { Length: > 0 } && user.Email.Contains('@') && user.Email.Contains('.');
        }

        private static bool ValidatePassword(UserModel user)
        {
            return user.Password.Length >= 8;
        }

        /// <summary>
        /// Валидация пользователя при обновлении.
        /// Проверяет валидность email и пароля, а также уникальность email (если email изменился).
        /// </summary>
        /// <param name="userModel">Модель пользователя для обновления</param>
        /// <param name="existingUser">Существующий пользователь из БД</param>
        /// <returns>Результат валидации</returns>
        private async Task<ResultModel<string, Exception>> ValidateUserForUpdate(UserModel userModel, User existingUser)
        {
            // Проверка валидности Email
            if (!ValidateEmail(userModel))
            {
                return ResultModel<string, Exception>.CreateFailedResult("Email is invalid");
            }

            // Проверка валидности пароля (если пароль указан)
            if (!string.IsNullOrWhiteSpace(userModel.Password) && !ValidatePassword(userModel))
            {
                return ResultModel<string, Exception>.CreateFailedResult("Password is invalid");
            }

            // Проверка уникальности email только если email изменился
            if (existingUser.Email != userModel.Email)
            {
                var userWithSameEmail = await context.Users
                    .Where(u => u.Email == userModel.Email && u.Id != userModel.Id)
                    .FirstOrDefaultAsync();
                
                if (userWithSameEmail != null)
                {
                    return ResultModel<string, Exception>.CreateFailedResult("Email is already in use");
                }
            }

            return ResultModel<string, Exception>.CreateSuccessfulResult();
        }
        
        /// <summary>
        /// Генерирует криптографически стойкую случайную соль.
        /// </summary>
        /// <returns>Соль в виде hex-строки длиной 32 символа</returns>
        private static string GenerateSalt()
        {
            // Генерируем 16 байт случайных данных (32 символа в hex)
            var saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToHexString(saltBytes);
        }

        /// <summary>
        /// Хеширует пароль с использованием соли.
        /// Возвращает Tuple: первый элемент - хеш пароля, второй элемент - соль.
        /// </summary>
        /// <param name="password">Пароль для хеширования</param>
        /// <returns>Tuple с хешем пароля и солью</returns>
        private Tuple<string, string> HashPassword(string password)
        {
            // Генерируем соль
            var salt = GenerateSalt();

            // Хешируем пароль с солью
            var hashedPassword = ComputeHash(password, salt);

            return new Tuple<string, string>(hashedPassword, salt);
        }

        /// <summary>
        /// Вычисляет SHA256 хеш пароля с солью.
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <param name="salt">Соль</param>
        /// <returns>Хеш в виде hex-строки</returns>
        private static string ComputeHash(string password, string salt)
        {
            // Объединяем пароль и соль
            var passwordWithSalt = password + salt;

            // Вычисляем SHA256 хеш
            var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(passwordWithSalt));
            return Convert.ToHexString(hashBytes);
        }

    }
}
