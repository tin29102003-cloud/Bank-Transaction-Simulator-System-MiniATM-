using MiniATM.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MiniATM.Infracstructure.SqlServer.Repositories.SqlServer.DataContext
{
    public class Transaction
    {
        public required Guid Id { get; set; }
        public required TransactionTypes transactionTypes { get; set; }
        [MaxLength(50)]
        public required string AccountId { get; set; }
        public required double Amount { get; set; }
        public DateTime DateTimeUTC { get; set; }
        [MaxLength(200)]
        public required string Notes { get; set; }
    }
}
