using SharedModule.Utils;
using UserModule.DTOs.AuthDTOs;

namespace UserModule.Interfaces.UserInterfaces
{
    public interface IAuthService
    {
        Task<ResponseDetail<AuthResponseDTO>> Login(AuthRequestDTO authReqBody);
        Task<string> GenerateRefreshToken(string token);
        Task<ResponseDetail<bool>> Logout(string token);
        Task<ResponseDetail<bool>> VerifyLoginToken(string token);
        Task<ResponseDetail<bool>> ChangePassword(PasswordChangeDTO reqBody);
        Task<ResponseDetail<bool>> VerifyAccount(string token);
    }
}
