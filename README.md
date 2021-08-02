# PaymentPortal

This Api contains calls to create customers, create payments, create invoice , get customerList, get invoicelist, getbalance and delete customer 

Using SSN (6 digit number as unique identifier while creating customer)

Deployed to AWS Lambda as a serverless Web APi  -find the URL - for the server AWS lambda- https://qnazema1wi.execute-api.us-east-1.amazonaws.com/Prod/

1.) https://qnazema1wi.execute-api.us-east-1.amazonaws.com/Prod/customerList
    GetAllCustomers()
    This call returns all customers present in the system


2.) https://qnazema1wi.execute-api.us-east-1.amazonaws.com/Prod/invoiceList
    GetAllInvoicesForCustomer([FromQuery] int ssn)
    This Call returns all Invoices for a particular Customer - provide SSN(6 digit number). Please provide ssn as query parameter
    
3.) https://qnazema1wi.execute-api.us-east-1.amazonaws.com/Prod/customerBalanceAfter30DaysOfInvoice
  GetCustomerBalance([FromQuery] int ssn)
  This call gives list of Customer balances which are more than 30 days old, for a particular customer . Please provide ssn as as query parameter

4.) https://qnazema1wi.execute-api.us-east-1.amazonaws.com/Prod/getBalance

  GetBalance([FromQuery] int ssn)
  This call gives total balance due for particular customer(given SSN). Please provide ssn as a query parameter
  
5.) https://qnazema1wi.execute-api.us-east-1.amazonaws.com/Prod/createcustomer
    CreateCustomerIntoPortal([FromBody] Customer customerModel)
    This call create customer into Portal, Customer datastructure is as follows:
    
    {
    "SSN":123453,
    "FName":"Devender",
    "LName":"Chhillar",
    "CreditCardAmountDue":0,    
    "Address":"2200 SW 34th St",
    "Email":"test1@gmail.com",
    "Phone":1234322899
}

6.) https://qnazema1wi.execute-api.us-east-1.amazonaws.com/Prod/createInvoice
    CreateCustomerInvoice([FromBody] PaymentInvoice payInvoice)
    This call creates invoice for the customer with given SSN, also invoice numbers should be unique means all invoice numbers for the customer should be unique and even for different customers.
    Invoice datastructure is as follows:
    {
    "SSN":123451,    
    "CreationDate":"2020-04-29",
    "invoiceNum":6,
    "prodType":0,
    "productName":"Tes4",
    "quantity":1,
    "amount":250,
    "description":"Chec4"
}

7.) https://qnazema1wi.execute-api.us-east-1.amazonaws.com/Prod/CreatePaymentForParticularInvoices
    CreatePaymentForParticularInvoices( [FromForm] int ssn, [FromForm] int invoice, [FromForm] int amount)
    This call takes 3 parameters Customer SSN, invoice number (against which we want to make payment) and amount.
    create partial payments or full payment for particular invoice
    
8.) https://qnazema1wi.execute-api.us-east-1.amazonaws.com/Prod/CreateTotalPaymentForAllInvoices

CreateTotalPaymentForAllInvoices([FromBody] Tuple<int, int> tp)
This call takes tuple as input where item1 is Customer SSN and item2 is amount which wev are creating payment.
In this call we are making payment against a particular Customer(which may contains many invoices).
Example - Customer A has 4 invoices with balances A $50,B $60,C $70,D $30

Now if we make this call with amount $210 then it will clear all invoice balances and make 0. 
if we call with amount $250 - it will give error that amount is greater than invoice,
if we call with amount $150 - it will clear whatever invoices it can in increasing order. After the call scenario will be means A -$0, B-$0, C-$60, D-$0


Output responses are self explainatory for any error.
    

