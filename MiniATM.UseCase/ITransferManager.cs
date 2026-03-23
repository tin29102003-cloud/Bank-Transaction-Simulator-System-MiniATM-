using System;
using System.Collections.Generic;
using System.Text;

namespace MiniATM.UseCase
{
    public interface ITransferManager//quản lý qua trình chuyển tièn
    {
        Task<TransactionResult> TransferAsync(string fromAccountId, string toAccountId, double amount);
    }
}
