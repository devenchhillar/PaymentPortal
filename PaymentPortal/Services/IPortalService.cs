using PaymentPortal.Models;
using System;
using System.Collections.Generic;

namespace PaymentPortal.Services
{
    public interface IPortalService
    {
        HashSet<Customer> GetCustomerList();
        HashSet<PaymentInvoice> GetInvoiceListfortheCustomer(int ssn,out string errMessage);
        Customer GetCustomerTotalBalance(int ssn,out string errorMessage);
        void CreateCustomer(Customer cs,out string error);
        void DeleteCustomer(int ssn);
        void CreateCustomerInvoice(PaymentInvoice payInvoice,out string invoiceErrorDetails);
        int GetBalance(int ssn);
        int GetBalance(int ssn,out string errMessage);
        Dictionary<int, int> GetCustomerBalance(int ssn, out string errMessage);
        void MakeInvoicePayment(int ssn, int invoice, int amount, out string errMessage);
        void MakeFullPayment(Tuple<int, int> tp, out string errMessage);
    }
}