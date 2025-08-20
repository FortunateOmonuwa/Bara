namespace UserModule.DTOs.UserDTO
{
    public class RegisterResponseDTO
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
