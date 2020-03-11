using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BankOfBIT_JG.Models;
using Utility;
using System.Data;
using System.Data.Linq;

namespace BankService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TransactionManager" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TransactionManager.svc or TransactionManager.svc.cs at the Solution Explorer and start debugging.
    public class TransactionManager : ITransactionManager
    {
        // Instance of the database to be used in methods.
        private BankOfBIT_JGContext db = new BankOfBIT_JGContext();

        /// <summary>
        /// Deposits the amount into the bank account.
        /// </summary>
        /// <param name="accountId">The bank account id to be deposited to.</param>
        /// <param name="amount">The positive amount to be deposited</param>
        /// <param name="notes">Any notes to add to the transaction, such as a memo.</param>
        /// <returns>The adjusted balance of the account.</returns>
        public double? Deposit(int accountId, double amount, string notes)
        {
            double? output = UpdateBalance(accountId, Math.Abs(amount));
            // If update was successful (not null) then create transaction.
            if (output != null)
            {
                // if an error occurs (returns false), then output null, and reset balance.
                if (!CreateTransaction(accountId, amount, notes, TransactionTypeValues.Deposit))
                {
                    output = null;
                    UpdateBalance(accountId, -Math.Abs(amount));
                }
            }
            return output;
        }

        /// <summary>
        /// Withdraws the amount into the bank account.
        /// </summary>
        /// <param name="accountId">The bank account id to withdraw from.</param>
        /// <param name="amount">The positive amount to be withdrawn</param>
        /// <param name="notes">Any notes to add to the transaction, such as a memo.</param>
        /// <returns>The adjusted balance of the account.</returns>
        public double? Withdrawal(int accountId, double amount, string notes)
        {
            double? output = UpdateBalance(accountId, -Math.Abs(amount));
            // If update was successful (not null) then create transaction.
            if (output != null)
            {
                // if an error occurs (returns false), then output null, and reset balance.
                if (!CreateTransaction(accountId, amount, notes, TransactionTypeValues.Withdrawal))
                {
                    output = null;
                    UpdateBalance(accountId, Math.Abs(amount));
                }
            }
            return output;
        }

        /// <summary>
        /// Withdraws the amount from the account as a bill payment.
        /// </summary>
        /// <param name="accountId">The bank account id to pay the bill.</param>
        /// <param name="amount">The positive amount to be paid.</param>
        /// <param name="notes">Any notes to add to the transaction, such as a memo.</param>
        /// <returns>The adjusted balance of the account.</returns>;
        public double? BillPayment(int accountId, double amount, string notes)
        {
            double? output = UpdateBalance(accountId, -Math.Abs(amount));
            // If update was successful (not null) then create transaction.
            if (output != null)
            {
                // if an error occurs (returns false), then output null, and reset balance.
                if (!CreateTransaction(accountId, amount, notes, TransactionTypeValues.BillPayment))
                {
                    output = null;
                    UpdateBalance(accountId, Math.Abs(amount));
                } 
            }
            return output;
        }

        /// <summary>
        /// Transfers the amount from the from account to the to account.
        /// </summary>
        /// <param name="fromAccountId">The account to transfer from.</param>
        /// <param name="toAccountId">The account to transfer to.</param>
        /// <param name="amount">The positive amount to transfer.</param>
        /// <param name="notes">Any notes to add to the transaction, such as a memo.</param>
        /// <returns></returns>
        public double? Transfer(int fromAccountId, int toAccountId, double amount, string notes)
        {
            double? output;
            try
            {
                Exception transferErrorException = new Exception();
                // Update balance, if errors then throw exception.
                if ((output = UpdateBalance(fromAccountId, -Math.Abs(amount))) == null)
                {
                    throw transferErrorException;
                }
                if ( UpdateBalance(toAccountId, Math.Abs(amount)) == null)
                {
                    throw transferErrorException;
                }

                // Create transactions, if errors then throw exception and reset balance.
                if (!CreateTransaction(fromAccountId, amount, notes, TransactionTypeValues.Transfer) || !CreateTransaction(toAccountId, amount, notes, TransactionTypeValues.TransferRecipient))
                {
                    // Reverse the transfer, as creating transaction failed.
                    UpdateBalance(fromAccountId, Math.Abs(amount));
                    UpdateBalance(toAccountId, -Math.Abs(amount));
                    throw transferErrorException;
                }
            }
            catch
            {
                output = null;
            }
            
            return output;
        }

        /// <summary>
        /// To be implemented.
        /// </summary>
        /// <param name="accountId">The account to calculate interest on.</param>
        /// <param name="notes">Any notes to add to the transaction, such as a memo.</param>
        /// <returns>The amount of interest to charge.</returns>
        public double? CalculateInterest(int accountId, string notes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the account's balance by the amount given.
        /// </summary>
        /// <param name="accountId">The account to update.</param>
        /// <param name="amount">The amount to update. Positive or negative, for deposit or withdrawal respectively.</param>
        /// <returns>The updated balance, or null if operation failed.</returns>
        private double? UpdateBalance(int accountId, double amount)
        {
            double? adjustedBalance;
            try
            {
                BankAccount account = db.BankAccounts.Find(accountId);
                account.Balance += amount;
                db.SaveChanges();
                account.ChangeState();             
                db.SaveChanges();
                adjustedBalance = account.Balance;
            }
            catch 
            {
                adjustedBalance = null;
            }
            return adjustedBalance;
            
        }

        /// <summary>
        /// Adds a new transaction to the database and associates it with the account.
        /// </summary>
        /// <param name="accountID">The account this transaction took place in.</param>
        /// <param name="amount">The amount of the transaction.</param>
        /// <param name="notes">Any notes associated with the transaction.</param>
        /// <param name="type">The type of transaction performed, example: "Bill Payment"</param>
        /// <returns>True id successful, false otherwise.</returns>
        private bool CreateTransaction(int accountID, double amount, string notes, TransactionTypeValues type)
        {
            bool success = true; 

            //try
            {
                Transaction transaction = new Transaction();
                transaction.BankAccountId = accountID;
                transaction.SetNextTransactionNumber();
                transaction.Notes = notes;
                transaction.DateCreated = DateTime.Now;

                // Switches through every type and sets typeID and deposit or withdrawal amount appropriately.
                switch (type)
                {
                    case TransactionTypeValues.Deposit:
                        transaction.Deposit = amount;
                        transaction.Withdrawal = 0;
                        transaction.TransactionTypeId = (int)TransactionTypeValues.Deposit;
                        break;
                    case TransactionTypeValues.Withdrawal:
                        transaction.Withdrawal = amount;
                        transaction.Deposit = 0;
                        transaction.TransactionTypeId = (int)TransactionTypeValues.Withdrawal;
                        break;
                    case TransactionTypeValues.BillPayment:
                        transaction.Withdrawal = amount;
                        transaction.Deposit = 0;
                        transaction.TransactionTypeId = (int)TransactionTypeValues.BillPayment;
                        break;
                    case TransactionTypeValues.Transfer:
                        transaction.Withdrawal = amount;
                        transaction.Deposit = 0;
                        transaction.TransactionTypeId = (int)TransactionTypeValues.Transfer;
                        break;
                    case TransactionTypeValues.TransferRecipient:
                        transaction.Deposit = amount;
                        transaction.Withdrawal = 0;
                        transaction.TransactionTypeId = (int)TransactionTypeValues.TransferRecipient;
                        break;
                    case TransactionTypeValues.CalculateInterest:
                        transaction.TransactionTypeId = (int)TransactionTypeValues.Withdrawal;
                        break;
                    default:
                        success = false;
                        Exception InvalidTransactionTypeException = new Exception("Invalid Transaction Type.");
                        throw InvalidTransactionTypeException;
                }
                db.Transactions.Add(transaction);
                db.SaveChanges();
            }
            //catch 
            //{
            //    success = false;
            //}

            return success;
        }
    }
}
