﻿using SharedModule.Utils;
using UserModule.DTOs.UserDTO;
using UserModule.Models;

namespace UserModule.Interfaces.UserInterfaces
{
    /// <summary>
    /// Defines the interface for user services, providing methods to manage blacklisted users.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Begins the registration process for a new user by validating their details and generating an email verification token.
        /// </summary>
        /// <param name="detail"> Contains the Email, password and Type of User registering</param>
        /// <returns></returns>
        Task<ResponseDetail<RegisterResponseDTO>> BeginRegistration(RegisterDTO detail);

        /// <summary>
        /// Updates a users kyc verification status using the verification id number
        /// </summary>
        /// <param name="verificationIdNumber"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="verificationType"></param>
        /// <returns></returns>
        Task<ResponseDetail<bool>> UpdateUserVerificationStatus(string verificationIdNumber, string dateOfBirth, string firstName, string lastName, string verificationType);
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
