namespace UserModule.DTOs.AuthDTOs
{
    public class PasswordChangeDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}
