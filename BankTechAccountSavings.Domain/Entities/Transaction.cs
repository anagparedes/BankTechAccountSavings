﻿using BankTechAccountSavings.Domain.Enums;
using BankTechAccountSavings.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTechAccountSavings.Domain.Entities
{
    public abstract class Transaction: BaseEntity
    {
        public DateTime TransactionDate { get; set; }
        public int ConfirmationNumber { get; set; }
        public long Voucher { get; set; }
        public string? Description { get; set; }
        public TransactionType TransactionType { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public decimal Amount { get; set; }
        public int Commission { get; set; }
        public double Tax { get; set; }
        public double Total { get; set; }
    }
}