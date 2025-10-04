using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LacToMinh.SachOnline
{
  public class MvcApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      AreaRegistration.RegisterAllAreas();
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);

      // Đọc file lưu lượt truy cập
      System.IO.StreamReader stReader = new System.IO.StreamReader(HttpContext.Current.Server.MapPath("~/LuotTruyCap.txt"));
      string s = stReader.ReadLine();
      stReader.Close();

      Application.Add("HitCounter", s);
      //Application["HitCounter"] = 0;   // giữ nguyên số trong file, không reset về 0
      Application["Online"] = 0;
    }

    void Session_Start(object sender, EventArgs e)
    {
      Application.Lock();
      Application["Online"] = int.Parse(Application["Online"].ToString()) + 1;
      Application["HitCounter"] = int.Parse(Application["HitCounter"].ToString()) + 1;
      Application.UnLock();

      // Ghi lại lượt truy cập mới vào file
      System.IO.StreamWriter stWriter = new System.IO.StreamWriter(HttpContext.Current.Server.MapPath("~/LuotTruyCap.txt"));
      stWriter.Write(Application["HitCounter"]);
      stWriter.Close();
    }

    void Session_End(object sender, EventArgs e)
    {
      Application.Lock();
      Application["Online"] = int.Parse(Application["Online"].ToString()) - 1;
      Application.UnLock();
    }
  }
}
