using OnlineVotingSystemForCollege.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineVotingSystemForCollege.Controllers
{
    public class AddCandidateController : Controller
    {
        // GET: AddCandidate
        OnlineVotingSystemEntities db = new OnlineVotingSystemEntities();
        [Authorize]
        public ActionResult AddCandidate()
        {
       
            return View();
        }
        public JsonResult ListElection()
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<ElectionViewModel> lst = new List<ElectionViewModel>();
            var Election = db.tblElections.ToList();
            foreach (var item in Election)
            {
                lst.Add(new ElectionViewModel() { ElectionId = item.ElectionId, ElectionName = item.ElectionName, ElectionStartDate = item.ElectionStartDate, ElectionEndDate = item.ElectionEndDate ,NoOfCandidate=item.NoOfCandidate});
            }

            return Json(new { data = lst }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult Add(int id)
        {
            CandidateViewModel cm = new CandidateViewModel();
            cm.CandidateName = "";
            cm.ElectionId = id;
            cm.ElectionName = db.tblElections.Where(a => a.ElectionId == id).Select(a => a.ElectionName).FirstOrDefault();
            cm.Description = "";
            cm.Photo = "";
            return View(cm);
            
        }

        [HttpPost]
        public ActionResult Add(CandidateViewModel pvm)
        {
            tblCandidate tbl = new tblCandidate();
            tbl.CandidateName = pvm.CandidateName;
            tbl.ElectionId = pvm.ElectionId;
            tbl.Description = pvm.Description;
            tbl.VoteObtained = 0;
            HttpPostedFileBase fup = Request.Files["Photo"];
            if (fup != null)
            {
                tbl.Photo = fup.FileName;
                fup.SaveAs(Server.MapPath("~/CandidateImages/" + fup.FileName));
            }
            var candidatenum = db.tblElections.Where(a => a.ElectionId == pvm.ElectionId).Select(a => a.NoOfCandidate).FirstOrDefault();
            tblElection tb = db.tblElections.Where(a => a.ElectionId == pvm.ElectionId).FirstOrDefault();
            tb.NoOfCandidate = candidatenum + 1;
                db.tblCandidates.Add(tbl);
            db.SaveChanges();

          return RedirectToAction("AddCandidate");
        
            


        }
        [Authorize]
        public ActionResult ViewCandidate()
        {
            List<CandidateViewModel> lst = new List<CandidateViewModel>();
            var candidate = db.tblCandidates.ToList();
            foreach(var item in candidate)
            {
                lst.Add(new CandidateViewModel() { CandidateId = item.CandidateId, CandidateName = item.CandidateName, ElectionName = item.tblElection.ElectionName, Description = item.Description, Photo = item.Photo });
            }

            return View(lst);
        }
    }

}