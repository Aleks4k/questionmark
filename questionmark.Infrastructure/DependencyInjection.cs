using questionmark.Application.Common.Contracts;
using questionmark.Application.Users.Contracts;
using questionmark.Infrastructure.Repository;
using questionmark.Infrastructure.Services;
using questionmark.Infrastructure.Settings;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using questionmark.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using questionmark.Application.Sessions.Contracts;
using questionmark.Application.Posts.Contracts;

namespace questionmark.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
            var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION");
            services.AddDbContext<AppDbContext>(
                dbContextOptions => dbContextOptions
                    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
            );
            var jwtSettings = new JwtSettings()
            {
                AccessTokenKey = Environment.GetEnvironmentVariable("JWT_AccessTokenKey")!,
                RefreshTokenKey = Environment.GetEnvironmentVariable("JWT_RefreshTokenKey")!,
                Issuer = Environment.GetEnvironmentVariable("JWT_Issuer")!,
                Audience = Environment.GetEnvironmentVariable("JWT_Audience")!,
                AccessTokenTTL = Convert.ToInt32(Environment.GetEnvironmentVariable("JWT_AccessTokenTTL")),
                RefreshTokenTTL = Convert.ToInt32(Environment.GetEnvironmentVariable("JWT_RefreshTokenTTL"))
            };
            services.AddSingleton(jwtSettings);
            services.AddScoped<IJwtService, JwtService>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.AccessTokenKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddAuthorization();
            services.AddScoped<IUser, UserRepository>();
            var hashSettings = new HashSettings()
            {
                authHashKey = Environment.GetEnvironmentVariable("HASH_AuthSalt")!, //192
                cipherHashKey = Environment.GetEnvironmentVariable("HASH_CipherSalt")! //192
            };
            services.AddSingleton(hashSettings);
            services.AddScoped<IHashService, HashService>();
            var encryptionSettings = new EncryptionSettings()
            {
                AESKey_user = Environment.GetEnvironmentVariable("AES_SessionKey")! //32 hex
            };
            services.AddSingleton(encryptionSettings);
            services.AddScoped<IEncryptionService, EncryptionService>();
            services.AddScoped<ISession, SessionRepository>();
            services.AddScoped<IPost, PostRepository>();
            return services;
        }
    }
}
