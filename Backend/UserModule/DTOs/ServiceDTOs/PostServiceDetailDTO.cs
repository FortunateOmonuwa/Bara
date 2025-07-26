using ScriptModule.Enums;
using SharedModule.Models;

namespace UserModule.DTOs.ServiceDTOs
{
    /// <summary>
    /// Represents the details of a service offered by a user, such as a producer or writer.
    /// </summary>
    /// 
    public record PostServiceDetailDTO
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required decimal MinPrice { get; init; }
        public required decimal MaxPrice { get; init; }
        public Currency Currency { get; init; }
        public IPDealType IPDealType { get; init; }
        public int SharePercentage { get; init; }
        public PaymentType PaymentType { get; init; }
        public List<string> Genre { get; init; } = [];
    }
}