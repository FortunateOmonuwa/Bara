﻿using SharedModule.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TransactionModule.Models;
using UserModule.Enums;

namespace UserModule.Models
{
    /// <summary>
    /// Represents the core user entity in the system, capturing identity, profile,
    /// verification, contact details, and related relationships like wallet and authentication.
    /// </summary>
    public class User : BaseEntity
    {
        /// <summary>
        /// The user's first name.
        /// </summary>
        [DataType(DataType.Text), Column(TypeName = "Nvarchar(60)")]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// The user's last name.
        /// </summary>
        [DataType(DataType.Text), Column(TypeName = "Nvarchar(60)")]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// The user's optional middle name.
        /// </summary>
        [DataType(DataType.Text), Column(TypeName = "Nvarchar(60)")]
        public string MiddleName { get; set; } = "";

        /// <summary>
        /// The user's email address.
        /// Must be in valid email format.
        /// </summary>
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$\r\n", ErrorMessage = "Please enter a valid Email Address"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// The user's phone number.
        /// Must be a valid international or local number (10 to 15 digits)..With country code.
        /// </summary>
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone no."), DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = string.Empty;
        /// <summary>
        /// A brief biography or description provided by the user.
        /// </summary>
        [DataType(DataType.Text), Column(TypeName = "Nvarchar(200)")]
        public string Bio { get; set; } = string.Empty;
        /// <summary>
        /// The user's gender.
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// The user's date of birth.
        /// </summary>
        public DateOnly DateOfBirth { get; set; }

        /// <summary>
        /// Indicates whether the user has been blacklisted due to policy violations or suspicious activity.
        /// </summary>
        public bool IsBlacklisted { get; set; }

        /// <summary>
        /// Indicates whether the user account is marked as deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// The user's physical address.
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// The verification document uploaded by the user for identity verification.
        /// </summary>
        public Document Document { get; set; }

        /// <summary>
        /// The current verification status of the user.
        /// </summary>
        public VerificationStatus VerificationStatus { get; set; } = VerificationStatus.Pending;

        /// <summary>
        /// The user's digital wallet used for transactions.
        /// </summary>
        public Wallet Wallet { get; set; } = new Wallet();

        /// <summary>
        /// The user's authentication profile, containing security-related info.
        /// </summary>
        public AuthProfile AuthProfile { get; set; }

        /// <summary>
        /// Defines the type of user... {Producer or Writer}
        /// </summary>
        public Role Type { get; set; }

        public List<BankDetail> BankDetails { get; set; } = new List<BankDetail>();

        /// <summary>
        /// Timestamp indicating when the user was soft-deleted.
        /// Null if not deleted.
        /// </summary>
        public DateTimeOffset? DeletedAt { get; set; }

        /// <summary>
        /// Extracted date component from <see cref="DeletedAt"/>.
        /// Defaults to <see cref="DateOnly.MinValue"/> if not deleted.
        /// </summary>
        public DateOnly DeletedAtDate => DeletedAt.HasValue ? DateOnly.FromDateTime(DeletedAt.Value.UtcDateTime) : DateOnly.MinValue;

        /// <summary>
        /// Extracted time component from <see cref="DeletedAt"/>.
        /// Defaults to <see cref="TimeOnly.MinValue"/> if not deleted.
        /// </summary>
        public TimeOnly DeletedAtTime => DeletedAt.HasValue ? TimeOnly.FromDateTime(DeletedAt.Value.UtcDateTime) : TimeOnly.MinValue;
    }
}
