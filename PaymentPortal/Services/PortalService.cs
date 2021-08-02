using Microsoft.AspNetCore.Mvc;
using PaymentPortal.Helper;
using PaymentPortal.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentPortal.DictionaryDB;
using PaymentPortal.ErrorDict;

namespace PaymentPortal.Services
{
    public class PortalService:IPortalService
    {
       
        private StringBuilder sbError = new StringBuilder();
        private ICustomerManipulation custManipulation;
        private IInputValidationAndFormatting validate;
        
        public PortalService(IInputValidationAndFormatting validate, ICustomerManipulation customManipul)
        {
            this.validate = validate;
            this.custManipulation = customManipul;
           
    }
        public HashSet<Customer> GetCustomerList()
        {
            try
            {
                custManipulation.GetCustomerList(out HashSet<Customer> customer);
                DataDict._customerSet = customer;
            }catch(Exception ex)
            {
                throw new Exception();
            }
            return DataDict._customerSet;
        }

        public HashSet<PaymentInvoice> GetInvoiceListfortheCustomer(int ssn,out string errMessage)
        {
            errMessage = string.Empty;
            try
            {
                if (!validate.IsValidSSN(ssn))
                {
                    sbError.AppendLine(ErrorClass._errorDict["SSN"]);
                    sbError.AppendLine();
                }
                if (!DataDict._dictCustomer.ContainsKey(ssn))
                {
                    sbError.AppendLine(ErrorClass._errorDict["NoCus"]);
                    sbError.AppendLine();
                }
                if (string.IsNullOrEmpty(sbError.ToString()))
                {
                    custManipulation.GetInvoiceList(ssn, out HashSet<PaymentInvoice> inList);
                    DataDict._customerInvoiceSet = inList;
                }
                else
                {
                    errMessage = sbError.ToString();
                    sbError.Clear();
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message + " " + sbError.ToString();
                sbError.Clear();
            }           
            return DataDict._customerInvoiceSet;
        }
        public void CreateCustomer(Customer cs,out string errorMessage)
        {
            errorMessage = string.Empty;
            
            //validate before inserting in dictionary
            if (!validate.IsValidSSN(cs.SSN))
            {
                sbError.AppendLine(ErrorClass._errorDict["SSN"]);
                sbError.AppendLine();
            }
            if(!validate.IsNameNotEmpty(cs.FName))
            {
                sbError.AppendLine(ErrorClass._errorDict["FirstName"]);
                sbError.AppendLine();

            }  
            if(!validate.IsValidEmail(cs.Email))
            {
                sbError.AppendLine(ErrorClass._errorDict["ValidEmail"]);
                sbError.AppendLine();
            }
            if(!validate.IsValidPhoneNumber(cs.Phone))
            {
                sbError.AppendLine(ErrorClass._errorDict["ValidNumber"]);
                sbError.AppendLine();
            }
            if(!validate.IsEmailExists(cs.Email))
            {
                sbError.AppendLine(ErrorClass._errorDict["EmailExists"]);
                sbError.AppendLine();
            }
            if (!validate.IsPhoneNumberExists(cs.Phone))
            {
                sbError.AppendLine(ErrorClass._errorDict["PhoneExists"]);
                sbError.AppendLine();
            }
            //if(!validate.IsAmountDueValid(cs.CreditCardAmountDue,cs.CreditCardLimit))
            //{
            //    sbError.AppendLine("Error! Initially due amount should be zero not greater than Card limit ");
            //    sbError.AppendLine();
            //}
            try
            {
                if (string.IsNullOrEmpty(sbError.ToString()))
                {
                    if (!DataDict._dictCustomer.ContainsKey(cs.SSN))
                    {
                        DataDict._dictCustomer.Add(cs.SSN, cs);
                       // DataDict._balanceTable.Add(cs.SSN, cs.CreditCardLimit);
                        DataDict._balanceDueTable.Add(cs.SSN, 0);
                        DataDict._emailSet.Add(cs.Email);
                        DataDict._phoneSet.Add(cs.Phone);
                      
                    }
                    else
                    {
                        sbError.AppendLine(ErrorClass._errorDict["SSNExists"]);
                        sbError.AppendLine();                        
                    }
                }
               
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message+" "+sbError.ToString();
                sbError.Clear();
            }
            errorMessage = sbError.ToString();
            sbError.Clear();
        }
        public void DeleteCustomer(int cusData)
        {
            //format ssn and dateof birth
           if(DataDict._dictCustomer.ContainsKey(cusData))
            {
                DataDict._dictCustomer.Remove(cusData);
            }
        }
        public void CreateCustomerInvoice(PaymentInvoice payInvoice,out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                if (!validate.IsCustomerExists(payInvoice.SSN))
                {
                    sbError.AppendLine(ErrorClass._errorDict["CustomerNotPresentSSN"]);
                    sbError.AppendLine();
                }
                if(!validate.IsInvoiceAlreadyCreated(payInvoice.InvoiceNum))
                {
                    sbError.AppendLine(ErrorClass._errorDict["InvoicePresent"]);
                    sbError.AppendLine();
                }
                if (!validate.IsValidSSN(payInvoice.SSN))
                {
                    sbError.AppendLine(ErrorClass._errorDict["SSN"]);
                    sbError.AppendLine();
                }
                if (!validate.IsProductNameNotEmpty(payInvoice.ProductName))
                {
                    sbError.AppendLine(ErrorClass._errorDict["ProductName"]);
                    sbError.AppendLine();

                }
                if (!validate.IsValidInvoiceCreationDate(payInvoice.CreationDate))
                {
                    sbError.Append(ErrorClass._errorDict["CreatedDate"]);
                    sbError.AppendLine();
                }
                //if (!validate.IsProductAmountValid(payInvoice))
                //{
                //    sbError.AppendLine("Error! Product purchased amount should be less than available on Card remaining Balance");
                //    sbError.AppendLine();
                //}
                if (!validate.IsQuantityGreaterThanZero(payInvoice.Quantity))
                {
                    sbError.AppendLine(ErrorClass._errorDict["Quantity"]);
                    sbError.AppendLine();
                } 
                if (string.IsNullOrEmpty(sbError.ToString()))
                    {
                    if (!DataDict._dictInvoice.ContainsKey(payInvoice.SSN))
                    {
                        DataDict._dictInvoice.Add(payInvoice.SSN, new List<PaymentInvoice>() { payInvoice });                       
                    }
                    else
                    {
                        DataDict._dictInvoice[payInvoice.SSN].Add(payInvoice);
                    }
                    //DataDict._balanceTable[payInvoice.SSN] -= payInvoice.Amount;
                    DataDict._balanceDueTable[payInvoice.SSN] += payInvoice.Amount;
                    DataDict._dictCustomer[payInvoice.SSN].CreditCardAmountDue = DataDict._balanceDueTable[payInvoice.SSN];
                    DataDict._invoiceNumbersWithBalance.Add(payInvoice.InvoiceNum,payInvoice.Amount);
                }
                else
                {
                    errorMessage = sbError.ToString();
                    sbError.Clear();
                }
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message + " " + sbError.ToString();
                sbError.Clear();
            }
        }
        public Customer GetCustomerTotalBalance(int ssn,out string errMessage)
        {
            errMessage = string.Empty;
            try
            {
                if (!validate.IsValidSSN(ssn))
                {
                    sbError.AppendLine(ErrorClass._errorDict["SSN"]);
                    sbError.AppendLine();
                }
                if (!DataDict._dictCustomer.ContainsKey(ssn))
                {
                    sbError.AppendLine(ErrorClass._errorDict["NoCus"]);
                    sbError.AppendLine();
                }
                if (string.IsNullOrEmpty(sbError.ToString()))
                    return DataDict._dictCustomer[ssn];
                else
                {
                    errMessage = sbError.ToString();
                    sbError.Clear();
                }
            }
            catch(Exception ex)
            {
                errMessage = ex.Message + " " + sbError.ToString();
                sbError.Clear();
            }
            return null;
            
        }

