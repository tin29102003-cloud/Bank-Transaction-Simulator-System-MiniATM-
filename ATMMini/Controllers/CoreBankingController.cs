using ATMMini.infrastructure;
using ATMMini.infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

using MiniATM.UseCase;

namespace MiniATM.Infrastructure.Controllers
{
    public class CoreBankingController : Controller
    {
        //cac thuoc tinh can dung da dang ky truoc do
        public readonly IBankAccountFinder accountFinder;
        private readonly ITransferManager transferManager;
        private readonly ICashWithdrawalManager cashWithdrawalManager;

        public CoreBankingController(IBankAccountFinder accountFinder, ITransferManager transferManager, ICashWithdrawalManager cashWithdrawalManager)
        {
            this.accountFinder = accountFinder;
            this.transferManager = transferManager;
            this.cashWithdrawalManager = cashWithdrawalManager;
        }

        private static Guid GetCustomerId()
        {//cai custom id này thầy tạo ra ngẫu nhiên gioongg nhu trong db nó phải giống với  id trong db
            return DemoConstants.CustomerId;
        }

        public async Task<IActionResult> ChooseBankAccountAsync(string returnUrl)
        {
            var customerId = GetCustomerId();
            var accounts = await accountFinder.FindByCustomerIdAsync(customerId) ?? [];

            return View(new ChooseAccountModel() { BankAccounts = accounts, ReturnUrl = returnUrl });
        }

        #region Transfer
        [HttpGet]
        public IActionResult Transfer(string bankAccount)
        {
            return View(new TransferModel()
            {
                FromBankAccount = bankAccount,
                ToBankAccount = string.Empty,
                Amount = 0,
            });
        }

        [HttpPost]
        public async Task<IActionResult> TransferAsync([FromForm] TransferModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var result = await transferManager.TransferAsync(model.FromBankAccount, model.ToBankAccount, model.Amount);
            //sau khi goi toiws ham tranfer no se tra veef them thuoc tinh result vaf messege
            return View("TransferResult", new TransferResultModel()
            {
                FromBankAccount = model.FromBankAccount,
                ToBankAccount = model.ToBankAccount,
                Amount = model.Amount,
                ResultCode = result.ResultCodes,
                Message = result.Message
            });
        }
        #endregion

        #region Withdrawal
        [HttpGet]
        public IActionResult WithDraw(string bankAccount)
        {
            return View(new WithdrawModel()
            {
                FromBankAccount = bankAccount,
                Amount = 0,
            });
        }
        public async Task<IActionResult> WithdrawAsync([FromForm] WithdrawModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var result = await cashWithdrawalManager.WithDrawAsync(model.FromBankAccount, model.Amount);

            return View("WithdrawResult", new WithdrawResultModel()
            {
                FromBankAccount = model.FromBankAccount,
                Amount = model.Amount,
                ResultCode = result.ResultCodes,
                Message = result.Message
            });
        }
        #endregion
    }
}
