using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentPortal.Models
{
    public enum ProductType{        
        service,
        product,
        loan
    }
    public class PaymentInvoice
    {
        public int SSN { get; set; }
        public DateTime CreationDate { get; set; }
        public int InvoiceNum { get; set; }
        public ProductType ProdType;
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
       
        
    }
}
