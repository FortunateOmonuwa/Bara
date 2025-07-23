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
    public record GetServiceDetailDTO(
         string Name,
         string Description,
         decimal MinPrice,
         decimal MaxPrice,
         string Currency,
         string CurrencySymbol,
         string IPDealType,
         int SharePercentage,
         string PaymentTyp,
         List<string> Genre
    );
}
