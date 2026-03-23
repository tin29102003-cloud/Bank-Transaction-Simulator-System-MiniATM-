namespace MiniATM.UseCase
{
    public class TransactionResult
    {
        public TransactionResult(TransactionResultCodes resultCode, string message)
        {
            ResultCodes = resultCode; 
            Message = message ?? string.Empty;
        }

        public TransactionResultCodes ResultCodes { get; }
        public string Message { get; }
        public static readonly TransactionResult SourceNotFound = new(TransactionResultCodes.SourceNotFound, "Tai khoan nguon tien ko tôn tai");
        public static readonly TransactionResult DestinationNotFound = new(TransactionResultCodes.DestinationNotFound, "Tai khoan can chuyen tien khong ton tai");
        public static readonly TransactionResult BalanceTooLow = new(TransactionResultCodes.BalanceTooLow, "so du qua thap");
        public static readonly TransactionResult CashNotAvailable = new(TransactionResultCodes.CashNotAvailable, "may ATM khong co du tien de rut");
        public static readonly TransactionResult CashWithdrawalError = new(TransactionResultCodes.CashWithdrawalError, "Rut tien gap qua trinh loi lien he ngan hien gan nhat  de lam viec");
        public static readonly TransactionResult Success = new(TransactionResultCodes.Success, "Rut tien thanh công");

    }
}