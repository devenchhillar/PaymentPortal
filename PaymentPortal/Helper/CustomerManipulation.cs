using PaymentPortal.DictionaryDB;
using PaymentPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentPortal.Helper
{
    public class CustomerManipulation: ICustomerManipulation
    {

        private int DaysData { get; set; } = 30;
        public void GetCustomerList(out HashSet<Customer> customerHashSet)
        {
           
                customerHashSet = new HashSet<Customer>();
            
            foreach(var dtItem in DataDict._dictCustomer)
            {
                customerHashSet.Add(dtItem.Value);
            }
        }

        public void GetInvoiceList(int ssn, out HashSet<PaymentInvoice> invoiceHashSet)
        {

            invoiceHashSet = new HashSet<PaymentInvoice>();
            if (DataDict._dictInvoice.ContainsKey(ssn))
            {
                foreach(var item in DataDict._dictInvoice[ssn])
                {
                    invoiceHashSet.Add(item);
                }
            }
            
        }

        public List<int> GetSpecificInvoiceList(int ssn, bool daysProvided = false)
        {
            List<int> invoiceList = new List<int>();
            if(!daysProvided)
            {
                DaysData = 0;
            }

            if (DataDict._dictInvoice.ContainsKey(ssn))
            {
                foreach (var item in DataDict._dictInvoice[ssn])
                {
                        var days = (DateTime.Now - item.CreationDate).TotalDays;
                        if (days >= DaysData)
                            invoiceList.Add(item.InvoiceNum);                        
                }
            }
            return invoiceList;
        }


        public Dictionary<int,int> GetBalanceListForCustomer(List<int> invoiceList)
        {
            Dictionary<int, int> cusBalanceList = new Dictionary<int, int>();
            foreach(var item in invoiceList)
            {
                cusBalanceList.Add(item, DataDict._invoiceNumbersWithBalance[item]);
            }

            return cusBalanceList;
        }
        public void UpdateInvoiceBalanceToZero(List<int> invoiceList)
        {
            foreach(var item in invoiceList)
            {
                DataDict._invoiceNumbersWithBalance[item] = 0;
            }
        }
        public void UpdateInvoiceBalance(List<int> invoiceList,int amount)
        {
            foreach(var item in invoiceList)
            {
                if (amount <= 0) break;

                if(amount<=DataDict._invoiceNumbersWithBalance[item])
                {                   
                    DataDict._invoiceNumbersWithBalance[item] -= amount;
                    amount = 0;
                }
                else
                {
                    var temp = DataDict._invoiceNumbersWithBalance[item];
                    DataDict._invoiceNumbersWithBalance[item] = 0;
                    amount -= temp;

                }

            }
        }


    }
}
