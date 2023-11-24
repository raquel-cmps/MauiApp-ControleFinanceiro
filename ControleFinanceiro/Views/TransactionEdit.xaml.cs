using CommunityToolkit.Mvvm.Messaging;
using ControleFinanceiro.Models;
using ControleFinanceiro.Repositories;

namespace ControleFinanceiro.Views;

public partial class TransactionEdit : ContentPage
{
    private Transaction _transaction;
    private ITransactionRepository _transactionRepository;

    public TransactionEdit(ITransactionRepository repository)
	{
		InitializeComponent();
        _transactionRepository = repository;
    }
    public void SetTransactionToEdit(Transaction transaction)
    {
        _transaction = transaction;
        
        if (transaction.TransactionType == TransactionType.Income)
            RadioIncome.IsChecked = true;
        else
            RadioExpense.IsChecked = true;

        EntryName.Text = transaction.Name;
        DatePickDate.Date = transaction.Date.Date;
        EntryValue.Text = transaction.Value.ToString();
        
    }
    private void TapGestureRecognizerTapped_ToClose(object sender, TappedEventArgs e)
    {   //pop - fecha a tela
        Navigation.PopModalAsync();
    }
    private void OnButtonClicked_Save(object sender, EventArgs e)
    {
        //validar os dados (pegar as informações)
        if (IsValidData() == false)
            return;//para a execução do método

        SaveTransactionInDataBase();

        Navigation.PopModalAsync();
        WeakReferenceMessenger.Default.Send<string>(string.Empty);
    }

    private void SaveTransactionInDataBase()
    {
        //criando objeto
        Transaction transaction = new Transaction()
        {   
            Id = _transaction.Id,
            //se tiver marcado como verdadeiro eu pego o income senao eu o expenses
            TransactionType = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expense,
            Name = EntryName.Text,
            Date = DatePickDate.Date,
            Value = double.Parse(EntryValue.Text)
        };

        //enviar no banco
        _transactionRepository.Update(transaction);

        /*obter dados da maneira facil
        var repository = this.Handler.MauiContext.Services.GetService<ITransactionRepository>();
        repository.Add(transaction);
        */
    }

    private bool IsValidData()
    {
        bool isValid = true;
        //StringBuilder sb = new StringBuilder();

        if (string.IsNullOrEmpty(EntryName.Text) || string.IsNullOrWhiteSpace(EntryName.Text)) //se for vazio ou nulo ou espaço em branco
        {
            //usuario nao digitou nenhuma informação
            //sb.Append("O campo 'Nome' deve ser preenchido!");
            isValid = false;
        }
        if (string.IsNullOrEmpty(EntryValue.Text) || string.IsNullOrWhiteSpace(EntryValue.Text)) //se for vazio ou nulo ou espaço em branco
        {
            //usuario nao digitou nenhuma informação

            //sb.Append("O campo 'Valor' deve ser preenchido!");
            isValid = false;
        }
        double result;                                  //caso nao aconteça a conversão
        if (!string.IsNullOrEmpty(EntryValue.Text) && !double.TryParse(EntryValue.Text, out result)) //vendo se da para converter em double e ja da uma saida(out)
        {
            //sb.Append("O campo 'Valor' é inválido");
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