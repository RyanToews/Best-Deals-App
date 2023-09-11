using BestDealLib.Models;
using CoreApi2.Data;
using CoreApi2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoreApi2.Controllers
{
    /// <summary>
    /// AuthController
    /// This class contains the methods available to the API that enable
    /// registration, login, and token verification services.
    /// </summary>
    public class AuthController : ControllerBase
    {
        // Database context object
        private readonly ApplicationDbContext _dbContext;

        // Constructor for the class
        public AuthController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Login(). This method takes the input values of the LoginViewModel and
        /// processes the login request.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/login")]
        public IActionResult Login([FromBody] LoginViewModel model)
        {
            if (model.Password == null || model.Password.Trim() == "" || model.Username == null || model.Username.Trim() == "")
            {
                return Unauthorized();
            }

            // Find the user based on the provided username
            var user = _dbContext.Users?.FirstOrDefault(u => u.Username == model.Username);

            if (user != null && VerifyPassword(user.HashedPassword!, model.Password!, Convert.FromBase64String(user.Salt!)))
            {
                var token = GenerateToken(model.Username!);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }

        /// <summary>
        /// Register(). This method handles the data from the RegisterViewModel
        /// to facilitate the creation of new user accounts.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>  
        [HttpPost]
        [Route("api/register")]
        public IActionResult Register([FromBody] RegisterViewModel model)
        {
            // Check if the username is already taken
            if (_dbContext.Users!.Any(u => u.Username == model.Username))
            {
                return BadRequest("Username is already taken.");
            }

            // Check that password and confirmPassword are both present
            if (model.Password == null || model.ConfirmPassword == null)
            {
                return BadRequest("Password is required.");
            }

            // Check if password matches confirmPassword
            if (model.Password != model.ConfirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }

            // Generate a random salt
            byte[] saltBytes = GenerateSalt();

            // Hash the password with the salt
            string hashedPassword = HashPassword(model.Password!, saltBytes);

            // Create a new user
            var newUser = new User
            {
                Username = model.Username,
                HashedPassword = hashedPassword,
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Email = model.Email,
                Address = model.Address,
                City = model.City,
                PostalCode = model.PostalCode,
                Province = model.Province,
                Phone = model.Phone,
                Salt = Convert.ToBase64String(saltBytes),
            };

            // Save the user to the database
            _dbContext.Users!.Add(newUser);
            _dbContext.SaveChanges();

            return Ok("User registered successfully.");
        }

        /// <summary>
        /// VerifyToken(). This method takes dat from the TokenViewModel to
        /// verify that the token is still valid.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/verify-token")]
        public IActionResult VerifyToken([FromBody] TokenVerificationViewModel model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(KeyService.GetSigningKey()!);

            try
            {
                // Validate the token
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(model.Token, validationParameters, out validatedToken);

                // Check if the user exists in the database
                var usernameClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                if (usernameClaim == null || !_dbContext.Users!.Any(u => u.Username == usernameClaim.Value))
                {
                    // User doesn't exist, token is invalid
                    return Ok(new { Valid = false });
                }

                // Token is valid
                return Ok(new { Valid = true });
            }
            catch (Exception)
            {
                // Token is invalid
                return Ok(new { Valid = false });
            }
        }

        /// <summary>
        /// GetUserInfo(). This method takes in a token and returns the user
        /// account info for the user login that created the token.
        /// </summary>
        /// <param name="tokenRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/user-info")]
        public IActionResult GetUserInfo([FromBody] TokenRequestModel tokenRequest)
        {
            string token = tokenRequest.Token!;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(KeyService.GetSigningKey()!);

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // Extract the username from the validated token
                var username = principal.Identity?.Name;

                // Retrieve the user based on the username
                var user = _dbContext.Users?.FirstOrDefault(u => u.Username == username);

                if (user != null)
                {
                    // Return the relevant user data
                    var userInfo = new
                    {
                        user.Username,
                        user.Firstname,
                        user.Lastname,
                        user.Email,
                        user.Address,
                        user.City,
                        user.PostalCode,
                        user.Province,
                        user.Phone
                    };

                    return Ok(userInfo);
                }

                // User not found
                return NotFound();
            }
            catch (Exception)
            {
                // Token is invalid
                return Unauthorized();
            }
        }

        /// <summary>
        /// VerifyPassword(). This is a helper method that compares a submitted
        /// string, hashes it and compares to the hashed password stored in the
        /// database.
        /// </summary>
        /// <param name="hashedPassword"></param>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private bool VerifyPassword(string hashedPassword, string password, byte[] salt)
        {
            string hashed = HashPassword(password, salt);
            return string.Equals(hashedPassword, hashed);
        }

        /// <summary>
        /// HashPassword(). This helper method takes the password, adds a salt
        /// to the string and then hashes it.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="saltBytes"></param>
        /// <returns></returns>
        private string HashPassword(string password, byte[] saltBytes)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32);
                return Convert.ToBase64String(hashBytes);
            }
        }

        /// <summary>
        /// GenerateToken(). This method create a JWT token for a user account.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private string GenerateToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(KeyService.GetSigningKey());
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// GenerateSalt(). This method randomly generates a list of bytes that 
        /// act as salts for the pasword hashing process.
        /// </summary>
        /// <returns></returns>
        private byte[] GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return saltBytes;
        }
    }
}