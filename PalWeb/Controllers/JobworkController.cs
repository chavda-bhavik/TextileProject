using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pal.Services;
using Pal.Entities.Models;
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

        // GET: Jobwork
        public ActionResult Index()
        {
            return View();
        }

        // GET: Jobwork/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Jobwork/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Jobwork/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Jobwork/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Jobwork/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Jobwork/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Jobwork/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
