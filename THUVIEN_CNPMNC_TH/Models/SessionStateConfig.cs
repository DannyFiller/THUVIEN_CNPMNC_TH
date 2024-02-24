using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace THUVIEN_CNPMNC_TH.Models
{
    public class SessionStateConfig
    {
        public static void RegisterSession()
        {

            HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);

        }
    }
}