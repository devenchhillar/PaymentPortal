using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentPortal.ErrorDict
{
    public class ErrorClass
    {
        public static Dictionary<string, string> _errorDict = new Dictionary<string, string>() {
            { "SSN","Error! SSN length should be 6 number length" },
            {"FirstName","Error! Name is empty" },
            {"ValidEmail","Error! Please provide valid email " },
            {"ValidNumber","Error! Please provide valid 10 digit PhoneNumber, without any special characters" },
            {"EmailExists","Error! Please provide another Email, it already exists"},
            { "PhoneExists","Error! Please provide another Phone, it already exists"},
            {"SSNExists","Error! Customer with same SSN already exists" },
            {"CustomerNotPresentSSN","Error! For the provided SSN customer doesn't exists, so cann't create invoice" },
            {"InvoicePresent","Error! Already exists, Provide a different invoice number"},
            {"ProductName","Error! ProductName is empty " },
            {"CreatedDate","Error! Invalid CreationDate, Format should be MM/DD/YY and should not be in future " },
            { "Quantity","Error! Product Quantity should be greater than zero"},
            {"NoCus","Error! Required Customer doesn't exists in DB " },
            {"NoSSNCreated","Error! Customer doesn't exists with the given SSN " },
            { "IncorrectInvoice","Error! Invoice number is incorrect , make payment against correct invoice "},
            {"AmountValid","Error! Payment amount should be greater than 0" },
            {"GreaterAmount","Amount provided is greater than required for the invoice" },
            {"InvoiceSuccess","Invoice Created successfully" },
            {"CusSuccess","Customer Created successfully" },
            {"NoInvoice","No invoice exists with balance more than 30 days" }
        };
    }
}
