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
    public partial class wfClient : System.Web.UI.Page
    {
        /// <summary>
        /// Sets the text ot the error message and sets visibility to true.
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
            int clientNumber = 0;

            try
            {
                // Get the currently logged in user's client number
                clientNumber = int.Parse(Utilities.ExtractClientNumberFromUserName(Page.User.Identity.Name));

                try
                {
                    // Get the current user's information from the database
                    Client currentClient = db.Clients.Where(value => value.ClientNumber == clientNumber).Single<Client>();
                    // Set the client number and name to session variables.
                    Session["ClientId"] = currentClient.ClientId;
                    Session["ClientFullName"] = currentClient.FullName;

                    // Use the Client's ID to get all their accounts.
                    IQueryable<BankAccount> clientAccounts = db.BankAccounts.Where(value => value.ClientId == currentClient.ClientId);
                    // Why doesn't this work?  IQueryable<BankAccount> clientAccounts = db.BankAccounts.Where(value => value.ClientId == (int)Session["ClientId"]);
                    // Bind accounts to the grid view.
                    gvClientAccounts.DataSource = clientAccounts.ToList();

                    // Set the Client's name to the label.
                    lblClientName.Text = Session["ClientFullName"].ToString();
                }
                catch (ArgumentNullException)
                {
                    SetErrorLabel("No client found with your client number, please check your user name.");
                }
                catch (InvalidOperationException)
                {
                    SetErrorLabel("Error loading your information.");
                }
                catch
                {
                    SetErrorLabel("Error occurred when loading accounts.");
                }
            }
            catch
            {
                SetErrorLabel("Invalid user name.");
            }
            

            
            this.DataBind();
        }

        /// <summary>
        /// Performs required operations on page load or post back.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // perform all actions that should only happen on page load.
            if (this.IsPostBack == false)
            {
                // Reset error label.
                lblErrorMessage.Visible = false;
                lblErrorMessage.Text = "";

                // redirects to login if not logged in.
                if (!this.Page.User.Identity.IsAuthenticated)
                {
                    Response.Redirect("~/Account/Login.aspx");
                }
                else
                {
                    BindControls();
                }             
            }
        }

        /// <summary>
        /// Handles the gvClientAccounts Selected Index Changed event, Directs user to selected bank account.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvClientAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            BankOfBIT_JGContext db = new BankOfBIT_JGContext();
            // Get the account number
            long accountNumber = long.Parse(gvClientAccounts.SelectedRow.Cells[1].Text);
            Session["AccountNumber"] = accountNumber;
            Session["Balance"] = db.BankAccounts.Where(x => x.AccountNumber == accountNumber).Single<BankAccount>().Balance;        
            Session["AccountId"] = db.BankAccounts.Where( x => x.AccountNumber == accountNumber).Single<BankAccount>().BankAccountId;
            
            // direct to the Account page
            Response.Redirect("~/wfAccount");
        }
    }
}