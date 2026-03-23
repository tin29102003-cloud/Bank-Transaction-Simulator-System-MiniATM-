using MiniATM.UseCase;

namespace ATMMini.infrastructure.Models
{
    public class TransferResultModel : TransferModel
    {
        public required TransactionResultCodes ResultCode { get; set; }
        public required string Message { get; set; }
    }
}
