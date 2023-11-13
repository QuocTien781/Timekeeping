using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToolsApp.Authentication;
using ToolsApp.EntityFramework;
using ToolsApp.Models;

namespace ToolsApp.Controllers
{
    [Authorize]
    [CustomAuthorize(Function = "Jobs/Index")]
    public class JobsController : BaseController
    {
        ChamCong_teamdevEntities db_ = new ChamCong_teamdevEntities();
        // GET: Jobs
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _GetList(string namLocationSearch, string administrativeAreaSearch, string subAdministrativeAreaSearch)
        {
            namLocationSearch = namLocationSearch?.Trim();
            var list = db_.Jobs.Where(a=>
            (namLocationSearch == "" || namLocationSearch == null || a.NameLocation.ToUpper().Contains(namLocationSearch.ToUpper())) &&
            (administrativeAreaSearch == "" || administrativeAreaSearch == null || a.administrativeArea.ToUpper().Contains(administrativeAreaSearch.ToUpper())) &&
            (subAdministrativeAreaSearch == "" || subAdministrativeAreaSearch == null || a.subAdministrativeArea.ToUpper().Contains(subAdministrativeAreaSearch.ToUpper())) &&
            a.UserCreate == User.UserName && a.status == true).ToList();
            ViewBag.list = list;
            return PartialView();
        }
        public ActionResult _Insert_View()
        {
            return PartialView();
        }
        public ActionResult _Edit(int Id)
        {
            var data = db_.Jobs.Where(a => a.IdDepartment == User.idDepartment && a.Id== Id).FirstOrDefault();
            ViewBag.model = data;
            return PartialView();
        }

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult _InsertFun(Job_ViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var tb = db_.Jobs.FirstOrDefault(p => p.NameLocation == model.NameLocation);
                    if (tb != null)
                    {
                        return Json(new { status = -1, title = "", text = "Thêm không thành công, Địa điểm làm việc đã tồn tại", obj = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (model.NameLocation == null || model.NameLocation == "")
                        {
                            return Json(new { status = -1, title = "", text = "Vui lòng nhập địa điểm làm việc", obj = "" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            if (model.subAdministrativeArea == null || model.subAdministrativeArea.Trim() == "")
                            {
                                return Json(new { status = -1, title = "", text = "Nhập quận / huyện .", obj = "" }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                if (model.Location_X == null)
                                {
                                    return Json(new { status = -1, title = "", text = "Vui lòng nhập vĩ độ X", obj = "" }, JsonRequestBehavior.AllowGet);

                                }
                                else
                                {
                                    if (model.Location_Y == null)
                                    {
                                        return Json(new { status = -1, title = "", text = "Vui lòng nhập tung độ Y ", obj = "" }, JsonRequestBehavior.AllowGet);

                                    }
                                    else
                                    {
                                        if (model.administrativeArea == null || model.administrativeArea == "")
                                        {
                                            return Json(new { status = -1, title = "", text = "Vui lòng nhập thành phố", obj = "" }, JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                }
                            }
                        }
                    }
                       

                    var model_copy = new Job();
                    model_copy.NameLocation = model.NameLocation;
                    model_copy.subAdministrativeArea = model.subAdministrativeArea;
                    model_copy.UserCreate = User.UserName;
                    model_copy.TimeCreate = DateTime.Now;
                    model_copy.Location_X = model.Location_X;
                    model_copy.Location_Y = model.Location_Y;
                    model_copy.IdDepartment = User.idDepartment;
                    model_copy.administrativeArea = model.administrativeArea;
                    model_copy.country = model.country;
                    model_copy.street = model.street;
                    model_copy.status = true;
                    db_.Jobs.Add(model_copy);
                    db_.SaveChanges();
                    return Json(new { status = 1, title = "", text = "Thêm thành công.", obj = "" }, JsonRequestBehavior.AllowGet);


                }
                catch (Exception ex)
                {
                    return Json(new { status = -1, title = "", text = ex.Message, obj = "" }, JsonRequestBehavior.AllowGet);
                }


            }
            else
            {

                return Json(new { status = -1, title = "", text = "Thêm không thành công.", obj = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult _EditFun(Job_ViewModel model, int Id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.NameLocation == null || model.NameLocation == "")
                    {
                        return Json(new { status = -1, title = "", text = "Vui lòng nhập địa điểm làm việc", obj = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (model.subAdministrativeArea == null || model.subAdministrativeArea.Trim() == "")
                        {
                            return Json(new { status = -1, title = "", text = "Nhập quận / huyện .", obj = "" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            if (model.Location_X == null)
                            {
                                return Json(new { status = -1, title = "", text = "Vui lòng nhập toạ độ X", obj = "" }, JsonRequestBehavior.AllowGet);

                            }
                            else
                            {
                                if (model.Location_Y == null)
                                {
                                    return Json(new { status = -1, title = "", text = "Vui lòng nhập toạ độ Y ", obj = "" }, JsonRequestBehavior.AllowGet);

                                }
                                else
                                {
                                    if (model.administrativeArea == null || model.administrativeArea == "")
                                    {
                                        return Json(new { status = -1, title = "", text = "Vui lòng nhập thành phố", obj = "" }, JsonRequestBehavior.AllowGet);
                                    }
                                }

                            }
                        }
                    }
                   
                    var model_copy = db_.Jobs.Where(a => a.Id == Id).FirstOrDefault();
                    model_copy.NameLocation = model.NameLocation;
                    model_copy.subAdministrativeArea = model.subAdministrativeArea;
                    model_copy.UserCreate = User.UserName;
                    model_copy.Location_X = model.Location_X;
                    model_copy.Location_Y = model.Location_Y;
                    model_copy.administrativeArea = model.administrativeArea;
                    model_copy.country = model.country;
                    model_copy.street = model.street;
                    db_.Entry(model_copy).State = EntityState.Modified;
                    db_.SaveChanges();
                    return Json(new { status = 1, title = "", text = "Cập nhật thành công.", obj = "" }, JsonRequestBehavior.AllowGet);


                }
                catch (Exception ex)
                {
                    return Json(new { status = -1, title = "", text = ex.Message, obj = "" }, JsonRequestBehavior.AllowGet);
                }


            }
            else
            {

                return Json(new { status = -1, title = "", text = "Cập nhật không thành công.", obj = "" }, JsonRequestBehavior.AllowGet);
            }
        }
        //Delete
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult _DeleteFun(int Id)
        {
            try
            {
                var item = db_.Jobs.FirstOrDefault(p => p.Id == Id);
                var checkDetailJob = db_.JobDetails.FirstOrDefault(a => a.idjob == Id);
                if (item != null && checkDetailJob !=null)
                {
                    return Json(new { status = -1, title = "", text = "Không thể xoá, công việc này đã có trong chi tiết công việc", obj = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    item.status = false;
                    db_.Entry(item).State = EntityState.Modified;
                    db_.SaveChanges(); 
                    return Json(new { status = 1, title = "", text = "Xóa thành công.", obj = "" }, JsonRequestBehavior.AllowGet);
                }
   
            }
            catch (Exception ex)
            {
                return Json(new { status = -1, title = "", text = "Không thể xoá", obj = "" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}