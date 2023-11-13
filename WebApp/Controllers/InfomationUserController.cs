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
    public class InfomationUserController : BaseController
    {
        ChamCong_teamdevEntities db_ = new ChamCong_teamdevEntities();
        // GET: Infomation
        public ActionResult Index(string Id)
        {
            return View();
        }
        public ActionResult _Image_Avatar_View(string Id)
        {
            ViewData["Id"] = Id;
            return PartialView();
        } 
        public ActionResult _Image_View(string Id)
        {
            var user = db_.ProfileUsers.FirstOrDefault(a => a.Id == Id);
            ViewBag.user = user;
            ViewData["Id"] = Id;
            return PartialView();
        }

        public ActionResult ThongTinCoBan_View(string Id)
        {
            ViewData["Id"] = Id;
            var data = db_.ProfileUsers.FirstOrDefault(a => a.Id == Id);
            ViewBag.data = data;
            var user = db_.AspNetUsers.FirstOrDefault(b => b.Id == Id);
            var token = User.token;
            ViewData["token"] = token;
            ViewBag.user = user;
            return PartialView();
        }
        public ActionResult ThongTinLienHe_View(string Id)
        {
            ViewData["Id"] = Id;
            var data = db_.ProfileUsers.FirstOrDefault(a => a.Id == Id);
            ViewBag.data = data;
            var user = db_.AspNetUsers.FirstOrDefault(b => b.Id == Id);
            var token = User.token;
            ViewData["token"] = token;
            ViewBag.user = user;
            return PartialView();
        }
        public ActionResult ThôngTinCaNhan_View(string Id)
        {
            ViewData["Id"] = Id;
            var data = db_.ProfileUsers.FirstOrDefault(a => a.Id == Id);
            ViewBag.data = data;
            var token = User.token;
            ViewData["token"] = token;
            return PartialView();
        }





        public ActionResult _ViewCCCD(string Id)
        {
            var user = db_.ProfileUsers.FirstOrDefault(a => a.Id == Id);
            ViewBag.user = user;
            return PartialView();
        }

        public ActionResult UserProfile(string Id)
        {
            var data = db_.ProfileUsers.FirstOrDefault(a => a.Id == Id);
            var _data = new ProfileUser();
            if (data == null)
            {
                _data.Id = Id;
                db_.ProfileUsers.Add(_data);
                db_.SaveChanges();
            }
            ViewBag.data = data == null ? _data : data;
            var user = db_.AspNetUsers.FirstOrDefault(b => b.Id == Id);
            var token = User.token;
            ViewData["token"] = token;
            var jobs = db_.Sp_LoadJobFromUserName(user.UserName).ToList();
            ViewBag.jobs = jobs;
            ViewBag.user = user;
            return View("User");
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult _SaveImageAvatar(string image, string Id)
        {
            try
            {
                var user = db_.ProfileUsers.FirstOrDefault(a => a.Id == Id);
                user.imageName = image;
                db_.Entry(user).State = EntityState.Modified;
                db_.SaveChanges();
                return Json(new { status = 1, title = "", text = "Cập nhật ảnh thành công.", obj = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = -1, title = "", text = "Lỗi: Không cấu trúc api", obj = "" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult _SaveImageCCCD(string frontCCCD,string backCCCD, string Id)
        {
            try
            {
                var user = db_.ProfileUsers.FirstOrDefault(a => a.Id == Id);
                user.imageFrontCCCD = frontCCCD;
                user.imageBackCCCD = backCCCD;
                db_.Entry(user).State = EntityState.Modified;
                db_.SaveChanges();
                return Json(new { status = 1, title = "", text = "Cập nhật ảnh thành công.", obj = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = -1, title = "", text = "Lỗi: Không cấu trúc api", obj = "" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult _SaveInfo(ProfileUserviewModel model, string Id)
        {
            var item = db_.ProfileUsers.FirstOrDefault(a => a.Id == Id);
            try
            {
                item.birtDate = model.birtDate;
                item.placeOfBirt = model.placeOfBirt;
                item.placeOfOrigin = model.placeOfOrigin;
                item.curResidence = model.curResidence;
                item.resident = model.resident;
                item.temResidence = model.temResidence;
                item.gender = model.gender;
                item.taxCode = model.taxCode;
                item.marital = model.marital;
                item.ethnicity = model.ethnicity;
                item.religion = model.religion;
                item.nationality = model.nationality;
                item.educationalLevel = model.educationalLevel;
                db_.Entry(item).State = EntityState.Modified;
                db_.SaveChanges();
                return Json(new { status = 1, title = "", text = "Cập nhật thông tin cơ bản thành công.", obj = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = -1, title = "", text = "Lỗi: Không cấu trúc api", obj = "" }, JsonRequestBehavior.AllowGet);
            }


        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult _SaveInfoContact(ProfileUserviewModel model, string Id, string Email)
        {
            var user = db_.AspNetUsers.FirstOrDefault(a => a.Id == Id);
            var item = db_.ProfileUsers.FirstOrDefault(a => a.Id == Id);
            if (model.otherNum !=null && model.otherNum.Length > 0)
            {
                if (model.otherNum.Length != 10)
                {
                    return Json(new { status = -1, title = "", text = "Số điện thoại chưa hợp lệ", obj = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    try
                    {
                        user.Email = Email;
                        item.otherNum = model.otherNum;
                        item.address = model.address;
                        item.zalo = model.zalo;
                        db_.Entry(item).State = EntityState.Modified;
                        db_.SaveChanges     ();
                        return Json(new { status = 1, title = "", text = "Cập nhật thông tin liên hệ thành công.", obj = "" }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        return Json(new { status = -1, title = "", text = "Lỗi: Không cấu trúc api", obj = "" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                try
                {
                    user.Email = Email;
                    item.otherNum = model.otherNum;
                    item.address = model.address;
                    item.zalo = model.zalo;
                    db_.Entry(item).State = EntityState.Modified;
                    db_.SaveChanges();
                    return Json(new { status = 1, title = "", text = "Cập nhật thông tin liên hệ thành công.", obj = "" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { status = -1, title = "", text = "Lỗi: Không cấu trúc api", obj = "" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult _SaveInfoPersonal(ProfileUserviewModel model, string Id)
        {
            var item = db_.ProfileUsers.FirstOrDefault(a => a.Id == Id);
            try
            {
                item.CCCD = model.CCCD;
                item.passPortNum = model.passPortNum;
                item.codeBank = model.codeBank;
                item.numBank = model.numBank;
                item.nameAccountBank = model.nameAccountBank;
                db_.Entry(item).State = EntityState.Modified;
                db_.SaveChanges();
                return Json(new { status = 1, title = "", text = "Cập nhật thông tin cá nhân thành công.", obj = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = -1, title = "", text = "Lỗi: Không cấu trúc api", obj = "" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}