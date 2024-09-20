using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using RDChartSite.Entities;
using RDChartSite.BussinessLayer;
using System.Runtime.Remoting.Contexts;
using RDChartSite.Filters;

namespace RDChartSite.Controllers
{
    public class UsersController : Controller
    {
       UserManager userManager =new UserManager();
       ProjectsManager projectsManager =new ProjectsManager();

        [Auth]
        [AuthAdmin]
        public ActionResult Index()
        {
            return View(userManager.List());
        }

        [Auth]
        public ActionResult Details()
        {
            Users user = new Users();
            if (Session["Login"] != null)
            {
                user = Session["Login"] as Users;
            }
          
            Users users = userManager.Find(x=>x.UserId ==user.UserId);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        [Auth]
        [AuthAdmin]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Surname,Email,Password,IsActive,IsAdmin,ActivateGuid")] Users users)
        {
            if (ModelState.IsValid)
            {
                userManager.Insert(users);
                userManager.Save();
                return RedirectToAction("Index");
            }

            return View(users);
        }

        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = userManager.Find(x => x.UserId == id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Users user)
        {

            if (ModelState.IsValid)
            {
                Users existinguser = userManager.Find(x=>x.UserId ==user.UserId);
                if (existinguser != null)
                {
                    existinguser.Name = user.Name;
                    existinguser.Email = user.Email;
                    existinguser.Surname = user.Surname;
                    existinguser.ActivateGuid = user.ActivateGuid;
                    existinguser.IsActive = user.IsActive;
                    existinguser.IsAdmin = user.IsAdmin;
                    existinguser.Password=user.Password;
                    userManager.Update(existinguser);
                }
                return RedirectToAction("Details");
            }
            return View(user);
        }

        [Auth]
        [AuthAdmin]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = userManager.Find(x => x.UserId == id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users users = userManager.Find(x => x.UserId == id);
            userManager.Delete(users);
            userManager.Save();
            return RedirectToAction("Index");
        }

        [Auth]
        public ActionResult UserProjectEdit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserProjectEdit(List<int> selectedProjects)
        {
            if (selectedProjects == null || !selectedProjects.Any())
            {
                return View();
            }
            else if (Session["Login"] != null)
            {
                var user = Session["Login"] as Users;
                var userId = user.UserId;
                var userToUpdate = userManager.GetUserIncludingProjects(userId);

                if (userToUpdate != null)
                {
                    foreach (int projectId in selectedProjects)
                    {
                        var projectToDelete = userToUpdate.Projects.FirstOrDefault(p => p.Id == projectId);
                        bool projectexistinguser = projectsManager.ProjectHasSameUserControl(projectId, userId);
                        bool userexistingproject = userManager.UserHasSameProjectControl(projectId, userId);
                        userManager.DeleteProjectFromUser(userId, projectId);

                    }
                }
                return View();
            }
           

            return View();
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        userManager.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
