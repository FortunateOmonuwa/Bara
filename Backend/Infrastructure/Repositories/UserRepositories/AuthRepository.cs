using SharedModule.Utils;
using UserModule.DTOs.AuthDTOs;
using UserModule.Interfaces.UserInterfaces;

namespace Infrastructure.Repositories.UserRepositories
{
    public class AuthRepository : IAuthService
    {
        public Task<ResponseDetail<bool>> ChangePassword(PasswordChangeDTO reqBody)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDetail<AuthResponseDTO>> Login(AuthRequestDTO authReqBody)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDetail<bool>> Logout(string token)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDetail<bool>> VerifyAccount(string token)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDetail<bool>> VerifyLoginToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