        public int GetBalance(int ssn)
        {
            return DataDict._balanceDueTable[ssn];           
        }
        public int GetBalance(int ssn,out string errMessage)
        {
            errMessage = string.Empty;
            if(!validate.IsValidSSN(ssn))
            {
                sbError.AppendLine(ErrorClass._errorDict["SSN"]);
                sbError.AppendLine();
            }
            if (!DataDict._dictCustomer.ContainsKey(ssn))
            {
                sbError.AppendLine(ErrorClass._errorDict["NoCus"]);
                sbError.AppendLine();
            }
            if (string.IsNullOrEmpty(sbError.ToString()))
            {
                return DataDict._balanceDueTable[ssn];
            }
            else
            {
                errMessage = sbError.ToString();
                sbError.Clear();
            }
            return 0;
        }
        public Dictionary<int,int> GetCustomerBalance(int ssn, out string errMessage)
        {
            //get all list of customer balances with 30 days old with invoice number
            errMessage = string.Empty;
            Dictionary<int, int> customerList = new Dictionary<int, int>();
            try
            {
                if (!validate.IsValidSSN(ssn))
                {
                    sbError.AppendLine(ErrorClass._errorDict["SSN"]);
                    sbError.AppendLine();
                }
                if (!DataDict._dictCustomer.ContainsKey(ssn))
                {
                    sbError.AppendLine(ErrorClass._errorDict["NoCus"]);
                    sbError.AppendLine();
                }
                if (string.IsNullOrEmpty(sbError.ToString()))
                {
                    //get all invoices for which balance left is more than 30 days old
                    var invoiceList = custManipulation.GetSpecificInvoiceList(ssn,true);
                    //foreach invoice get the list with balance left
                    customerList = custManipulation.GetBalanceListForCustomer(invoiceList);
                }
                else
                {
                    errMessage = sbError.ToString();
                    sbError.Clear();
                }            
            }
            catch (Exception ex)
            {
                errMessage = ex.Message + " " + sbError.ToString();
                sbError.Clear();
            }
            return customerList;
        }
        public void MakeInvoicePayment(int ssn, int invoice, int amount, out string errMessage)
        {
            errMessage = string.Empty;
            try
            {
                if(!validate.IsSSNCreated(ssn))
                {
                    sbError.AppendLine(ErrorClass._errorDict["NoSSNCreated"]);
                    sbError.AppendLine();
                }
                if (!validate.IsValidSSN(ssn))
                {
                    sbError.AppendLine(ErrorClass._errorDict["SSN"]);
                    sbError.AppendLine();
                }
                if(!validate.IsInvoiceForPaymentExists(invoice))
                {
                    sbError.AppendLine(ErrorClass._errorDict["IncorrectInvoice"]);
                    sbError.AppendLine();
                }
                if(!validate.IsAmountValid(amount))
                {
                    sbError.AppendLine(ErrorClass._errorDict["AmountValid"]);
                    sbError.AppendLine();
                }
                if(string.IsNullOrEmpty(sbError.ToString()))
                {
                    //update invoice table     
                    if (amount <= DataDict._invoiceNumbersWithBalance[invoice])
                    {
                        DataDict._invoiceNumbersWithBalance[invoice] -= amount;
                        //update _dictCustomer table with amount due
                        DataDict._dictCustomer[ssn].CreditCardAmountDue -= amount;
                        DataDict._balanceDueTable[ssn] -= amount;
                    }
                    else
                    {
                        sbError.AppendLine(ErrorClass._errorDict["GreaterAmount"]);
                        sbError.AppendLine();

                    }

                }                
            }
            catch (Exception ex)
            {
                errMessage = ex.Message + " " + sbError.ToString();
                sbError.Clear();
            }
            errMessage = sbError.ToString();
            sbError.Clear();

        }
        public void MakeFullPayment(Tuple<int, int> tp, out string errMessage)
        {
            errMessage = string.Empty;
            try
            {
                var ssn = tp.Item1;
                var amount = tp.Item2;
                if(!validate.IsSSNCreated(ssn))
                {
                    sbError.AppendLine(ErrorClass._errorDict["NoSSNCreated"]);
                    sbError.AppendLine();
                }
                if (!validate.IsValidSSN(ssn))
                {
                    sbError.AppendLine(ErrorClass._errorDict["SSN"]);
                    sbError.AppendLine();
                }
                if (!validate.IsAmountValid(amount))
                {
                    sbError.AppendLine(ErrorClass._errorDict["AmountValid"]);
                    sbError.AppendLine();
                }
                if (string.IsNullOrEmpty(sbError.ToString()))
                {
                    //update invoice table 
                    if (amount == DataDict._dictCustomer[ssn].CreditCardAmountDue)
                    {
                        DataDict._dictCustomer[ssn].CreditCardAmountDue = 0; ;
                        DataDict._balanceDueTable[ssn] = 0;
                        var invoice = custManipulation.GetSpecificInvoiceList(ssn);
                        //make balance value for each invoice to zero in 
                        custManipulation.UpdateInvoiceBalanceToZero(invoice);
                    }
                    else if(amount< DataDict._dictCustomer[ssn].CreditCardAmountDue)
                    {
                        DataDict._dictCustomer[ssn].CreditCardAmountDue -= amount;
                        DataDict._balanceDueTable[ssn] -= amount;
                        var invoice = custManipulation.GetSpecificInvoiceList(ssn);
                        //update the invoice balances until amount =0;
                        custManipulation.UpdateInvoiceBalance(invoice,amount);
                    }
                    else
                    {
                        sbError.AppendLine(ErrorClass._errorDict["GreaterAmount"]);
                        sbError.AppendLine();
                    }
                }               
            }
            catch (Exception ex)
            {
                errMessage = ex.Message + " " + sbError.ToString();
                sbError.Clear();
            }
            errMessage = sbError.ToString();
            sbError.Clear();

        }

    }

    
}
