using ControleFinanceiro.Views;

namespace ControleFinanceiro
{
    public partial class App : Application
    {
        public App(TransactionList listPage)
        {
            InitializeComponent();
            //criando navegação
            MainPage = new NavigationPage(listPage);
        }
    }
}
