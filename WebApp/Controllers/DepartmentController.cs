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
    [CustomAuthorize(Function = "Department/Index")]
    public class DepartmentController : BaseController
    {
        ChamCong_teamdevEntities db_ = new ChamCong_teamdevEntities();
        // GET: Department
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _GetList(string NameCompany)
        {
            NameCompany = NameCompany?.Trim();
            var list = db_.Departments.Where(a => 
            (NameCompany == "" || NameCompany == null || a.Name.ToUpper().Contains(NameCompany.ToUpper()))).ToList();
            ViewBag.list = list;
            return PartialView();
        }
        public ActionResult _Insert()
        {
          
            return PartialView();
        }
        public ActionResult _Edit(int Id)
        {
            var data = db_.Departments.FirstOrDefault(p => p.Id == Id); 
            ViewBag.model = data;
            return PartialView();
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult _InsertFun(DepartmentView_Model model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var taxCode = db_.Departments.FirstOrDefault( a => a.taxCode.Trim() == model.taxCode.Trim());
                    if (taxCode != null )
                    {
                        return Json(new { status = -1, title = "", text = "Mã số thuế không được trùng!!.", obj = "" }, JsonRequestBehavior.AllowGet);
                    }
                     if (model.Name == null || model.Name == "")
                        {

                            return Json(new { status = -1, title = "", text = "Vui lòng nhập tên công ty", obj = "" }, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {
                            if (model.city == null)
                            {
                                return Json(new { status = -1, title = "", text = "Vui lòng chọn thành phố", obj = "" }, JsonRequestBehavior.AllowGet);

                            }
                            else
                            {
                                if (model.district == null)
                                {
                                    return Json(new { status = -1, title = "", text = "Vui lòng chọn quận / huyện", obj = "" }, JsonRequestBehavior.AllowGet);

                                }
                                else
                                {
                                    if (model.phone == null)
                                    {
                                        return Json(new { status = -1, title = "", text = "Vui lòng nhập số điện thoại công ty", obj = "" }, JsonRequestBehavior.AllowGet);

                                    }
                                }
                            }

                        }
                        var model_copy = new Department();
                        model_copy.Name = model.Name;
                        model_copy.taxCode = model.taxCode;
                        model_copy.Email = model.Email;
                        model_copy.phone = model.phone;
                        model_copy.country = model.country;
                        model_copy.Location = model.Location;
                        model_copy.city = model.city;
                        model_copy.district = model.district;
                        model_copy.ward = model.ward;
                        model_copy.street = model.street;
                        model_copy.status = model.status;
                        db_.Departments.Add(model_copy);
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
        public JsonResult _EditFun(DepartmentView_Model model, int Id)
        {
            if (ModelState.IsValid)
            {

                try
                {

                    if (model.Name == null || model.Name == "")
                    {
                        return Json(new { status = -1, title = "", text = "Vui lòng nhập tên công ty", obj = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (model.city == null)
                        {
                            return Json(new { status = -1, title = "", text = "Vui lòng chọn thành phố", obj = "" }, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {
                            if (model.district == null)
                            {
                                return Json(new { status = -1, title = "", text = "Vui lòng chọn quận / huyện", obj = "" }, JsonRequestBehavior.AllowGet);

                            }
                            else
                            {
                                if (model.phone == null)
                                {
                                    return Json(new { status = -1, title = "", text = "Vui lòng nhập số điện thoại công ty", obj = "" }, JsonRequestBehavior.AllowGet);

                                }
                            }
                        }

                    }
                    var taxCode = db_.Departments.Where(a => a.taxCode.Trim().ToUpper() == model.taxCode.Trim().ToUpper() && a.Id != Id).ToList();
                    if (taxCode.Count() == 0)
                    {
                        try
                        {
                            var model_copy = db_.Departments.Where(a => a.Id == Id).FirstOrDefault();
                            model_copy.Name = model.Name;
                            model_copy.taxCode = model.taxCode.Trim().ToUpper();
                            model_copy.Email = model.Email;
                            model_copy.phone = model.phone;
                            model_copy.country = model.country;
                            model_copy.Location = model.Location;
                            model_copy.city = model.city;
                            model_copy.district = model.district;
                            model_copy.ward = model.ward;
                            model_copy.street = model.street;
                            model_copy.status = model.status;
                            db_.Entry(model_copy).State = EntityState.Modified;
                            db_.SaveChanges();
                        }
                        catch
                        {
                            return Json(new { status = -1, title = "", text = "Lỗi dữ liệu đầu vào.", obj = "" }, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new { status = 1, title = "", text = "Cập nhật thành công.", obj = "" }, JsonRequestBehavior.AllowGet);

                    }
                    return Json(new { status = -1, title = "", text = "Cập nhật không thành công mã số thuế đả tồn tại .", obj = "" }, JsonRequestBehavior.AllowGet);
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
                 
                var item = db_.Departments.FirstOrDefault(p => p.Id == Id);

                if (item != null)
                {
                    item.status = false;
                    db_.Entry(item).State = EntityState.Modified;
                    db_.SaveChanges(); 
                }

                return Json(new { status = 1, title = "", text = "Deleted.", obj = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = -1, title = "", text = "Không thể xoá", obj = "" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
