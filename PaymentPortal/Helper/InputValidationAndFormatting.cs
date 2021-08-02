using PaymentPortal.DictionaryDB;
using PaymentPortal.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PaymentPortal.Helper
{
    public class InputValidationAndFormatting: IInputValidationAndFormatting
    {       

        public bool CharacterLengthInvalid(int ssn)
        {
            if(ssn.ToString().Length<6|| ssn.ToString().Length >6)
            {
                return false;
            }
            return true;

        }        
       // private bool ContainsOnlyIntegers(int ssn)
       // {
             
       // }
        //check for valid key to put in dictionary

        //check if 10 characters long and all are numbers
        public bool IsValidSSN(int ssn)
        {
            if (!CharacterLengthInvalid(ssn))
            {
                 return false;
            }
            return true;
           
        }
        public bool IsSSNCreated(int ssn)
        {
            if (!DataDict._dictCustomer.ContainsKey(ssn))
            {
                return false;
            }
            return true;

        }

        public bool IsNameNotEmpty(string name)
        {
            if(string.IsNullOrEmpty(name) ||string.IsNullOrWhiteSpace(name))
            {
                return false;
            }
            return true;
        }
        public bool IsProductNameNotEmpty(string pName)
        {
            if (string.IsNullOrEmpty(pName) || string.IsNullOrWhiteSpace(pName))
            {
                return false;
            }
            return true;
        }
        public bool IsValidInvoiceCreationDate(DateTime ds)
        {
            if ((ds - DateTime.Now).TotalDays > 0)
                return false;
            return true;
        }
        public bool IsAmountDueValid(int amount,int limit)
        {
            if (amount > limit)
                return false;
            return true;
        }
        //public bool IsProductAmountValid(PaymentInvoice payInvoice)
        //{            
        //   // if (DataDict._balanceTable[payInvoice.SSN] < payInvoice.Amount)
        //      //  return false;
        //   // return true;
           
        //}
        public bool IsAmountValid(int amount)
        {
            if (amount <= 0)
                return false;
            return true;
        }
        public bool IsQuantityGreaterThanZero(int quantity)
        {
            if (quantity <= 0)
                return false ;

            return true;

        }
       public bool IsCustomerExists(int ssn)
        {
            if (!DataDict._dictCustomer.ContainsKey(ssn))
                return false;
            return true;

        }
        public bool IsInvoiceAlreadyCreated(int invoiceNumber)
        {
            if(DataDict._invoiceNumbersWithBalance.ContainsKey(invoiceNumber))
            {
                return false;
            }
            return true;
        }
        public bool IsInvoiceForPaymentExists(int inNumber)
        {
            if(!DataDict._invoiceNumbersWithBalance.ContainsKey(inNumber))
            {
                return false;
            }
            return true;
        }
        public bool IsEmailExists(string email)
        {
            if(DataDict._emailSet.Contains(email))
            {
                return false;
            }
            return true;
        }
        public bool IsPhoneNumberExists(int phone)
        {
            if(!IsValidPhoneNumber(phone))
            { 
                return false;
            }
            if (DataDict._phoneSet.Contains(phone))
            {
                return false;
            }
            return true;
        }
        public bool IsValidEmail(string email)
        {            
                if (string.IsNullOrWhiteSpace(email))
                    return false;
                try
                {
                    // Normalize the domain
                    email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                          RegexOptions.None, TimeSpan.FromMilliseconds(200));

                    // Examines the domain part of the email and normalizes it.
                    string DomainMapper(Match match)
                    {
                        // Use IdnMapping class to convert Unicode domain names.
                        var idn = new IdnMapping();

                        // Pull out and process domain name (throws ArgumentException on invalid)
                        string domainName = idn.GetAscii(match.Groups[2].Value);

                        return match.Groups[1].Value + domainName;
                    }
                }
                catch (RegexMatchTimeoutException e)
                {
                    return false;
                }
                catch (ArgumentException e)
                {
                    return false;
                }

                try
                {
                    return Regex.IsMatch(email,
                        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                        RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
                }
                catch (RegexMatchTimeoutException)
                {
                    return false;
                }            

        }
        public bool IsValidPhoneNumber(int phone)
        {
            var phoneString = phone.ToString();
            if (phoneString[0] == '0'||phoneString.Length<10)
                return false;
            var count = 0;
            for(int i=0;i<phoneString.Length;i++)
                if (char.IsNumber(phoneString, i)){
                    count++;
                }
            if (count != 10||count>10) return false;
            return true;

        }

    }
  
}
