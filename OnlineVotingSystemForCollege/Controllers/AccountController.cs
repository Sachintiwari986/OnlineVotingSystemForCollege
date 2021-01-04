using OnlineVotingSystemForCollege.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineVotingSystemForCollege.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Models.ViewModel.LoginViewModel iv, string ReturnUrl = "")
        {
            using (OnlineVotingSystemEntities db = new OnlineVotingSystemEntities())
            {
                var password = OnlineVotingSystemForCollege.Models.EncryptPassword.textToEncrypt(iv.Password);
                var user = db.tblUsers.Where(a => a.UserName == iv.UserName && a.Password == password).FirstOrDefault();
                if (user != null)
                {
                    if (user.EmailVerification == true)
                    {
                        Session.Add("FullName", user.Fullname);
                        Session.Add("UserName", user.UserName);
                        Session.Add("UserId", user.UserId);
                        FormsAuthentication.SetAuthCookie(iv.UserName, iv.RememberMe);
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {


                            return RedirectToAction("DashBoard", "Admin");


                        }
                    }
                    else
                    {
                        {
                            ViewBag.Message = "Email not verified";
                        }
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Invalid User");
                    ViewBag.Action = "Invalid User";
                }

                return View();
            }
        }
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Login", "Account");

        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel cp)
        {
            using (OnlineVotingSystemEntities db = new OnlineVotingSystemEntities())
            {
                string username = Session["UserName"].ToString();
                var user = db.tblUsers.Where(a => a.UserName == username).FirstOrDefault();
                if (user != null)
                {
                    var OldPassword = OnlineVotingSystemForCollege.Models.EncryptPassword.textToEncrypt(cp.OldPassword);
                    if (user.Password == OldPassword)
                    {
                        if (cp.NewPassword == cp.ConfirmNew)
                        {
                            user.Password = OnlineVotingSystemForCollege.Models.EncryptPassword.textToEncrypt(cp.NewPassword);
                            db.SaveChanges();
                            ViewBag.Message = "Password Changed";

                        }
                        else
                        {
                            ViewBag.Message = "New Password and Confirm New Mismatched";
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Wrong Password";
                    }



                }

                return View();
            }
        }
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
     
        public ActionResult ForgetPassword(UserViewModel fp)
        {
            using (OnlineVotingSystemEntities db = new OnlineVotingSystemEntities())
            {
                var user = db.tblUsers.Where(a => a.Email == fp.Email).FirstOrDefault();
                if (user != null)
                {
                    try
                    {
                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                        mail.From = new MailAddress("dotnetgroup789@gmail.com");
                        mail.To.Add(user.Email);
                        mail.Subject = "Password Recovery Sent From Sachin";
                        mail.Body = "<b>Password </b>:" +OnlineVotingSystemForCollege.Models.EncryptPassword.textToEncrypt( user.Password);
                        mail.IsBodyHtml = true;
                        SmtpServer.Port = 587;
                        SmtpServer.Credentials = new System.Net.NetworkCredential("onlinevotingforcollege@gmail.com", "Online@12345");
                        SmtpServer.EnableSsl = true;
                        ViewBag.Message = "Mail Sending";

                        SmtpServer.Send(mail);
                        ViewBag.Action = "Mail Sent ! Use Your Password To Login";
                        return View("Login");
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Action = "Email Sending Failed" + ex.ToString();
                    }

                }
                else
                {
                    ViewBag.Action = "Invalid Email";
                    
                }
           
                return View();
            }
        }
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registration(RegistrationViewModel uv)
        {
            using (OnlineVotingSystemEntities db = new OnlineVotingSystemEntities())
            {
                tblUser tbl = db.tblUsers.Where(u => u.UserName == uv.UserName || u.Email==uv.Email).FirstOrDefault();
                if (tbl != null)
                {
                    return Json(new { success = false, message = "User Already Register" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    tblUser tb = new tblUser();
                    tb.UserName = uv.UserName;
                    tb.Password = OnlineVotingSystemForCollege.Models.EncryptPassword.textToEncrypt(uv.Password);
                    tb.Fullname = uv.FullName;
                    tb.Email = uv.Email;
                    tb.EmailVerification = false;
                    tb.ActivationCode = Guid.NewGuid();
                    db.tblUsers.Add(tb);
                    db.SaveChanges();

                    tblUserRole ud = new tblUserRole();
                    ud.UserId = tb.UserId;
                    ud.RoleId = 2;
                    db.tblUserRoles.Add(ud);
                    db.SaveChanges();

                    tblIdRequest tbid = new tblIdRequest();
                    tbid.UserEmail = tb.Email;
                    tbid.FullName = tb.Fullname;
                    tbid.UserId = tb.UserId;
                    db.tblIdRequests.Add(tbid);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Registration Completed . A Email would be send to your email" + tb.Email +"If You are a Valid User" }, JsonRequestBehavior.AllowGet);
                }
            }


        }
      
        public ActionResult UserVerification(String id)
        {
            using (OnlineVotingSystemEntities db = new OnlineVotingSystemEntities())
            {
                bool Status = false;

                db.Configuration.ValidateOnSaveEnabled = false;
                var IsVerify = db.tblUsers.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();

                if (IsVerify != null)
                {
                    IsVerify.EmailVerification = true;
                    db.SaveChanges();
                    ViewBag.Message = "Email Verification completed";
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request...Email not verify";
                    ViewBag.Status = false;
                }

                return View();
            }
        }
    }

}