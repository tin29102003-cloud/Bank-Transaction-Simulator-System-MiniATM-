using System;
using System.Collections.Generic;
using System.Text;

namespace MiniATM.UseCase
{
    public interface ICashStorage
    {//cái này là xem máy atm còn tiền ko với có rút thành dcoong hay ko
        bool IsCashAmountAvailable(double amount);
        bool Withdraw(double amount);
    }
}
