using SharedModule.Utils;
using UserModule.DTOs.ProducerDTOs;

namespace UserModule.Interfaces.UserInterfaces
{
    /// <summary>
    /// Defines the interface for producer services, providing methods to manage producers
    /// </summary>
    public interface IProducerService
    {
        /// <summary>
        /// Adds a new producer profile
        /// </summary>
        /// <param name="producerDetailDTO"></param>
        /// <returns>The newly created producer detail</returns>
        Task<ResponseDetail<GetProducerDetailDTO>> AddProducer(PostProducerDetailDTO producerDetailDTO);
        /// <summary>
        /// Updates a producer's profile 
        /// </summary>
        /// <param name="producerDetailDTO"></param>
        /// <returns>The updated producer detail</returns>
        Task<ResponseDetail<GetProducerDetailDTO>> UpdateProducer(PostProducerDetailDTO producerDetailDTO, Guid producerId);
        /// <summary>
        /// Deletes a producer's profile
        /// </summary>
        /// <param name="producerId"></param>
        /// <returns></returns>
        Task<bool> DeleteProducer(Guid producerId);
        /// <summary>
        /// Gets a producer's profile
        /// </summary>
        /// <param name="producerId"></param>
        /// <returns></returns>
        Task<ResponseDetail<GetProducerDetailDTO>> GetProducer(Guid producerId);
    }
}
