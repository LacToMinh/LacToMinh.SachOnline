using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LacToMinh.SachOnline.Models
{
  public class GioHang
  {
    LacToMinh_SachOnlineEntities Minh_db = new LacToMinh_SachOnlineEntities();

    public int iMaSach { get; set; }
    public String sTenSach { get; set; }
    public String sAnhBia { get; set; }
    public double dDonGia { get; set; }
    public int iSoLuong { get; set; }
    public double dThanhTien
    {
      get { return iSoLuong * dDonGia; }
    }

    public GioHang (int ms)
    {
      iMaSach = ms;
      SACH s = Minh_db.SACH.Single(n => n.MaSach == iMaSach);
      sTenSach = s.TenSach;
      sAnhBia = s.AnhBia;
      dDonGia = double.Parse(s.GiaBan.ToString());
      iSoLuong = 1;
    }
  }
}