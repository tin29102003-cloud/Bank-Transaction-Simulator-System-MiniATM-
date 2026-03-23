using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MiniATM.Entities;
using MiniATM.Infracstructure.SqlServer.Repositories.SqlServer.DataContext;
using MiniATM.UseCase.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniATM.Infracstructure.SqlServer.Repositories.SqlServer
{
    public class SqlServerBankAccountrepository : IBankAccountRepository
    {
        private readonly MiniATMContext context;
        private readonly IMapper mapper;

        public SqlServerBankAccountrepository(MiniATMContext context, IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Entities.BankAccount>> FindByCustomerIdAsync(Guid customerId)
        {
            var dbaccounts = await context.BankAccounts.Where(ba => ba.CustomerId == customerId).ToListAsync();
            return mapper.Map<IEnumerable<Entities.BankAccount>>(dbaccounts);
        }

        public async Task<Entities.BankAccount?> FindByIdAsync(string accountId)
        {
            var dbaccount = await context.BankAccounts.Where(ba => ba.Id == accountId).FirstOrDefaultAsync();
            return mapper.Map<Entities.BankAccount>(dbaccount);
        }

        public async Task UpdateAsync(Entities.BankAccount account)
        {
            var dbaccount = await context.BankAccounts.Where(ba => ba.Id == account.Id).FirstOrDefaultAsync();
            if(dbaccount != null)
            {
                mapper.Map(account, dbaccount);
            }
            await context.SaveChangesAsync();
        }
    }
}
