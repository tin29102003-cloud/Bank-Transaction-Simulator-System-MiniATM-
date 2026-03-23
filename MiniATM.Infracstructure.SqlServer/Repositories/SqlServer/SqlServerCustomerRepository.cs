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
    public class SqlServerCustomerRepository : ICustomerRepository
    {
        private readonly IMapper mapper;
        private readonly MiniATMContext context;
        public SqlServerCustomerRepository(MiniATMContext context, IMapper mapper) 
        { 
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Entities.Customer?> FindByIdAsync(Guid id)
        {
            var dbCustomer = await context.Customers.Where(ba => ba.Id == id).FirstOrDefaultAsync();
            return   mapper.Map<Entities.Customer>(dbCustomer);
        }
    }
}
