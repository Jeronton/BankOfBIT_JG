using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class Utilities
    {
        /// <summary>
        /// Returns the sub-string of the full string that precedes the end string.
        /// </summary>
        /// <param name="fullString">The string to be processed.</param>
        /// <param name="endString">The string at the end of the full string that is to be omitted.</param>
        /// <returns>A sub-string omitting the endString.</returns>
        static public String GetPriorString(string fullString, string endString)
        {
            return fullString.Substring(0, fullString.IndexOf(endString));
        }

        /// <summary>
        /// Extracts the Client number from the user name.
        /// </summary>
        /// <param name="userName">The full user name to be processed.</param>
        /// <returns>The Client number from the given user name.</returns>
        static public string ExtractClientNumberFromUserName(string userName)
        {
            string clientNumber = userName.Split('@')[0];
            return clientNumber;
        }
    }
}
