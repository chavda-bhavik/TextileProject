using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pal.Services;
using Pal.Entities.Models;
using Newtonsoft.Json;
using Repository.Pattern.UnitOfWork;

namespace PalWeb.Controllers
{
    public class JobworkController : Controller
    {
        //private IListService _ListService;
        private IUnitOfWorkAsync unitOfWork;
        private IJobworkService jobworkService;
        private IOutwardsService outwardsService;
        private IOutwardsDetailsService detailsService;
        public JobworkController(IUnitOfWorkAsync _unitofwork, IJobworkService _jobworkService, IOutwardsService _outwardsService, IOutwardsDetailsService _detailsService)
        {
            //_ListService = listService;
            unitOfWork = _unitofwork;
            jobworkService = _jobworkService;
            outwardsService = _outwardsService;
            detailsService = _detailsService;
        }

        public JsonResult CheckVoucherNo(int voucherno)
        {
            var results = jobworkService.Find(voucherno.ToString());
            if (results != null)
            {
                return Json("available", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("not available", JsonRequestBehavior.AllowGet);
            }
        }
        // GET: Jobwork
        public ActionResult Index()
        {
            return View();
        }
    }
}
