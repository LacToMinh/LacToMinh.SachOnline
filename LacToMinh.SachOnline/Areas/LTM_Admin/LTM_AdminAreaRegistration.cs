using System.Web.Mvc;

namespace LacToMinh.SachOnline.Areas.LTM_Admin
{
    public class LTM_AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "LTM_Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "LTM_Admin_default",
                "LTM_Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}