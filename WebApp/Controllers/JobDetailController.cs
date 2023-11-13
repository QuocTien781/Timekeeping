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
    [CustomAuthorize(Function = "JobDetail/Index")]
    public class JobDetailController : BaseController
    {
        ChamCong_teamdevEntities db_ = new ChamCong_teamdevEntities();
        // GET: JobDetail
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _GetList(string JobNameSearch)
        {
            JobNameSearch = JobNameSearch?.Trim();
            var list = db_.JobDetails.Where(a =>
            (JobNameSearch == "" || JobNameSearch == null || a.job_Detail_Name.ToUpper().Contains(JobNameSearch.ToUpper())) &&
            a.UserCreate == User.UserName && a.IsStatus == true).ToList();
            ViewBag.list = list;
            return PartialView();
        }
        public ActionResult _Insert()
        {
            var list = db_.Jobs.Where(a => a.IdDepartment == User.idDepartment && a.status == true).ToList();
            ViewBag.list = list;
            return PartialView();
        }
        public ActionResult _Edit(int Id)
        {
            var data = db_.JobDetails.FirstOrDefault(p => p.Id == Id);
            var list = db_.Jobs.Where(a => a.IdDepartment == User.idDepartment && a.status == true).ToList();
            ViewBag.list = list;    
            ViewBag.model = data;
           return PartialView();
        }

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult _InsertFun(JobDetail_ViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                  
                        if (model.job_Detail_Name == null || model.job_Detail_Name == "")
                        {
                            return Json(new { status = -1, title = "", text = "Vui lòng nhập mô tả công việc", obj = "" }, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {
                            if (model.JobDescription == null || model.JobDescription == "")
                            {
                                return Json(new { status = -1, title = "", text = "Vui lòng nhập chi tiết công việc", obj = "" }, JsonRequestBehavior.AllowGet);

                            }
                        }
                    

                    var model_copy = new JobDetail();
                    model_copy.idjob = model.idjob;
                    model_copy.job_Detail_Name = model.job_Detail_Name;
                    model_copy.JobDescription = model.JobDescription;
                    model_copy.TimeIn = model.TimeIn;
                    model_copy.TimeOut = model.TimeOut;
                    model_copy.IsStatus = true;
                    model_copy.UserCreate = User.UserName;
                    model_copy.CreateTime = DateTime.Now;   
                    db_.JobDetails.Add(model_copy);
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
        public JsonResult _EditFun(JobDetail_ViewModel model, int Id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.job_Detail_Name == null || model.job_Detail_Name == "")
                    {
                        return Json(new { status = -1, title = "", text = "Vui lòng nhập mô tả công việc", obj = "" }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        if (model.JobDescription == null || model.JobDescription == "")
                        {
                            return Json(new { status = -1, title = "", text = "Vui lòng nhập chi tiết công việc", obj = "" }, JsonRequestBehavior.AllowGet);

                        }
                    }


                    var model_copy = db_.JobDetails.Where(a => a.Id == Id).FirstOrDefault();
                    model_copy.idjob = model.idjob;
                    model_copy.job_Detail_Name = model.job_Detail_Name;
                    model_copy.JobDescription = model.JobDescription;
                    model_copy.TimeIn = model.TimeIn;
                    model_copy.TimeOut = model.TimeOut;
                    model_copy.UserUpdate = User.UserName;
                    model_copy.UpdateTime = DateTime.Now;
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
            if (ModelState.IsValid)
            {
                try
                {

                    var item = db_.JobDetails.FirstOrDefault(p => p.Id == Id);
                    var checkJob = db_.UserJobs.FirstOrDefault(a => a.Id_Detail_Job == item.Id && a.Status == true);
                    if (item != null && checkJob != null)
                    {
                        return Json(new { status = -1, title = "", text = "Không thể xoá, công việc này đã được phân công cho nhân viên.", obj = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {

                        item.IsStatus = false;
                        item.UserDelete = User.UserName;
                        item.DeleteTime = DateTime.Now;
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
            else
            {
                return Json(new { status = -1, title = "", text = "Lỗi khi xóa.", obj = "" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
