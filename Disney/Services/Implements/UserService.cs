using AutoMapper;
using Disney.Auth.Request;
using Disney.Auth.Response;
using Disney.Entities;
using Disney.Services.Interfaces;
using Disney.UOWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Disney.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uowork;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork uowork, IConfiguration config, IMapper mapper)
        {
            _uowork = uowork;
            _config = config;
            _mapper = mapper;
        }

        #region GetToken
        public string GetToken(UserResponse userResponse)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, userResponse.Email),
                new Claim(ClaimTypes.NameIdentifier, userResponse.Id.ToString()),
                new Claim(ClaimTypes.Name, userResponse.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Expires = DateTime.UtcNow.AddMinutes(120),
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials
            };
            var claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        #endregion

        #region VerifyPassword
        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            var hMac = new HMACSHA512(passwordSalt);
            var hash = hMac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (var i = 0; i < hash.Length; i++)
            {
                if (hash[i] != passwordHash[i])
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region CrearPassHash
        private void CrearPassHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            var hMac = new HMACSHA512();
            passwordSalt = hMac.Key;
            passwordHash = hMac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
        #endregion

        #region Login
        public UserResponse Login(LoginRequest userLogin)
        {
            if(_uowork.UserRepo.UserExists(userLogin.Email))
            {
                UserResponse response = new UserResponse();
                User user = _uowork.UserRepo.GetByEmail(userLogin.Email);

                if(!VerifyPassword(userLogin.Password, user.PasswordHash, user.PasswordSalt))
                {
                    return null;
                }

                response = _mapper.Map<UserResponse>(user);
                return response;
            }
            return null;
        }
        #endregion

        #region Register
        public UserResponse Register(RegisterRequest userRequest, string password)
        {
            byte[] passwordHash;
            byte[] passwordSalt;

            CrearPassHash(password, out passwordHash, out passwordSalt);
            User user = _mapper.Map<User>(userRequest);
            user.CreationDate = DateTime.Now;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _uowork.UserRepo.Insert(user);
            _uowork.Save();

            UserResponse response = _mapper.Map<UserResponse>(user);
            return response;
        }
        #endregion
    
    }
}
