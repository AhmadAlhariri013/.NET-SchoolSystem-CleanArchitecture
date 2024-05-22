using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchoolProject.Data.Entities.Identities;
using SchoolProject.Data.Helpers;
using SchoolProject.Data.Responses;
using SchoolProject.Infrustructure.Interfaces;
using SchoolProject.Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SchoolProject.Service.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Fields
        private readonly JwtSettings _jwtSettings;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly UserManager<User> _userManager;
        #endregion

        #region Constructors
        public AuthenticationService(JwtSettings jwtSettings,
                                     IRefreshTokenRepository refreshTokenRepository,
                                     UserManager<User> userManager)
        {
            _jwtSettings = jwtSettings;
            _refreshTokenRepository = refreshTokenRepository;
            _userManager = userManager;
        }


        #endregion

        #region Handle Functions
        public async Task<JwtAuthResponse> GetJWTToken(User user)
        {
            // Generate Access Token
            var (jwtToken, accessToken) = await GenerateJWTToken(user);

            // Generate Refresh Token && Store It In Database
            var refreshToken = GetRefreshToken(user.UserName);

            var refreshTokenToStore = new UserRefreshToken
            {
                UserId = user.Id,
                RefreshToken = refreshToken.TokenString,
                AccessToken = accessToken,
                JwtId = jwtToken.Id,
                IsUsed = true,
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMonths(_jwtSettings.RefreshTokenExpireDate),
            };

            // Store Refresh Token To Database
            await _refreshTokenRepository.AddAsync(refreshTokenToStore);

            // Return JwtAuthResponse
            return new JwtAuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };


        }

        private async Task<(JwtSecurityToken, string)> GenerateJWTToken(User user)
        {
            // Get User Claims
            var claims = await GetClaims(user);

            // JWT Object
            var jwtToken = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: DateTime.Now.AddDays(_jwtSettings.AccessTokenExpireDate),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)), SecurityAlgorithms.HmacSha256Signature));

            // Converts the JWT object to a string representation (access token).
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            // Returns a tuple containing both the JWT object and the access token string.
            return (jwtToken, accessToken);
        }

        private RefreshToken GetRefreshToken(string userName)
        {
            // Return a new RefreshToken object
            return new RefreshToken
            {
                UserName = userName,
                ExpierdAt = DateTime.UtcNow.AddMonths(_jwtSettings.RefreshTokenExpireDate),
                TokenString = GenerateRefreshToken()  // Generate a random string for the refresh token

            };
        }

        public async Task<JwtAuthResponse> GetRefreshToken(User user, JwtSecurityToken jwtToken, DateTime? expireDate, string refreshToken)
        {
            // Generate New JWT Token and Deconstruct the result 
            var (jwtSecurityToken, newToken) = await GenerateJWTToken(user);

            // Create an object of JwtAuthResponse and one of the RefreshToken
            var refreshTokenResult = new RefreshToken();
            refreshTokenResult.UserName = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimModel.UserName)).Value;
            refreshTokenResult.TokenString = refreshToken;
            refreshTokenResult.ExpierdAt = (DateTime)expireDate;

            var response = new JwtAuthResponse();
            response.AccessToken = newToken;
            response.RefreshToken = refreshTokenResult;

            return response;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<List<Claim>> GetClaims(User user)
        {
            // Get User's Roles
            var userRoles = await _userManager.GetRolesAsync(user);

            // Create claims 
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(nameof(UserClaimModel.PhoneNumber), user.PhoneNumber),
                new Claim(nameof(UserClaimModel.Id), user.Id.ToString())
            };

            // Add User's Roles to the claims list
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Get User's Claims
            var userClaims = await _userManager.GetClaimsAsync(user);

            // Add User's Claims to claims list
            claims.AddRange(userClaims);

            // Return the claims list that will be added to the token when generate it
            return claims;
        }

        public JwtSecurityToken ReadJWTToken(string accessToken)
        {
            // Check If The Access Token Null Or Empty
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            // Parse the access token string into a JwtSecurityToken object
            var response = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);

            // Return response object that contains the claims and other information encoded within the token.
            return response;

        }

        public async Task<string> ValidateToken(string accessToken)
        {
            // Specify the validation rules
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = _jwtSettings.ValidateIssuer,
                ValidIssuers = new[] { _jwtSettings.Issuer },
                ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
                ValidAudience = _jwtSettings.Audience,
                ValidateAudience = _jwtSettings.ValidateAudience,
                ValidateLifetime = _jwtSettings.ValidateLifeTime,
            };


            try
            {
                // Validate the access token against the specified parameters
                var validator = new JwtSecurityTokenHandler().ValidateToken(accessToken, parameters, out SecurityToken validatedToken);

                // checks if the token is valid
                if (validator == null)
                {
                    return "InvalidToken";
                }

                return "NotExpired";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string accessToken, string refreshToken)
        {
            // Check on the jwt Token if its valid and the algorithm used is the same algorithm in the header
            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
            {
                return ("AlgorithmIsWrong", null);
            }

            // Check if JWT Token is not expired
            if (jwtToken.ValidTo > DateTime.UtcNow)
            {
                return ("TokenIsNotExpired", null);
            }

            // Extracts the user ID from the claims within the token.
            var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimModel.Id)).Value;

            // Searches for a matching refresh token record
            var userRefreshToken = await _refreshTokenRepository.GetTableNoTracking()
                                             .FirstOrDefaultAsync(x => x.AccessToken == accessToken &&
                                                                       x.RefreshToken == refreshToken &&
                                                                       x.UserId == int.Parse(userId));

            // Check If no matching record
            if (userRefreshToken == null)
            {
                return ("RefreshTokenIsNotFound", null);
            }

            // Check its expiry date
            if (userRefreshToken.ExpiresAt < DateTime.UtcNow)
            {
                userRefreshToken.IsRevoked = true;
                userRefreshToken.IsUsed = false;
                await _refreshTokenRepository.UpdateAsync(userRefreshToken);
                return ("RefreshTokenIsExpired", null);
            }

            var expirydate = userRefreshToken.ExpiresAt;
            return (userId, expirydate);
        }




        #endregion
    }
}
