using ControleFinanceiro.Repositories;
using ControleFinanceiro.Views;
using LiteDB;
using Microsoft.Extensions.Logging;

namespace ControleFinanceiro
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .RegisterDataBaseAndRepositories()
                .RegisterViews();
                

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        //conteiner de injeção de dependencia
        public static MauiAppBuilder RegisterDataBaseAndRepositories(this MauiAppBuilder mauiAppBuilder)
        {
            //uma unica instancia
            mauiAppBuilder.Services.AddSingleton<LiteDatabase>(
                options =>
                {   
                    return new LiteDatabase($"Filename={AppSettings.DataBasePath};Connection=Shared");
                }
            );
            //uma instancia para cada vez que precisar
            //Qualquer classe que atende a minha interface serve, isso faz com que eu desacople a minha classe abstrata
            mauiAppBuilder.Services.AddTransient<ITransactionRepository, TransactionRepository>();
            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
        {
            //uma instancia para cada vez que precisar
            //Qualquer classe que atende a minha interface serve, isso faz com que eu desacople a minha classe abstrata
            mauiAppBuilder.Services.AddTransient<TransactionAdd>();
            mauiAppBuilder.Services.AddTransient<TransactionEdit>();
            mauiAppBuilder.Services.AddTransient<TransactionList>();

            return mauiAppBuilder;
        }
    }
}
