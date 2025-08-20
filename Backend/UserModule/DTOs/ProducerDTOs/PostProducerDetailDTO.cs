﻿using UserModule.DTOs.AddressDTOs;
using UserModule.DTOs.DocumentDTOs;
using UserModule.Enums;

namespace UserModule.DTOs.ProducerDTOs
{
    /// <summary>
    /// Represents the information required to create or update a producer profile.
    /// </summary>
    public record PostProducerDetailDTO
    {
        /// <summary>
        /// The first name of the producer.
        /// </summary>
        public required string FirstName { get; init; }

        /// <summary>
        /// The last name (surname) of the producer.
        /// </summary>
        public required string LastName { get; init; }

        /// <summary>
        /// The middle name of the producer, if applicable.
        /// </summary>
        public string MiddleName { get; init; } = string.Empty;

        /// <summary>
        /// The producer's phone number.
        /// </summary>
        public required string PhoneNumber { get; init; }

        /// <summary>
        /// The date of birth of the producer.
        /// </summary>
        public required DateOnly DateOfBirth { get; init; }

        /// <summary>
        /// The gender of the producer.
        /// </summary>
        public required Gender Gender { get; init; }

        /// <summary>
        /// The address details of the producer.
        /// </summary>
        public required AddressDetail AddressDetail { get; init; }

        /// <summary>
        /// The verification document required for validating the producer's identity.
        /// </summary>
        public required PostDocumentDetailDTO VerificationDocument { get; init; }

        /// <summary>
        /// A short biography or description about the producer.
        /// </summary>
        public string Bio { get; init; } = "";
    }
}
