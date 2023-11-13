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
using ToolsApp.Authentication;
using ToolsApp.EntityFramework;
using ToolsApp.Models;
using ToolsApp.Utilities;

namespace ToolsApp.Controllers
{
    [Authorize]
    [CustomAuthorize(Function = "ManagerStaff/Index")]
    public class ManagerStaffController : BaseController
    {
        ChamCong_teamdevEntities db_ = new ChamCong_teamdevEntities();
        // GET: ManagerStaff
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _Image_View(string Id)
        {
            ViewData["Id"] = Id;
            return PartialView();
        }
        public ActionResult _GetList(string UsernameSearch, string AccountType, string FullnameSearch)
        {
            UsernameSearch = UsernameSearch?.Trim();
            FullnameSearch = FullnameSearch?.Trim();
            var list = db_.AspNetUsers.Where(a =>
            (UsernameSearch == "" || UsernameSearch == null || a.UserName.ToUpper().Contains(UsernameSearch.ToUpper())) &&
            (AccountType == "" || AccountType == null || a.AccoutType.ToUpper().Contains(AccountType.ToUpper()))&&
            (FullnameSearch == "" || FullnameSearch == null || a.Fullname.ToUpper().Contains(FullnameSearch.ToUpper()))&&
            a.idDepartment == User.idDepartment && a.AccoutType == "User").ToList();
            ViewBag.list = list;
            var dataUser = db_.AspNetUsers.Where(a => a.UserName == User.UserName).FirstOrDefault();
            ViewBag.dataUser = dataUser;
            return PartialView();
        }
        public ActionResult _Insert_View()
        {
            var data = db_.Departments.FirstOrDefault(a => a.Id == User.idDepartment);
            ViewBag.data = data;
            return PartialView();
        }
        public ActionResult _Update_View(string Id)
        {

            var data = db_.AspNetUsers.FirstOrDefault(p => p.Id == Id);
            ViewBag.model = data;
            var list = db_.Departments.Where(a => a.Id == User.idDepartment).ToList();
            ViewBag.list = list;
            return PartialView();
        }
        public ActionResult ChangePassword(string Id)
        {

            var user = db_.AspNetUsers.FirstOrDefault(p => p.Id == Id);
            ViewBag.user = user;
            return PartialView();
        }
        #region Register
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> Register(RegisterViewModel model)
        {
            if (model.fullName == null || model.fullName == "")
            {
                return Json(new { status = -1, title = "", text = "Chưa nhập Họ và tên.", obj = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (model.username.Trim() == "" || model.username == null)
                {
                    return Json(new { status = -1, title = "", text = "Vui lòng nhập Username", obj = "" }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    if (model.password.Trim() == "" || model.password == null)
                    {
                        return Json(new { status = -1, title = "", text = "Vui lòng nhập Password", obj = "" }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        if (model.email.Trim() == "" || model.email == null)
                        {
                            return Json(new { status = -1, title = "", text = "Vui lòng nhập Email", obj = "" }, JsonRequestBehavior.AllowGet);

                        }

                    }
                }
            }
      
            model.timeInWorking = DateTime.Now;
            model.accoutType = "User";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string token = User.token;
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    // Gửi GET request tới API
                    HttpResponseMessage response = await client.PostAsJsonAsync("https://api.hoanglongsecurity.info/api/Authenticate/register", model); // Thay đổi URL tới API của bạn
                    HttpStatusCode statusCode = response.StatusCode;
                    // Lấy dữ liệu từ response
                    string responseData = await response.Content.ReadAsStringAsync();
                    dynamic json = JsonConvert.DeserializeObject<dynamic>(responseData);
                    ResponseObject responseObject = JsonConvert.DeserializeObject<ResponseObject>(responseData);
                    if (response.StatusCode.ToString() == "200" || response.ReasonPhrase =="OK")
                    {
                            return Json(new { status = 1, title = "", text = responseObject.data, obj = "" }, JsonRequestBehavior.AllowGet);

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
        #endregion
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult _Lock_User(string Id)
        {
            try
            {
                
                var item = db_.AspNetUsers.FirstOrDefault(p => p.Id == Id);
                if (item.IsLocked == true)
                {
                    item.IsLocked = false;
                    item.Time_Locked = DateTime.Now;
                    item.User_Locked = User.UserName;
                    db_.Entry(item).State = EntityState.Modified;
                    db_.SaveChanges();
                    return Json(new { status = 1, title = "", text = "Mở khoá thành công", obj = "" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    if (item.UserName != "Admin")
                    {
                        if (item.UserName != User.UserName)
                        {
                            item.IsLocked = true;
                            item.Time_Locked = DateTime.Now;
                            item.User_Locked = User.UserName;
                            db_.Entry(item).State = EntityState.Modified;
                            db_.SaveChanges();
                            return Json(new { status = 1, title = "", text = "Khoá thành công", obj = "" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    return Json(new { status = -1, title = "", text = "Không thể khoá User Này " + User.UserName, obj = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = -1, title = "", text = ex.Message, obj = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult EditUser(RegisterViewModel model, string Id)
        {
            var item = db_.AspNetUsers.FirstOrDefault(p => p.Id == Id);

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.fullName == null || model.fullName == "")
                    {
                        return Json(new { status = -1, title = "", text = "Chưa nhập Họ và tên.", obj = "" }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        if (model.email == null || model.email == "")
                        {
                            return Json(new { status = -1, title = "", text = "Email không được để trống.", obj = "" }, JsonRequestBehavior.AllowGet);

                        }

                    }
                    item.NormalizedEmail = model.email.ToUpper();
                    item.Fullname = model.fullName;
                    item.UserName = model.username;
                    item.Email = model.email;
                    item.PasswordHash = model.password;
                    item.PhoneNumber = model.username;
                    item.AccoutType = "User";
                    item.idDepartment = model.idDepartment;
                    db_.Entry(item).State = EntityState.Modified;
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
                return Json(new { status = -1, title = "", text = "Error: " + UtilsLocal.ModelStateError(ModelState), obj = "" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> SaveNewPassword(string Username, string newPass)
        {
            try
            {
                // Phân tích chuỗi JSON thành danh sách đối tượng


                using (HttpClient client = new HttpClient())
                {
                    var data = new { Username = Username, newPass = newPass };
                    string token = User.token;
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    // Gửi GET request tới API
                    HttpResponseMessage response = await client.PostAsJsonAsync($"https://api.hoanglongsecurity.info/api/Authenticate/ChangePassword?Username={data.Username}&newPass={data.newPass}", data);
                    HttpStatusCode statusCode = response.StatusCode;
                    string responseData = await response.Content.ReadAsStringAsync();
                    dynamic json = JsonConvert.DeserializeObject<dynamic>(responseData);
                    ResponseObject responseObject = JsonConvert.DeserializeObject<ResponseObject>(responseData);
                    if (response.StatusCode.ToString() == "200" || response.ReasonPhrase == "OK")
                    {
                        return Json(new { status = 1, title = "", text = responseObject.data, obj = "" }, JsonRequestBehavior.AllowGet);


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

    }

}

 