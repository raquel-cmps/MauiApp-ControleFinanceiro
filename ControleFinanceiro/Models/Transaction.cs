using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleFinanceiro.Models
{
    public class Transaction
    {
        [BsonId]
        public int Id { get; set; }
        public TransactionType TransactionType{ get; set; }
        public String Name { get; set; }
        public DateTimeOffset Date { get; set; } //registra o fusiorario
        public double Value { get; set; }
    }

    public enum TransactionType
    {
        Income,
        Expense
    }
}
