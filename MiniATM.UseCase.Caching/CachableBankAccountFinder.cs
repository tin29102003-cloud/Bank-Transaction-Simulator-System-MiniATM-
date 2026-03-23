using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MiniATM.Entities;
using MiniATM.UseCase.Exceptions;
using System.Text.Json;

namespace MiniATM.UseCase.Caching
{//đay là 1 decorator cclass 
    public class CachableBankAccountFinder : IBankAccountFinder // a cachable finder using 'cache aside' strategy
    {
        private static readonly JsonSerializerOptions serializerOptions = new()
        {

        };

        private readonly CachableBankAccountFinderOptions options;
        private readonly IBankAccountFinder parentFinder;
        private readonly IDistributedCache cache;
        private readonly ILogger<CachableBankAccountFinder> logger;
        private readonly DistributedCacheEntryOptions cacheEntryOptions;
        //nhan vao 1 ddoi tuong cha co kieeu  bankacount fider va cos the truyen vao idsitruccache laf sql redis  va  memory
        public CachableBankAccountFinder(IBankAccountFinder parentFinder, IDistributedCache cache, CachableBankAccountFinderOptions options, ILogger<CachableBankAccountFinder> logger)
        {
            this.parentFinder = parentFinder ?? throw new ArgumentNullException(nameof(parentFinder));

            this.options = options;
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
            this.logger = logger;
            //pptions.AbsoluteExpiration = new DateTimeOffset(2026, 2, 22, 23, 0, 0, TimeSpan.Zero); tạo đc thời diêm có đinh chết
            //nó cung 3 tham số truyên vào tham số  thứ  1  hệt  hạn vào 1 thời diểm cosos định  kể từ lúc set nếu dung liên tục nhưng hết hạn vẫn chết binh thuong
            //tham số thứ 2 hết hạn sau một khoản thời gian tính từ bây giờ set time tính tư lúc tạo
            //tham sooss thứ 3 hết hạn khong bị  truy cập trong thời gian đó thằng này ko đc lớn hơn tham số của thằn 1
            cacheEntryOptions = new DistributedCacheEntryOptions()//có thuôc tinh dat thoi gian hêt hạn
            {
                AbsoluteExpirationRelativeToNow = options.CacheTimeSpan//tính từ thời diê hiện tai
            };
        }
        //thang nay  no se  mo rong tinh nang cuar thang cha tu bo nho cache ra
        public async Task<IEnumerable<BankAccount>> FindByCustomerIdAsync(Guid customerId)
        {
            //dung len 1 key
            var cacheKey = $"{options.CacheKey}#{customerId}";
            //timf xem trong object naof cos chuaw key
            var cachedData = await cache.GetStringAsync(cacheKey);
            //ko cos
            if (cachedData == null)
            {
                // cache missed!
                logger.LogInformation("Loading accounts from parent...");
                //ko cos thi ta lay thong tin tu object cha
                var accounts = await parentFinder.FindByCustomerIdAsync(customerId) ?? throw new AccountListNullException();
                //chuyen nó thành bộ nhớ json 
                cachedData = JsonSerializer.Serialize(accounts, serializerOptions);//chuyeenr  objec chuoi json

                logger.LogInformation("Storing {data} to cache...", cachedData);
                //lưu vô  bộ nhớ cache
                await cache.SetStringAsync(cacheKey, cachedData, cacheEntryOptions);

                return accounts;
            }
            else
            {
                // cache hit!
                var accounts = JsonSerializer.Deserialize<IEnumerable<BankAccount>>(cachedData) ?? [];//chuyeenr json thanh object bank acount
                return accounts;
            }
        }
    }
}
