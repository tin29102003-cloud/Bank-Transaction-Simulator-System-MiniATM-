using MiniATM.UseCase;

namespace ATMMini.infrastructure.Models
{
    public class WithdrawResultModel : WithdrawModel
    {
        public required TransactionResultCodes ResultCode { get; set; }
        public required string Message { get; set; }
    }
}
