using System;
using System.Collections.Generic;
using System.Text;

namespace MiniATM.UseCase
{
    public interface ICashWithdrawalManager
    {//method trù tượng của inter face đẻ  kiêm tra rut tiền
        Task<TransactionResult> WithDrawAsync(string accountId, double amount);
    }
}
