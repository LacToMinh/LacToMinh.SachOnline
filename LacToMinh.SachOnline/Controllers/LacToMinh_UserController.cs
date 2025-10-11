using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using LacToMinh.SachOnline.Models;
using System.Net;
using System.Net.Mail;
//using Microsoft.AspNet.Identity;

//using LacToMinh.SachOnline.Helpers;


namespace LacToMinh.SachOnline.Controllers
{

  public class LacToMinh_UserController : Controller
  {
    //private readonly PasswordHasher passwordHasher = new PasswordHasher();
    LacToMinh_SachOnlineEntities Minh_db = new LacToMinh_SachOnlineEntities();
    // GET: LacToMinh_User
    //private string HashMD5(string input)
    //{
    //  using (MD5 md5 = MD5.Create())
    //  {
    //    byte[] inputBytes = Encoding.UTF8.GetBytes(input);
    //    byte[] hashBytes = md5.ComputeHash(inputBytes);

    //    // Chuyển sang chuỗi hex
    //    StringBuilder sb = new StringBuilder();
    //    for (int i = 0; i < hashBytes.Length; i++)
    //    {
    //      sb.Append(hashBytes[i].ToString("x2"));
    //    }
    //    return sb.ToString();
    //  }
    //}

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
          SendLoginEmail(kh.Email, kh.HoTen); // Gửi mail thông báo

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

    private void SendLoginEmail(string toEmail, string userName)
    {
      try
      {
        var fromAddress = new MailAddress("2324802010293@student.tdmu.edu.vn", "Sách Online");
        var toAddress = new MailAddress(toEmail);
        const string fromPassword = "tnwxablxfngrqkfq"; // Mật khẩu ứng dụng Gmail (16 ký tự)
        string subject = "Đăng nhập thành công";
        string body = $"Xin chào {userName},\n\nBạn đã đăng nhập thành công vào hệ thống Sách Online lúc {DateTime.Now:HH:mm:ss - dd/MM/yyyy}.\n\nNếu không phải bạn, vui lòng đổi mật khẩu ngay.";

        var smtp = new SmtpClient
        {
          Host = "smtp.gmail.com",
          Port = 587,
          EnableSsl = true,
          DeliveryMethod = SmtpDeliveryMethod.Network,
          UseDefaultCredentials = false,
          Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
        };

        using (var message = new MailMessage(fromAddress, toAddress)
        {
          Subject = subject,
          Body = body
        })
        {
          smtp.Send(message);
        }
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debug.WriteLine("Lỗi gửi email: " + ex.Message);
      }
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