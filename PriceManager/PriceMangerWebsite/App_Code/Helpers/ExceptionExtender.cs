using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ExceptionExtender
/// </summary>
public class InvalidEbayCredentialsException : Exception
{
    public string Message = "Invalid Auth Token or Credentials to connect to Ebay";
    public InvalidEbayCredentialsException()
	{
        
	}
}