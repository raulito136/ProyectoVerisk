using System.Security.Claims; // Necesario para Claims

namespace ReferenceData.Api.Middleware
{
    public class ServiceKeyMiddleware(RequestDelegate next, IConfiguration config)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            // 1. Si no hay JWT, buscamos la Service Key
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                var serviceKey = context.Request.Headers["X-Service-Key"].FirstOrDefault();
                var expectedKey = config["ServiceKey"];

                if (string.IsNullOrEmpty(serviceKey) || serviceKey != expectedKey)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsJsonAsync(new { errors = new[] { new { field = "", message = "Unauthorized" } } });
                    return;
                }

                // --- ESTA ES LA PARTE CLAVE ---
                // Creamos una "identidad" manual. Al darle un nombre y un tipo de auth, 
                // .NET marca la petición como "Autenticada".
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, "ExternalService"),
                    new Claim("Role", "ServiceAccount")
                };

                var identity = new ClaimsIdentity(claims, "ServiceKeyMode");
                context.User = new ClaimsPrincipal(identity);
                // ------------------------------
            }

            // Si llegamos aquí, o había JWT o la Service Key era correcta y ya tenemos identidad.
            await next(context);
        }
    }
}