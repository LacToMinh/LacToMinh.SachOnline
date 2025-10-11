using LacToMinh.SachOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SachOnline.Controllers
{
  public class LacToMinh_GioHangController : Controller
  {
    LacToMinh_SachOnlineEntities Minh_db = new LacToMinh_SachOnlineEntities();

    // ✅ Lấy giỏ hàng từ Session
    public List<GioHang> LayGioHang()
    {
      List<GioHang> lst = Session["GioHang"] as List<GioHang>;
      if (lst == null)
      {
        lst = new List<GioHang>();
        Session["GioHang"] = lst;
      }
      return lst;
    }

    // ✅ Thêm sản phẩm
    public ActionResult ThemGioHang(int id, string url)
    {
      List<GioHang> lst = LayGioHang();
      GioHang sp = lst.Find(n => n.iMaSach == id);
      if (sp == null)
      {
        sp = new GioHang(id);
        lst.Add(sp);
      }
      else sp.iSoLuong++;
      return Redirect(url);
    }

    // ✅ Tính tổng số lượng
    private int TongSoLuong()
    {
      int iTong = 0;
      List<GioHang> lst = Session["GioHang"] as List<GioHang>;
      if (lst != null) iTong = lst.Sum(n => n.iSoLuong);
      return iTong;
    }

    // ✅ Tính tổng tiền
    private double TongTien()
    {
      double iTong = 0;
      List<GioHang> lst = Session["GioHang"] as List<GioHang>;
      if (lst != null) iTong = lst.Sum(n => n.dThanhTien);
      return iTong;
    }

    // ✅ Hiển thị giỏ hàng
    public ActionResult GioHang()
    {
      List<GioHang> lst = LayGioHang();
      if (lst.Count == 0) return RedirectToAction("Index", "LacToMinh_SachOnline");
      ViewBag.TongSoLuong = TongSoLuong();
      ViewBag.TongTien = TongTien();
      return View(lst);
    }

    // ✅ Xóa 1 sản phẩm
    public ActionResult XoaSPKhoiGioHang(int iMaSach)
    {
      List<GioHang> lst = LayGioHang();
      lst.RemoveAll(n => n.iMaSach == iMaSach);
      return RedirectToAction("GioHang");
    }

    // ✅ Cập nhật số lượng
    public ActionResult CapNhatGioHang(int iMaSach, FormCollection f)
    {
      List<GioHang> lst = LayGioHang();
      GioHang sp = lst.SingleOrDefault(n => n.iMaSach == iMaSach);
      if (sp != null)
        sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
      return RedirectToAction("GioHang");
    }

    // ✅ Xóa toàn bộ giỏ
    public ActionResult XoaGioHang()
    {
      List<GioHang> lst = LayGioHang();
      lst.Clear();
      return RedirectToAction("Index", "LacToMinh_SachOnline");
    }

    // ✅ Partial hiển thị icon giỏ
    public ActionResult GioHangPartial()
    {
      ViewBag.TongSoLuong = TongSoLuong();
      ViewBag.TongTien = TongTien();
      return PartialView();
    }

    [HttpGet]
    public ActionResult DatHang()
    {
      // Kiểm tra đăng nhập
      if (Session["user"] == null)
        return RedirectToAction("DangNhap", "LacToMinh_User");

      // Kiểm tra giỏ hàng
      if (Session["GioHang"] == null)
        return RedirectToAction("Index", "LacToMinh_SachOnline");

      // Lấy giỏ hàng từ Session
      List<GioHang> lstGioHang = LayGioHang();
      ViewBag.TongSoLuong = TongSoLuong();
      ViewBag.TongTien = TongTien();

      return View(lstGioHang);
    }

    [HttpPost]
    public ActionResult DatHang(FormCollection f)
    {
      List<GioHang> lstGioHang = LayGioHang();
      KHACHHANG kh = (KHACHHANG)Session["user"];
      DONDATHANG ddh = new DONDATHANG();

      // Gán thông tin đơn hàng
      ddh.MaKH = kh.MaKH;
      ddh.NgayDat = DateTime.Now;
      ddh.NgayGiao = DateTime.Parse(f["NgayGiao"]);
      ddh.TinhTrangGiaoHang = 1;
      ddh.DaThanhToan = false;

      Minh_db.DONDATHANG.Add(ddh);
      Minh_db.SaveChanges();

      // Lưu chi tiết đơn hàng
      foreach (var item in lstGioHang)
      {
        CHITIETDATHANG ct = new CHITIETDATHANG();
        ct.MaDonHang = ddh.MaDonHang;
        ct.MaSach = item.iMaSach;
        ct.SoLuong = item.iSoLuong;
        ct.DonGia = (decimal)item.dDonGia;
        Minh_db.CHITIETDATHANG.Add(ct);
      }
      Minh_db.SaveChanges();

      // Xóa giỏ hàng sau khi đặt
      Session["GioHang"] = null;
      return RedirectToAction("XacNhanDonHang", "LacToMinh_GioHang");
    }

    public ActionResult XacNhanDonHang()
    {
      return View();
    }


  }
}
