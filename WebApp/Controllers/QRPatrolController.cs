using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToolsApp.Authentication;
using ToolsApp.EntityFramework;
using ToolsApp.Models;
using ZXing;
namespace ToolsApp.Controllers
{
    [Authorize]
    [CustomAuthorize(Function = "QRPatrol/Index")]
    public class QRPatrolController : BaseController
    {
        ChamCong_teamdevEntities db_ = new ChamCong_teamdevEntities();
        // GET: QRCode

        public ActionResult Index()
        {   
            return View();
        }
        public ActionResult _GetList(string id_Job)
        {
            var job = db_.Jobs.Where(a => a.UserCreate == User.UserName).ToList();
            var data = db_.Sp_GetQRPatrol(User.idDepartment).Where(a =>
             (id_Job == "" || id_Job == null || a.id_Job.ToString() == id_Job)).ToList();
            ViewBag.data = data;
            return PartialView();
        }
        public ActionResult _InsertCode()
        {
           
            var list = db_.Jobs.Where(a => a.IdDepartment == User.idDepartment && a.status == true).ToList();
            ViewBag.list = list;
            return PartialView();
        }

        public ActionResult _viewCodeQR()
        {
            var data = db_.Jobs.Where(a => a.IdDepartment == User.idDepartment).ToList();
            ViewBag.data = data;
            return PartialView(); 
        }
        public ActionResult _Edit(int Id)
        {
            var data = db_.Qr_Patrol.Where(a => a.Id == Id).FirstOrDefault();
            ViewBag.data = data;
            return PartialView();
        }
        public ActionResult _PrintView(int id)
        {
            var data = db_.Qr_Patrol.FirstOrDefault();
            ViewBag.data = data;
            return PartialView();
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult _GetLocation(int selectedValue)
        {
            var jobLocation = db_.Jobs.FirstOrDefault(a => a.Id == selectedValue);
            return Json(new { status = 1, title = "", text = "", obj = new { LocationX = jobLocation.Location_X, LocationY = jobLocation.Location_Y } }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Create_QrCode(Qr_Patrol_View model)
        {
            if (model.Name == "" || model.Name == null)
            {
                return Json(new { status = -1, title = "", text = "Vui lòng nhập vị trí làm việc", obj = "" }, JsonRequestBehavior.AllowGet);
            }
            if (model.Description == "" || model.Description == null)
            {
                return Json(new { status = -1, title = "", text = "Vui lòng nhập mô tả QR", obj = "" }, JsonRequestBehavior.AllowGet);
            }
            if (model.Location_X == "" || model.Location_X == null)
            {
                return Json(new { status = -1, title = "", text = "Vui lòng nhập Vị Trí X từ bản đồ", obj = "" }, JsonRequestBehavior.AllowGet);
            }
            if (model.Location_Y == "" || model.Location_Y == null)
            {
                return Json(new { status = -1, title = "", text = "Vui lòng nhập vị trí Y từ bản đồ", obj = "" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                //Get random Code
                string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                Random random = new Random();
                char[] result = new char[5];

                for (int i = 0; i < 5; i++)
                {
                    result[i] = characters[random.Next(characters.Length)];
                }

                var Code = new string(result);

                //Add Qr
                var item = new Qr_Patrol();
                item.id_Job = model.id_Job;
                item.code = Code;
                item.Name = model.Name;
                item.path_Qr = "";
                item.Description = model.Description;
                item.Location_X = model.Location_X;
                item.Location_Y = model.Location_Y;
                item.time_Check = 15;
                item.id_Job = model.id_Job;
                db_.Qr_Patrol.Add(item);
                db_.SaveChanges();
                BarcodeWriter barcodeWriter = new BarcodeWriter();
                barcodeWriter.Format = BarcodeFormat.QR_CODE;
                Bitmap qrCodeBitmap = barcodeWriter.Write(item.Id.ToString());
                MemoryStream ms = new MemoryStream();
                qrCodeBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                var base64Image = Convert.ToBase64String(ms.ToArray());
                var qrCodeData = new Qr_Patrol_View();
                item.path_Qr = "data:image / png; base64," + base64Image;
                db_.Entry(item).State = EntityState.Modified;
                db_.SaveChanges();
                return Json(new { status = 1, title = "", text = "Thêm QR thành công.", obj = "" }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                return Json(new { status = -1, title = "", text = ex.Message, obj = "" }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult Edit_QrCode(Qr_Patrol_View model)
        {
            if (model.Name == "" || model.Name == null)
            {
                return Json(new { status = -1, title = "", text = "Vui lòng nhập vị trí làm việc", obj = "" }, JsonRequestBehavior.AllowGet);
            }
            if (model.Description == "" || model.Description == null)
            {
                return Json(new { status = -1, title = "", text = "Vui lòng nhập mô tả QR", obj = "" }, JsonRequestBehavior.AllowGet);
            }
            if (model.Location_X == "" || model.Location_X == null)
            {
                return Json(new { status = -1, title = "", text = "Vui lòng nhập Vị Trí X từ bản đồ", obj = "" }, JsonRequestBehavior.AllowGet);
            }
            if (model.Location_Y == "" || model.Location_Y == null)
            {
                return Json(new { status = -1, title = "", text = "Vui lòng nhập vị trí Y từ bản đồ", obj = "" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                //Add Qr
                var item = db_.Qr_Patrol.Where(a => a.Id == model.Id).FirstOrDefault();
                item.Id = model.Id;
                item.id_Job = model.id_Job;
                BarcodeWriter barcodeWriter = new BarcodeWriter();
                barcodeWriter.Format = BarcodeFormat.QR_CODE;
                Bitmap qrCodeBitmap = barcodeWriter.Write(item.Id.ToString());
                MemoryStream ms = new MemoryStream();
                qrCodeBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                var base64Image = Convert.ToBase64String(ms.ToArray());
                var qrCodeData = new Qr_Patrol_View();
                item.Name = model.Name;
                item.path_Qr = "data:image/png;base64," + base64Image;
                item.Description = model.Description;
                item.Location_X = model.Location_X;
                item.Location_Y = model.Location_Y;
                item.id_Job = model.id_Job;
                db_.Entry(item).State = EntityState.Modified;
                db_.SaveChanges();
                return Json(new { status = 1, title = "", text = "Cập nhật QR thành công.", obj = "" }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                return Json(new { status = -1, title = "", text = ex.Message, obj = "" }, JsonRequestBehavior.AllowGet);
            }


        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult _Delete(int Id)
        {
            try
            {
                var item = db_.Qr_Patrol.FirstOrDefault(p => p.Id == Id);
                var checkUserCheckPatrol = db_.User_Check_Patrol.FirstOrDefault(a => a.QrCheck == Id);
                if (item != null && checkUserCheckPatrol!= null)
                {

                    return Json(new { status = -1, title = "", text = "Không thể xoá, dữ liệu QR này đã được dùng để quản lý tuần tra ", obj = "" }, JsonRequestBehavior.AllowGet);
                   
                }
                else
                {
                    db_.Qr_Patrol.Remove(item);
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


