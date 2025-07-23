using ScriptModule.DTOs;
using ScriptModule.Models;
using SharedModule.Interfaces;
using SharedModule.Utils;

namespace ScriptModule.Interfaces
{
    public interface IScriptService : IBaseService<Script>
    {
        /// <summary>
        /// Retrieves a script detail by its ID.
        /// </summary>
        /// <param name="scriptId">The ID of the script to retrieve.</param>
        /// <param name="writerId">The ID of the writer ID who owns the script.</param>
        /// <returns>The script detail with the specified script ID and writer ID, or null if not found.</returns>
        Task<ResponseDetail<ScriptDetailGetDTO>> GetScriptDetailById(Guid scriptId, Guid? writerId);

        /// <summary>
        /// Retrieves all scripts details associated with a specific writer.
        /// </summary>
        /// <param name="writerId">The ID of the writer whose scripts are to be retrieved.</param>
        /// <returns>A list of scripts details associated with the specified writer.</returns>
        Task<ResponseDetail<List<ScriptDetailGetDTO>>> GetScriptDetailsByWriterId(Guid writerId, int pageNumber, int pageSize);

        /// <summary>
        /// Retrieves all scripts details
        /// </summary>
        /// <returns>A list of scripts details</returns>
        Task<ResponseDetail<List<ScriptDetailGetDTO>>> GetScriptDetails(int pageNumber, int pageSize); //Remember to create 2 endpoints... one for all scripts, second for premium members.

        /// <summary>
        /// Retrieves the actual script
        /// </summary>
        /// <param name="scriptId">The ID of the script to be retrieved</param>
        /// <returns>A script file</returns>
        Task<ResponseDetail<byte[]>> GetScriptFile(Guid scriptId);

        /// <summary>
        /// Adds a new script.
        /// </summary>
        /// <param name="scriptDetails">Represents the details of the script </param>
        /// <param name="writerId">Represents the id of the writer</param>
        /// <returns></returns>
        Task<ResponseDetail<ScriptDetailGetDTO>> AddScript(ScriptDetailPostDTO scriptDetails, Guid writerId);

        /// <summary>
        /// Updates a script.
        /// </summary>
        /// <param name="scriptDetails">Represents the details of the script </param>
        /// <param name="writerId">Represents the id of the writer</param>
        /// <param name="scriptId">Represents the id of the script to be updated</param>
        Task<ResponseDetail<ScriptDetailGetDTO>> UpdateScript(ScriptDetailPostDTO scriptDetails, Guid writerId, Guid scriptId);

        /// <summary>
        /// Deletes a script.
        /// </summary>
        /// <param name="scriptId">Represents the id of the script to be deleted</param>
        /// <param name="writerId">Represents the id of the writer</param>
        Task<ResponseDetail<bool>> DeleteScript(Guid scriptId, Guid? writerId);
    }
}
