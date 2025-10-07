using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LacToMinh.SachOnline.Models;
using PagedList;

namespace LacToMinh.SachOnline.Controllers
{

  public class LacToMinh_SearchController : Controller
  {
    LacToMinh_SachOnlineEntities Minh_db = new LacToMinh_SachOnlineEntities();
    // GET: Search
    public ActionResult Index()
    {
      return View();
    }

    public ActionResult Search(string strSearch = null, int maCD = 0, int page = 1, int pageSize = 5)
    {
      ViewBag.Search = strSearch;
      ViewBag.MaCD = new SelectList(Minh_db.CHUDE, "MaCD", "TenChuDe");

      var kq = Minh_db.SACH.Include("CHUDE").Include("NHAXUATBAN").ToList();

      if (!string.IsNullOrEmpty(strSearch))
      {
        string keyword = strSearch.ToLower();
        kq = kq.Where(s =>
            (s.TenSach ?? "").ToLower().Contains(keyword) ||
            (s.MoTa ?? "").ToLower().Contains(keyword) ||
            (s.CHUDE.TenChuDe ?? "").ToLower().Contains(keyword) ||
            (s.NHAXUATBAN.TenNXB ?? "").ToLower().Contains(keyword)
        ).ToList();
      }

      if (maCD != 0)
        kq = kq.Where(s => s.MaCD == maCD).ToList();

      // Gán tổng số trang để hiển thị
      int totalPages = (int)Math.Ceiling((double)kq.Count / pageSize);
      ViewBag.Page = page;
      ViewBag.PageSize = pageSize;
      ViewBag.TotalPages = totalPages;

      ViewBag.TotalResults = kq.Count;
      // Trả dữ liệu phân trang
      return View(kq.ToPagedList(page, pageSize));
    }


    public ActionResult Group()
    {
      // Gom nhóm sách theo mã chủ đề
      var kq = Minh_db.SACH
                      .Include("CHUDE")   // để lấy cả tên chủ đề
                      .GroupBy(s => s.MaCD);

      return View(kq.ToList());
    }

    public ActionResult ThongKe()
    {
      var kq = from s in Minh_db.SACH
               join cd in Minh_db.CHUDE on s.MaCD equals cd.MaCD
               group s by new { cd.MaCD, cd.TenChuDe } into g
               select new ReportInfo
               {
                 Id = g.Key.MaCD.ToString(),
                 Name = g.Key.TenChuDe,
                 Count = g.Count(),
                 Sum = g.Sum(n => n.SoLuongBan),
                 Max = g.Max(n => n.SoLuongBan),
                 Min = g.Min(n => n.SoLuongBan),
                 Avg = g.Average(n => (decimal?)n.SoLuongBan)


               };

      return View(kq);
    }
  }
}