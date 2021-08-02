using PaymentPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentPortal.Helper
{
   public interface IInputValidationAndFormatting
    {
        bool CharacterLengthInvalid(int ssn);
        bool IsValidSSN(int ssn);
        bool IsSSNCreated(int ssn);
        bool IsNameNotEmpty(string name);
        bool IsValidInvoiceCreationDate(DateTime ds);
        bool IsAmountDueValid(int amount, int limit);
        bool IsProductNameNotEmpty(string pName);
       // bool IsProductAmountValid(PaymentInvoice payIn);
        bool IsQuantityGreaterThanZero(int quantity);
        bool IsCustomerExists(int ssn);
        bool IsInvoiceAlreadyCreated(int invoiceNumber);
        bool IsAmountValid(int amount);
        bool IsInvoiceForPaymentExists(int invoice);
        bool IsEmailExists(string email);
        bool IsPhoneNumberExists(int phone);
        bool IsValidEmail(string email);
        bool IsValidPhoneNumber(int phone);

    }
}
