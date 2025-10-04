using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using LacToMinh.SachOnline.Models;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;


namespace LacToMinh.SachOnline.Controllers
{
    public class LacToMinhSendMailController : Controller
    {
      // GET: FileAndMail
      public ActionResult Index()
      {
          return View();
      }
      [HttpGet]
      public ActionResult SendMail()
      {
        return View();
      }
      [HttpPost]
      public ActionResult SendMail(Mail model)
      {
        var mail = new SmtpClient("smtp.gmail.com", 587)
        {
          Credentials = new NetworkCredential("2324802010293@student.tdmu.edu.vn", "tnwxablxfngrqkfq"),
          EnableSsl = true
        };

        var message = new MailMessage();
        message.From = new MailAddress(model.From);
        message.ReplyToList.Add(model.From);
        message.To.Add(new MailAddress(model.To));
        message.Subject = model.Subject;
        message.Body = model.Notes;

        // Xử lý file đính kèm
        var f = Request.Files["attachment"];
        if (f != null && f.ContentLength > 0)
        {
          var path = Path.Combine(Server.MapPath("~/UploadFile"), f.FileName);
          if (!System.IO.File.Exists(path))
          {
            f.SaveAs(path);
          }

          Attachment data = new Attachment(path, MediaTypeNames.Application.Octet);
          message.Attachments.Add(data);
        }

        mail.Send(message);
        return View("SendMail");
      }

  }
}