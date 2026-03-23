using MiniATM.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniATM.UseCase.Repositories
{
    public interface ITransactionRepository
    {
        Task Add(Transaction transaction);
    }
}
