using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LacToMinh.SachOnline.Models;
//using LacToMinh.SachOnline.Helpers;


namespace LacToMinh.SachOnline.Controllers
{
  public class LacToMinh_UserController : Controller
  {
    LacToMinh_SachOnlineEntities Minh_db = new LacToMinh_SachOnlineEntities();
    // GET: LacToMinh_User
    public ActionResult Index()
    {
      return View();
    }
    //public ActionResult DangNhap()
    //{
    //  return View();
    //}

    public ActionResult DangNhap(FormCollection f)
    {
      var tenDN = f["TenDN"];
      var matKhau = f["MatKhau"];

      if (String.IsNullOrEmpty(tenDN) || String.IsNullOrEmpty(matKhau))
      {
        ViewBag.ThongBao = "Tên đăng nhập và mật khẩu không được trống.";
      }
      else
      {
        KHACHHANG kh = Minh_db.KHACHHANG.SingleOrDefault(n => n.TaiKhoan == tenDN && n.MatKhau == matKhau);


        if (kh == null)
        {
          ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
        }
        else
        {
          ViewBag.ThongBao = "Bạn đã đăng nhập thành công.";
          Session["user"] = kh;

          if (f["remember"].Contains("true"))
          {
            Response.Cookies["TenDN"].Value = tenDN;
            Response.Cookies["MatKhau"].Value = matKhau;
            Response.Cookies["TenDN"].Expires = DateTime.Now.AddDays(1);
            Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(1);
          }
          else
          {
            Response.Cookies["TenDN"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(-1);
          }

          return RedirectToAction("Index", "LacToMinh_SachOnline"); // Trả về trang chủ
        }
      }
      return View();

    }

    public ActionResult DangXuat()
    {
      Session["user"] = null;
      return RedirectToAction("Index", "LacToMinh_SachOnline"); // Trả về trang chủ

    }
    [HttpGet]
    public ActionResult DangKy()
    {
      return View();
    }

    [HttpPost]
    public ActionResult DangKy(FormCollection collection, KHACHHANG kh)
    {
      // Gán giá trị nhập vào cho biến
      var sHoTen = collection["HoTen"];
      var sTenDN = collection["TenDN"];
      var sMatKhau = collection["MatKhau"];
      var sMatKhauNhapLai = collection["MatKhauNL"];
      var sEmail = collection["Email"];
      var sDienThoai = collection["DienThoai"];
      var sDiaChi = collection["DiaChi"];
      var dNgaySinh = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);
        
      // Validate
      if (String.IsNullOrEmpty(sHoTen))
      {
        ViewData["err1"] = "Họ tên không được rỗng";
      }
      else if (String.IsNullOrEmpty(sTenDN))
      {
        ViewData["err2"] = "Tên đăng nhập không được rỗng";
      }
      else if (String.IsNullOrEmpty(sMatKhau))
      {
        ViewData["err3"] = "Phải nhập mật khẩu";
      }
      else if (String.IsNullOrEmpty(sMatKhauNhapLai))
      {
        ViewData["err4"] = "Phải nhập lại mật khẩu";
      }
      else if (sMatKhau != sMatKhauNhapLai)
      {
        ViewData["err4"] = "Mật khẩu nhập lại không khớp";
      }
      else if (String.IsNullOrEmpty(sEmail))
      {
        ViewData["err5"] = "Email không được rỗng";
      }
      else if (String.IsNullOrEmpty(sDienThoai))
      {
        ViewData["err6"] = "Số điện thoại không được rỗng";
      }
      else if (Minh_db.KHACHHANG.SingleOrDefault(n => n.TaiKhoan == sTenDN) != null)
      {
        ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
      }
      else if (Minh_db.KHACHHANG.SingleOrDefault(n => n.Email == sEmail) != null)
      {
        ViewBag.ThongBao = "Email đã được sử dụng";
      }
      else
      {
        // Gán giá trị cho đối tượng KH
        kh.HoTen = sHoTen;
        kh.TaiKhoan = sTenDN;
        kh.MatKhau = sMatKhau;
        kh.Email = sEmail;
        kh.DiaChi = sDiaChi;
        kh.DienThoai = sDienThoai;
        kh.NgaySinh = DateTime.Parse(dNgaySinh);

        Minh_db.KHACHHANG.Add(kh);
        Minh_db.SaveChanges();


        return RedirectToAction("DangNhap", "LacToMinh_User");
      }
      return View();
    }
  }
}