using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToolsApp.Authentication;
using ToolsApp.EntityFramework;

namespace ToolsApp.Controllers
{
    [Authorize]
    [CustomAuthorize(Function = "ReportSalaries/Index")]
    public class ReportSalariesController : BaseController
    {
        ChamCong_teamdevEntities db_ = new ChamCong_teamdevEntities();
        // GET: BaoCaoBangLuong
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _GetList(string UsernameSearch, string ThangSearch)
        {
            DateTime _thangNam = DateTime.UtcNow.AddHours(7);
            if (!string.IsNullOrEmpty(ThangSearch))
            {
                CultureInfo cul = CultureInfo.GetCultureInfo("en-GB");
                try
                {
                    ThangSearch += "-01";
                    _thangNam = Convert.ToDateTime(ThangSearch);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            var data = db_.Sp_GetListReportSalaries(User.idDepartment).Where(a =>
            a.dateTime.Year == _thangNam.Year && a.dateTime.Month == _thangNam.Month &&
            ((UsernameSearch == "" || UsernameSearch == null || a.Username.ToString() == UsernameSearch)
            )).ToList();
            ViewBag.data = data;
            return PartialView();
        }
        public ActionResult _Search()
        {
            var data = db_.AspNetUsers.Where(a => a.idDepartment == User.idDepartment && a.AccoutType == "User").ToList();
            ViewBag.data = data;
            return PartialView();
        }
        public ActionResult _SelectMonth()
        {
            ViewData["token"] = User.token;
            ViewData["username"] = User.UserName;
            return PartialView();
        }
        public ActionResult _DemoImportExcel()
        {
            ViewData["Username"] = User.UserName;
            var data = db_.ReportSalariesTmps.ToList();
            ViewBag.data = data;
            return PartialView();
        }
    }
}