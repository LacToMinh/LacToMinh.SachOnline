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

        public ActionResult Search(string strSearch = null, int maCD = 0)
        {
          ViewBag.Search = strSearch;

          // Danh sách chủ đề cho dropdown
          ViewBag.MaCD = new SelectList(Minh_db.CHUDE, "MaCD", "TenChuDe");

          // Truy vấn cơ bản, có Include() để load cả CHUDE và NHAXUATBAN
          var kq = Minh_db.SACH
                          .Include("CHUDE")
                          .Include("NHAXUATBAN")
                          .Select(s => s);

          // Lọc theo tên sách
          if (!string.IsNullOrEmpty(strSearch))
          {
            kq = kq.Where(s => s.TenSach.Contains(strSearch));
          }

          // Lọc theo mã chủ đề
          if (maCD != 0)
          {
            kq = kq.Where(s => s.MaCD == maCD);
          }

          // Trả kết quả ra View
          return View(kq.ToList());
    }

    //  public ActionResult SearchTheoDanhMuc(string strSearch = null, int maCD = 0)
    //    {
    //      // 1. Lưu từ khóa tìm kiếm
    //      ViewBag.Search = strSearch;

    //      // 2. Câu truy vấn cơ bản
    //      var kq = Minh_db.SACH.Select(s => s);

    //      // 3. Lọc theo tên sách
    //      if (!string.IsNullOrEmpty(strSearch))
    //      {
    //        kq = kq.Where(s => s.TenSach.Contains(strSearch));
    //      }

    //      // 4. Lọc theo mã chủ đề (nếu người dùng chọn)
    //      if (maCD != 0)
    //      {
    //        kq = kq.Where(s => s.CHUDE.MaCD == maCD);
    //      }

    //      // 5. Tạo danh sách chủ đề để hiển thị trong dropdown
    //      ViewBag.MaCD = new SelectList(Minh_db.CHUDE, "MaCD", "TenChuDe");

    //      // Trả về kết quả
    //      return View(kq.ToList());
    //    } 
    //}
}