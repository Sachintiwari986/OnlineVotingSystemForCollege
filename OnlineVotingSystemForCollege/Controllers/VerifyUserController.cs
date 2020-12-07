using OnlineVotingSystemForCollege.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace OnlineVotingSystemForCollege.Controllers
{
    public class VerifyUserController : Controller
    {
        OnlineVotingSystemEntities db = new OnlineVotingSystemEntities();
        // GET: VerifyUser
        [Authorize]
        public ActionResult VerifyUser()
        {
            return View();
        }
        [Authorize]
        public  ActionResult IdRequestList()
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<UserViewModel> lst = new List<UserViewModel>();
            var User = db.tblIdRequests.ToList();
            foreach (var item in User)
            {
                lst.Add(new UserViewModel() { IdRequestId = item.IdRequestId, UserId = item.UserId, Email = item.UserEmail, FullName = item.FullName });
            }

            return Json(new { data = lst }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult Verify(int id)
        {
            var user = db.tblUsers.Where(a => a.UserId == id).FirstOrDefault();
            if (user!=null)
            {
                SendEmailToUser(user.Email, user.ActivationCode.ToString());
                tblIdRequest tb= db.tblIdRequests.Where(a => a.UserId == id).FirstOrDefault();
                db.tblIdRequests.Remove(tb);
                db.SaveChanges();
            }
            return View("UserVerified");
        }
        [Authorize]
        private void SendEmailToUser(string email, string v)
        {
            //var GenarateUserVerificationLink = "/Account/UserVerification/" + v;

            //var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, GenarateUserVerificationLink);
            var link = string.Format("{0}://{1}/Account/UserVerification/{2}", Request.Url.Scheme, Request.Url.Authority, v);
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("onlinevotingforcollege@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Registration Verification";
                mail.Body = "<br/> Your registration completed succesfully." +
                           "<br/> please click on the below link for account verification" +
                           "<br/><br/><a href=" + link + ">" + link + "</a>";
                mail.IsBodyHtml = true;
                SmtpServer.Port = 587;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("onlinevotingforcollege@gmail.com", "Online@12345");
                SmtpServer.EnableSsl = true;
                ViewBag.Message = "Mail Sending";

                SmtpServer.Send(mail);
                ViewBag.Message = "Mail Sent";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Email Sending Failed" + ex.ToString();
            }

        }
        public ActionResult UserVerified()
        {
            return View();
        }



    }
}