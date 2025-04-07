using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Smile_Simulation.Application.Mapping;
using Smile_Simulation.Application.Services;
using Smile_Simulation.Domain.DTOs.EmailDto;
using Smile_Simulation.Domain.Entities;
using Smile_Simulation.Domain.Interfaces;
using Smile_Simulation.Domain.Interfaces.Services;
using Smile_Simulation.Infrastructure.Data;
using System;
using System.Text;
using AutoMapper;
using Smile_Simulation.Domain.Interfaces.Repositories;
using Smile_Simulation.Infrastructure.Repositories;

namespace Smile_Simulation.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // Add services to the container.
            builder.Services.AddDbContext<SmileDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new ProducesAttribute("application/json")); // Force JSON response
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure Identity
            builder.Services.AddIdentity<UserApp, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<SmileDbContext>()
                .AddDefaultTokenProviders();

            // Register services
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IAdviceService, AdviceService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<ILikeRepository, LikeRepository>();
            builder.Services.AddScoped<CommentService>();
            builder.Services.AddScoped<PostService>();
            builder.Services.AddScoped<LikeService>();
            builder.Services.AddScoped<SearchService>();
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.Configure<EmailDto>(configuration.GetSection("MailSettings"));
            builder.Services.AddTransient<IEmailService, EmailService>();
            builder.Services.AddMemoryCache();


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])),

                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWT:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:Audience"],

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            var app = builder.Build();

            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    var response = new { message = "An internal server error occurred." };
                    await context.Response.WriteAsJsonAsync(response);
                });
            });


            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers(); // No need for app.UseEndpoints()

            app.Run();
        }
    }
}