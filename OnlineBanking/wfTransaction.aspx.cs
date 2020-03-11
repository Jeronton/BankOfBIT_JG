using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BankOfBIT_JG.Models;
using System.Data.Linq;
using System.Data;
using Utility;

namespace OnlineBanking
{
    public partial class wfTransaction : System.Web.UI.Page
    {
        // The context object to be used by the form.
        BankOfBIT_JGContext db = new BankOfBIT_JGContext();

        // The Transaction web service.
        TransactionService.TransactionManagerClient transactionService = new TransactionService.TransactionManagerClient();
        
        /// <summary>
        /// Enables the error label and sets its text.
        /// </summary>
        /// <param name="error">The message to display.</param>
        private void SetError(string error)
        {
            this.lblError.Text = error;
            this.lblError.Visible = true;
        }

        /// <summary>
        /// Performs transactions and displays an error message if transaction fails.
        /// </summary>
        /// <param name="transactionType">The type of transaction to complete.</param>
        /// <param name="transactionMessage">The message to assign to the transaction.</param>
        /// <param name="amount">The amount of the transaction.</param>
        private void PerformTransaction(TransactionTypeValues transactionType, string notes, double amount, int accountId)
        {
            double? balance = null;
            string errorMessage = "";
            switch (transactionType)
            {
                case TransactionTypeValues.BillPayment:
                    // Performs the transaction.
                    balance = transactionService.BillPayment(accountId, amount, notes);
                    errorMessage = "Unable to make bill payment at this time, please try again later.";
                    break;
                case TransactionTypeValues.Transfer:
                    balance = transactionService.Transfer(accountId, int.Parse(this.ddlRecipient.SelectedValue), amount, notes);
                    errorMessage = "Unable to complete transfer at this time, please try again later.";
                    break;
                default:
                    errorMessage ="Invalid Transaction type, please try again.";
                    break;
            }

            // if null then error otherwise we are good!
            if (balance == null)
            {
                SetError(errorMessage);
            }
            else
            {
                Session["Balance"] = (double)balance;
                ResetForm();
            }
        }

        /// <summary>
        /// Resets the amount and bindings.
        /// </summary>
        private void ResetForm()
        {
            BindRecipients(TransactionTypeValues.BillPayment);
            BindTransactionTypes();
            txtAmount.Text = null;
            lblBalance.Text = string.Format("{0:C}", Session["Balance"]);

            // reset the error label
            lblError.Visible = false;
            lblError.Text = "";
        }

        /// <summary>
        /// Resets any binding on the drop down list.
        /// </summary>
        /// <param name="control">The drop down to reset.</param>
        private void ResetDropDownBinding(DropDownList control)
        {
            control.DataSource = null;
            control.DataValueField = null;
            control.DataTextField = null;
        }

        /// <summary>
        /// Binds the Recipients drop down list with the appropriate data.
        /// </summary>
        /// <param name="transactionType">The type of transaction currently selected, 
        /// will bind recipients accordingly. Currently only excepts billPayment and transfer. </param>
        private void BindRecipients(TransactionTypeValues transactionType)
        {
            
            try
            {
                ResetDropDownBinding(this.ddlRecipient);

                // if transfer then bind to accounts, otherwise bind to payees.
                if (transactionType == TransactionTypeValues.Transfer)
                {
                    int clientID = (int)Session["ClientID"];
                    int accountID = (int)Session["AccountID"];
                    // Get the clients accounts except the current account.
                    IQueryable<BankAccount> accounts = db.BankAccounts.Where(x => x.ClientId == clientID).Where(x => x.BankAccountId != accountID);
                    this.ddlRecipient.DataSource = accounts.ToList();
                    this.ddlRecipient.DataValueField = "BankAccountId";
                    this.ddlRecipient.DataTextField = "AccountNumber";
                }
                else if (transactionType == TransactionTypeValues.BillPayment)
                {
                    // Get Recipients to bind to Recipients drop down
                    IQueryable<Payee> payees = db.Payees;
                    this.ddlRecipient.DataSource = payees.ToList();
                    this.ddlRecipient.DataValueField = "PayeeId";
                    this.ddlRecipient.DataTextField = "Description";
                }
                else
                {
                    throw new ArgumentException();
                }

                this.ddlRecipient.DataBind();
            }
            catch
            {
                SetError("Error loading Recipients, please try again later.");
            }
        }

