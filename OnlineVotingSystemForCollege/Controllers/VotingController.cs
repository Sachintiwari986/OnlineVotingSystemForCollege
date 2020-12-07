using OnlineVotingSystemForCollege.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineVotingSystemForCollege.Controllers
{
    public class VotingController : Controller
    {
        // GET: Voting
        // GET: AddCandidate
        OnlineVotingSystemEntities db = new OnlineVotingSystemEntities();
        [Authorize]
        public ActionResult Vote()
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
                lst.Add(new ElectionViewModel() { ElectionId = item.ElectionId, ElectionName = item.ElectionName, ElectionStartDate = item.ElectionStartDate, ElectionEndDate = item.ElectionEndDate ,NoOfCandidate=item.NoOfCandidate });
            }

            return Json(new { data = lst }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult CandidateList(int id)
        {
            var election = db.tblElections.Where(a => a.ElectionId == id).FirstOrDefault();
            if (election != null)
            {
                DateTime todaydate = DateTime.Now;
                DateTime startdate = Convert.ToDateTime(election.ElectionStartDate);
                DateTime enddate = Convert.ToDateTime(election.ElectionEndDate);

                if (todaydate >= startdate && todaydate <= enddate)
                {
                    var candidate = db.tblCandidates.Where(a => a.ElectionId == id).ToList();
                    List<CandidateViewModel> lst = new List<CandidateViewModel>();
                    foreach (var item in candidate)
                    {
                        lst.Add(new CandidateViewModel() { ElectionId = item.ElectionId, CandidateId = item.CandidateId, ElectionName = item.tblElection.ElectionName, CandidateName = item.CandidateName, Photo = item.Photo });
                    }

                    return View(lst);

                }
                else if (todaydate < startdate)
                {
                    ViewBag.Action = "Election has not Started Yet";
                    return View();



                }
                else
                {
                    ViewBag.Action = "Election has already Ended";
                    return View();
                }

            }
            ViewBag.Action = "Election You are Searching Didn't Found";
            return View();
        
       
       
            
        }
        [Authorize]
        public ActionResult Voted(int id)
        {
          var  electionid = db.tblCandidates.Where(b => b.CandidateId == id).Select(b => b.ElectionId).FirstOrDefault();
            var userid = Convert.ToInt32(Session["UserId"]);
            var isvoted = db.tblResults.Where(a => a.ElectionId == electionid && a.UserId == userid).FirstOrDefault();
            if(isvoted!=null)
            {
                var CandidateName =isvoted.tblCandidate.CandidateName;
                ViewBag.Action = "You have already Casted Your Vote To" + CandidateName;
                return View();
            }
            tblResult tb = new tblResult();
            tb.CandidateId = id;
            tb.ElectionId = electionid;
            tb.UserId = userid;
            tb.UserName= Convert.ToString((Session["UserName"]));
            var votecounted = db.tblCandidates.Where(a => a.CandidateId == id).FirstOrDefault();
            var oldvote = votecounted.VoteObtained;
            votecounted.VoteObtained = oldvote + 1;
            var candidatename = votecounted.CandidateName;
           
            db.tblResults.Add(tb);
            db.SaveChanges();
            ViewBag.Action="You Have Sucessfully Casted Your Vote " + candidatename;


            return View();
        }


    }
  
}