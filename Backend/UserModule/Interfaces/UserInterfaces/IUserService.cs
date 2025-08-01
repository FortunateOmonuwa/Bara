using SharedModule.Utils;
using UserModule.Models;

namespace UserModule.Interfaces.UserInterfaces
{
    /// <summary>
    /// Defines the interface for user services, providing methods to manage blacklisted users.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Adds a user to the blacklist with an optional reason.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="reason"></param>
        /// <returns>a true or false value</returns>
        Task<ResponseDetail<bool>> BlackListUser(Guid userId, string? reason);

        /// <summary>
        /// Removes a user from the blacklist.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>a true or false value</returns>
        Task<ResponseDetail<bool>> RemoveUserFromBlackList(Guid userId);

        /// <summary>
        /// Retrieves details of all blacklisted users.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>A list of blacklisted users</returns>
        Task<ResponseDetail<List<BlackListedUser>>> GetBlackListedUsers(int pageNumber, int pageSize);

        /// <summary>
        /// Retrieves details of a specific blacklisted user by their ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns> The detail of specific blacklisted user</returns>
        Task<ResponseDetail<BlackListedUser>> GetBlackListedUser(Guid userId);
    }
}
