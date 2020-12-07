using OnlineVotingSystemForCollege.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineVotingSystemForCollege.Controllers
{
    public class ResultController : Controller
    {
        OnlineVotingSystemEntities db = new OnlineVotingSystemEntities();
        // GET: Result
        [Authorize]
        public ActionResult ViewResult()
        {
            ViewBag.Action = "You Can View The Result Of Only Those Election Which Are Closed ";
            return View();
        }
        [Authorize]
        public JsonResult ListOfElection()
        {
            if (User.IsInRole("User"))
            { 
            
                db.Configuration.LazyLoadingEnabled = false;
                var currentdate = DateTime.Now;


            var election = db.tblElections.ToList().Where(a => Convert.ToDateTime(a.ElectionEndDate) < currentdate);
               

             
                List<ElectionViewModel> lst = new List<ElectionViewModel>();
                foreach (var item in election)
                {
                    lst.Add(new ElectionViewModel() { ElectionId = item.ElectionId, ElectionName = item.ElectionName, ElectionStartDate = item.ElectionStartDate, ElectionEndDate = item.ElectionEndDate });
                    
                }
              
                return Json(new { data = lst }, JsonRequestBehavior.AllowGet);



            }
            else
            {

                db.Configuration.LazyLoadingEnabled = false;
                DateTime currentdate = DateTime.Now;
                var election = db.tblElections.ToList();
                List<ElectionViewModel> lst = new List<ElectionViewModel>();
                foreach (var item in election)
                {
                    lst.Add(new ElectionViewModel() { ElectionId = item.ElectionId, ElectionName = item.ElectionName, ElectionStartDate = item.ElectionStartDate, ElectionEndDate = item.ElectionEndDate });

                }
               
                return Json(new { data = lst }, JsonRequestBehavior.AllowGet);
            }
        
        }
        [Authorize]
        public ActionResult ListResult(int id)
        {
            var result = db.tblCandidates.Where(a => a.ElectionId == id).ToList();
            List<ResultViewModel> lst = new List<ResultViewModel>();
            foreach (var item in result)
            {
                lst.Add(new ResultViewModel() { CandidateName = item.CandidateName, ElectionName = item.tblElection.ElectionName, Photo = item.Photo, VoteObtained = item.VoteObtained });

            }
            return View(lst);
        }

    }
}