namespace ATMMini.infrastructure.Models
{
    public class TransferModel
    {
        public required string FromBankAccount { get; set; }
        public required string ToBankAccount { get; set; }
        public double Amount { get; set; }
    }
}
