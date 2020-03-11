using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class BusinessRules
    {
        const int BANK_OF_BIT_NUMBER = 45910;
        const string UNDEFINED = "";

        /// <summary>
        /// returns the BankOfBIT bank identification number
        /// </summary>
        /// <returns>int: bank of bit number</returns>
        public static int BankNumber()
        {
            return BANK_OF_BIT_NUMBER;
        }

        /// <summary>
        /// defines the mask display format for the various account types
        /// </summary>
        /// <param name="accountType">string account type name</param>
        /// <returns>string format</returns>
        public static string AccountFormat(string accountType)
        {
            string[] ACCOUNT_TYPE = { "Savings", "Mortgage", "Investment", "Chequing" };
            string[] ACCOUNT_MASK = { "0-000-0", "000-000", "00-000-00", "000-00-000" };


            //initial format (empty string)
            string format = UNDEFINED;

            //compare account type to predefined types
            for (int i = 0; i < ACCOUNT_TYPE.Length; i++)
            {
                //if a match, return the corresonding mask
                if (accountType.ToLower() == ACCOUNT_TYPE[i].ToLower())
                {
                    format = ACCOUNT_MASK[i];
                    break;
                }
            }
            //return the mask or empty string
            return format;
        }
    }

}
