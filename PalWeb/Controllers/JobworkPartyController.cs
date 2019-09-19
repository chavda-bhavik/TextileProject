using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pal.Services;
using Pal.Entities.Models;
using Repository.Pattern.UnitOfWork;
using Newtonsoft.Json;

namespace PalWeb.Controllers
{
    public class JobworkPartyController : Controller
    {
        //private IListService _ListService;
        private IUnitOfWorkAsync _unitOfWork;
        private IJobworkPartyService _JWService;
        public JobworkPartyController(IUnitOfWorkAsync unitofwork, IJobworkPartyService jwservice)
        {
            //_ListService = listService;
            _unitOfWork = unitofwork;
            _JWService = jwservice;
        }
        // GET: JobworkParty
        public ActionResult Index()
        {
            return View(_JWService.Queryable());
        }

        public JsonResult AddJobworkParty(JobworkParty JParty)
        {
            //String party = JParty.ToString();
            //string message = JParty.Address;
            //JobworkParty jobworkparty = JsonConvert.DeserializeObject<JobworkParty>(party);
            //message = message + " " + jobworkparty.AgencyName;
            _JWService.Insert(JParty);
            _unitOfWork.SaveChanges();
            return Json(JParty, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateJobworkParty(JobworkParty Jparty)
        {
            _JWService.Update(Jparty);
            _unitOfWork.SaveChanges();
            return Json(Jparty, JsonRequestBehavior.AllowGet);
        }
        public void DeleteJobworkParty(JobworkParty Jparty)
        {
            _JWService.Delete(Jparty.Code);
            _unitOfWork.SaveChanges();
        }
        public JsonResult GetJobworkParties()
        {
            return Json(_JWService.Queryable(), JsonRequestBehavior.AllowGet);
        }
    }
}
