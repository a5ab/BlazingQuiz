using BlazingQuiz.Api.Services;
using BlazingQuiz.Shared.DTOs;
using Microsoft.AspNetCore.Routing;

namespace BlazingQuiz.Api.Endpoints
{
    public static class AuthEndpoints
    {
        public static string Login = "api/auth/login";
        public static string Register = "api/auth/register";

        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost(Login, async (LoginDto dto, AuthService authService) => Results.Ok(await authService.LoginAsync(dto)));


                return app;


        }

    }
}
