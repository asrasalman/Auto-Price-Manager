using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;

/// <summary>
/// Summary description for StateHelper
/// </summary>
public class StateHelper
{
    public static Dictionary<string,string> States
    {
        get{
            Dictionary<string, string> states = new Dictionary<string, string>();
            states.Add("victoria", "VIC");
            states.Add("new south wales", "NSW");
            states.Add("queensland", "QLD");
            states.Add("south australia", "SA");
            states.Add("tasmania", "TAS");
            states.Add("western australia", "WA");
            states.Add("australian capital territory", "ACT");
            states.Add("northern territory", "NT");
            states.Add("SK", "SK");
            return states;
        }
    }


    public StateHelper()
    {

    }
}