namespace SharedModule.DTOs.AddressDTOs
{
    /// <summary>
    /// Represents the details of an address associated with a user or entity.
    /// </summary>
    /// <param name="Street"></param>
    /// <param name="City"></param>
    /// <param name="State"></param>
    /// <param name="Country"></param>
    /// <param name="PostalCode"></param>
    /// <param name="AdditionalDetails"></param>
    public record AddressDetail(
        string Street,
        string City,
        string State,
        string Country,
        string? PostalCode,
        string AdditionalDetails
        );
}
