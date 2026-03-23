using MiniATM.Entities.Exceptions;

namespace MiniATM.Entities
{
    public class BankAccount
    {
        private double balance;
        public required string Id { get; set; }
        public Guid CustomerId { get; set; }
        public double Balance
        {
            get
            {
                return balance;
            }
            set
            {
                if (value < MinimumRequireAmount) throw new InvalidBalanceException();
                balance = value;//neeus so du nho hon so du toi thieu thì tra ra exeption
            }
        }
        public required string Currency { get; set; }
        public bool IsLocked { get; set; }
        public double MinimumRequireAmount { get; set; }//gioi han so tien con trong tk
    }
}
