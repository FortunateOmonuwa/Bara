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
    public record class AddressDetail
    {
        public required string Street { get; init; }
        public required string City { get; init; }
        public required string State { get; init; }
        public required string Country { get; init; }
        public string PostalCode { get; init; } = "";
        public string AdditionalDetails { get; init; } = "";
    }

}
