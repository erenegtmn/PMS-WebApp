using RDChartSite.BussinessLayer;
using RDChartSite.BussinessLayer.Results;
using RDChartSite.Entities;
using RDChartSite.Entities.Messages;
using RDChartSite.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RDChartSite.Controllers
{
    public class HomeController : Controller
    {
        
        // GET: Home
        public ActionResult Index()
        {

            
            return RedirectToAction("Login");
        }

        #region Giriş Yap
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserManager userMng = new UserManager();
                BussinessLayerResult<Users> result = userMng.AuthUser(model);
                if (result.Errors.Count > 0)
                {
                    if (result.Errors.Find(x=>x.Code == ErrorMessageCode.UserIsNotAuthorized)!=null)
                    {
                        ViewBag.SetLink = "http://Home/Activate/1234-4545-12312";
                    }
                    result.Errors.ForEach(error => ModelState.AddModelError("", error.Message));
                    return View(model);
                }

                Session["Login"] = result.Results;
                return RedirectToAction("Mainpage");
            }

            return View(model);
        }
        #endregion

        #region Kayıt Oluşturma
        public ActionResult SignUp()
        {
            SignUpViewModel viewModel = new SignUpViewModel();
            TeamsManager teamsManager = new TeamsManager();
            viewModel.AvailableTeams = teamsManager.GetTeams();
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult SignUp(SignUpViewModel model)
        {
            if(!ModelState.IsValid)
            {
                TeamsManager teamsManager = new TeamsManager();
                model.AvailableTeams = teamsManager.GetTeams();
                return View(model);
            }

            UserManager userMng = new UserManager();
            BussinessLayerResult<Users> result =  userMng.RegisterUser(model);

            if (result.Errors.Count>0)
            {
                result.Errors.ForEach(error => ModelState.AddModelError("",error.Message));
                return View(model);
            }

  
            return RedirectToAction("RegisterOk");
        }

        public ActionResult RegisterOk()
        {

            return View();
        }
        #endregion

        #region Aktivasyon
        public ActionResult UserActivate(Guid id)
        {
           UserManager userMan = new UserManager();
            BussinessLayerResult<Users> result = userMan.ActivateUser(id);

            if (result.Errors.Count>0)
            {
                TempData["Errors"] = result.Errors;
                return RedirectToAction("UserActivateCancel");
            }

            return RedirectToAction("UserActivateOk");
        }
        public ActionResult UserActivateOk()
        {   
            return View();
        }
        public ActionResult UserActivateCancel()
        {
            List<ErrorMessageObj> errors = null;
            if (TempData["Errors"] != null)
            {
                errors= TempData["Errors"] as List<ErrorMessageObj>;
            }
            return View(errors);
        }
        #endregion

        [HttpPost]
        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        public ActionResult Mainpage()
        {

            return View();
        }
        #region Şifremi Unuttum
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            UserManager userManager = new UserManager();
            Users user = userManager.Find(x=>x.Email == email);
               
            return View();
        }
        #endregion

        public ActionResult AccesDenied()
        {
            return View();
        }
    }
}