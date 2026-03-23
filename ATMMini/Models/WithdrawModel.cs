namespace ATMMini.infrastructure.Models
{
    public class WithdrawModel
    {
        public required string FromBankAccount { get; set; }
        public double Amount { get; set; }
    }
}
