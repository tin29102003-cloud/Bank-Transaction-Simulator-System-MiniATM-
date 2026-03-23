using MiniATM.UseCase.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniATM.UseCase
{
    public class CashWithdrawalManager(ITransactionUnitOfWork transactionUnitOfWork, ICashStorage cashStorage, bool useSafeCaseWithdrawal = true) : ICashWithdrawalManager
    {
        private readonly ICashStorage cashStorage = cashStorage ?? throw new ArgumentNullException(nameof(cashStorage));
        private readonly ITransactionUnitOfWork transactionUnitOfWork = transactionUnitOfWork ?? throw new ArgumentNullException(nameof(transactionUnitOfWork));
        public async Task<TransactionResult> WithDrawAsync(string accountId, double amount)
        {
            try
            {
                await transactionUnitOfWork.BegintransactionAsync();
                var fromAccount = await transactionUnitOfWork.BankAccountRepository.FindByIdAsync(accountId);
                if(fromAccount == null  || fromAccount.IsLocked)
                {
                    return TransactionResult.SourceNotFound;
                }
                var balanceLeft = fromAccount.Balance - amount;
                if(balanceLeft < fromAccount.MinimumRequireAmount)
                {
                    return TransactionResult.BalanceTooLow;
                }
                if (!cashStorage.IsCashAmountAvailable(amount))//nếu iscasshamou là false thì tien déo còn để rút 
                {
                    return TransactionResult.CashNotAvailable;
                }
                if (useSafeCaseWithdrawal) {
                    fromAccount.Balance -= amount;
                    await transactionUnitOfWork.BankAccountRepository.UpdateAsync(fromAccount);
                    await transactionUnitOfWork.SaveChangesAsync();
                    if (!cashStorage.Withdraw(amount))//nếu withdrawn thất bại thì vẫn trừ tiền khách nhưng ngân hàng sẽ ko mất tiền
                    {
                        return TransactionResult.CashWithdrawalError;
                    }
                }
                else
                {
                    fromAccount.Balance -= amount;
                    await transactionUnitOfWork.BankAccountRepository.UpdateAsync(fromAccount);
                    if (cashStorage.Withdraw(amount))//nếu rút tiền đ thì save lại những thay đoio
                    {
                        await transactionUnitOfWork.SaveChangesAsync();//nếu th hàm này lỗi thi ngân  hàng  mất tiền do cây atm đã nhả tiền
                    }else
                    {
                        await transactionUnitOfWork.CancelAsync();//còn ko thì hủy và ko rút dc tiền
                    }
                }
                return TransactionResult.Success;
            }catch (Exception ex)
            {
                return new TransactionResult(TransactionResultCodes.Error, ex.Message);
            }
        }
    }
}
