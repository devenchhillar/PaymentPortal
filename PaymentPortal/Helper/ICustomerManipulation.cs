using PaymentPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentPortal.Helper
{
    public interface ICustomerManipulation
    {
        void GetCustomerList( out HashSet<Customer> customerHashSet);
        void GetInvoiceList(int ssn, out HashSet<PaymentInvoice> invoiceHashSet);
        List<int> GetSpecificInvoiceList(int ssn,bool daysProvided=false);
        Dictionary<int, int> GetBalanceListForCustomer(List<int> invoiceList);
        void UpdateInvoiceBalanceToZero(List<int> invoice);
        void UpdateInvoiceBalance(List<int> invoice,int amount);


    }
}
