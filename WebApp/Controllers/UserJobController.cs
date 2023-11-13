using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ToolsApp.Authentication;
using ToolsApp.EntityFramework;
using ToolsApp.Models;

namespace ToolsApp.Controllers
{
    [Authorize]
    [CustomAuthorize(Function = "UserJob/Index")]
    public class UserJobController : BaseController
    {
        ChamCong_teamdevEntities db_ = new ChamCong_teamdevEntities();
        // GET: Jobs
        public ActionResult Index()
        {
            var listJob = db_.JobDetails.Where(a => a.UserCreate == User.UserName && a.IsStatus == true).ToList();
            ViewBag.listJob = listJob;
            var listStaff = db_.AspNetUsers.Where(a => a.AccoutType == "User" && a.idDepartment == User.idDepartment && a.IsLocked == false).ToList();
            ViewBag.listStaff = listStaff;
            return View();
        }
        public ActionResult _GetList()
        {

            var data = db_.AspNetUsers.Where(a => a.AccoutType == "User" && a.idDepartment == User.idDepartment).ToList();
            ViewBag.data = data;
            return PartialView();
        }
        public ActionResult _Edit(int Id)
        {
            var model = db_.UserJobs.FirstOrDefault(p => p.Id == Id);
            var list = db_.UserJobs.ToList();
            ViewBag.list = list;
            return PartialView();
        }
        public ActionResult FilterDataJob(string searchJobValue)
        {
            searchJobValue = searchJobValue?.Trim();
            var filteredDataJob = db_.JobDetails
                               .Where(b => b.job_Detail_Name.ToUpper().Contains(searchJobValue.ToUpper()) && b.UserCreate == User.UserName && b.IsStatus == true)
                               .ToList();
          

            return Json(filteredDataJob, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FilterDataStaff(string searchStaffValue)
        {
            searchStaffValue = searchStaffValue?.Trim();
            var filteredDataStaff = db_.AspNetUsers
                               .Where(a => a.Fullname.ToUpper().Contains(searchStaffValue.ToUpper()) && a.idDepartment == User.idDepartment && a.IsLocked == false)
                               .ToList();

            return Json(filteredDataStaff, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> _Insert(string data)
            {

            // Phân tích chuỗi JSON thành danh sách đối tượng
            var serializer = new JavaScriptSerializer();
            List<Dictionary<string, string>> dataList = serializer.Deserialize<List<Dictionary<string, string>>>(data);
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string token = User.token;
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    // Gửi GET request tới API
                    HttpResponseMessage response = await client.PostAsJsonAsync("https://api.hoanglongsecurity.info/api/Job/InsertUserJob", dataList); // Thay đổi URL tới API của bạn
                    HttpStatusCode statusCode = response.StatusCode;
                    string responseData = await response.Content.ReadAsStringAsync();
                    dynamic json = JsonConvert.DeserializeObject<dynamic>(responseData);
                    ResponseObject responseObject = JsonConvert.DeserializeObject<ResponseObject>(responseData);
                    if (response.StatusCode.ToString() == "200" || response.ReasonPhrase == "OK")
                        {
                            return Json(new { status = 1, title = "", text = "Phân công thành công", obj = "" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { status = -1, title = "", text = responseObject.message, obj = "" }, JsonRequestBehavior.AllowGet);
                        }
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = -1, title = "", text = "Lỗi: Không cấu trúc api", obj = "" }, JsonRequestBehavior.AllowGet);
            }
          
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult _DeleteJob(int id)
        {
            try
            {
                var item = db_.UserJobs.FirstOrDefault(p => p.Id == id);

                if (item != null)
                {
                    item.Status = false;
                    item.timeDelete = DateTime.Now;
                    db_.Entry(item).State = EntityState.Modified;
                    db_.SaveChanges(); 
                }

                return Json(new { status = 1, title = "", text = "Xoá thành công.", obj = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = -1, title = "", text = "Không thể xoá", obj = "" }, JsonRequestBehavior.AllowGet);
            }
        }
         [ValidateInput(false)]
        [HttpPost]
        public ActionResult _ResetJob(string Username)
        {
            try
            {
                var listUser = db_.UserJobs.Where(a => a.Username == Username).ToList();
                for (int i = listUser.Count() - 1; i >=0; i--)
                {
                    var user = listUser[i];
                    user.Status = false;
                    user.timeDelete = DateTime.Now;
                    db_.Entry(user).State = EntityState.Modified;
                }
                db_.SaveChanges();
                return Json(new { status = 1, title = "", text = "Xoá thành công.", obj = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = -1, title = "", text = "Không thể xoá", obj = "" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
