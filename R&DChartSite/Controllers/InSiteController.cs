using RDChartSite.BussinessLayer;
using RDChartSite.Entities;
using RDChartSite.Entities.ValueObjects;
using RDChartSite.Filters;
using RDChartSite.BussinessLayer.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RDChartSite.Controllers
{
    //[Auth]
    public class InSiteController : Controller
    {
        private ProjectsManager projectsManager = new ProjectsManager();
        private ActivitiesManager activitiesManager = new ActivitiesManager();
        private UserManager userManager = new UserManager();
        private FormsManager formManager = new FormsManager();

        // GET: InSite
        public ActionResult DailySchedule()
        {
            if (HttpContext.Cache["model"] != null)
            {
                var cachedViewModel = (DailyScheduleViewModel)HttpContext.Cache["model"];
                HttpContext.Cache.Remove("model");
                return View(cachedViewModel);
            }

            var viewModel = new DailyScheduleViewModel
            {
                Projects = projectsManager.List(),
                DailyActivity = new DailyActivity(),
                HourlyActivity = new HourlyActivity()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DailySchedule(DailyScheduleViewModel viewModel)
        {
            if (Session["Login"] == null)
            {
                TempData["ErrorMessage"] = "Lütfen Önce Kullanıcı Girişi Yapınız.";
                return RedirectToAction("Login", "Home");
            }
            var user = Session["Login"] as Users;
            var userId = user.UserId;

            string dateString = Request.Form["DailyActivities.Date"];
            if (!DateTime.TryParse(dateString, out DateTime date))
            {
                ModelState.AddModelError(string.Empty, "Lütfen geçerli bir tarih seçin");
                HttpContext.Cache.Add("model", viewModel, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 5, 0), System.Web.Caching.CacheItemPriority.Normal, null);
                return View(viewModel);
            }

            var dailyLog = activitiesManager.Find(x => x.Date == date && x.UserId == userId);

            if (dailyLog == null)
            {
                dailyLog = new DailyActivity
                {
                    UserId = userId,
                    Date = date,
                    HourlyActivity = new List<HourlyActivity>()
                };

                activitiesManager.Insert(dailyLog);
            }
            else if (dailyLog.HourlyActivity == null) 
            {
                dailyLog.HourlyActivity = new List<HourlyActivity>();
            }
            bool hasExistingLogs = false;
            for (int i = 8; i < 24; i++)
            {
                TimeSpan time = TimeSpan.FromHours(i);
                var hourlyDescription = Request.Form[$"HourlyActivities[{i - 8}].Description"];
                var hourlyProjectCode = Request.Form[$"HourlyActivities[{i - 8}].ProjectCode"];

                if (string.IsNullOrEmpty(hourlyDescription) && hourlyProjectCode == "Proje Kodu Seçiniz")
                {
                    continue;
                }

                var existingRecord = activitiesManager.HourlyControl(userId, date, time);
                if (existingRecord)
                {
                    hasExistingLogs = true;
                    ModelState.AddModelError($"HourlyActivities[{i - 8}].Description", $"{i}:00 record already exists.");
                    continue; 
                }

                var hourlyActivity = new HourlyActivity
                {
                    DailyActivityId = dailyLog.Id,
                    Time = time,
                    Description = hourlyDescription,
                    ProjectCode = hourlyProjectCode
                };

                if (hourlyActivity.ProjectCode == "Proje Kodu Seçiniz")
                {
                    ModelState.AddModelError($"HourlyActivities[{i - 8}].ProjectCode", $"{i}:00 requires a project code.");
                    return View(viewModel); 
                }

                dailyLog.HourlyActivity.Add(hourlyActivity);
            }

            for (int i = 0; i < 8; i++)
            {
                TimeSpan time = TimeSpan.FromHours(i);
                var hourlyDescription = Request.Form[$"HourlyActivities[{i + 16}].Description"];
                var hourlyProjectCode = Request.Form[$"HourlyActivities[{i + 16}].ProjectCode"];

                if (string.IsNullOrEmpty(hourlyDescription) && hourlyProjectCode == "Proje Kodu Seçiniz")
                {
                    continue; 
                }

                var existingRecord = activitiesManager.HourlyControl(userId, date, time);
                if (existingRecord)
                {
                    hasExistingLogs = true;
                    ModelState.AddModelError($"HourlyActivities[{i + 16}].Description", $"{i}:00 saatinde bir girdiniz zaten mevcuttur.");
                    continue; 
                }

                var hourlyActivity = new HourlyActivity
                {
                    DailyActivityId = dailyLog.Id,
                    Time = time,
                    Description = hourlyDescription,
                    ProjectCode = hourlyProjectCode
                };

                if (hourlyActivity.ProjectCode == "Proje Kodu Seçiniz")
                {
                    ModelState.AddModelError($"HourlyActivities[{i + 16}].ProjectCode", $"{i}:00 Saati için proje kodunu belirtiniz.");
                    return View(viewModel); 
                }

                dailyLog.HourlyActivity.Add(hourlyActivity);
            }

            try
            {
                activitiesManager.Save(); 
                if (hasExistingLogs)
                {
                    TempData["WarningMessage"] = $"Bazı Saatler için Kaydınız Mevcut. Kaydı Olmayan Saatler Eklenmiştir.";
                }
                else
                {
                    TempData["SuccessMessage"] = $"{date.ToShortDateString()} Tarihindeki Günlük Faaliyet Kaydınız Güncellenmiştir.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Günlük Faaliyet kaydınız işlenirken bir hata oluştu. Lütfen tekrar deneyin.";
               
            }

            return View(viewModel);
        }




        public ActionResult MonthlySchedule()
        {

            return View();
        }


        [AuthAdmin]
        public ActionResult Dashboard()
        {

            return View();
        }

        [AuthAdmin]
        public ActionResult WeeklyActivities()
        {

            List<DailyActivity> weeklyActivities = activitiesManager.GetDailyActivitiesCurrentWeek();

            return View(weeklyActivities);
        }


        public ActionResult ProjectsDashboardList()
        {

            return View(projectsManager.List());
        }
        public ActionResult OffDayForm()
        {

            return View();
        }

        [HttpPost]
        public ActionResult OffDayForm(OffDayFormsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                BussinessLayerResult<OffDayForms> result = formManager.InsertIntoDb(model);
                if (result.Errors.Count > 0)
                {
                    result.Errors.ForEach(error => ModelState.AddModelError("", error.Message));
                    return View(model);
                }
                else
                {
                    TempData["Notification"] = "İzin talebiniz başarıyla gönderildi.";
                    return RedirectToAction("OffDayForm");
                }
            }
        }
        [Auth]
        [AuthAdmin]
        public ActionResult FormApprovalPage(Guid FormGuid)
        {

            OffDayForms offDayForm = formManager.Find(x => x.FormGuid == FormGuid);

            return View(offDayForm);
        }


        [HttpPost]
        public ActionResult FormApprovalPage(Guid FormGuid, bool approval)
        {
            OffDayForms offDayForm = formManager.Find(x => x.FormGuid == FormGuid);
            if (approval)
            {
                if (offDayForm != null)
                {
                    offDayForm.IsApproved = true;
                    formManager.Update(offDayForm);
                }
                return RedirectToAction("Mainpage", "Home");
            }
            else
            {
                formManager.Delete(offDayForm);
                return RedirectToAction("Mainpage", "Home");
            }
        }

        public ActionResult UsersList() { return View(); }
    }

}

