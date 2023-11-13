using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToolsApp.Authentication;
using ToolsApp.EntityFramework;

namespace ToolsApp.Controllers
{
    [Authorize]
    [CustomAuthorize(Function = "PleaseTakeLeave/Index")]
    public class PleaseTakeLeaveController : BaseController
    {
        ChamCong_teamdevEntities db_ = new ChamCong_teamdevEntities();
        // GET: PleaseTakeLeave
        public ActionResult Index()
        {
            var DataUser = db_.AspNetUsers.Where(a => a.idDepartment == User.idDepartment && a.UserName != "Admin").ToList();
            ViewBag.DataUser = DataUser;
            return View();
        }
       
        public ActionResult _GetList(string name,string Ngay)
        {

            var lst = db_.Sp_LoadTakeLeave(User.idDepartment, name).ToList(); 
            if (!string.IsNullOrEmpty(Ngay)||Ngay!="")
            {
                CultureInfo cul = CultureInfo.GetCultureInfo("en-GB");
                var _NgayTime = new DateTime();
                DateTime _Ngay = new DateTime();
                try
                {
                    _NgayTime = DateTime.ParseExact(Ngay, "dd/MM/yyyy", cul);

                    _Ngay = new DateTime(_NgayTime.Year, _NgayTime.Month, _NgayTime.Day, 0, 0, 0);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                var data = lst.Where(a => a.DateTime >= _Ngay && a.DateTime <= _Ngay).ToList();
                ViewBag.DataSearch = data;
            }
            else
            {
                ViewBag.DataSearch = lst;
            }
            return PartialView();
        }
        public ActionResult _PrintView(int id)
        {
            var data = db_.Sp_PrintTakeLeave(id).FirstOrDefault();
            ViewBag.data = data;
            return PartialView();
        }
        [HttpPost]
        public JsonResult IsConfim(int id)
        {
            var dataUser = db_.AspNetUsers.Where(a => a.UserName == User.UserName).FirstOrDefault();
            if (dataUser.IsManager == false)
            {
                return Json(new { status = -1, title = "", text = "Bạn không có quyền sử dụng chức năng này vui lòng liên hệ Manager.", obj = "" }, JsonRequestBehavior.AllowGet);
            }
            var data = db_.PleaseTakeLeaves.Where(a => a.Id == id).FirstOrDefault();
            if (data == null)
            {
                return Json(new { status = -1, title = "", text = "Không tìm thấy data.", obj = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                data.isComfim = true;
                data.UserConfim = User.UserName;
                data.TimeConfim = DateTime.UtcNow.AddHours(7);
                db_.Entry(data).State = EntityState.Modified;
                db_.SaveChanges();
                return Json((new { status = 1, title = "", text = "Đã xác nhận nghỉ phép  " + data.username, obj = "" }), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult print(int id)
        {
             return Json(new { status = -1, title = "", text = "Bạn không có quyền sử dụng chức năng này vui lòng liên hệ Manager.", obj = "" }, JsonRequestBehavior.AllowGet);
          
        }
    }
}