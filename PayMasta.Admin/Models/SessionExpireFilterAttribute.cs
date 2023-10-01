using PayMasta.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PayMasta.Admin.Models
{
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {
        private ICommonService _commonService;
        public SessionExpireFilterAttribute()
        {
            _commonService = new CommonService();
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext httpContext = HttpContext.Current;
            CustomPrincipal customPrincipal = (CustomPrincipal)httpContext.User;
            var IsValidToken = _commonService.IsSessionValid(customPrincipal.Token.ToString());
            if (IsValidToken)
            {
                if (httpContext.User == null)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Landing", action = "Index" }));
                    return;
                }
                if (httpContext.User.Identity.IsAuthenticated == false)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Landing", action = "Index" }));
                    return;
                }

                base.OnActionExecuting(filterContext);
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Landing", action = "Index" }));
                return;
            }
        }
    }
}