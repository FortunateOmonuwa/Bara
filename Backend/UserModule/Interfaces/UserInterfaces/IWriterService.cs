using SharedModule.Utils;
using UserModule.DTOs.WriterDTOs;

namespace UserModule.Interfaces.UserInterfaces
{
    /// <summary>
    /// Defines the interface for writer services, providing methods to manage writers' details.
    /// </summary>
    public interface IWriterService
    {
        /// <summary>
        /// Creates a new writer detail with the provided information.
        /// </summary>
        /// <param name="writerDetail"></param>
        /// <returns></returns>
        Task<ResponseDetail<GetWriterDetailDTO>> AddWriter(PostWriterDetailDTO writerDetail);
        /// <summary>
        /// Retrieves the details of a writer by their ID.
        /// </summary>
        /// <param name="writerId"> Represents the writer id of the writer being retrieved</param>
        /// <returns>A writer's detail</returns>
        Task<ResponseDetail<GetWriterDetailDTO>> GetWriterDetail(Guid writerId);

        /// <summary>
        /// Retrieves a list of writers with pagination support.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<ResponseDetail<List<GetWriterDetailDTO>>> GetWriters(int pageNumber, int pageSize);

        /// <summary>
        /// Updates a new writer detail with the provided information.
        /// </summary>
        /// <param name="writerId"></param>
        /// <param name="updateWriterDetail">Represents the new params used to update a writer's detail </param>
        /// <returns>The updated writer's detail</returns>
        Task<ResponseDetail<GetWriterDetailDTO>> UpdateWriterDetail(Guid writerId, PostWriterDetailDTO updateWriterDetail);

        /// <summary>
        /// Deletes a writer by their ID.
        /// </summary>
        /// <param name="writerId"></param>
        /// <returns></returns>
        Task<ResponseDetail<bool>> DeleteWriter(Guid writerId);
    }
}
