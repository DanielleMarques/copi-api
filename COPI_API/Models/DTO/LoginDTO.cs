namespace COPI_API.Models.DTO
{
    public class LoginDTO
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
    // DTO para registro
    public class RegisterDTO
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    // DTO para promoção
    public class PromoverDTO
    {
        public string Email { get; set; }
        public string NovaRole { get; set; } // "Admin", "Gestor", "UsuarioPlus", "UsuarioComum"
    }

}
