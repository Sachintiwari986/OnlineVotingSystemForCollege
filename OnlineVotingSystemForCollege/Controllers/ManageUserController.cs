using OnlineVotingSystemForCollege.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineVotingSystemForCollege.Controllers
{
    public class ManageUserController : Controller
    {
        // GET: ManageUser
        public ActionResult ManageUser()
        {
            return View();
        }
        public JsonResult GetData()
        {
            using (OnlineVotingSystemEntities db = new OnlineVotingSystemEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                List<UserViewModel> lstitem = new List<UserViewModel>();
                var lst = db.tblUsers.ToList();
                foreach (var item in lst)
                {
                    lstitem.Add(new UserViewModel() { UserId = item.UserId, UserName = item.UserName, FullName = item.Fullname, Email = item.Email  });
                }
                return Json(new { data = lstitem }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult AddOrEdit()
        {
            return View();
        }
        [HttpPost]

        public ActionResult AddOrEdit(UserViewModel uv)
        {
            using (OnlineVotingSystemEntities db = new OnlineVotingSystemEntities())
            {
                tblUser tb = new tblUser();
                tb.UserName = uv.UserName;
                tb.Password = OnlineVotingSystemForCollege.Models.EncryptPassword.textToEncrypt(uv.Password);
                tb.Fullname = uv.FullName;
                tb.Email = uv.Email;
                tb.EmailVerification = true;

                db.tblUsers.Add(tb);
                db.SaveChanges();

                tblUserRole ud = new tblUserRole();
                ud.UserId = tb.UserId;
                ud.RoleId = 1;
                db.tblUserRoles.Add(ud);
                db.SaveChanges();
                ViewBag.Message = "User Created Successfully";


                return RedirectToAction("ManageUser");
            }
        }
        [HttpPost]

        public ActionResult Delete(int id)
        {
            using (OnlineVotingSystemEntities db = new OnlineVotingSystemEntities())
            {
                var result = db.tblResults.Where(a => a.UserId == id).ToList();
                foreach (var item in result)
                {
                    var VoteObtained = db.tblCandidates.Where(a => a.CandidateId == item.CandidateId).Select(a => a.VoteObtained).FirstOrDefault();
                    tblCandidate tbc = db.tblCandidates.Where(a => a.CandidateId == item.CandidateId).FirstOrDefault();
                    tbc.VoteObtained = VoteObtained - 1;
                    db.tblResults.Remove(item);
                    db.SaveChanges();
                }
              

                var tb= db.tblUserRoles.Where(x => x.UserId == id).FirstOrDefault();
                db.tblUserRoles.Remove(tb);
                db.SaveChanges();

                tblUser sm = db.tblUsers.Where(x => x.UserId == id).FirstOrDefault();
                db.tblUsers.Remove(sm);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}