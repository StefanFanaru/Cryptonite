﻿using System;
using Cryptonite.Infrastructure.CQRS;
using MediatR;

namespace Cryptonite.Infrastructure.Commands.TradeEntries.Insert
{
    public class InsertTradeEntryCommand : UserBasedCommand<Unit>
    {
        public decimal PaidAmount { get; set; }
        public decimal GainedAmount { get; set; }
        public string PaidCryptocurrency { get; set; }
        public string GainedCryptocurrency { get; set; }
        public DateTime TradedAt { get; set; }
    }
}