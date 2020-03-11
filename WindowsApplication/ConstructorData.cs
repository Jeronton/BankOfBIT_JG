using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankOfBIT_JG.Models;

namespace WindowsApplication
{
    /// <summary>
    /// given:TO BE MODIFIED
    /// this class is used to capture data to be passed
    /// among the windows forms
    /// </summary>
    public class ConstructorData
    {
        /// <summary>
        /// Stores an instance of the current client.
        /// </summary>
        public Client Client { get; set; }

        /// <summary>
        /// Stores and instance of the current bank account.
        /// </summary>
        public BankAccount BankAccount { get; set; }
    }
}
