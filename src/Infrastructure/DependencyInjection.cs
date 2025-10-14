using Application.Common.Interfaces;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using TodoListApiCA.Application.Common.Interfaces;
using TodoListApiCA.Domain.Constants;
using TodoListApiCA.Infrastructure.Identity;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("TodoListApiDb");

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
            options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        });

        builder.Services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        builder.Services.AddAuthentication()
           .AddBearerToken(IdentityConstants.BearerScheme);

        builder.Services.AddAuthorizationBuilder();

        builder.Services
            .AddIdentityCore<AppUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddApiEndpoints();

        builder.Services.AddTransient<IIdentityService, IdentityService>();

        //builder.Services.AddAuthorization(options =>
        //    options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));

        builder.Services.AddAuthorization(options =>
        {
            // Policy 1: chỉ Admin được Purge
            options.AddPolicy(Policies.CanPurge, policy => 
                policy.RequireRole(Roles.Administrator));

            // Policy 2: Admin or chủ TodoList được Delete
            options.AddPolicy(Policies.CanDeleteTodo, policy =>
                policy.RequireAssertion(context =>
                    context.User.IsInRole(Roles.Administrator) ||
                    context.User.HasClaim("TodoOwnerId", "true")));
        });
    }
}
