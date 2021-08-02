using Microsoft.AspNetCore.Mvc;
using PaymentPortal.ErrorDict;
using PaymentPortal.Models;
using PaymentPortal.Services;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentPortal.Controllers
{
    
    public class PaymentPortal : Controller
    {
        private readonly IPortalService portalService;
        private string errorMessage;
        private JsonObject responseBody = new JsonObject();
        public PaymentPortal(IPortalService portservice)
        {
            this.portalService = portservice;
        }

        [Route("/customerList")]
        [HttpGet]// get Customers List 
        public JsonObject GetAllCustomers()
        {
            var res = portalService.GetCustomerList();
            responseBody.Add("Output", res);
            return responseBody;
        }

        [Route("/invoiceList")]
        [HttpGet]// get List of Invoices for a customer
        public JsonObject GetAllInvoicesForCustomer([FromQuery] int ssn)
        {
            var res = portalService.GetInvoiceListfortheCustomer(ssn, out string message);
            errorMessage = message;            
            if (!string.IsNullOrEmpty(errorMessage))
            {
                {
                    responseBody.Add("Error", message);
                    responseBody.Add("StatusCode", "BadInput");
                    return responseBody;
                }                
            }
            var balance = portalService.GetBalance(ssn);
            responseBody.Add("Output", res);
            responseBody.Add("Remaining Balance", balance);
            return responseBody;
        }

        [Route("/customerBalanceAfter30DaysOfInvoice")]
        [HttpGet]// get Customers List 
        public JsonObject GetCustomerBalance([FromQuery] int ssn)
        {
            Dictionary<int,int> customerDict = new Dictionary<int, int>();
            try
            {
                customerDict = portalService.GetCustomerBalance(ssn, out string errMessage);
                errorMessage = errMessage;
                if (!string.IsNullOrEmpty(errorMessage))
                {

                    responseBody.Add("Error", errorMessage);
                    responseBody.Add("StatusCode", "BadInput");
                    return responseBody;
                }
            }
            catch (Exception ex)
            {
                responseBody.Add("Exception !", ex.Message);
                responseBody.Add("StackTrace ", ex.TargetSite + " " + ex.StackTrace);
                return responseBody;
            }
            responseBody.Add("Success", "Fetched the remaining balance for <" + ssn + "> each Invoice successfully");
            responseBody.Add("StatusCode", "Success");
            responseBody.Add("Output ", "Balance with Each invoice ");
            if(customerDict.Count==0)
            {
                responseBody.Add("Results", ErrorClass._errorDict["NoInvoice"]);
                return responseBody;
            }
            foreach(var item in customerDict)
            {
                responseBody.Add(" "+ item.Key, " : " + item.Value);
            }
            return responseBody;
        }

        [Route("/getBalance")]
        [HttpGet]// get Total balance for particular SSN 
        public JsonObject GetBalance([FromQuery] int ssn)
        {
            int outputBalance = 0;
            try
            {
                outputBalance = portalService.GetBalance(ssn, out string errMessage);
                errorMessage = errMessage;
                if (!string.IsNullOrEmpty(errorMessage))
                {

                    responseBody.Add("Error", errorMessage);
                    responseBody.Add("StatusCode", "BadInput");
                    return responseBody;
                }
            }
            catch (Exception ex)
            {
                responseBody.Add("Exception !", ex.Message);
                responseBody.Add("StackTrace ", ex.TargetSite + " " + ex.StackTrace);
                return responseBody;
            }
            responseBody.Add("Success", "Fetched the remaining balance for <" + ssn + "> successfully");
            responseBody.Add("StatusCode", "Success");
            responseBody.Add("Output ", outputBalance);
            return responseBody;
        }

        [Route("/createcustomer")]
        [HttpPost]// create new Customer into Portal 
        public JsonObject CreateCustomerIntoPortal([FromBody] Customer customerModel)
        {
            try
            {
                portalService.CreateCustomer(customerModel, out string error);
                errorMessage = error;
                if (!string.IsNullOrEmpty(errorMessage))
                {

                    responseBody.Add("Error", errorMessage);
                    responseBody.Add("StatusCode", "BadInput");
                    return responseBody;
                }
            }
            catch(Exception ex)
            {
                responseBody.Add("Exception !", ex.Message);
                responseBody.Add("StackTrace ", ex.TargetSite+" "+ex.StackTrace);
                return responseBody;
            }
             responseBody.Add("Success", ErrorClass._errorDict["CusSuccess"]);
             responseBody.Add("StatusCode", "Success");
             return responseBody;
        }

        [Route("/createInvoice")]
        [HttpPost]//create invoice for the customer
        public JsonObject CreateCustomerInvoice([FromBody] PaymentInvoice payInvoice)
        {
            try 
            {
                //set invoice creation date  
                if ((payInvoice.CreationDate - DateTime.Now).TotalDays > 0)
                {
                    payInvoice.CreationDate = DateTime.Now;
                }
                portalService.CreateCustomerInvoice(payInvoice,out string error);
                errorMessage = error;
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    responseBody.Add("Error", errorMessage);
                    responseBody.Add("StatusCode", "BadInput");
                    return responseBody;
                }
            }
            catch(Exception ex)
            {
                responseBody.Add("Exception !", ex.Message);
                responseBody.Add("StatusCode ", ex.TargetSite+" "+ex.StackTrace);
                return responseBody;
            }
            responseBody.Add("Success", ErrorClass._errorDict["InvoiceSuccess"]);
            return responseBody;
           
        }

        [HttpDelete]// delete the 
        public IActionResult DeleteCustomerFromPortal([FromBody] int delCustomer )
        {
           portalService.DeleteCustomer(delCustomer);
            return Ok();
        }
        [HttpPut] //Update
        public IActionResult UpdateCustomerPaymentDetails()
        {
            return Ok();
        }


        //input are three parameters 
        //and value amount against that invoice number
        [Route("/CreatePaymentForParticularInvoices")]
        [HttpPut]
        public JsonObject CreatePaymentForParticularInvoices( [FromForm] int ssn, [FromForm] int invoice, [FromForm] int amount)
        {
            //create partial payments or full payment for particular invoice either he can make full payment for that invoice
            // or partial only for that invoice
            try
            {
                portalService.MakeInvoicePayment(ssn, invoice, amount, out string errMessage);
                errorMessage = errMessage;
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    responseBody.Add("Error", errorMessage);
                    responseBody.Add("Status", "BadInput");
                    return responseBody;
                }
            }
            catch (Exception ex)
            {
                responseBody.Add("Exception !", ex.Message);
                responseBody.Add("StatusMessage ", ex.TargetSite + " " + ex.StackTrace);
                return responseBody;
            }
            responseBody.Add("Success", "Payment recieved successfully for invoice <"+invoice+"> for Customer with SSN<"+ssn+">");
            return responseBody;
        }
        //input is tuple item1 is ssn - item2 is amount against total due for this customer
        [Route("/CreateTotalPaymentForAllInvoices")]
        [HttpPut]
        public JsonObject CreateTotalPaymentForAllInvoices([FromBody] Tuple<int, int> tp)
        {
            try
            {
                portalService.MakeFullPayment(tp, out string errMessage);
                errorMessage = errMessage;
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    responseBody.Add("Error", errorMessage);
                    responseBody.Add("Status", "BadInput");
                    return responseBody;
                }
            }
            catch (Exception ex)
            {
                responseBody.Add("Exception !", ex.Message);
                responseBody.Add("StatusMessage ", ex.TargetSite + " " + ex.StackTrace);
                return responseBody;
            }
            responseBody.Add("Success", "Payment recieved successfully for invoice <" + tp.Item2 + "> for Customer with SSN<" + tp.Item1 + ">");
            return responseBody;
        }
    }
}
