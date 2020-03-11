using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BankOfBIT_JG.Models;
using System.Data;
using System.Data.Linq;

namespace BankService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITransactionManager" in both code and config file together.
    [ServiceContract]
    public interface ITransactionManager
    {
        /// <summary>
        /// Deposits the amount into the bank account.
        /// </summary>
        /// <param name="accountId">The bank account id to be deposited to.</param>
        /// <param name="amount">The amount to be deposited</param>
        /// <param name="notes">Any notes to add to the transaction, such as a memo.</param>
        /// <returns>The adjusted balance of the account.</returns>
        [OperationContract]
        double? Deposit(int accountId, double amount, string notes);

        /// <summary>
        /// Withdraws the amount into the bank account.
        /// </summary>
        /// <param name="accountId">The bank account id to withdraw from.</param>
        /// <param name="amount">The amount to be Withdrawn</param>
        /// <param name="notes">Any notes to add to the transaction, such as a memo.</param>
        /// <returns>The adjusted balance of the account.</returns>
        [OperationContract]
        double? Withdrawal(int accountId, double amount, string notes);

        /// <summary>
        /// Withdraws the amount from the account as a bill payment.
        /// </summary>
        /// <param name="accountId">The bank account id to pay the bill.</param>
        /// <param name="amount">The amount to be paid.</param>
        /// <param name="notes">Any notes to add to the transaction, such as a memo.</param>
        /// <returns>The adjusted balance of the account.</returns>;
        [OperationContract]
        double? BillPayment(int accountId, double amount, string notes);

        /// <summary>
        /// Transfers the amount from the from account to the to account.
        /// </summary>
        /// <param name="fromAccountId">The account to transfer from.</param>
        /// <param name="toAccountId">The account to transfer to.</param>
        /// <param name="amount">The amount to transfer.</param>
        /// <param name="notes">Any notes to add to the transaction, such as a memo.</param>
        /// <returns></returns>
        [OperationContract]
        double? Transfer(int fromAccountId, int toAccountId, double amount, string notes);

        /// <summary>
        /// To be implemented.
        /// </summary>
        /// <param name="accountId">The account to calculate interest on.</param>
        /// <param name="notes">Any notes to add to the transaction, such as a memo.</param>
        /// <returns>The amount of interest to charge.</returns>
        double? CalculateInterest(int accountId, string notes);
    }
}
