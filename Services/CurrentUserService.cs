using System.Security.Claims;

namespace CSharpApi.Services
{
    public class CurrentUserService(IHttpContextAccessor acessor)
    {
        private ClaimsPrincipal User =>
            acessor.HttpContext?.User
            ?? throw new UnauthorizedAccessException("Usuário não autenticado");

        public int UserId
        {
            get
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return int.Parse(userIdClaim!);
            }
        }

        public string Role =>
            User.FindFirstValue(ClaimTypes.Role)!;

        public bool IsAdmin =>
            User.IsInRole("Admin");
    }
}