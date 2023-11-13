using System;
using System.Linq;
using System.Web.Mvc;
using ToolsApp.Authentication;
using ToolsApp.EntityFramework;
using ToolsApp.Models;

namespace ToolsApp.Controllers
{
    public class NotificationController : BaseController
    {
        ChamCong_teamdevEntities db_ = new ChamCong_teamdevEntities();
        [Authorize]
        [CustomAuthorize(Function = "Notification/Index")]
        public ActionResult Index()
        {
            ViewData["token"] = User.token??"";
            var listNoti = db_.Sp_GetNotification(User.idDepartment).Where(a => a.Status == true).ToList();
            ViewBag.listNoti = listNoti;
            var listStaff = db_.AspNetUsers.Where(a => a.TokenNotification != null).ToList();
            ViewBag.listStaff = listStaff;
            return View();
        }
        public ActionResult _GetList(string search)
        {
            search = search?.Trim();
            var data = db_.Sp_GetNotification(User.idDepartment).Where(a =>
               (search == "" || search == null || a.Username.ToUpper().Contains(search.ToUpper())) &&
                a.Status == true ).ToList();
            ViewBag.data = data;
            return PartialView();
        }
        public ActionResult _Insert()
        {
            return PartialView();
        }
        public ActionResult _GetListUser()
        {
            var listStaff = db_.AspNetUsers.Where(a => a.AccoutType == "User" && a.idDepartment == User.idDepartment && a.TokenNotification != null).ToList();
            ViewBag.listStaff = listStaff;
            return PartialView();
        }
        public ActionResult _Search()
        {
            var listStaff = db_.AspNetUsers.Where(a => a.AccoutType == "User" && a.idDepartment == User.idDepartment).ToList();
            ViewBag.listStaff = listStaff;
            return PartialView();
        }
        public ActionResult FilterDataStaff(string searchStaffValue)
        {
            searchStaffValue = searchStaffValue?.Trim();
            var filteredDataStaff = db_.AspNetUsers
                               .Where(a => a.Fullname.ToUpper().Contains(searchStaffValue.ToUpper()) && a.idDepartment == User.idDepartment && a.TokenNotification != null)
                               .ToList();

            return Json(filteredDataStaff, JsonRequestBehavior.AllowGet);
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult _PushNSaveNotification(notification_ViewModel model)
        {
            try
            {

                if (model.Title == "" || model.Title == null)
                {
                    return Json(new { status = -1, title = "", text = "Vui lòng nhập tên thông báo", obj = "" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    if (model.Message == "" || model.Message == null)
                    {
                        return Json(new { status = -1, title = "", text = "Vui lòng nhập nội dung thông báo", obj = "" }, JsonRequestBehavior.AllowGet);

                    }
                }
                var model_copy = new notification();
                model_copy.Title = model.Title;
                model_copy.Username = model.Username;
                model_copy.Time = DateTime.Now;
                model_copy.Status = true;
                model_copy.Message = model.Message;
                model_copy.Token = "";
                db_.notifications.Add(model_copy);
                db_.SaveChanges();
                return Json(new { status = 1, title = "", text = "Lưu thông báo thành công", obj = "" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { status = -1, title = "", text = ex.Message, obj = "" }, JsonRequestBehavior.AllowGet);
            }
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult _Delete( int Id)
        {
            var item = db_.notifications.FirstOrDefault(a => a.Id == Id);
            if (item == null)
            {
                return Json(new { status = -1, title = "", text = "Lỗi không tìm thấy thông báo", obj = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                db_.notifications.Remove(item);
                db_.SaveChanges();
                return Json(new { status = 1, title = "", text = "Xóa thành công.", obj = "" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
        
}