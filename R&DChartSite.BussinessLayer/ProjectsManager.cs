using RDChartSite.DataAccessLayer.EntitiyFramework;
using RDChartSite.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDChartSite.BussinessLayer.Abstract;
using RDChartSite.BussinessLayer.Results;
using RDChartSite.Entities.ValueObjects;
using System.Data.Entity;

namespace RDChartSite.BussinessLayer
{
    public class ProjectsManager : ManagerBase<Projects>
    {
        private Repository<Projects> repo_projects = new Repository<Projects>();
        private Repository<DailyActivity> repo_dailyactivity = new Repository<DailyActivity>();
        private Repository<HourlyActivity> repohourly = new Repository<HourlyActivity>();
        public List<Projects> GetProjects()
        {
            using (RdDbContext db = new RdDbContext())
            {
                var projects = db.Projects.Include(x => x.Users).ToList();
                return projects;
            }
           
     
        }

        public int TotalProjectsCount()
        {
            using (RdDbContext db = new RdDbContext())
            {
            
                int count;
                count = db.Projects.Count();
                return count;
            }

        }

        public Dictionary<string, int> UsersForProjectsCount()
        {
            Dictionary<string, int> UserCountByProjectCode = new Dictionary<string, int>();
            using (RdDbContext db = new RdDbContext())
            {
                var projects = db.Projects.Include(x => x.Users);
                foreach (var project in projects)
                {
                    int userCount = 0;
                    string projectCode = project.ProjectCode;
                    if (project.Users != null && project.Users.Any())
                    {
                        userCount = project.Users.Count();
                    }
                    UserCountByProjectCode.Add(projectCode, userCount);
                }
            }
            return UserCountByProjectCode;
        }

        public List<Projects> GetCurrentUsersProjects(int userId)
        {
            List<Projects> userProjects = new List<Projects>();
            using (RdDbContext db = new RdDbContext())
            {
                Users user = db.Users.FirstOrDefault(u => u.UserId == userId);
                if (user != null)
                {
                    userProjects = db.Entry(user)
                        .Collection(u => u.Projects)
                        .Query()
                        .ToList();
                }
            }

            return userProjects;
        }
        public bool ProjectHasSameUserControl(int projectId, int userId)
        {
            using (RdDbContext db = new RdDbContext())
            {
                Projects project = db.Projects.Include(x => x.Users).FirstOrDefault(m => m.Id == projectId);

                if (project != null && project.Users.Any(x => x.UserId == userId))
                {
                    return true;
                }
            }

            return false;
        }

        public string FindProjectNameByCode(string code)
        {
            using (RdDbContext db = new RdDbContext())
            {
                Projects project = db.Projects.FirstOrDefault(x => x.ProjectCode == code);
                string projectName = project.ProjectName;
                return projectName;
            }
        }
        public string FindProjectCodeById(int id)
        {
            using (RdDbContext db = new RdDbContext())
            {
                Projects project = db.Projects.FirstOrDefault(x => x.Id == id);
                string projectCode = project.ProjectCode;
                return projectCode;
            }
        }

        public int FindProjectIdbyCode(string Code)
        {
            using (RdDbContext db = new RdDbContext())
            {
                Projects project = db.Projects.FirstOrDefault(x => x.ProjectCode == Code);
                int projectId = project.Id;
                return projectId;
            }
        }

        public int UsersForProjectById(int id)
        {
            using (RdDbContext db = new RdDbContext())
            {
                Projects project = db.Projects.
                    Include(u => u.Users).
                    FirstOrDefault(x => x.Id == id);
                int userscount = 0;
                userscount = project.Users.Count();
                return userscount;

            }
        }

        public decimal CalculateProjectProgress(int Id)
        {
            decimal progress = 0;
            Projects project = repo_projects.Find(x => x.Id == Id);
            List<HourlyActivity> hourlies = repohourly.List(x => x.ProjectCode == project.ProjectCode);


            return progress;
        }
    }
}
