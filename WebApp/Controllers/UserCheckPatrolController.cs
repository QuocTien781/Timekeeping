using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToolsApp.Authentication;
using ToolsApp.EntityFramework;
using ToolsApp.Utilities;

namespace ToolsApp.Controllers
{
    [Authorize]
    [CustomAuthorize(Function = "UserCheckPatrol/Index")]
    public class UserCheckPatrolController : BaseController
    {
        ChamCong_teamdevEntities db_ = new ChamCong_teamdevEntities();
        // GET: UserCheckPatrol
        public ActionResult Index()
        {
            var nameLocation = db_.Jobs.Where(a => a.UserCreate == User.UserName && a.status == true).ToList();
            ViewBag.nameLocation = nameLocation;
            var dataUser = db_.AspNetUsers.Where(a => a.idDepartment == User.idDepartment && a.AccoutType == "User").ToList();
            ViewBag.dataUser = dataUser;
            return View();
        }
        public ActionResult checkImage(int Id)
        {   
            var data = db_.User_Check_Patrol.FirstOrDefault(a => a.Id == Id);
            var token = User.token;
            ViewData["token"] = token;
            ViewBag.data = data;
            return PartialView();
        }
        public ActionResult _GetList(string name, string nameJob, string time_InSearch, string time_OutSearch)
        {
            int isConfirm;
            int isNotConfirm;
            var lst = db_.Sp_LoadUserCheckPatrol(User.idDepartment, name, nameJob).ToList();
            try
            {
                #region xữ lý ngày
                if (!string.IsNullOrEmpty(time_InSearch) && !string.IsNullOrEmpty(time_OutSearch))
                {
                    CultureInfo cul = new CultureInfo("vi-VN");
                    var _NgayBatDau = new DateTime();
                    var _NgayKetthuc = new DateTime();
                    DateTime _NgayBD = new DateTime();
                    DateTime _NgayKT = new DateTime();
                    try
                    {
                        _NgayBatDau = DateTime.ParseExact(time_InSearch, "dd/MM/yyyy", cul);

                        _NgayBD = new DateTime(_NgayBatDau.Year, _NgayBatDau.Month, _NgayBatDau.Day, 0, 0, 0);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    try
                    {
                        _NgayKetthuc = DateTime.ParseExact(time_OutSearch, "dd/MM/yyyy", cul);

                        _NgayKT = _NgayKetthuc.Date.AddDays(1).AddSeconds(-1);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                    var data = lst.Where(a =>
                    (name == "" || name == null || a.UserName.ToLower().Contains(name.ToLower())) &&
                    (nameJob == "" || nameJob == null || a.NameLocation.ToLower().Contains(nameJob.ToLower())) &&
                    (time_InSearch == "" || time_InSearch == null || a.Time_Check >= _NgayBD) &&
                    (time_OutSearch == "" || time_OutSearch == null || a.Time_Check <= _NgayKT)
                    ).ToList();
                    isConfirm = data.Count(a => a.Is_Confim == true);
                    isNotConfirm = data.Count(a => a.Is_Confim == false);
                    ViewData["isConfirm"] = isConfirm;
                    ViewData["isNotConfirm"] = isNotConfirm;
                    ViewBag.data = data;
                }
                else
                {
                    var data = lst.Where(a =>
                     (name == "" || name == null || a.UserName.ToLower().Contains(name.ToLower())) &&
                     (nameJob == "" || nameJob == null || a.NameLocation.ToLower().Contains(nameJob.ToLower()))).ToList();
                    isConfirm = data.Count(a => a.Is_Confim == true);
                    isNotConfirm = data.Count(a => a.Is_Confim == false);
                    ViewData["isConfirm"] = isConfirm;
                    ViewData["isNotConfirm"] = isNotConfirm;
                    ViewBag.data = data;
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return PartialView();
   
        }
        [HttpPost]
        public JsonResult _ExportExcelAll(string name, string nameJob, string timeIn, string timeOut)
        {
         
            var job = db_.Jobs.FirstOrDefault(a => a.NameLocation == nameJob);
            int nameJobInt;
            bool success = int.TryParse(job.Id.ToString(), out nameJobInt);
            if (success)
            {
                if (!string.IsNullOrEmpty(timeIn) && !string.IsNullOrEmpty(timeOut))
                {
                    CultureInfo cul = CultureInfo.GetCultureInfo("en-GB");
                    var _NgayBatDau = new DateTime();
                    var _NgayKetthuc = new DateTime();
                    DateTime _NgayBD = new DateTime();
                    DateTime _NgayKT = new DateTime();
                    try
                    {
                        _NgayBatDau = DateTime.ParseExact(timeIn, "dd/MM/yyyy", cul);

                        _NgayBD = new DateTime(_NgayBatDau.Year, _NgayBatDau.Month, _NgayBatDau.Day, 0, 0, 0);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    try
                    {
                        _NgayKetthuc = DateTime.ParseExact(timeOut, "dd/MM/yyyy", cul);

                        _NgayKT = _NgayKetthuc.Date.AddDays(1).AddSeconds(-1);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    var lst = (name == null || name == "" ? (from a in db_.User_Check_Patrol
                                                             join b in db_.Jobs on a.id_Job equals b.Id
                                                             join c in db_.Qr_Patrol on a.QrCheck equals c.Id
                                                             join d in db_.AspNetUsers on a.Username_Check equals d.UserName
                                                             select new
                                                             {
                                                                 Id = a.Id,
                                                                 Username_Check = a.Username_Check,
                                                                 Fullname = d.Fullname,
                                                                 NameLocation = b.NameLocation,
                                                                 QR_Check = c.Name,
                                                                 Time_Check = a.Time_Check,
                                                                 location_X_Check = a.location_X_Check,
                                                                 location_Y_Check = a.location_Y_Check,
                                                                 isLocation = a.isLocation
                                                             }) :
                                            (from a in db_.User_Check_Patrol
                                             join b in db_.Jobs on a.id_Job equals b.Id
                                             join c in db_.Qr_Patrol on a.QrCheck equals c.Id
                                             join d in db_.AspNetUsers on a.Username_Check equals d.UserName
                                             where (a.Username_Check == name && a.id_Job == nameJobInt)
                                             select new
                                             {
                                                 Id = a.Id,
                                                 Username_Check = a.Username_Check,
                                                 Fullname = d.Fullname,
                                                 NameLocation = b.NameLocation,
                                                 QR_Check = c.Name,
                                                 Time_Check = a.Time_Check,
                                                 location_X_Check = a.location_X_Check,
                                                 location_Y_Check = a.location_Y_Check,
                                                 isLocation = a.isLocation
                                             })).ToList();
                    var data = lst.Where(a =>
                      (a.Time_Check != null && a.Time_Check >= _NgayBD && a.Time_Check <= _NgayKT)).ToList();
                    if (data.Count() > 0)
                    {
                        string fileIn = Request.PhysicalApplicationPath + @"UserFiles\Template\" + "Template_TuanTra.xlsx";

                        string fileOutName = "DachSachTuanTra_" + DateTime.Now.Ticks + "_" + Guid.NewGuid().ToString().Substring(0, 7) + ".xlsx";
                        SLDocument sl = new SLDocument(fileIn);
                        sl.SelectWorksheet(sl.GetSheetNames()[0]);
                        SLWorksheetStatistics stats = sl.GetWorksheetStatistics();

                        var row = 3;
                        #region style
                        SLStyle style = sl.CreateStyle();
                        style.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
                        style.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
                        style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
                        style.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
                        style.Border.TopBorder.Color = System.Drawing.Color.Black;
                        style.Border.BottomBorder.Color = System.Drawing.Color.Black;
                        style.Border.LeftBorder.Color = System.Drawing.Color.Black;
                        style.Border.RightBorder.Color = System.Drawing.Color.Black;
                        
                        #endregion
                        try
                        {
                        
                            sl.SetCellValue("A1", "CHƯƠNG TRÌNH BÁO CÁO TUẦN TRA TỪ NGÀY" + " " + _NgayBD.Day + "-" + _NgayBD.Month + "-" + _NgayBD.Year + " ĐẾN NGÀY" + " "+ _NgayKT.Day + "-" + _NgayKT.Month + "-" + _NgayKT.Year);
                            foreach (var item in data)
                            {
                                string location = $"{item.location_X_Check} - {item.location_Y_Check}";
                                sl.SetCellValue("A" + row.ToString(), item.Id);
                                sl.SetCellValue("B" + row.ToString(), item.Username_Check);
                                sl.SetCellValue("C" + row.ToString(), item.Fullname);
                                sl.SetCellValue("D" + row.ToString(), item.NameLocation);
                                sl.SetCellValue("E" + row.ToString(), item.QR_Check);
                                sl.SetCellValue("F" + row.ToString(), item.Time_Check);
                                sl.SetCellValue("G" + row.ToString(), location);
                                sl.SetCellValue("H" + row.ToString(), item.isLocation);
                                sl.SetCellValue("I" + row.ToString(), item.Time_Check == null ? "Chưa xác nhận" : item.Time_Check.ToString("dd/MM/yyyy HH:mm:ss"));
                                row++;
                            }
                            sl.SetCellStyle("A3", "I" + (row - 1), style);
                            string targetDirectory = Server.MapPath("~/UserFiles/Download/");
                            sl.SaveAs(Path.Combine(targetDirectory, fileOutName));
                            return Json((new { status = 1, title = "", text = "Xuất excel thành công.", obj = Utils.WebConfigKey.Domain + "/UserFiles/Download/" + fileOutName }), JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception ex)
                        {
                            return Json((new { status = -1, title = "", text = ex.Message, obj = "" }), JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json((new { status = -1, title = "", text = "Không có dữ liệu để xuất. Vui lòng chọn lại ngày bắt đầu hoặc địa điểm khác.", obj = "" }), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json((new { status = -1, title = "", text = "Vui lòng nhập ngày bắt đầu ngày kết thúc", obj = "" }), JsonRequestBehavior.AllowGet);
                }

            }
        
            else
            {
                return Json((new { status = -1, title = "", text = "Vui lòng chọn lại địa điểm làm việc", obj = "" }), JsonRequestBehavior.AllowGet);
            }

        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult Confirm(int Id)
        {
            if (ModelState.IsValid)
            {


                var item = db_.User_Check_Patrol.Where(a => a.Id == Id).FirstOrDefault();
                if (item == null)
                {
                    return Json(new { status = -1, title = "", text = "Không tìm thấy data", obj = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (item.location_X_Check == null || item.location_X_Check == "")
                    {
                        return Json(new { status = -1, title = "", text = "Xác nhận không thành công. Người dùng chưa check in (X)", obj = "" }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        if (item.location_Y_Check == null || item.location_Y_Check == "")
                        {
                            return Json(new { status = -1, title = "", text = "Xác nhận không thành công. Người dùng chưa check in (Y)", obj = "" }, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {
                            if (item.imageName == null)
                            {
                                return Json(new { status = -1, title = "", text = "Xác nhận không thành công. Chưa cập nhật được ảnh người dùng", obj = "" }, JsonRequestBehavior.AllowGet);

                            }
                            else
                            {
                                if (item.Time_Check == null)
                                {
                                    return Json(new { status = -1, title = "", text = "Xác nhận không thành công. Chưa kiểm tra được thời gian checkIn ", obj = "" }, JsonRequestBehavior.AllowGet);

                                }  
                            }
                        }
                    }
                }

                try
                {
                    item.Is_Confim = true;
                    item.TimeConfim = DateTime.Now;
                    db_.Entry(item).State = EntityState.Modified;
                    db_.SaveChanges();
                    return Json(new { status = 1, title = "", text = "Xác nhận thành công", obj = "" }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception e)
                {
                    return Json(new { status = -1, title = "", text = "Xác nhận không thành công. Lỗi api", obj = "" }, JsonRequestBehavior.AllowGet);

                }
            }
            else
            {
                return Json(new { status = -1, title = "", text = "Xác nhận không thành công. Lỗi", obj = "" }, JsonRequestBehavior.AllowGet);

            }
        }
    }
}

