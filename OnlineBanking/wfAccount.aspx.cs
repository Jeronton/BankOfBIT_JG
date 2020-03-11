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
    public partial class wfAccount : System.Web.UI.Page
    {
        /// <summary>
        /// Sets the text of the error message and sets visibility to true.
        /// </summary>
        /// <param name="message">The message to set to the error label.</param>
        public void SetErrorLabel(string message)
        {
            lblErrorMessage.Visible = true;
            lblErrorMessage.Text = message;
        }

        /// <summary>
        /// Performs all binding for the form.
        /// </summary>
        private void BindControls()
        {
            BankOfBIT_JGContext db = new BankOfBIT_JGContext();

            try
            {
                lblClientName.Text = Session["ClientFullName"].ToString();
                lblBalance.Text = string.Format("{0:C}", Session["Balance"]);
                lblAccountNumber.Text = Session["AccountNumber"].ToString();
                int accountId = (int)Session["AccountId"];
                IEnumerable<Transaction> transactions = db.Transactions.Where(x => x.BankAccountId == accountId );

                gvTransactions.DataSource = transactions.ToList();

                this.DataBind();

            }
            catch (ArgumentNullException)
            {
                SetErrorLabel("No transactions found for this account.");
            }
            catch
            {
                SetErrorLabel("Error occurred when trying to load transactions.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Only run this code on pages load.
            if (this.IsPostBack == false)
            {
                if (!this.Page.User.Identity.IsAuthenticated)
                {
                    Response.Redirect("~/Account/Login.aspx");
                }
                else
                {
                    // Reset error label.
                    lblErrorMessage.Visible = false;
                    lblErrorMessage.Text = "";

                    BindControls();
                }
            }
        }

        /// <summary>
        /// Redirects user the Transaction form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbTransactions_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/wfTransaction.aspx");
        }

        /// <summary>
        /// Redirects user the Client form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbAccountList_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/wfClient.aspx");
        }
    }
}