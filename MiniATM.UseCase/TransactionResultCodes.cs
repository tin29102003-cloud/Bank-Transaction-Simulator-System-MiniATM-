namespace MiniATM.UseCase
{
    public enum TransactionResultCodes
    {
        Success,
        SourceNotFound,
        DestinationNotFound,
        BalanceTooLow,
        CashNotAvailable,//không dủ cash trong atm
        CashWithdrawalError,//số du thay doior nhung user ko nhận đc tiền
        Error
    }
}