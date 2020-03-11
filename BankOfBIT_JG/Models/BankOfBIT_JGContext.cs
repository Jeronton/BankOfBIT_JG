using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BankOfBIT_JG.Models
{
    public class BankOfBIT_JGContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    


        public BankOfBIT_JGContext() : base("name=BankOfBIT_JGContext")
        {

        }

        public System.Data.Entity.DbSet<BankOfBIT_JG.Models.Client> Clients { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_JG.Models.AccountState> AccountStates { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_JG.Models.BronzeState> BronzeStates { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_JG.Models.SilverState> SilverStates { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_JG.Models.GoldState> GoldStates { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_JG.Models.PlatinumState> PlatinumStates { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_JG.Models.BankAccount> BankAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_JG.Models.SavingsAccount> SavingsAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_JG.Models.ChequingAccount> ChequingAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_JG.Models.MortgageAccount> MortgageAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_JG.Models.InvestmentAccount> InvestmentAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_JG.Models.TransactionType> TransactionTypes { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_JG.Models.Transaction> Transactions { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_JG.Models.Payee> Payees { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_JG.Models.Institution> Institutions { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_JG.Models.NextTransactionNumber> NextTransactionNumbers { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_JG.Models.NextSavingsAccount> NextSavingsAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_JG.Models.NextMortgageAccount> NextMortgageAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_JG.Models.NextChequingAccount> NextChequingAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_JG.Models.NextInvestmentAccount> NextInvestmentAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_JG.Models.NextClientNumber> NextClientNumbers { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_JG.Models.RFIDTag> RFIDTags { get; set; }
    }
}
