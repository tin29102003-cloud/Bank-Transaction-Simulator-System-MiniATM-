using AutoMapper;
using MiniATM.Infracstructure.SqlServer.Repositories.SqlServer.DataContext;
using MiniATM.UseCase.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniATM.Infracstructure.SqlServer.Repositories.SqlServer
{
    public class SqlServerTransactionRepository: ITransactionRepository
    {
        private readonly IMapper mapper;
        private readonly MiniATMContext context;
        public SqlServerTransactionRepository(IMapper mapper, MiniATMContext context)
        {
            this.mapper = mapper ?? throw  new ArgumentNullException(nameof(mapper));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Add(Entities.Transaction transaction)
        {
            var dbtransaction = mapper.Map<DataContext.Transaction>(transaction);//nó sẽ copy entity thành datacontext
            context.Add(dbtransaction);//đánh dáu entity là add

            await context.SaveChangesAsync();//và cuois cùng là lưu
        }
    }
}
