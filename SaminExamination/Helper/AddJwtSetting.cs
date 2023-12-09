using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace SaminExamination.Helper
{
    public static class JwtAuthentication
    {
        public static void AddJwtAuthentication(this IServiceCollection services, JwtSettingsDto jwtSettings)
        {
            var secretkey = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
            var encryptionkey = Encoding.UTF8.GetBytes(jwtSettings.Encryptkey);
            var validationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero, // default: 5 min
                RequireSignedTokens = true,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretkey),

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ValidateAudience = true, //default : false
                ValidAudience = jwtSettings.Audience,

                ValidateIssuer = true, //default : false
                ValidIssuer = jwtSettings.Issuer,

                TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey)
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddCookie(options =>
             {
                 options.SlidingExpiration = true;
                 options.LoginPath = "/auth/signin";
                 //options.Cookie.Expiration = TimeSpan.FromMinutes(60);
             })
             .AddJwtBearer(options =>
             {
                 options.RequireHttpsMetadata = false;
                 options.SaveToken = true;
                 options.TokenValidationParameters = validationParameters;
                 options.Events = new JwtBearerEvents
                 {
                     OnAuthenticationFailed = context =>
                     {
                         //var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                         //logger.LogError("Authentication failed.", context.Exception);

                         if (context.Exception != null)
                             throw new Exception("توکن جاری شما معتبر نمی‌باشد، مجددا تلاش کنید!");

                         return Task.CompletedTask;
                     },
                     OnTokenValidated = async context =>
                     {
                         //var signInManager = context.HttpContext.RequestServices.GetRequiredService<SignInManager<User>>();
                         //var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserBL>();

                         //var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                         //if (claimsIdentity.Claims?.Any() != true)
                         //    context.Fail("This token has no claims.");

                         //var securityStamp = claimsIdentity.FindFirstValue(new ClaimsIdentityOptions().SecurityStampClaimType);
                         //if (!securityStamp.HasValue())
                         //    context.Fail("This token has no secuirty stamp");

                         //Find user and token from database and perform your custom validation
                         //var userId = claimsIdentity.Name;
                         //var user = userRepository.GetById(userId);

                         //if (user.SecurityStamp != Guid.Parse(securityStamp))
                         //    context.Fail("Token secuirty stamp is not valid.");

                         //var validatedUser = await signInManager.ValidateSecurityStampAsync(context.Principal);
                         //if (validatedUser == null)
                         //    context.Fail("Token secuirty stamp is not valid.");



                         //var roles = claimsIdentity.FindAll(claimsIdentity.RoleClaimType);

                         //add to context
                         //await userRepository.UpdateLastLoginDate(user);
                     },
                     OnChallenge = context =>
                     {
                         //var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                         //logger.LogError("OnChallenge error", context.Error, context.ErrorDescription);

                         if (context.AuthenticateFailure != null)
                             throw new SecurityTokenExpiredException("توکن جاری شما معتبر نمی‌باشد، مجددا تلاش کنید!");

                         return Task.CompletedTask;
                     }
                 };
             });
        }

    }
    public class JwtSettingsDto
    {
        public string SecretKey { get; set; }
        public string Encryptkey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int NotBeforeMinutes { get; set; }
        public int ExpirationMinutes { get; set; }

    }
}


