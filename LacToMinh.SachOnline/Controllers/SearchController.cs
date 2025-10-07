using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LacToMinh.SachOnline.Models;

namespace LacToMinh.SachOnline.Controllers
{
    
    public class SearchController : Controller
    {
        LacToMinh_SachOnlineEntities Minh_db = new LacToMinh_SachOnlineEntities();
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string strSearch)
        {
          ViewBag.Search = strSearch;

          if (!string.IsNullOrEmpty(strSearch))
          {
            var kq = from s in Minh_db.SACH 
                     where s.TenSach.Contains(strSearch)
                        || s.MaCD.ToString() == strSearch
                        || s.MoTa.Contains(strSearch)
                        || s.CHUDE.TenChuDe.Contains(strSearch)
                        || s.NHAXUATBAN.TenNXB.Contains(strSearch)
                     orderby s.SoLuongBan descending, s.NgayCapNhat descending
                     select s;

            return View(kq.ToList());
          }

          return View();
        }
  }
}