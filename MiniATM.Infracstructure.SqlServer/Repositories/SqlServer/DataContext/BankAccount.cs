using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Transactions;

namespace MiniATM.Infracstructure.SqlServer.Repositories.SqlServer.DataContext
{
    public class BankAccount
    {
        [MaxLength(50)]
        public required string Id { get; set; }
        public required Guid CustomerId { get; set; }
        public double Balance { get; set; } = 0;
        [MaxLength(3)]
        public required string Currency { get; set; }
        public bool IsLocked { get; set; }
        public double MinimumRequiredAmount { get; set; }
        public ICollection<Transaction> transactions { get; } = [];
    }
}
