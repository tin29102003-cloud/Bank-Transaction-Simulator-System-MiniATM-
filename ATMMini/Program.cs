using ATMMini.infrastructure.CashStorage;
using ATMMini.infrastructure.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MiniATM.Infracstructure.SqlServer.Repositories.SqlServer;
using MiniATM.Infracstructure.SqlServer.Repositories.SqlServer.DataContext;
using MiniATM.Infracstructure.SqlServer.Repositories.SqlServer.MapperProfile;
using MiniATM.Infrastructure.InMemory;
using MiniATM.UseCase;
using MiniATM.UseCase.Caching;
using MiniATM.UseCase.Repositories;
using MiniATM.UseCase.UnitOfWork;
namespace ATMMini.infrastructure;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        RegisterInfrastructureServices(builder.Configuration, builder.Services);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }

    private static void RegisterInfrastructureServices(ConfigurationManager configuration, IServiceCollection services)
    {
        var repositoryOptions = configuration.GetSection("Repository").Get<RepositoryOptions>() ?? throw new Exception("No RepositoryOptions found");
        //mình sẽ đẵng ký các dịch vụ cho no
        //phaan nay la noi imlenment cac class ddc su dung trong use case vaf  sql
        if (repositoryOptions.RepositoryType == RepositoryTypes.SqlServer)///nay lấy các option ra nếu các opton có sqlserver thì chạy cái này
        {//dang ký các dịch vụ cho sql
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<SqlServer2EntityProfile>();
            });
            services.AddDbContext<MiniATMContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("MiniATMDatabase")));

            services.AddTransient<IBankAccountRepository>(services => new SqlServerBankAccountrepository(
                services.GetRequiredService<MiniATMContext>(), services.GetRequiredService<IMapper>()
                ));
            services.AddTransient<ICustomerRepository>(services => new SqlServerCustomerRepository(
                services.GetRequiredService<MiniATMContext>(), services.GetRequiredService<IMapper>()
                ));
            services.AddTransient<ITransactionRepository>(services => new SqlServerTransactionRepository(
                services.GetRequiredService<IMapper>(), services.GetRequiredService<MiniATMContext>()
                ));
            services.AddTransient<ITransactionUnitOfWork>(services => new SqlServerTransactionUnitOfWork(
                services.GetRequiredService<MiniATMContext>(), services.GetRequiredService<IMapper>()
                ));
        }
        else
        {
            services.AddTransient<IBankAccountRepository>(services => new InMemoryBankAccountRepository());
            services.AddTransient<ICustomerRepository>(services => new InMemoryCustomerRepository());
            services.AddTransient<ITransactionRepository>(services => new InMemoryTransactionRepository());
            services.AddTransient<ITransactionUnitOfWork>(services => new InMemoryTransactionUnitOfWork());
        }
        //se lay thong tin tu bo nho tu file appsetting
        var cacheOptions = configuration.GetSection("Cache").Get<CacheOptions>() ?? new CacheOptions();
        InitializeCache(services, configuration, cacheOptions);

        services.AddSingleton<ICashStorage>(services => new InMemoryCashStorage(
            services.GetRequiredService<ILogger<InMemoryCashStorage>>(),
            10000
            )); // must be singleton//đay là lớp đe giới hạn số tiền có trong cây atm

        services.AddTransient<IBankAccountFinder>(services => new CachableBankAccountFinder(new RepositoryBankAccountFinder(
            services.GetRequiredService<IBankAccountRepository>()
            ),
            services.GetRequiredService<IDistributedCache>(),
            configuration.GetSection("CachableBankAccountFinderOptions").Get<CachableBankAccountFinderOptions>() ?? new(),
            services.GetRequiredService<ILogger<CachableBankAccountFinder>>()
            ));

        services.AddTransient<ICashWithdrawalManager>(services => new CashWithdrawalManager(
            services.GetRequiredService<ITransactionUnitOfWork>(),
            services.GetRequiredService<ICashStorage>(),
            true
            ));

        services.AddTransient<ITransferManager>(services => new TransferManeger(
            services.GetRequiredService<ITransactionUnitOfWork>()
            ));
    }

    private static void InitializeCache(IServiceCollection services, ConfigurationManager configuration, CacheOptions cacheOptions)
    {
        switch (cacheOptions.Type)
        {
            case CacheTypes.Memory:
                services.AddDistributedMemoryCache();
                break;
            case CacheTypes.SqlServer:
                /*
                 * Run this SQL to create cache table: dotnet sql-cache create <connection string> dbo <cache table>
                 */

                if (cacheOptions.SqlServerOptions == null)
                {
                    throw new Exception("Missing option: CachingOptions:SqlServer");
                }
                services.AddDistributedSqlServerCache(options => {
                    options.ConnectionString = configuration.GetConnectionString(cacheOptions.SqlServerOptions.ConnectionStringName);
                    options.TableName = cacheOptions.SqlServerOptions.TableName;
                    options.SchemaName = cacheOptions.SqlServerOptions.SchemaName;
                });
                break;
            case CacheTypes.Redis:
                if (cacheOptions.RedisOptions == null)
                {
                    throw new Exception("Missing option: CachingOptions:Redis");
                }
                services.AddStackExchangeRedisCache(options => {
                    options.Configuration = configuration.GetConnectionString(cacheOptions.RedisOptions.ConnectionStringName);
                });
                break;
            default:
                throw new Exception("Unknown cache type");
        }
    }
}
