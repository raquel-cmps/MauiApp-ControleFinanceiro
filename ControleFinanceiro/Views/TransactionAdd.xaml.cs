using CommunityToolkit.Mvvm.Messaging;
using ControleFinanceiro.Models;
using ControleFinanceiro.Repositories;
using System.Diagnostics.SymbolStore;

namespace ControleFinanceiro.Views;

public partial class TransactionAdd : ContentPage
{
    private ITransactionRepository _transactionRepository;
	public TransactionAdd(ITransactionRepository repository)
	{
        _transactionRepository = repository;
		InitializeComponent();
	}

    private void TapGestureRecognizerTapped_ToClose(object sender, TappedEventArgs e)
    {	//pop - fecha a tela
		Navigation.PopModalAsync();
    }

    private void OnButtonClicked_Save(object sender, EventArgs e)
    {
        //validar os dados (pegar as informa��es)
        if (IsValidData() == false)
            return;//para a execu��o do m�todo

        SaveTransactionInDataBase();

        Navigation.PopModalAsync();
        WeakReferenceMessenger.Default.Send<string>(string.Empty);

        var count = _transactionRepository.GetAll().Count;
        App.Current.MainPage.DisplayAlert("Mensagem", $"Existem {count} registro(s) no banco", "ok");
    }

    private void SaveTransactionInDataBase()
    {
        //criando objeto
        Transaction transaction = new Transaction()
        {
            //se tiver marcado como verdadeiro eu pego o income senao eu o expenses
            TransactionType = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expense,
            Name = EntryName.Text,
            Date = DatePickDate.Date,
            Value = double.Parse(EntryValue.Text)
        };

        //enviar no banco
        _transactionRepository.Add(transaction);

        /*obter dados da maneira facil
        var repository = this.Handler.MauiContext.Services.GetService<ITransactionRepository>();
        repository.Add(transaction);
        */
    }

    private bool IsValidData()
    {
        bool isValid = true;
        //StringBuilder sb = new StringBuilder();

        if (string.IsNullOrEmpty(EntryName.Text) || string.IsNullOrWhiteSpace(EntryName.Text)) //se for vazio ou nulo ou espa�o em branco
        {
            //usuario nao digitou nenhuma informa��o
            //sb.Append("O campo 'Nome' deve ser preenchido!");
            isValid = false;
        }
        if (string.IsNullOrEmpty(EntryValue.Text) || string.IsNullOrWhiteSpace(EntryValue.Text)) //se for vazio ou nulo ou espa�o em branco
        {
            //usuario nao digitou nenhuma informa��o

            //sb.Append("O campo 'Valor' deve ser preenchido!");
            isValid = false;
        }
        double result;                                  //caso nao aconte�a a convers�o
        if (!string.IsNullOrEmpty(EntryValue.Text) && !double.TryParse(EntryValue.Text, out result)) //vendo se da para converter em double e ja da uma saida(out)
        {
            //sb.Append("O campo 'Valor' � inv�lido");
            isValid = false;
        }

        if (isValid == false)
        {
            LabelError.IsVisible = true;
            LabelError.Text = "ERRO!";
        }
        return isValid;
    }
}