namespace UserModule.DTOs.AuthDTOs
{
    public class AuthResponseDTO
    {
        public Guid UserId { get; set; }
        public Guid AuthProfileID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string Role { get; set; }
        public string IsBlackListed { get; set; }
        public string IsVerified { get; set; }
        public string VerificationStatus { get; init; }
    }
}
