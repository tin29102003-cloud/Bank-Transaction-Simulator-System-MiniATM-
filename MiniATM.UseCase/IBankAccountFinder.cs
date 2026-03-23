using MiniATM.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniATM.UseCase
{
    public interface IBankAccountFinder
    {//tìm tk bank theo customId
        Task<IEnumerable<BankAccount>> FindByCustomerIdAsync(Guid customerId);//task là kiểu dữ liệu dung cho bất đồng bộ nếu muôn lấy kết quả bặt buộc phải có await
     }
}
