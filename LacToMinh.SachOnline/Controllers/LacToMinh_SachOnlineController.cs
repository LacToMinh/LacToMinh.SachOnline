  using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using LacToMinh.SachOnline.Models;

namespace LacToMinh.SachOnline.Controllers
{
  public class LacToMinh_SachOnlineController : Controller
  {
      LacToMinh_SachOnlineEntities Minh_db = new LacToMinh_SachOnlineEntities();

      // GET: SachOnline
      public ActionResult Index(int page = 1, int pageSize = 6)
      {
        var allBooks = Minh_db.SACH.OrderByDescending(s => s.NgayCapNhat).ToList();

        int totalItems = allBooks.Count();
        var pagedBooks = allBooks.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        return View(pagedBooks);
      }

      private List<SACH> LaySachMoi(int count)
      {
        return Minh_db.SACH.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
      }

      private List<SACH> LaySachBanNhieu(int count)
      {
        return Minh_db.SACH.OrderByDescending(a => a.GiaBan).Take(count).ToList();
      }

      public ActionResult ChuDePartial()
      {
        var kq = Minh_db.CHUDE.ToList();
        //var cdLacToMinh = new CHUDE { MaCD = 0, TenChuDe = "Lạc Tô Minh" };

        //kq.Insert(0, cdLacToMinh);
        return PartialView(kq);
      }
      public ActionResult SachTheoChuDe(int id)
      {
        var sachTheoChuDe = Minh_db.SACH.Where(s => s.MaCD == id).ToList();
        
        var tenChuDe = Minh_db.CHUDE.Where(cd =>  cd.MaCD == id).Select(cd =>  cd.TenChuDe).FirstOrDefault();
        ViewBag.TenChuDe = tenChuDe;
        
        return View(sachTheoChuDe);
      }

      public ActionResult NhaXuatBanPartial()
      {
        var kq = Minh_db.NHAXUATBAN.ToList();
        var cdLacToMinh = new NHAXUATBAN
        {
          MaNXB = 0,
          TenNXB = "Lạc Tô Minh",
          DiaChi = "Tổ 11, khu phố 1, phường Hòa Lợi, Hồ Chí Minh",
          DienThoai = "0859585739",
        };

        kq.Insert(0, cdLacToMinh);

        return PartialView(kq);
      }
      public ActionResult NavPartial()
      {
        return PartialView();
      }
      public ActionResult SliderPartial()
      {
        return PartialView();
      }
      public ActionResult SachBanNhieuPartial()
      {
        var listSachBanNhieu = LaySachBanNhieu(6);
        return PartialView(listSachBanNhieu);
      }
      public ActionResult FooterPartial()
      {
        return PartialView();
      }
      public ActionResult SachTheoNXB(int id)
      {
        // Lấy tất cả sách có mã NXB = id
        var kq = (from s in Minh_db.SACH
                  where s.MaNXB == id
                  select s).ToList();

        return View(kq);
      }
      public ActionResult ChiTietSach(int id)
      {
        var sach = from s in Minh_db.SACH
                   where s.MaSach == id
                   select s;

        return View(sach.Single());
      }

  }
}