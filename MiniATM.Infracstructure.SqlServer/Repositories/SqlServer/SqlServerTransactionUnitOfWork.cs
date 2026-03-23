using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using MiniATM.Infracstructure.SqlServer.Repositories.SqlServer.DataContext;
using MiniATM.UseCase.Repositories;
using MiniATM.UseCase.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniATM.Infracstructure.SqlServer.Repositories.SqlServer
{
    //gôm các thao taccs lại commit 1 lần
    public class SqlServerTransactionUnitOfWork : ITransactionUnitOfWork
    {
        private readonly IMapper mapper;
        private readonly MiniATMContext context;
        //private IDbContextTransaction transaction;
        public ITransactionRepository TracsactionRepository { get; }
        public IBankAccountRepository BankAccountRepository { get; }

        

        public SqlServerTransactionUnitOfWork(MiniATMContext context, IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            TracsactionRepository = new SqlServerTransactionRepository(mapper, context);
            BankAccountRepository = new SqlServerBankAccountrepository(context, mapper);

        }

        public  Task BegintransactionAsync()
        {
            //// Chúng ta có thể sử dụng context.Database.BeginTransaction(), nhưng vì đây là một UoW, chúng ta sẽ âm thầm loại bỏ các thay đổi nếu SaveChangesAsync không được gọi.
            return Task.CompletedTask;
            //await context.Database.BeginTransactionAsync();
        }

        public   Task CancelAsync()
        {
            return Task.CompletedTask;
            //await transaction.RollbackAsync();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
