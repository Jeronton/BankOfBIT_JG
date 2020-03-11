using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Utility;
using System.Data.SqlClient;
using System.Data;

namespace BankOfBIT_JG.Models
{
    /// <summary>
    /// Represents the Clients table in the database.
    /// </summary>
    public class Client
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClientId { get; set; }

        [Display(Name ="Client")]
        public long ClientNumber { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 1)]
        [Display(Name = "First\nName")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 1)]
        [Display(Name = "Last\nName")]
        public string LastName { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 1)]
        [Display(Name = "Street\nAddress")]
        public string Address { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 1)]
        public string City { get; set; }

        [Required]
        [RegularExpression("(?:AB|BC|MB|N[BLTSU]|ON|PE|QC|SK|YT)", ErrorMessage = "Invalid Province.")]
        public string Province { get; set; }

        [Required]
        [Display(Name = "Postal\nCode")]
        [RegularExpression("([ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVXY] [0-9][ABCEGHJKLMNPRSTVXY][0-9])", ErrorMessage = "Invalid Postal Code.")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Date\nCreated")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        [Display(Name ="Client\nNotes")]
        public string Notes { get; set; }

        /// <summary>
        /// Gets the full name of the client.
        /// </summary>
        [Display(Name ="Name")]
        public string FullName 
        { 
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        /// <summary>
        /// To be implemented.
        /// </summary>
        [Display(Name ="Address")]
        public string FullAddress
        {
            get
            {
                return string.Format("{0} {1}, {2} {3}", Address, City, Province, PostalCode);
            }
        }

        // Navigation Property, a client can have many accounts.
        public virtual ICollection<BankAccount> BankAccount { get; set; }

        /// <summary>
        /// Gets the next available client number.
        /// </summary>
        public void SetNextClientNumber()
        {
            ClientNumber = (long)StoredProcedures.NextNumber("NextClientNumbers");
        }


    }

    /// <summary>
    /// Represents the BankAccounts Table in the database.
    /// </summary>
    public abstract class BankAccount
    {
        private BankOfBIT_JGContext db = new BankOfBIT_JGContext();

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BankAccountId { get; set; }

        [Display(Name ="Account\nNumber")]
        public long AccountNumber { get; set; }

        [Required]
        [ForeignKey("Client")]
        public int ClientId { get; set; }

        [Required]
        [ForeignKey("AccountState")]
        public int AccountStateId { get; set; }

        [Required]
        [Display(Name = "Current\nBalance")]
        [DisplayFormat(DataFormatString ="{0:C}")]
        public double Balance { get; set; }

        [Required]
        [Display(Name ="Opening\nBalance")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double OpeningBalance { get; set; }

        [Required]
        [Display(Name ="Date\nCreated")]
        [DisplayFormat(DataFormatString ="{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        [Display(Name ="Account\nNotes")]
        public string Notes { get; set; }

        /// <summary>
        /// Gets the account type (description).
        /// </summary>
        [Display(Name ="Account\nType")]
        public string Description 
        {
            get
            {
                return Utilities.GetPriorString(GetType().Name, "Account");
            }
        }

        // Navigation property - A Bank account can have only one client.
        public virtual Client Client { get; set; }

        // Navigation property - A bank account can have only 1 account state.
        public virtual AccountState AccountState { get; set; }

        // Navigation property - A bank account can have many Transactions.
        public virtual ICollection<Transaction> Transaction { get; set; }

        /// <summary>
        /// Abstract method that will get the next available account number.
        /// </summary>
        public abstract void SetNextAccountNumber();

        /// <summary>
        /// Checks if the balance has exceeded or dropped below the limits of the current account state
        /// and updates state accordingly. 
        /// </summary>
        public void ChangeState()
        {
            AccountState before, after = db.AccountStates.Find(this.AccountStateId);

            // While the current id does not equal the previous id do:
            do   
            {
                before = after;
                before.StateChangeCheck(this);
                after = db.AccountStates.Find(this.AccountStateId);
                db.SaveChanges();
            } while (! before.Equals(after));

        }
    }

    /// <summary>
    /// Represents the AccountState Table of the database.
    /// </summary>
    public abstract class AccountState
    {
        protected static BankOfBIT_JGContext db = new BankOfBIT_JGContext();

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountStateId { get; set; }

        [Display(Name = "Lower\nLimit")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double LowerLimit { get; set; }

        [Display(Name = "Upper\nLimit")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double UpperLimit { get; set; }

        [Required]
        [Display(Name = "Interest\nRate")]
        [DisplayFormat(DataFormatString = "{0:P}")]
        public double Rate { get; set; }

        // Navigation property, an AccountState can have many BankAccounts.
        public virtual ICollection<BankAccount> BankAccount{get; set;}

        /// <summary>
        /// Gets the state name.
        /// </summary>
        [Display(Name ="Account\nState")]
        public string Description 
        {
            get
            {
                return Utilities.GetPriorString(GetType().Name, "State");
            }
        }

        /// <summary>
        /// Gets the adjusted rate for the current state and balance of the account.
        /// </summary>
        /// <param name="bankAccount">The bank account to adjust the rate of.</param>
        /// <returns>The adjusted rate.</returns>
        public virtual double RateAdjustment(BankAccount bankAccount)
        {
            return 0;
        }

        /// <summary>
        /// Moves the account's state one level if required.
        /// </summary>
        /// <param name="bankAccount">The bank account to update.</param>
        public virtual void StateChangeCheck(BankAccount bankAccount)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Represents the bronze instance of an account state.
    /// </summary>
    public class BronzeState : AccountState
    {
        private static BronzeState bronzeState;

        /// <summary>
        /// Creates an instance of BronzeState with default values.
        /// </summary>
        private BronzeState()
        {
            LowerLimit = 0;
            UpperLimit = 5000;
            Rate = 0.01;
        }

        /// <summary>
        /// Gets the single instance of BronzeState. 
        /// </summary>
        /// <returns>The instance of BronzeState.</returns>
        public static BronzeState GetInstance()
        {
            if (bronzeState == null)
            {
                bronzeState = db.BronzeStates.SingleOrDefault();
                // if not in database then create new instance and add to database.
                if (bronzeState == null)
                {
                    bronzeState = new BronzeState();
                    db.BronzeStates.Add(bronzeState);
                    db.SaveChanges();
                }             
            }
            return bronzeState;
        }

        /// <summary>
        /// Adjusts the interest rate according to the balance of the account.
        /// </summary>
        /// <param name="bankAccount">The bank account for rate to be adjusted. </param>
        /// <returns></returns>
        public override double RateAdjustment(BankAccount bankAccount)
        {
            double adjustedRate = Rate;
            if (bankAccount.Balance >= 0)
            {
                adjustedRate = 0.055;
            }
            return adjustedRate;
        }

        /// <summary>
        /// Moves the account's state one level if required.
        /// </summary>
        /// <param name="bankAccount">The bank account to update.</param>
        public override void StateChangeCheck(BankAccount bankAccount)
        {
            if (bankAccount.Balance > UpperLimit)
            {
                bankAccount.AccountStateId = SilverState.GetInstance().AccountStateId;
            }
        }
    }

    /// <summary>
    /// Represents the silver instance of an account state.
    /// </summary>
    public class SilverState : AccountState
    {
        private static SilverState silverState;

        /// <summary>
        /// Creates an instance of SilverState with default values.
        /// </summary>
        private SilverState()
        {
            LowerLimit = 5000;
            UpperLimit = 10000;
            Rate = 0.0125;
        }

        /// <summary>
        /// Gets the single instance of SilverState.
        /// </summary>
        /// <returns>The instance of SilverState.</returns>
        public static SilverState GetInstance()
        {
            if (silverState == null)
            {
                silverState = db.SilverStates.SingleOrDefault();
                // If not in database create new instance.
                if (silverState == null)
                {
                    silverState = new SilverState();
                    db.SilverStates.Add(silverState);
                    db.SaveChanges();
                }
            }
            return silverState;
        }

        /// <summary>
        /// Adjusts the interest rate according to the balance of the account.
        /// </summary>
        /// <param name="bankAccount">The bank account for rate to be adjusted. </param>
        /// <returns></returns>
        public override double RateAdjustment(BankAccount bankAccount)
        {
            return Rate;
        }

        /// <summary>
        /// Moves the account's state one level if required.
        /// </summary>
        /// <param name="bankAccount">The bank account to update.</param>
        public override void StateChangeCheck(BankAccount bankAccount)
        {
            if (bankAccount.Balance < LowerLimit)
            {
                bankAccount.AccountStateId = BronzeState.GetInstance().AccountStateId;
            }
            else if (bankAccount.Balance > UpperLimit)
            {
                bankAccount.AccountStateId = GoldState.GetInstance().AccountStateId;
            }
        }
    }


    /// <summary>
    /// Represents the Gold instance of an account state.
    /// </summary>
    public class GoldState : AccountState
    {
        static GoldState goldState;

        /// <summary>
        /// Creates a instance of GoldState with default values.
        /// </summary>
        private GoldState()
        {
            LowerLimit = 10000;
            UpperLimit = 20000;
            Rate = 0.02;
        }

        /// <summary>
        /// The single instance of GoldState.
        /// </summary>
        /// <returns>The instance of GoldState.</returns>
        public static GoldState GetInstance()
        {
            if (goldState == null)
            {
                goldState = db.GoldStates.SingleOrDefault();
                // if not in database then create new instance.
                if (goldState == null)
                {
                    goldState = new GoldState();
                    db.GoldStates.Add(goldState);
                    db.SaveChanges();
                }
            }
            return goldState;
        }

        /// <summary>
        /// Adjusts the interest rate according to the balance of the account.
        /// </summary>
        /// <param name="bankAccount">The bank account for rate to be adjusted. </param>
        /// <returns></returns>
        public override double RateAdjustment(BankAccount bankAccount)
        {
            double adjustedRate = Rate;
            if (bankAccount.DateCreated.AddYears(10) <= DateTime.Now)
            {
                adjustedRate += 0.01;
            }
            return adjustedRate;
        }

        /// <summary>
        /// Moves the account's state one level if required.
        /// </summary>
        /// <param name="bankAccount">The bank account to update.</param>
        public override void StateChangeCheck(BankAccount bankAccount)
        {
            if (bankAccount.Balance < LowerLimit)
            {
                bankAccount.AccountStateId = SilverState.GetInstance().AccountStateId;
            }
            else if (bankAccount.Balance > UpperLimit)
            {
                bankAccount.AccountStateId = PlatinumState.GetInstance().AccountStateId;
            }
        }
    }


    /// <summary>
    /// Represents the platinum state instance of an account state.
    /// </summary>
    public class PlatinumState : AccountState
    {
        private static PlatinumState platinumState;

        /// <summary>
        /// Creates an instance of PlatinumState with default values.
        /// </summary>
        private PlatinumState()
        {
            LowerLimit = 20000;
            UpperLimit = 0;
            Rate = 0.0250;
        }

        /// <summary>
        /// Gets the single instance of PlatinumState.
        /// </summary>
        /// <returns>The instance of PlatinumState.</returns>
        public static PlatinumState GetInstance()
        {
            if (platinumState == null)
            {
                platinumState = db.PlatinumStates.SingleOrDefault();
                // If not in database then create new instance.
                if (platinumState == null)
                {
                    platinumState = new PlatinumState();
                    db.PlatinumStates.Add(platinumState);
                    db.SaveChanges();
                }
            }
            return platinumState;
        }

        /// <summary>
        /// Adjusts the interest rate according to the balance of the account.
        /// </summary>
        /// <param name="bankAccount">The bank account for rate to be adjusted. </param>
        /// <returns></returns>
        public override double RateAdjustment(BankAccount bankAccount)
        {
            double adjustedRate = Rate;
            if (bankAccount.DateCreated.AddYears(10) <= DateTime.Now)
            {
                adjustedRate += 0.01;
            }
            if (bankAccount.Balance > 2 * LowerLimit)
            {
                adjustedRate += 0.005;
            }
            return adjustedRate;
        }

        /// <summary>
        /// Moves the account's state one level if required.
        /// </summary>
        /// <param name="bankAccount">The bank account to update.</param>
        public override void StateChangeCheck(BankAccount bankAccount)
        {
            if (bankAccount.Balance < LowerLimit)
            {
                bankAccount.AccountStateId = GoldState.GetInstance().AccountStateId;
            }
        }
    }

    /// <summary>
    /// Represents a savings account.
    /// </summary>
    public class SavingsAccount : BankAccount
    {
        [Required]
        [Display(Name ="Service\nCharges")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double SavingsServiceCharges { get; set; }

        /// <summary>
        /// Sets the AccountNumber to the next available number
        /// </summary>
        override public void SetNextAccountNumber()
        {
            AccountNumber = (long)StoredProcedures.NextNumber("NextSavingsAccounts");
        }
    }


    /// <summary>
    /// Represents a mortgage account.
    /// </summary>
    public class MortgageAccount : BankAccount
    {
        [Required]
        [Display(Name ="Interest\nRate")]
        [DisplayFormat(DataFormatString = "{0:P}")]
        public double MortgageRate { get; set; }

        [Required]
        public int Amortization { get; set; }

        /// <summary>
        /// Sets the AccountNumber to the next available number
        /// </summary>
        override public void SetNextAccountNumber()
        {
            AccountNumber = (long)StoredProcedures.NextNumber("NextMortgageAccounts");
        }
    }


    /// <summary>
    /// Represents a investment account.
    /// </summary>
    public class InvestmentAccount : BankAccount
    {
        [Required]
        [Display(Name ="Interest\nRate")]
        [DisplayFormat(DataFormatString = "{0:P}")]
        public double InterestRate { get; set; }

        /// <summary>
        /// Sets the AccountNumber to the next available number
        /// </summary>
        override public void SetNextAccountNumber()
        {
            AccountNumber = (long)StoredProcedures.NextNumber("NextInvestmentAccounts");
        }
    }


    /// <summary>
    /// Represents a Chequing account.
    /// </summary>
    public class ChequingAccount : BankAccount
    {
        [Required]
        [Display(Name ="Service\nCharges")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double ChequingServiceCharges { get; set; }

        /// <summary>
        /// Sets the AccountNumber to the next available number
        /// </summary>
        override public void SetNextAccountNumber()
        {
            AccountNumber = (long)StoredProcedures.NextNumber("NextChequingAccounts");
        }     
    }

    /// <summary>
    /// Represents a bank transaction.
    /// </summary>
    public class Transaction
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        [Display(Name = "Transaction\nNumber")]
        public long TransactionNumber { get; set; }

        [Required]
        [ForeignKey("BankAccount")]
        public int BankAccountId { get; set; }

        [Required]
        [Display(Name ="Transaction\nType")]
        [ForeignKey("TransactionType")]
        public int TransactionTypeId { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public double Deposit { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public double Withdrawal { get; set; }

        [Required]
        [Display(Name = "Date\nCreated")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        public string Notes { get; set; }

        // Navigation property: A transaction have 1 TransactionType.
        public virtual TransactionType TransactionType { get; set; }

        // Navigation property: A Transaction can have one bank account.
        public virtual BankAccount BankAccount { get; set; }

        /// <summary>
        /// Sets the next available transaction number.
        /// </summary>
        public void SetNextTransactionNumber()
        {
            TransactionNumber = (long)StoredProcedures.NextNumber("NextTransactionNumbers");
        }
    }

    /// <summary>
    /// Represents the Type of transaction completed.
    /// </summary>
    public class TransactionType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionTypeId { get; set; }

        [Display(Name ="Transaction\nType")]
        public string Description { get; set; }

        [Display(Name = "ServiceCharge\nCharges")]
        [DisplayFormat(DataFormatString ="{0:c}")]
        public double ServiceCharges { get; set; }

        // Does not navigate to Transaction.
    }

    #region Next_Numbers

    /// <summary>
    /// Contains functionality to get next available id value.
    /// </summary>
    public class NextTransactionNumber
    {
        protected static BankOfBIT_JGContext db = new BankOfBIT_JGContext();

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NextTransactionNumberId { get; set; }


        public long NextAvailableNumber { get; set; }


        private static NextTransactionNumber nextTransactionNumber;

        /// <summary>
        /// Creates an instance of NextTransactionNumber with default values.
        /// </summary>
        private NextTransactionNumber()
        {
            NextAvailableNumber = 700;
        }

        /// <summary>
        /// Gets the single instance of NextTransactionNumber.
        /// </summary>
        /// <returns>The single instance of NextTransactionNumber.</returns>
        public static NextTransactionNumber GetInstance()
        {
            if (nextTransactionNumber == null)
            {
                nextTransactionNumber = db.NextTransactionNumbers.SingleOrDefault();
                if (nextTransactionNumber == null)
                {
                    nextTransactionNumber = new NextTransactionNumber();
                    db.NextTransactionNumbers.Add(nextTransactionNumber);
                    db.SaveChanges();
                }
            }
            return nextTransactionNumber;
        }
    }

    /// <summary>
    /// Contains functionality to get next available id value.
    /// </summary>
    public class NextSavingsAccount
    {
        protected static BankOfBIT_JGContext db = new BankOfBIT_JGContext();

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NextSavingsAccountId { get; set; }


        public long NextAvailableNumber { get; set; }


        private static NextSavingsAccount nextSavingsAccount;

        /// <summary>
        /// Creates an instance of NextSavingsAccount with default values.
        /// </summary>
        private NextSavingsAccount()
        {
            NextAvailableNumber = 20000;
        }

        /// <summary>
        /// Gets the single instance of NextSavingsAccount.
        /// </summary>
        /// <returns>The single instance of NextSavingsAccount.</returns>
        public static NextSavingsAccount GetInstance()
        {
            if (nextSavingsAccount == null)
            {
                nextSavingsAccount = db.NextSavingsAccounts.SingleOrDefault();
                if (nextSavingsAccount == null)
                {
                    nextSavingsAccount = new NextSavingsAccount();
                    db.NextSavingsAccounts.Add(nextSavingsAccount);
                    db.SaveChanges();
                }
            }
            return nextSavingsAccount;
        }
    }

    /// <summary>
    /// Contains functionality to get next available id value.
    /// </summary>
    public class NextMortgageAccount
    {
        protected static BankOfBIT_JGContext db = new BankOfBIT_JGContext();

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NextMortgageAccountId { get; set; }


        public long NextAvailableNumber { get; set; }


        private static NextMortgageAccount nextMortgageAccount;

        /// <summary>
        /// Creates an instance of NextMortgageAccount with default values.
        /// </summary>
        private NextMortgageAccount()
        {
            NextAvailableNumber = 200000;
        }

        /// <summary>
        /// Gets the single instance of NextMortgageAccount.
        /// </summary>
        /// <returns>The single instance of NextMortgageAccount.</returns>
        public static NextMortgageAccount GetInstance()
        {
            if (nextMortgageAccount == null)
            {
                nextMortgageAccount = db.NextMortgageAccounts.SingleOrDefault();
                if (nextMortgageAccount == null)
                {
                    nextMortgageAccount = new NextMortgageAccount();
                    db.NextMortgageAccounts.Add(nextMortgageAccount);
                    db.SaveChanges();
                }
            }
            return nextMortgageAccount;
        }
    }

    /// <summary>
    /// Contains functionality to get next available id value.
    /// </summary>
    public class NextInvestmentAccount
    {
        protected static BankOfBIT_JGContext db = new BankOfBIT_JGContext();

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NextInvestmentAccountId { get; set; }

        public long NextAvailableNumber { get; set; }

        private static NextInvestmentAccount nextInvestmentAccount;

        /// <summary>
        /// Creates an instance of NextInvestmentAccount with default values.
        /// </summary>
        private NextInvestmentAccount()
        {
            NextAvailableNumber = 2000000;
        }

        /// <summary>
        /// Gets the single instance of NextInvestmentAccount.
        /// </summary>
        /// <returns>The single instance of NextInvestmentAccount.</returns>
        public static NextInvestmentAccount GetInstance()
        {
            if (nextInvestmentAccount == null)
            {
                nextInvestmentAccount = db.NextInvestmentAccounts.SingleOrDefault();
                if (nextInvestmentAccount == null)
                {
                    nextInvestmentAccount = new NextInvestmentAccount();
                    db.NextInvestmentAccounts.Add(nextInvestmentAccount);
                    db.SaveChanges();
                }
            }
            return nextInvestmentAccount;
        }
    }

    /// <summary>
    /// Contains functionality to get next available id value.
    /// </summary>
    public class NextChequingAccount
    {
        protected static BankOfBIT_JGContext db = new BankOfBIT_JGContext();

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NextChequingAccountId { get; set; }

        public long NextAvailableNumber { get; set; }

        private static NextChequingAccount nextChequingAccount;

        /// <summary>
        /// Creates an instance of NextChequingAccount with default values.
        /// </summary>
        private NextChequingAccount()
        {
            NextAvailableNumber = 20000000;
        }

        /// <summary>
        /// Gets the single instance of NextChequingAccount.
        /// </summary>
        /// <returns>The single instance of NextChequingAccount.</returns>
        public static NextChequingAccount GetInstance()
        {
            if (nextChequingAccount == null)
            {
                nextChequingAccount = db.NextChequingAccounts.SingleOrDefault();
                if (nextChequingAccount == null)
                {
                    nextChequingAccount = new NextChequingAccount();
                    db.NextChequingAccounts.Add(nextChequingAccount);
                    db.SaveChanges();
                }
            }
            return nextChequingAccount;
        }
    }

    /// <summary>
    /// Contains functionality to get next available id value.
    /// </summary>
    public class NextClientNumber
    {
        protected static BankOfBIT_JGContext db = new BankOfBIT_JGContext();

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NextClientNumberId { get; set; }

        public long NextAvailableNumber { get; set; }

        private static NextClientNumber nextClientNumber;

        /// <summary>
        /// Creates an instance of NextClientNumber with default values.
        /// </summary>
        private NextClientNumber()
        {
            NextAvailableNumber = 20000000;
        }

        /// <summary>
        /// Gets the single instance of NextClientNumber.
        /// </summary>
        /// <returns>The single instance of NextClientNumber.</returns>
        public static NextClientNumber GetInstance()
        {
            if (nextClientNumber == null)
            {
                nextClientNumber = db.NextClientNumbers.SingleOrDefault();
                if (nextClientNumber == null)
                {
                    nextClientNumber = new NextClientNumber();
                    db.NextClientNumbers.Add(nextClientNumber);
                    db.SaveChanges();
                }
            }
            return nextClientNumber;
        }
    }


    #endregion
    
    /// <summary>
    /// Represents a Payee.
    /// </summary>
    public class Payee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PayeeId { get; set; }

        [Required]
        [Display(Name ="Payee")]
        public string Description { get; set; }
    }

    /// <summary>
    /// Represents an institution.
    /// </summary>
    public class Institution
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InstitutionId { get; set; }

        [Display(Name = "Institution\nNumber")]
        public int InstitutionNumber { get; set; }

        [Display(Name = "Institution")]
        public string Description { get; set; }
    }

    /// <summary>
    /// Represents a RFID tag.
    /// </summary>
    public class RFIDTag
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RFIDTagId { get; set; }

        public long CardNumber { get; set; }

        [Required]
        [ForeignKey("Client")]
        public int ClientId { get; set; }

        // Navigation property - RFIDTag can have one client.
        public virtual Client Client { get; set; }
    }

    /// <summary>
    /// Contains methods to manage stored procedures.
    /// </summary>
    public static class StoredProcedures
    {
       /// <summary>
       /// Gets the next available number for the provided table.
       /// </summary>
       /// <param name="tableName">The table to find next number from.</param>
       /// <returns>The next available id of provided table.</returns>
        public static long? NextNumber(string tableName)
        {
            // create a null-able long variable to eventually return as the next number.
            long? returnValue = 0;
            try
            {
                // Create a new connection to the database, but don't open it yet.
                SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=BankOfBIT_JGContext;Integrated Security=True");
                // Create/get the procedure we added.
                SqlCommand storedProcedure = new SqlCommand("next_number", connection);
                // Sets the type of command to execute the procedure.
                storedProcedure.CommandType = CommandType.StoredProcedure;
                // Adds the tableName parameter as the tableName required by the procedure.
                storedProcedure.Parameters.AddWithValue("@TableName", tableName);
                // creates a sqlParameter instance which handles the output parameter of the procedure.
                // This provides the procedure with a variable to output its data to.
                SqlParameter outputParameter = new SqlParameter("@NewVal", SqlDbType.BigInt)
                {
                    // specifies that the parameter is output type.
                    Direction = ParameterDirection.Output
                };
                // Adds the output parameter to the procedure.
                storedProcedure.Parameters.Add(outputParameter);
                // Open the connection.
                connection.Open();
                // Execute the procedure.
                storedProcedure.ExecuteNonQuery();
                // Close the connection.
                connection.Close();
                // Set returnValue to the output parameter of the procedure.
                returnValue = (long?)outputParameter.Value;               
            }
            catch 
            {
                // If any exception occurs return null.
                returnValue = null;
            }
            // Return the next number.
            return returnValue;
        }
    }
}