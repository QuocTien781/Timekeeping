using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToolsApp.App_Start;
using ToolsApp.Authentication;
using ToolsApp.EntityFramework;
using ToolsApp.Models;
using ToolsApp.Utilities;

namespace ToolsApp.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        ChamCong_teamdevEntities db_ = new ChamCong_teamdevEntities();
        // GET: Home
        public ActionResult Index(string Id = "")
        {
            var data_SumUser = db_.AspNetUsers.Where(a => a.idDepartment == User.idDepartment).ToList();
            ViewData["data_SumUser"] = data_SumUser.Where(a => a.AccoutType == "User").ToList().Count();
            ViewData["data_SumAdmin"] = data_SumUser.Where(a => a.AccoutType == "Admin" && a.UserName != "Admin").ToList().Count();
            ViewData["data_SumManager"] = data_SumUser.Where(a => a.AccoutType == "Admin" && a.IsManager == true && a.UserName != "Admin").ToList().Count();
            var data_SumDepartment = db_.Jobs.Where(a => a.IdDepartment == User.idDepartment).ToList();
            ViewData["data_SumDepartment"] = data_SumDepartment.Count();
            var data_SumDetailJob = (from a in db_.Jobs
                                     join b in db_.JobDetails on a.Id equals b.idjob
                                     where a.IdDepartment == User.idDepartment
                                     select b);
            ViewData["data_SumDetailJob"] = data_SumDetailJob.Count();
            return View();
        }
        public ActionResult PageNotFound()
        {

            return PartialView();
        }
        public ActionResult NotAuthentizace()
        {

            return PartialView();
        }

    }
}