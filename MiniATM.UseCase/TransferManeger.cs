using MiniATM.Entities;
using MiniATM.UseCase.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;


namespace MiniATM.UseCase
{///dây là lớp sử lý chuyển tiền từ tk này sang tk khác nó làm việc với entity và các interface
    public class TransferManeger(ITransactionUnitOfWork tracsactionUnitOfWork
        ) : ITransferManager
    {
        private readonly ITransactionUnitOfWork transactionUnitOfWork = tracsactionUnitOfWork ?? throw new ArgumentNullException(nameof(tracsactionUnitOfWork));
        public async Task<TransactionResult> TransferAsync(string fromAccountId, string toAccountId, double amount)
        {
            try
            {
                await transactionUnitOfWork.BegintransactionAsync();
                var fromAccount = await transactionUnitOfWork.BankAccountRepository.FindByIdAsync(fromAccountId);
                if(fromAccount == null || fromAccount.IsLocked)
                {
                    return TransactionResult.SourceNotFound;
                }
                var balanceLeft = fromAccount.Balance - amount;
                if(balanceLeft < fromAccount.MinimumRequireAmount)
                {
                    return TransactionResult.BalanceTooLow;
                }
                var toAccount = await transactionUnitOfWork.BankAccountRepository.FindByIdAsync(toAccountId);
                if(toAccount == null || toAccount.IsLocked)
                {
                    return TransactionResult.DestinationNotFound;
                }
                fromAccount.Balance -= amount;//sau đó trừ tiền tk sau đó update để lưu lại db
                await transactionUnitOfWork.BankAccountRepository.UpdateAsync(fromAccount);
                var now = DateTime.UtcNow;
                //sau đó lưu lại vào lịch sử giao dịch
                await transactionUnitOfWork.TracsactionRepository.Add(new Transaction()
                {
                    Id = Guid.NewGuid(),
                    Amount = amount,
                    AccountId = fromAccount.Id,
                    DateTimeUTC = now,
                    TransactionTypes = TransactionTypes.Withdrawal,
                    Notes = $"tai  khoan {fromAccount.Id} chuyen tien toi {toAccount.Id}"
                });
                toAccount.Balance += amount;
                await transactionUnitOfWork.BankAccountRepository.UpdateAsync(toAccount);
                await transactionUnitOfWork.TracsactionRepository.Add(new Transaction()
                {
                    Id = Guid.NewGuid(),
                    Amount = amount,
                    AccountId = toAccount.Id,
                    DateTimeUTC = now,
                    TransactionTypes = TransactionTypes.Withdrawal,
                    Notes = $"tai  khoan {toAccount.Id} nhan tien tu tai khoan {fromAccount.Id}"
                });
                await transactionUnitOfWork.SaveChangesAsync();//luu nhung thay dổi lai
                return TransactionResult.Success;
            }catch(Exception ex)
            {
                return new TransactionResult(TransactionResultCodes.Error, ex.Message);
            }
        }
    }
}
