using Microsoft.AspNetCore.Mvc;
using NSwag;
using NSwag.Generation.Processors.Security;
using TodoListApiCA.Application.Common.Interfaces;
using TodoListApiCA.WebAPI.Infrustructure;
using TodoListApiCA.WebAPI.Services;

namespace WebAPI;

public static class DependencyInjection
{
    public static void AddWebAPIServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUser, CurrentUser>();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        // Customize default API behavior
        builder.Services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true); // vô hiệu hóa (suppress) hành vi mặc đinh của framework khi Model Validation thất bại.

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddOpenApiDocument((cfg, sp) =>
        {
            cfg.Title = "TodoList API";

            // Add JWT
            cfg.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into textbox: Bearer {your JWT token}."
            });

            cfg.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT")); // nói vs NSwage rằng: Hãy áp dụng security scheme tên là JWT cho tất cả các API operations khi sinh tài liệu Swagger.
        });
    }
}
