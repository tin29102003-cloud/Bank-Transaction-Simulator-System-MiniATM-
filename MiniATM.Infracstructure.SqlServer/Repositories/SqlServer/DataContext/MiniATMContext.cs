using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace MiniATM.Infracstructure.SqlServer.Repositories.SqlServer.DataContext
{
    public class MiniATMContext: DbContext//db context  là tria tim của EF core có nhiêm vụ kết nối db thwo doi entity chuyên linq sang sql
        // thuc hiện save change
    {
        private readonly string connectionString;

        //contructor ko có tham số
        public MiniATMContext()//nếu ko có gì truyền vao mặc định dùng chui này
        {
            connectionString = @"Server=.;Database=MiniATM;Trusted_Connection=True;TrustServerCertificate=True;";
        }
        //contructor có tham số khi cần truyền connnection string vào
        public MiniATMContext(string connectionString)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }
        //db set là dại dienj cho 1 bảng trong db

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Transaction> transactions { get; set; }
        public DbSet<Customer> Customers { get; set;  }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);//cacis này là dung sqlserver và  connectring
        }
    }
}
