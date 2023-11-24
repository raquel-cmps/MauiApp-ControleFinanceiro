using CommunityToolkit.Mvvm.Messaging;
using ControleFinanceiro.Models;
using ControleFinanceiro.Repositories;


namespace ControleFinanceiro.Views;

public partial class TransactionList : ContentPage
{
    /* DESACOPLAMENTO, TORNA O CODIGO MAIS LIMPO
		 * Publisher - subscrivers:
		 * TransactionAdd -> publisher(publicador) > cadastro
		 * TransactionList -> subscrivers(objetos interresados em saber qunado um objeto é publicado, cadastrado)
		 * Mando uma mensagem no cadastro com o novo obj add e a listagem vai receber o obj que acabou de ser cadastrado
		 * TROCAM MSG MAS ELES NAO SE CONHECEM
		 */

    private ITransactionRepository _repository;
    public TransactionList(ITransactionRepository repository)
	{
		_repository = repository;
		
		InitializeComponent();

		Reaload();

		//so vai rodar qunado tiver uma mensagem, quando for feito um novo cadastro 
        WeakReferenceMessenger.Default.Register<string>(this, (e, msg) => //posso passar a Transaction, que dai a minha lista nem precisa buscar no banco
		{
			Reaload();
        });

	}
	private void Reaload()
	{
		var items = _repository.GetAll();
        CollectionViewTransactions.ItemsSource = items;

		double income = items.Where(a => a.TransactionType == Models.TransactionType.Income).Sum(a => a.Value);
		double expense = items.Where(a => a.TransactionType == Models.TransactionType.Expense).Sum(a => a.Value);
		double balance = income - expense;

		LabelIncome.Text = income.ToString("C");
		LabelExpense.Text = expense.ToString("C");
		LabelBalance.Text = balance.ToString("C");

    }

    //sender = o elemento que chamou o método
    private void OnButtonClicked_To_TransactionAdd(object sender, EventArgs args)
	{
		var transactionAdd = Handler.MauiContext.Services.GetService<TransactionAdd>(); //toda vez que a tela for aberta irá gerar uma nova instancia
        Navigation.PushModalAsync(transactionAdd);
	}
	//push para abrir uma tela

    private void TapGestureRecognizerTapped_To_TransactionEdit(object sender, TappedEventArgs e)
    {
		var grid = (Grid)sender;
		var gesture = (TapGestureRecognizer) grid.GestureRecognizers[0];


		Transaction transaction = (Transaction)gesture.CommandParameter;


        var transactionEdit = Handler.MauiContext.Services.GetService<TransactionEdit>();
		transactionEdit.SetTransactionToEdit(transaction);
        Navigation.PushModalAsync(transactionEdit);
    }

    private async void TapGestureRecognizerTapped_To_Delete(object sender, TappedEventArgs e)
    {
		await AnimationBorder((Border)sender, true);
		bool result = await App.Current.MainPage.DisplayAlert("Excluir", "Tem certeza que deseja excluir?", "Sim", "Não");

		if (result)
		{
			Transaction transaction = (Transaction)e.Parameter; //equivalente ao command parameter
			_repository.Delete(transaction);

			Reaload();
		}
		else
		{
			await AnimationBorder((Border)sender, false);
		}
    }
	private Color _borderOriginalBackgroundColor;
	private string _labelOriginalText;
    
    private async Task AnimationBorder(Border border, bool IsDeleteAnimation)
	{
        var label = (Label)border.Content;

        if (IsDeleteAnimation)
		{
            _borderOriginalBackgroundColor = border.BackgroundColor;
			_labelOriginalText = label.Text;
            await border.RotateYTo(90, 250);

			border.BackgroundColor = Colors.DarkRed;
			label.TextColor = Colors.WhiteSmoke;
			label.Text = "X";

            await border.RotateYTo(180, 250);

        }
        else
		{
			await border.RotateYTo(90, 250);

			border.BackgroundColor = _borderOriginalBackgroundColor;
			label.TextColor = Colors.Black;
			label.Text = _labelOriginalText;

            await border.RotateYTo(0, 250);

        }
    }

}