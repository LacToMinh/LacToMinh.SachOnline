using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LacToMinh.SachOnline.Models;

namespace LacToMinh.SachOnline.Areas.LTM_Admin.Controllers
{
    public class NhaXuatBanController : Controller
    {
      LacToMinh_SachOnlineEntities minh_db = new LacToMinh_SachOnlineEntities();
        // GET: LTM_Admin/NhaXuatBan
        public ActionResult Index()
        {
            return View(minh_db.NHAXUATBAN);
        }
        public ActionResult ChiTietNXB(int id)
        {
          //int id = 0;
          return View(minh_db.NHAXUATBAN.Where(nxb => nxb.MaNXB == id).SingleOrDefault());
        }
        [HttpGet]
        public ActionResult ThemNXB()
        {
          return View();
        }
    [HttpPost]
    public ActionResult ThemNXB(string tenNXB, string dcNXB, string dtNXB)
    {
      NHAXUATBAN nxb = new NHAXUATBAN();
      nxb.TenNXB = tenNXB;
      nxb.DiaChi = dcNXB;
      nxb.DienThoai = dtNXB;
      minh_db.NHAXUATBAN.Add(nxb);
      minh_db.SaveChanges();
      return RedirectToAction("Index");
    }
    //[HttpPost]
    //public ActionResult ThemNXB2(NHAXUATBAN nxb)
    //{
    //  minh_db.NHAXUATBAN.Add(nxb);
    //  minh_db.SaveChanges();
    //  return View();
    //}
    [HttpGet]
    public ActionResult Edit(int id)
    {
      var nxb = minh_db.NHAXUATBAN.SingleOrDefault(n => n.MaNXB == id);
      return View(nxb);
    }
    [HttpPost]
    public ActionResult Edit(NHAXUATBAN nxb)
    {
      var nxbCu = minh_db.NHAXUATBAN.SingleOrDefault(n => n.MaNXB == nxb.MaNXB);
      if (nxbCu != null)
      {
        nxbCu.TenNXB = nxb.TenNXB;
        nxbCu.DiaChi = nxb.DiaChi;
        nxbCu.DienThoai = nxb.DienThoai;
        minh_db.SaveChanges();
      }
      return RedirectToAction("Index");
    }
    [HttpGet]
    public ActionResult Delete(int id)
    {
      var nxb = minh_db.NHAXUATBAN.SingleOrDefault(n => n.MaNXB == id);
      return View(nxb);
    }
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      var nxb = minh_db.NHAXUATBAN.SingleOrDefault(n => n.MaNXB == id);
      if (nxb != null)
      {
        minh_db.NHAXUATBAN.Remove(nxb);
        minh_db.SaveChanges();
      }
      return RedirectToAction("Index");
    }


  }
}