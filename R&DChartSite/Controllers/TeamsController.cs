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
using RDChartSite.Entities.ValueObjects;

namespace RDChartSite.Controllers
{
    public class TeamsController : Controller
    {
        TeamsManager teamsManager = new TeamsManager();
        UserManager userManager = new UserManager();

        // GET: Teams
        public ActionResult Index()
        {
            return View(teamsManager.List());
        }

        // GET: Teams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teams teams = teamsManager.Find(x => x.TeamId == id);
            if (teams == null)
            {
                return HttpNotFound();
            }
            return View(teams);
        }

        // GET: Teams/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Teams team, int leaderId)
        {
            if (ModelState.IsValid)
            {
                Teams existingTeam = teamsManager.GetTeamByName(team.TeamName);
                if (existingTeam != null)
                {
                    ModelState.AddModelError(string.Empty, "Bu ekip zaten mevcut.");
                    return View(team);
                }

                if (leaderId != 0)
                {
                    Users user = userManager.Find(x => x.UserId == leaderId);
                    if (user != null)
                    {
                        bool IsUserLeader = teamsManager.IsUserLeader(user.UserId);
                        if (!IsUserLeader)
                        {
                            teamsManager.CreateTeam(team, leaderId);
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Bu kullanıcı başka bir ekipte lider.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Seçilen lider bulunamadı.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Lütfen bir lider seçin.");
                }
            }

            return View(team);
        }


        // GET: Teams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teams teams = teamsManager.Find(x => x.TeamId == id);
            if (teams == null)
            {
                return HttpNotFound();
            }
            return View(teams);
        }

        // POST: Teams/Edit/5
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TeamId,TeamName,LeaderId")] Teams teams)
        {
            if (ModelState.IsValid)
            {

                teamsManager.Save();
                return RedirectToAction("Index");
            }
            return View(teams);
        }

        // GET: Teams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teams teams = teamsManager.Find(x => x.TeamId == id);
            if (teams == null)
            {
                return HttpNotFound();
            }
            return View(teams);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Teams teams = teamsManager.Find(x => x.TeamId == id);
            teamsManager.Delete(teams);
            teamsManager.Save();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AssignUserToTeam()
        {
            UserManager userManager = new UserManager();
            var viewModel = new TeamAssignmentViewModel
            {
                Users = userManager.GetUsers(),
                Teams = teamsManager.GetTeams()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignUserToTeam(TeamAssignmentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = userManager.GetUser(viewModel.SelectedUserId);
                var team = teamsManager.GetTeam(viewModel.SelectedTeamId);

                if (user != null && team != null)
                {
                    team.Users.Add(user);
                    teamsManager.Update(team);
                    teamsManager.Save();

                    return RedirectToAction("AssignUserToTeam");
                }
            }

            viewModel.Users = userManager.GetUsers();
            viewModel.Teams = teamsManager.GetTeams();

            return View(viewModel);
        }
    }
}
