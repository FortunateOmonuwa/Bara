using ScriptModule.Enums;
using SharedModule.Models;

namespace UserModule.DTOs.ServiceDTOs
{
    /// <summary>
    /// Represents the details of a service offered by a writer.
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Description"></param>
    /// <param name="MinPrice"></param>
    /// <param name="MaxPrice"></param>
    /// <param name="Currency"></param>
    /// <param name="CurrencySymbol"></param>
    /// <param name="IPDealType"></param>
    /// <param name="SharePercentage"></param>
    /// <param name="PaymentTyp"></param>
    /// <param name="Genre"></param>
    public class GetServiceDetailDTO
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public decimal MinPrice { get; init; }
        public decimal MaxPrice { get; init; }
        public Currency Currency { get; init; }
        public string? CurrencySymbol { get; init; }
        public IPDealType IPDealType { get; init; }
        public int SharePercentage { get; init; }
        public PaymentType PaymentType { get; init; }
        public List<string>? Genre { get; init; }
    }

}
