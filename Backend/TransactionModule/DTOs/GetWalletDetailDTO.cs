﻿using SharedModule.Models;

namespace TransactionModule.DTOs
{
    public record GetWalletDetailDTO
    {
        public Guid Id { get; init; }
        public decimal Balance { get; init; }
        public decimal LockedBalance { get; init; }
        public Currency Currency { get; init; }
        public string? CurrencySymbol { get; init; }
        public Guid UserId { get; init; }
    }
}
