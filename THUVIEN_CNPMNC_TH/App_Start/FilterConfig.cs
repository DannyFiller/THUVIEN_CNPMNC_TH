using System.Web;
using System.Web.Mvc;

namespace THUVIEN_CNPMNC_TH
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
