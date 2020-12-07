using OnlineVotingSystemForCollege.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineVotingSystemForCollege.Controllers
{
    public class ManageElectionController : Controller
    {
        OnlineVotingSystemEntities db = new OnlineVotingSystemEntities();
        // GET: ManageElection
        [Authorize]
        public ActionResult ManageElection()
        {

            return View();
        }
        [Authorize]
        public JsonResult ListElection()
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<ElectionViewModel> lst = new List<ElectionViewModel>();
            var Election = db.tblElections.ToList();
            foreach (var item in Election)
            {
                lst.Add(new ElectionViewModel() { ElectionId = item.ElectionId, ElectionName = item.ElectionName ,ElectionStartDate=item.ElectionStartDate,ElectionEndDate=item.ElectionEndDate });
            }

            return Json(new { data = lst }, JsonRequestBehavior.AllowGet);
        }
        [Authorize ]
        public ActionResult AddEdit(int id = 0)
        {
            if (id == 0)
            {
                ViewBag.Action = "New Election";
                return PartialView("AddEdit");
            }
            else
            {
                var Election = db.tblElections.Where(a => a.ElectionId == id).FirstOrDefault();
                ElectionViewModel cv = new ElectionViewModel();
                cv.ElectionId = Election.ElectionId;
                cv.ElectionName = Election.ElectionName;
                cv.ElectionStartDate = Election.ElectionStartDate;
                cv.ElectionEndDate = Election.ElectionEndDate;
                ViewBag.Action = "Edit Election";
                return PartialView("AddEdit", cv);

            }
        }

        [HttpPost]
        public JsonResult AddEdit(ElectionViewModel pvm)
        {

                if (pvm.ElectionId > 0)
                {
                    tblElection tb = db.tblElections.Where(x => x.ElectionId == pvm.ElectionId).FirstOrDefault();
                    tb.ElectionName = pvm.ElectionName;
                    tb.ElectionStartDate = pvm.ElectionStartDate;
                    tb.ElectionEndDate = pvm.ElectionEndDate;

                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    tblElection tb = new tblElection();
                    tb.ElectionName = pvm.ElectionName;
                    tb.ElectionStartDate = pvm.ElectionStartDate;
                    tb.ElectionEndDate = pvm.ElectionEndDate;
                    tb.NoOfCandidate = 0;
                    db.tblElections.Add(tb);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
            
          
        }
        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            tblElection tbl = db.tblElections.Where(a => a.ElectionId == id).FirstOrDefault();
            db.tblElections.Remove(tbl);
            db.SaveChanges();

            return Json(new { success = true, message = "Deleted Sucessfully" }, JsonRequestBehavior.AllowGet);
        }
    }
}
