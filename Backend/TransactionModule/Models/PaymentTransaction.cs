﻿using SharedModule.Models;
using System.ComponentModel.DataAnnotations.Schema;
using TransactionModule.Enums;
using TransactionStatus = TransactionModule.Enums.TransactionStatus;

namespace TransactionModule.Models
{
    public class PaymentTransaction : BaseEntity
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; } = Guid.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Fee { get; set; }
        public Currency Currency { get; set; } = Currency.NAIRA;
        public string CurrencySymbol => Currency switch
        {
            Currency.NAIRA => "₦",
            Currency.USD => "$",
            Currency.EUR => "€",
            Currency.GBP => "£",
            _ => "₦"
        };
        public TransactionType TransactionType { get; set; }
        public TransactionStatus Status { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }
        public DateOnly? DateCompleted => CompletedAt.HasValue ? DateOnly.FromDateTime(CompletedAt.Value.UtcDateTime) : null;
        public TimeOnly? TimeCompleted => CompletedAt.HasValue ? TimeOnly.FromDateTime(CompletedAt.Value.UtcDateTime) : null;

        /// <summary>
        /// Reference ID for the transaction, such as a payment gateway transaction ID or internal reference number.
        /// </summary>
        public string? ReferenceId { get; set; }
        /// <summary>
        /// Optional notes or comments about the transaction, such as payment details or special instructions.
        /// </summary>
        public string? Notes { get; set; }
        /// <summary>
        /// Optional response from the payment gateway or service provider.
        /// </summary>
        public string? GatewayResponse { get; set; }
        //[ForeignKey(nameof(PaymentDetail))]
        //public Guid? PaymentDetailId { get; set; }
        //public PaymentDetail? PaymentDetail { get; set; }

        [ForeignKey("Wallet")]
        public Guid? WalletID { get; set; }
        public Wallet? Wallet { get; set; }

        /// <summary>
        /// Payment method used
        /// </summary>
        public PaymentMethod PaymentMethod { get; set; }
    }
}