        /// <summary>
        /// Binds the Transaction Types drop down.
        /// </summary>
        private void BindTransactionTypes()
        {
            
            try
            {
                ResetDropDownBinding(this.ddlTransactionType);

                // Get Transaction Types to bind to transaction type drop down.
                IQueryable<TransactionType> types = db.TransactionTypes
                    .Where(x => x.Description.Equals("Bill Payment")
                            || x.Description.Equals("Transfer"));
                this.ddlTransactionType.DataSource = types.ToList();
                this.ddlTransactionType.DataTextField = "Description";
                this.ddlTransactionType.DataValueField = "TransactionTypeId";

                this.ddlTransactionType.DataBind();

            }
            catch
            {
                SetError("Error loading transaction types, please try again later.");
            }
        }

        /// <summary>
        /// Performs logic on postback.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Only execute if page load.
            if (!this.IsPostBack)
            {
                // If not logged in then login.
                if (!this.Page.User.Identity.IsAuthenticated)
                {
                    Response.Redirect("~/Account/Login.aspx");
                }
                else
                {

                    // Set TxtAmount's text to be right aligned.
                    this.txtAmount.CssClass += " AlignTextRight";
                    this.lbCompleteTransaction.CssClass += " Margin-Right-80";

                    // Bind drop downs
                    BindRecipients(TransactionTypeValues.BillPayment);
                    BindTransactionTypes();

                    // Update labels with account information
                    try
                    {
                        // set the labels to their value
                        this.lblAccountNumber.Text = Session["AccountNumber"].ToString();
                        this.lblBalance.Text = string.Format("{0:C}", Session["Balance"]);
                    }
                    catch
                    {
                        SetError("Error loading account information, please try again later.");
                    }
                }
            }

        }

        /// <summary>
        /// Processes the transaction.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbCompleteTransaction_Click(object sender, EventArgs e)
        {     
            try
            {
                if (txtAmount.Text == null || txtAmount.Text == "")
                {
                    throw new NoNullAllowedException();
                }

                double balance = (double)Session["Balance"];

                // If insufficient funds then throw exception.
                if (double.Parse(txtAmount.Text) > balance)
                {
                    throw new ArgumentOutOfRangeException(); 
                }

                string notes;

                // if Bill Payment
                if (ddlTransactionType.SelectedItem.Text.Equals("Bill Payment"))
                {
                    notes = string.Format("Bill Payment to {0}", ddlRecipient.SelectedItem.Text);
                    PerformTransaction(TransactionTypeValues.BillPayment, notes, 
                        double.Parse(txtAmount.Text), (int)Session["AccountId"]);
                }
                else if (ddlTransactionType.SelectedItem.Text.Equals("Transfer"))
                {
                    notes = string.Format("Money Transfer from {0} to {1}", Session["AccountNumber"], ddlRecipient.SelectedItem.Text);
                    PerformTransaction(TransactionTypeValues.Transfer, notes,
                        double.Parse(txtAmount.Text), (int)Session["AccountId"]);
                }

            }
            catch (ArgumentOutOfRangeException)
            {
                SetError("Insufficient funds to perform transaction.");
            }
            catch (NoNullAllowedException)
            {
                SetError("Must choose an amount.");
            }
            catch
            {
                SetError("An unknown error occurred while performing transaction.");
            }
        }

        /// <summary>
        /// Redirects to the Account Page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbwfAccount_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/wfAccount.aspx");
        }

        /// <summary>
        /// Updates the Recipients drop down when a Transaction Type is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTransactionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTransactionType.SelectedItem.Text.Equals("Bill Payment"))
            {
                BindRecipients(TransactionTypeValues.BillPayment);
            }
            else if (ddlTransactionType.SelectedItem.Text.Equals("Transfer")) 
            {
                BindRecipients(TransactionTypeValues.Transfer);
            }
            else
            {
                ResetDropDownBinding(this.ddlRecipient);
            }
        }
    }
}