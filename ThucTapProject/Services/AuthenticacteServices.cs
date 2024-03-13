using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ThucTapProject.DAO;
using ThucTapProject.EditModel;
using ThucTapProject.Entities;
using ThucTapProject.EntityModel;
using ThucTapProject.IServices;
using ThucTapProject.ViewModel;
using ThucTapProject.ViewModel.response;

namespace ThucTapProject.Services
{
    public class AuthenticacteServices : IAuthenticacteServices
    {
        private readonly AccountService _accountService;
        private readonly AppDbContext _context;
        private IConfiguration _config;
        private readonly UserService _userService;

        public AuthenticacteServices(IConfiguration config)
        {
            _accountService = new AccountService();
            _config = config;
            _context = new AppDbContext();
            _userService = new UserService();
        }
        public async Task<UserLoginModel?> AuthenticateUser(LogInCredential login)
        {
            var accountLogin = await _accountService.Get(login);
            if (accountLogin == null)
            {
                return null;
            }
            return new UserLoginModel()
            {
                UserId = accountLogin.Users.FirstOrDefault().UserId,
                AccountName = accountLogin.AccountName,
                UserName = accountLogin.Users.FirstOrDefault().UserName,
                Email = accountLogin.Users.FirstOrDefault().Email,
                Role = accountLogin.Decentralization.AuthorityName
            };
        }

        public async Task<ApiResponse> GenerateJWT(UserLoginModel UserInfor)
        {
            // xoa ma token cu
            await _context.RefreshTokens.Where(c => c.UserId == UserInfor.UserId).ExecuteDeleteAsync();

            var jwtHandler = new JwtSecurityTokenHandler();
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));
            var Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, UserInfor.AccountName),
                    new Claim("User name", UserInfor.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, UserInfor.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserId", UserInfor.UserId.ToString()),
                    new Claim(ClaimTypes.Role, UserInfor.Role),

                }),
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = Credentials
            };

            var token = jwtHandler.CreateToken(tokenDescription);
            string tokenString = jwtHandler.WriteToken(token);
            string refreshToken = await GenterateRefreshToken();

            var NewRefreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                JwtId = token.Id,
                UserId = UserInfor.UserId,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IsUsedAt = DateTime.Now,
                ExpiredAt = DateTime.Now.AddHours(1)
            };

            await _context.RefreshTokens.AddAsync(NewRefreshTokenEntity);
            await _context.SaveChangesAsync();

            return new ApiResponse()
            {
                success = true,
                message = "Xác thực thành công",
                data = new TokenModel
                {
                    AccessToken = tokenString,
                    RefreshToken = refreshToken
                }
            };
        }

        private async Task<string> GenterateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);

                return Convert.ToBase64String(random);
            }
        }

        public async Task<ApiResponse> RenewToken(TokenModel model)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var SecurityKeyBytes = Encoding.UTF8.GetBytes(_config["Jwt:key"]);
            var TokenValidateParams = new TokenValidationParameters
            {
                // tu cap token
                ValidateIssuer = false,
                ValidateAudience = false,

                // ky vao token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(SecurityKeyBytes),

                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false // khong kiem tra token het han
            };

            try
            {
                //check token format 
                var tokenInverification = jwtHandler.ValidateToken(model.AccessToken, TokenValidateParams, out var ValidatedToken);

                //check algorithm
                if (ValidatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    bool result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.EcdsaSha512, StringComparison.CurrentCultureIgnoreCase);
                    if (result) return new ApiResponse { message = "invalid token", success = false, };
                }

                //check time expires
                string expDate = tokenInverification.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp).Value;
                var expireDate = long.Parse(expDate);
                var ExpireDate = convertTimeToDatetime(expireDate);

                if (ExpireDate > DateTime.Now) return new ApiResponse { message = "Mã token chua hết hạn", success = false };

                //check refresh Token Existed in db
                RefreshToken? tokenExisted = await _context.RefreshTokens.Where(x => x.Token == model.RefreshToken).FirstOrDefaultAsync();
                if (tokenExisted == null) return new ApiResponse { message = "Mã refresh token không tồn tại", success = false };

                //Kiem tra ma token refresh đã dùng hoặc thu hồi
                if (tokenExisted.IsUsed) return new ApiResponse { message = "Mã token refresh đã sử dụng", success = false };
                if (tokenExisted.IsRevoked) return new ApiResponse { message = "Mã token refresh đã thu hồi", success = false };

                // Kiểm tra token trùng id không
                var jti = tokenInverification.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
                if (jti != tokenExisted.JwtId) return new ApiResponse { message = "Token không khớp", success = false };

                //delete old token
                await _context.RefreshTokens.Where(c => c.UserId == tokenExisted.UserId && c.Token != model.RefreshToken).ExecuteDeleteAsync();

                // cập nhật trạng thái token
                /*tokenExisted.IsRevoked = true;
                tokenExisted.IsUsed = true;
                _context.RefreshTokens.Update(tokenExisted);
                await _context.SaveChangesAsync();*/

                //tạo token mới
                User user = await _context.User
                    .Include(c => c.Accountt)
                    .ThenInclude(c => c.Decentralization)
                    .SingleOrDefaultAsync(c => c.UserId == tokenExisted.UserId);

                var token = await GenerateJWT(new UserLoginModel
                {
                    UserId = user.UserId,
                    AccountName = user.Accountt.AccountName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = user.Accountt.Decentralization.AuthorityName
                });

                return new ApiResponse { message = "Tạo mới token thành công", data = token.data, success = true };
            }
            catch (Exception ex)
            {
                return new ApiResponse { message = "Something went wrong", success = false };
            }
        }

        private DateTime convertTimeToDatetime(long expireDate)
        {
            var dateTimeInterval = new DateTime(1960, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            dateTimeInterval.AddSeconds(expireDate).ToLocalTime();

            return dateTimeInterval;
        }
    }
}
