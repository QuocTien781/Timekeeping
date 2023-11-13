using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
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
    [Authorize]
    [CustomAuthorize(Function = "BaoCaoCongLam/Index")]
    public class BaoCaoCongLamController : BaseController
    {
        ChamCong_teamdevEntities db_ = new ChamCong_teamdevEntities();
        // GET: BaoCaoCongLam
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _Search()
        {
            var DataUser = db_.AspNetUsers.Where(a => a.idDepartment == User.idDepartment && a.AccoutType == "User").ToList();
            ViewBag.DataUser = DataUser;
            var Datajob = db_.Jobs.Where(a => a.IdDepartment == User.idDepartment).ToList();
            ViewBag.Datajob = Datajob;
            return PartialView();
        }
        public ActionResult _Salaries(string Thangnam, string idJob)
        {
            ViewData["Thangnam"] = Thangnam;
            ViewData["idJob"] = idJob;
            return PartialView();
        }
        public ActionResult _Detail(string username, string ThangSearch, int? idJob)
        {
            var FullName = db_.AspNetUsers.FirstOrDefault(a => a.UserName == username).Fullname;
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
            var ngayNghi = db_.PleaseTakeLeaves.Where(a =>a.username ==username && a.isComfim == true && a.timeStart.Month == _thangNam.Month && a.timeStart.Year == _thangNam.Year).ToList();
            if (idJob != null)
            {
            
                var data = db_.Sp_Report_time_user(User.idDepartment, username, _thangNam).Where(a => a.idjob == idJob).ToList();
                if (data.Count() == 0)
                {
                    return Json(new { status = -1, title = "", text = "Chưa có dữ liệu để hiển thị.", obj = "" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    int thang = _thangNam.Month;
                    ViewData["thangNam"] = thang;
                    ViewData["FullName"] = FullName;
                    ViewBag.ngayNghi = ngayNghi;
                    ViewBag.data = data;
                    return PartialView();
                }
            }
            else
            {
                var data = db_.Sp_Report_time_user(User.idDepartment, username, _thangNam).ToList();
                if (data.Count == 0) 
                {
                    return Json(new { status = -1, title = "", text = "Chưa có dữ liệu để hiển thị.", obj = "" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    int thang = _thangNam.Month;
                    ViewData["thangNam"] = thang;
                    ViewData["FullName"] = FullName;
                    ViewBag.ngayNghi = ngayNghi;
                    ViewBag.data = data;
                    return PartialView();
                }
             
            }
        }


        public ActionResult _GetList(string UsernameSearch, string ThangSearch, string jobSearch)
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
            var data = db_.Sp_Report_Luong(User.idDepartment, _thangNam)
                .Where(a =>
            (UsernameSearch == "" || UsernameSearch == null || a.Username.ToUpper().Contains(UsernameSearch.ToUpper())) &&
            (jobSearch == "" || jobSearch == null || a.idJob.ToString().ToUpper().Contains(jobSearch.ToUpper()))
            )
                .ToList();

            ViewBag.data = data;
            return PartialView();
        }
   

        [HttpPost]
        public JsonResult ExportExcel(Salaries_ViewModel model)
            {
            var Thangnam = model.Thangnam;
            var idJob = model.idJob;
            var job = db_.Jobs.FirstOrDefault(a => a.Id.ToString() == idJob).NameLocation;
            string Time_Tmp = Thangnam;
            var _NgayKetthuc = new DateTime();
            DateTime _thangNam = DateTime.UtcNow.AddHours(7);
            if (!string.IsNullOrEmpty(Thangnam) || Thangnam != "")
            {
                CultureInfo cul = CultureInfo.GetCultureInfo("en-GB");
                DateTime _NgayKT = new DateTime();
                try
                {
                    Thangnam += "-01";
                    _thangNam = Convert.ToDateTime(Thangnam);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                return Json((new { status = -1, title = "", text = "Vui lòng nhập tháng cần xuất báo cáo", obj = "" }), JsonRequestBehavior.AllowGet);
            }
       
            var data = db_.Sp_Report_Luong(User.idDepartment, _thangNam).Where(a => a.idJob.ToString() == idJob).ToList();
            if (data.Count() > 0)
            {
                string fileIn = Request.PhysicalApplicationPath + @"UserFiles\Template\" + "Template_ReportChamCong2023.xlsx";
                //string fileOutName = "ReportChamCong2023_"+ _thangNam + DateTime.Now.Ticks + "_" + Guid.NewGuid().ToString().Substring(0, 7) + ".xlsx";
                string currentDate = _thangNam.ToString("yyyy_MM_dd");
                string fileOutName = "ReportChamCong_" + currentDate + "_" + DateTime.Now.Ticks + ".xlsx";  
                SLDocument sl = new SLDocument(fileIn);
                sl.SelectWorksheet(sl.GetSheetNames()[0]);
                SLWorksheetStatistics stats = sl.GetWorksheetStatistics();
                var row = 5;
                SLStyle style = sl.CreateStyle();
                style.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
                style.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
                style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
                style.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
                style.Border.TopBorder.Color = System.Drawing.Color.Black;
                style.Border.BottomBorder.Color = System.Drawing.Color.Black;
                style.Border.LeftBorder.Color = System.Drawing.Color.Black;
                style.Border.RightBorder.Color = System.Drawing.Color.Black;
                try
                {
                    decimal LuongCoBanGoc = model.LuongCoBanGoc;
                    decimal ThamNien = model.ThamNien;
                    decimal xangxe = model.xangxe;
                    decimal ChuyenCan = model.ChuyenCan;
                    decimal DienThoai = model.DienThoai;
                    decimal Com = model.Com;
                    decimal HieuQuaCV = model.HieuQuaCV;
                    decimal STT = 0;
                    decimal CongTet = model.CongTet * model.HeSoThuong;
                    sl.SetCellValue("A1", "CHƯƠNG TRÌNH BÁO CÁO BẢNG LƯƠNG TỪ THÁNG" + " " + _thangNam.ToString("MM/yyyy") + " " + "TẠI"+ " " + job.ToUpper());
                    foreach (var item in data)
                    {
                        var Luongcoban = (LuongCoBanGoc) / 30 * item.TongSoNgayLamQuyDoi;
                        var thuongthamnien = (ThamNien/ 30) * item.TongSoNgayLamQuyDoi;
                        var ThuongChuyenCan = (ChuyenCan/ 30) * item.TongSoNgayLamQuyDoi;
                        var Pcxangxe = (xangxe / 30)* item.TongSoNgayLamQuyDoi;
                        var PcCom = (Com/ 30)* item.TongSoNgayLamQuyDoi;
                        var PcDienThoai = (DienThoai / 30)* item.TongSoNgayLamQuyDoi;
                        var ThuongHQCV = (HieuQuaCV / 30)* item.TongSoNgayLamQuyDoi;
                        var tangca = (Luongcoban + ThamNien + xangxe + ChuyenCan + Com + HieuQuaCV) / 30 * item.TongGioDu;
                        var thuongCongTet = (Luongcoban + ThamNien + ChuyenCan + Pcxangxe + PcDienThoai + PcCom + ThuongHQCV) / 30 * CongTet;
                        var tongluong = (Luongcoban + thuongthamnien + Pcxangxe + ThuongChuyenCan + PcDienThoai + PcCom + ThuongHQCV + tangca + thuongCongTet);
                        STT++;
                        sl.SetCellValue("A" + row.ToString(), STT);
                        sl.SetCellValue("B" + row.ToString(), item.Username.ToString());
                        sl.SetCellValue("C" + row.ToString(), item.Fullname); 
                        sl.SetCellValue("D" + row.ToString(), Convert.ToInt32(item.CaLam));
                        sl.SetCellValue("E" + row.ToString(), (float)item.TongSoNgayLamQuyDoi);
                        sl.SetCellValue("F" + row.ToString(), (float)item.TongGioDu);
                        sl.SetCellValue("G" + row.ToString(), (int)item.DiTre);
                        sl.SetCellValue("H" + row.ToString(), "");//Nghỉ có phép
                        sl.SetCellValue("I" + row.ToString(), "");// Nghỉ ko phép
                        sl.SetCellValue("J" + row.ToString(),  LuongCoBanGoc);
                        sl.SetCellValue("K" + row.ToString(), (decimal)Luongcoban);
                        sl.SetCellValue("L" + row.ToString(), (float)thuongthamnien);
                        sl.SetCellValue("M" + row.ToString(), (decimal)Pcxangxe);
                        sl.SetCellValue("N" + row.ToString(), (decimal)PcDienThoai);
                        sl.SetCellValue("O" + row.ToString(), (decimal)PcCom);
                        sl.SetCellValue("P" + row.ToString(), (decimal)ThuongChuyenCan);
                        sl.SetCellValue("Q" + row.ToString(), (decimal)ThuongHQCV);
                        sl.SetCellValue("R" + row.ToString(), (decimal)tangca); 
                        sl.SetCellValue("S" + row.ToString(), (decimal)thuongCongTet); 
                        sl.SetCellValue("T" + row.ToString(), (decimal)tongluong); 
                        sl.SetCellValue("U" + row.ToString(), ""); 
                        sl.SetCellValue("V" + row.ToString(), "" ); 
                        row++;
                    }

                    sl.SetCellStyle("A2", "W" + (row - 1), style);
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
    }
}  
    


