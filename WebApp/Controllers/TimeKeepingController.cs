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
using ToolsApp.Models;
using ToolsApp.Utilities;

namespace ToolsApp.Controllers
{
    public class TimeKeepingController : BaseController
    {
        ChamCong_teamdevEntities db_ = new ChamCong_teamdevEntities();

        public ActionResult Index()
        {
            var dataUser = db_.AspNetUsers.Where(a => a.idDepartment == User.idDepartment && a.AccoutType == "User").ToList();
            var dataLocation = db_.Jobs.Where(a => a.UserCreate == User.UserName && a.status == true).ToList();
            ViewBag.dataUser = dataUser;
            ViewBag.dataLocation = dataLocation;
            return View();
        }
     
        public ActionResult update_view(int Id)
        {
            var dataUser = db_.AspNetUsers.Where(a => a.UserName == User.UserName).FirstOrDefault();
            if (dataUser.IsManager == false)
            {
                return RedirectToAction("NotAuthentizace", "Home");
            }
            else
            {
                var data = db_.Sp_LoadTimeKeeping(User.idDepartment).Where(a => a.id == Id).FirstOrDefault();
                ViewBag.data = data;
            }
            return PartialView();
        }
        public ActionResult _GetList(string FullnameSearch, string time_InSearch, string time_OutSearch, string locationSearch)
        {
            int isConfirm;
            int isNotConfirm;
            int isUpdate;
            var lst = db_.Sp_LoadTimeKeeping(User.idDepartment).Where(a => 
            (FullnameSearch == "" || FullnameSearch == null || a.Username.ToLower().Contains(FullnameSearch.ToLower())) &&
            (locationSearch == "" || locationSearch == null || a.NameLocation.ToLower().Contains(locationSearch.ToLower()))).ToList() ;
      
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

                        _NgayKT = new DateTime(_NgayKetthuc.Year, _NgayKetthuc.Month, _NgayKetthuc.Day, 0, 0, 0);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    var data = lst.Where(a =>
                    (FullnameSearch == "" || FullnameSearch == null || a.Username.ToLower().Contains(FullnameSearch.ToLower())) &&
                    (locationSearch == "" || locationSearch == null || a.NameLocation.ToLower().Contains(locationSearch.ToLower())) &&
                    (time_InSearch == "" || time_InSearch == null || a.Time_In >= _NgayBD) &&
                    (time_OutSearch == "" || time_OutSearch == null || a.Time_Out <= _NgayKT)
                    ).ToList();
                    isConfirm = data.Count(a => a.isConfirm == true);
                    isNotConfirm = data.Count(a => a.isConfirm == false);
                    isUpdate = data.Count(a => a.Admin_Messger_Update != null);
                    ViewData["isConfirm"] = isConfirm;
                    ViewData["isNotConfirm"] = isNotConfirm;
                    ViewData["isUpdate"] = isUpdate;
                    ViewBag.data = data;
                }
                else
                {
                    var data = lst.Where(a =>
                   (FullnameSearch == "" || FullnameSearch == null || a.Username.ToLower().Contains(FullnameSearch.ToLower()))
                   ).ToList();
                    isConfirm = data.Count(a => a.isConfirm == true);
                    isNotConfirm = data.Count(a => a.isConfirm == false);
                    isUpdate = data.Count(a => a.Admin_Messger_Update != null); 
                    ViewData["isConfirm"] = isConfirm;
                    ViewData["isNotConfirm"] = isNotConfirm;
                    ViewData["isUpdate"] = isUpdate;
                    ViewBag.data = data;
                }
                #endregion
            }
            catch (Exception ex)
            {
                ViewBag.data = lst;
            }
            return PartialView();
        }
        [NonAction]
        private double Sumtime(DateTime timeIn, DateTime timeOut)
        {
            try
            {
                if (timeIn == null || timeOut == null) return 0;
                else
                {
                    if (timeOut <= timeIn)
                    {
                        return 0;
                    }
                    else
                    {
                        TimeSpan workDuration = timeOut - timeIn;
                        double totalHours = workDuration.TotalHours;
                        double roundedHours = Math.Round(totalHours, 1);
                        return roundedHours;
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        [HttpPost]
        public JsonResult _ExportExcelAll(string Username, string timeIn, string timeOut)
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

                    _NgayKT = new DateTime(_NgayKetthuc.Year, _NgayKetthuc.Month, _NgayKetthuc.Day, 0, 0, 0);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                var lst = (Username == null || Username == "" ? (from a in db_.AttendanceFaces
                                                                 join b in db_.UserJobs on a.IdUserJob equals b.Id
                                                                 join c in db_.JobDetails on b.Id_Detail_Job equals c.Id
                                                                 join d in db_.AspNetUsers on b.Username equals d.UserName
                                                                 where b.Username == Username
                                                                 select new
                                                                 {
                                                                     id = a.id,
                                                                     Username = b.Username,
                                                                     FullName = d.Fullname,
                                                                     JobDetailName = c.job_Detail_Name,                                                                    
                                                                     locationInX = a.Location_In_X,
                                                                     locationInY = a.Location_In_Y,
                                                                     checkLocationIn = a.Check_Location_In,
                                                                     Time_In = a.Time_In,
                                                                     LocationOutX = a.Location_Out_X,
                                                                     locationOutY = a.Location_Out_Y,
                                                                     checkLocationOut = a.Check_Location_Out,
                                                                     Time_Out = a.Time_Out,
                                                                     Admin_Messger_Update = a.Admin_Messger_Update,
                                                                     TimeAdminMessageUpdate = a.Admin_time_Update,
                                                                     TimeConfirm = a.TimeConfirm,
                                                                     sumTime = a.sumTime,
                                                                 }) :
                                        (from a in db_.AttendanceFaces
                                         join b in db_.UserJobs on a.IdUserJob equals b.Id
                                         join c in db_.JobDetails on b.Id_Detail_Job equals c.Id
                                         join d in db_.AspNetUsers on b.Username equals d.UserName
                                         where b.Username == Username
                                         select new
                                         {
                                             id = a.id,
                                             Username = b.Username,
                                             FullName = d.Fullname, 
                                             JobDetailName = c.job_Detail_Name,
                                             locationInX = a.Location_In_X,
                                             locationInY = a.Location_In_Y,
                                             checkLocationIn = a.Check_Location_In,
                                             Time_In = a.Time_In,
                                             LocationOutX = a.Location_Out_X,
                                             locationOutY = a.Location_Out_Y,
                                             checkLocationOut = a.Check_Location_Out,
                                             Time_Out = a.Time_Out,
                                             Admin_Messger_Update = a.Admin_Messger_Update,
                                             TimeAdminMessageUpdate = a.Admin_time_Update,
                                             TimeConfirm = a.TimeConfirm,
                                             sumTime = a.sumTime,
                                         })).ToList();
                var data = lst.Where(a =>
                  (a.Time_In != null && a.Time_In >= _NgayBD && a.Time_In <= _NgayKT)).ToList();
                if (data.Count() > 0)
                {
                    string fileIn = Request.PhysicalApplicationPath + @"UserFiles\Template\" + "Template_ChamCong.xlsx";

                    string fileOutName = "DachSachChamCong_" + DateTime.Now.Ticks + "_" + Guid.NewGuid().ToString().Substring(0, 7) + ".xlsx";
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
 
                        double sum_time = 0;
                        sl.SetCellValue("A1", "CHƯƠNG TRÌNH BÁO CÁO CHẤM CÔNG TỪ NGÀY" +" " + _NgayBD.Day + "-" + _NgayBD.Month + "-" + _NgayBD.Year + " ĐẾN NGÀY" + " " + _NgayKT.Day + "-" + _NgayKT.Month + "-" + _NgayKT.Year);
                        foreach (var item in data)
                        {
                            string locationIn = $"{item.locationInX} - {item.locationInY}";
                            string locationOut = $"{item.LocationOutX} - {item.locationOutY}";
                            double sumtime_ = Sumtime(item.Time_In, item.Time_In);
                            sl.SetCellValue("A" + row.ToString(), item.id);
                            sl.SetCellValue("B" + row.ToString(), item.Username);
                            sl.SetCellValue("C" + row.ToString(), item.FullName);
                            sl.SetCellValue("D" + row.ToString(), item.JobDetailName);
                            sl.SetCellValue("E" + row.ToString(), locationIn);
                            sl.SetCellValue("F" + row.ToString(), item.checkLocationIn.ToString());
                            sl.SetCellValue("G" + row.ToString(), item.Time_In);
                            sl.SetCellValue("H" + row.ToString(), locationOut);
                            sl.SetCellValue("I" + row.ToString(), item.checkLocationOut.ToString());
                            if (item.Time_Out != DateTime.MinValue)
                            {
                                sl.SetCellValue("J" + row.ToString(), item.Time_Out);
                            }
                            else
                            {
                                sl.SetCellValue("J" + row.ToString(), "Chưa check out");
                            }
                            sl.SetCellValue("K" + row.ToString(), item.Admin_Messger_Update);
                            if (item.TimeAdminMessageUpdate != default(DateTime))
                            { 
                            sl.SetCellValue("L" + row.ToString(), item.TimeAdminMessageUpdate);
                            }
                            if (item.TimeConfirm != DateTime.MinValue)
                            {
                            sl.SetCellValue("M" + row.ToString(), item.TimeConfirm);
                            }
                            else
                            {
                                sl.SetCellValue("M" + row.ToString(), "Chưa xác nhận");
                            }
                            sl.SetCellValue("N" + row.ToString(), item.sumTime == null ? sumtime_.ToString() : item.sumTime.ToString());
                            row++;
                            sum_time += (Convert.ToDouble(item.sumTime) == null) ?0: Convert.ToDouble(item.sumTime);
                        }
                        sl.SetCellValue("N" + row.ToString(),"Tổng time: "+ sum_time.ToString());
                        sl.SetCellStyle("A3", "N" + (row - 1), style);
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
                    return Json((new { status = -1, title = "", text = "Nothing to export.", obj = "" }), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json((new { status = -1, title = "", text = "Vui lòng nhập ngày bắt đầu ngày kết thúc", obj = "" }), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult Update(AttendanceFaceViewModel model)
        {
            var dataUser = db_.AspNetUsers.Where(a => a.UserName == User.UserName).FirstOrDefault();
            if (dataUser.IsManager == false)
            {
                return Json((new { status = -1, title = "", text = "Bạn không có quyền sử dụng chức năng này, Vui lòng liên hệ ITC", obj = "" }), JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = db_.AttendanceFaces.Where(a => a.id == model.Id).FirstOrDefault();
                if (model.Location_In_X == "" || model.Location_In_Y == null)
                {
                    return Json((new { status = -1, title = "", text = "Vui lòng nhập vị trí check in (X)", obj = "" }), JsonRequestBehavior.AllowGet);
                }
                if (model.Location_In_Y == "" || model.Location_In_Y == null)
                {
                    return Json((new { status = -1, title = "", text = "Vui lòng nhập vị trí check in (Y)", obj = "" }), JsonRequestBehavior.AllowGet);
                }
                if (model.Location_Out_X == "" || model.Location_Out_X == null)
                {
                    return Json((new { status = -1, title = "", text = "Vui lòng nhập vị trí check out (X)", obj = "" }), JsonRequestBehavior.AllowGet);
                }
                if (model.Location_Out_Y == "" || model.Location_Out_Y == null)
                {
                    return Json((new { status = -1, title = "", text = "Vui lòng nhập vị trí check out (Y)", obj = "" }), JsonRequestBehavior.AllowGet);
                }
                if ( model.Time_In == null)
                {
                    return Json((new { status = -1, title = "", text = "Chưa có giá trị trong Time_In", obj = "" }), JsonRequestBehavior.AllowGet);
                }
                if (model.Time_Out == null)
                {
                    return Json((new { status = -1, title = "", text = "Chưa có giá trị trong Time_Out", obj = "" }), JsonRequestBehavior.AllowGet);
                }
                if (model.Admin_Messger_Update == "" || model.Admin_Messger_Update == null)
                {
                    return Json((new { status = -1, title = "", text = "Vui lòng nhập nội dung chỉnh sửa", obj = "" }), JsonRequestBehavior.AllowGet);
                }
                string format = "yyyy-MM-dd HH:mm:ss";
                DateTime time_In;
                DateTime time_Out;

                try
                {
                    if (DateTime.TryParseExact(model.Time_In, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out time_In))
                    {
                        if (DateTime.TryParseExact(model.Time_Out, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out time_Out))
                        {
                            data.Time_In = time_In;
                            data.Time_Out = time_Out;
                            data.Location_In_X = model.Location_In_X;
                            data.Location_In_Y = model.Location_In_Y;
                            data.Location_Out_X = model.Location_Out_X;
                            data.Location_Out_Y = model.Location_Out_Y;
                            data.Check_Location_In = model.Check_Location_In;
                            data.Check_Location_Out = model.Check_Location_Out;
                            data.sumTime = Sumtime(time_In, time_Out);
                            data.Admin_UserName_Update = User.UserName;
                            data.Admin_time_Update = DateTime.Now;
                            data.Admin_Messger_Update = model.Admin_Messger_Update;
                            data.isStatus = true;
                            db_.Entry(data).State = EntityState.Modified;
                            db_.SaveChanges();
                            return Json((new { status = 1, title = "Cập nhật thành công", text = "Cập nhật thành công ", obj = "" }), JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json((new { status = -1, title = "", text = "Nhập giờ ra chưa đúng định dạng", obj = "" }), JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json((new { status = -1, title = "", text = "Nhập giờ vào chưa đúng định dạng", obj = "" }), JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception e)
                {
                    return Json((new { status = -1, title = "", text = "Lỗi khi format ngày và giờ", obj = "" }), JsonRequestBehavior.AllowGet);
                }

            }

        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult Confirm(int Id)
        {
            if (ModelState.IsValid)
            {

                var item = db_.AttendanceFaces.Where(a => a.id == Id).FirstOrDefault();
                if (item == null)
                {
                    return Json(new { status = -1, title = "", text = "Không tìm thấy data", obj = "" }, JsonRequestBehavior.AllowGet);
                }
                if (item.Time_In == DateTime.MinValue)
                {
                    return Json(new { status = -1, title = "", text = "Không thể xác nhận. Chưa có thời gian check in", obj = "" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    if(item.Time_Out == null || item.Time_Out == DateTime.MinValue)
                    {
                        return Json(new { status = -1, title = "", text = "Không thể xác nhận. Chưa có thời gian check out", obj = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if(item.Time_Out == new DateTime(2010, 1, 1, 1, 1, 1))
                        {
                            return Json(new { status = -1, title = "", text = "Không thể xác nhận. Chưa có thời gian check out", obj = "" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                try
                {
                    item.isConfirm = true;
                    item.TimeConfirm = DateTime.Now;
                    item.Admin_UserName_Confirm = User.UserName;
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
                   
                    
      
