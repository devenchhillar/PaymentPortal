using PaymentPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentPortal.DictionaryDB
{
    public static class DataDict
    {
        public static readonly Dictionary<int, Customer> _dictCustomer = new Dictionary<int, Customer>();
        public static readonly Dictionary<int, List<PaymentInvoice>> _dictInvoice = new Dictionary<int, List<PaymentInvoice>>();
        //public static readonly Dictionary<int, int> _balanceTable = new Dictionary<int, int>();
        public static readonly Dictionary<int, int> _balanceDueTable = new Dictionary<int, int>();
        public static readonly Dictionary<int,int> _invoiceNumbersWithBalance = new Dictionary<int,int>();
        public static readonly HashSet<string> _emailSet = new HashSet<string>();
        public static readonly HashSet<int> _phoneSet = new HashSet<int>();
        public static HashSet<Customer> _customerSet = new HashSet<Customer>();
        public static HashSet<PaymentInvoice> _customerInvoiceSet = new HashSet<PaymentInvoice>();
    }
}
