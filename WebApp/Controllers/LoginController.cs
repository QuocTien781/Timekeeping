using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ToolsApp.Authentication;
using ToolsApp.Utilities;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualBasic.ApplicationServices;
using ToolsApp.Models;
using System.Net.Http.Headers;

namespace ToolsApp.Controllers
{
    public class LoginController : BaseController
    {


        // GET: Login
        [AllowAnonymous]
        public ActionResult Index(string returnUrl = "")
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        #region Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> _FunLogin(string Username, string Password, string ReturnUrl = "", string Remember = "", bool CaptchaValid = false)
        {
            var userName = Username.Trim();
            var password = Password.Trim();

            #region Login
            UserJson user = new UserJson
            {
                username = userName,
                password = password,
            };

            using (HttpClient client = new HttpClient())
            {
                // Gửi GET request tới API
                HttpResponseMessage response = await client.PostAsJsonAsync("https://api.hoanglongsecurity.info/api/Authenticate/LoginUser", user); // Thay đổi URL tới API của bạn

                // Kiểm tra xem request có thành công không
                if (response.IsSuccessStatusCode)
                {
                    // Đọc nội dung của response và xử lý dữ liệu nhận được
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    try
                    {
                        dynamic json = JsonConvert.DeserializeObject<dynamic>(jsonResult);
                        var responseObject = JsonConvert.DeserializeObject<ResponseObject>(json.data.ToString());
                        if (responseObject != null && responseObject.token != null)
                        {

                            if (responseObject.role == "User")
                            {
                                return Json(new { status = -1, title = "", text = "User không có quyền truy cập.", obj = "" }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                #region Session + Cookies
                                CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                                serializeModel.UserId = responseObject.id;
                                serializeModel.UserName = responseObject.username;
                                serializeModel.FullName = responseObject.name;
                                serializeModel.Email = responseObject.email;
                                serializeModel.idDepartment = responseObject.idDepartment;
                                serializeModel.role = responseObject.role;
                                serializeModel.token = responseObject.token;

                                try
                                {
                                    string userData = JsonConvert.SerializeObject(serializeModel);
                                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                                             1,
                                             serializeModel.UserId.ToString(),
                                             DateTime.Now,
                                             DateTime.Now.AddMinutes(60 * 24 * 365),
                                             false,
                                             userData);

                                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName + Parameters.CookieName.Cookie_Name, encTicket);
                                    Response.Cookies.Add(faCookie);
                                }
                                catch (Exception ex)
                                {
                                    return Json(new { status = -1, title = "", text = "Lỗi: " + ex.Message, obj = "" }, JsonRequestBehavior.AllowGet);
                                }
                                #endregion

                                return Json(new
                                {
                                    status = 1,
                                    title = "",
                                    text = "Đăng nhập thành công.",
                                    obj = ToolsApp.Utilities.AppParameters.Domain //+ model.ReturnUrl
                                }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { status = -1, title = "", text = "User không tồn tại hoặc đã bị vô hiệu.", obj = "" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json(new { status = -1, title = "", text = ex.Message, obj = "" }, JsonRequestBehavior.AllowGet);
                    }
                    
                }
                else
                {
                    return Json(new { status = -1, title = "", text = "Lỗi: Không đọc được data api hoặc tài khoản hoặc mật khẩu không đúng", obj = "" }, JsonRequestBehavior.AllowGet);
                }
            }
           
            
            #endregion
        }
        #endregion

        #region Logout
        public void LogOutFun()
        {
            FormsAuthentication.SignOut();

            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName + Parameters.CookieName.Cookie_Name];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                         1,
                         User.UserName,
                         DateTime.Now,
                         DateTime.Now.AddMinutes(-120),
                         false,
                         "");

                string encTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName + Parameters.CookieName.Cookie_Name, encTicket);
                Response.Cookies.Add(faCookie);
            }
        }
        [AllowAnonymous]
        public ActionResult LogOut()
        {
            LogOutFun();

            return RedirectToAction("Index", "Login", new { area = "" });
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}