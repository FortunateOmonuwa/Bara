﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserModule.Models
{
    public class BankDetail
    {
        // <summary>
        /// The unique identifier of the user who owns this bank account.
        /// Links this bank account to a specific user in the system for withdrawal purposes.
        /// </summary>
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Navigation property to the User entity who owns this bank account.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// The 10-digit NUBAN (Nigeria Uniform Bank Account Number) format account number.
        /// Required for bank transfers and account verification.
        /// Example: "0123456789"
        /// </summary>
        [DataType(DataType.Text), Column(TypeName = "Nvarchar(50)")]
        public string AccountNumber { get; set; }

        /// <summary>
        /// The full name of the bank where the account is held.
        /// Retrieved from Paystack's bank list API during account verification.
        /// Example: "First Bank of Nigeria", "Guaranty Trust Bank"
        /// </summary>
        [DataType(DataType.Text), Column(TypeName = "Nvarchar(100)")]
        public string BankName { get; set; }

        /// <summary>
        /// The unique 3-digit numerical code assigned to each bank by the Central Bank of Nigeria.
        /// Used by Paystack API to identify which bank to transfer money to.
        /// Example: "011" (First Bank), "058" (GTBank), "044" (Access Bank)
        /// </summary>
        [DataType(DataType.Text), Column(TypeName = "Nvarchar(10)")]
        public string BankCode { get; set; }

        /// <summary>
        /// The account holder's full name as registered with the bank.
        /// Retrieved during account verification to ensure account number matches actual holder.
        /// Example: "John Olumide Adebayo"
        /// </summary>
        [DataType(DataType.Text), Column(TypeName = "Nvarchar(100)")]
        public string AccountName { get; set; }

        ///// <summary>
        ///// Indicates whether this bank account has been verified with the bank.
        ///// Only verified accounts can be used for withdrawals.
        ///// </summary>
        //public bool IsVerified { get; set; } = false;

        /// <summary>
        /// Indicates whether this bank account is currently active for withdrawals.
        /// Provides soft-delete functionality for bank accounts.
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
