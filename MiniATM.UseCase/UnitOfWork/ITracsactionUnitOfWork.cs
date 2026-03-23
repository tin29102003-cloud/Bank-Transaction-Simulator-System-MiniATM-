using MiniATM.UseCase.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniATM.UseCase.UnitOfWork
{
    public interface ITransactionUnitOfWork
    {
        ITransactionRepository TracsactionRepository { get; }
        IBankAccountRepository BankAccountRepository { get; }
        Task BegintransactionAsync();
        Task SaveChangesAsync();
        Task CancelAsync();//this method should be called ASAP before leaving,
    }
}
