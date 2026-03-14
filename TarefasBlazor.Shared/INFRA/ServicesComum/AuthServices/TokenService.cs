using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TarefasBlazor.Shared.INFRA.ServicesComum.GeradorDeIDsService;
using TarefasBlazor.Shared.MODULOS.COMUM.Entidades;

namespace TarefasBlazor.Shared.INFRA.ServicesComum.AuthServices
{
    public class TokenService
    {
        private readonly JwtSettings _jwtOptions;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public TokenService(JwtSettings jwtOptions)
        {
            _jwtOptions = jwtOptions;            
            
            _tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret)),
                ValidateIssuer = true,
                ValidIssuer = _jwtOptions.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtOptions.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = (SymmetricSecurityKey)_tokenValidationParameters.IssuerSigningKey;
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshToken GenerateRefreshToken(Guid IDUsuario)
        {
            return new RefreshToken
            {
                ID = CriarIDService.CriarNovoID(),
                IDUsuario = IDUsuario,
                Token = GenerateRandomTokenString(),
                Expires = DateTime.Now.AddDays(_jwtOptions.RefreshTokenExpirationDays),
                Created = DateTime.Now,
                IsUsed = false,
                IsRevoked = null
            };
        }
                
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = _tokenValidationParameters.Clone();         
            tokenValidationParameters.ValidateLifetime = false;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
               
                if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        private static string GenerateRandomTokenString()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}