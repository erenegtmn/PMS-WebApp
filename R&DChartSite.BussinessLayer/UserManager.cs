using RDChartSite.Entities;
using RDChartSite.BussinessLayer.Abstract;
using RDChartSite.BussinessLayer.Results;
using RDChartSite.Common.Helpers;
using RDChartSite.DataAccessLayer.EntitiyFramework;
using RDChartSite.Entities.Messages;
using RDChartSite.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace RDChartSite.BussinessLayer
{
    public class UserManager : ManagerBase<Users>
    {

        public List<Users> GetUsers() {

            using (RdDbContext db = new RdDbContext())
            {
                List<Users> users = db.Users
                    .Include(p => p.Projects)
                    .Include(da => da.DailyActivities)
                    .Include(t=> t.Team)
                    .ToList();
            
                return users;
            }

        }

       
        public BussinessLayerResult<Users> RegisterUser(SignUpViewModel data)
        {

            Users user = Find(x => x.Email == data.Email);
            BussinessLayerResult<Users> result = new BussinessLayerResult<Users>();

            if (user != null)
            {
                if (user.Email == data.Email)
                {
                    result.AddError(ErrorMessageCode.EmailAlreadyExists, "Zaten bu mail adresine ait bir kayıt bulunmaktadır.");
                }
            }
            else
            {
                int dbInsertResult = Insert(new Users()
                {
                    Email = data.Email,
                    Name = data.firstName,
                    Surname = data.lastName,
                    Password = data.Password,
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false,
                    TeamId = data.SelectedTeamId

                });
                if (dbInsertResult > 0)
                {
                    result.Results = Find(x => x.Email == data.Email);
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{result.Results.ActivateGuid}";
                    string body = $"<h3>Merhaba {result.Results.Name + " " + result.Results.Surname}</h3> <br> <br>" +
                        $"Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız</a>.";
                    MailHelper.SendMail("Hesap Aktivasyonu", body, result.Results.Email);

                }
            }


            return result;
        }

        public BussinessLayerResult<Users> AuthUser(LoginViewModel data)
        {
            Users user = Find(x => x.Email == data.Email && x.Password == data.password);
            BussinessLayerResult<Users> result = new BussinessLayerResult<Users>();
            result.Results = user;
            if (user != null)
            {
                if (!user.IsActive)
                {
                    result.AddError(ErrorMessageCode.UserIsNotAuthorized, "Hesabınız aktifleştirilmemiştir. Lütfen e-posta adresinizi kontrol edin.");
                }
            }
            else
            {
                result.AddError(ErrorMessageCode.UserNameorPassWrong, "Hatalı mail adresi veya şifre girdiniz");
            }
            return result;
        }

        public BussinessLayerResult<Users> ActivateUser(Guid id)
        {
            Guid activate_id = id;
            BussinessLayerResult<Users> result = new BussinessLayerResult<Users>();
            Users user = Find(x => x.ActivateGuid == activate_id);
            if (user != null)
            {
                if (!user.IsActive)
                {
                    user.IsActive = true;
                    Update(user);
                }

            }
            else
            {
                result.AddError(ErrorMessageCode.ActivateIdDoesNotExists, "");
            }
            return result;
        }

        public int TotalUserCount()
        {
            using (RdDbContext db = new RdDbContext())
            {
                int count;
                count = db.Users.Count();
                return count;
            }

        }
        public bool UserHasSameProjectControl(int projectId, int userId)
        {
            using (RdDbContext db = new RdDbContext())
            {
                Users user = db.Users.Include(x => x.Projects).FirstOrDefault(x => x.UserId == userId);

                if (user != null && user.Projects.Any(x => x.Id == projectId))
                {
                    return true;
                }
            }

            return false;
        }
        public Users GetUserIncludingProjects(int userId)
        {
            using (RdDbContext db = new RdDbContext())
            {
                Users user = db.Users.Include(x => x.Projects).FirstOrDefault(x => x.UserId == userId);
                if (user != null)
                {
                    return user;
                }
                return null;
            }
        }
        public List<Users> GetAllUsersIncludingProjects()
        {
            using (RdDbContext db = new RdDbContext())
            {
                List<Users> users = db.Users.Include(x => x.Projects).ToList();
                return users;
            }
        }
        public void DeleteProjectFromUser(int userId, int projectId)
        {
            using (RdDbContext db = new RdDbContext())
            {

                var user = db.Users.Include(x => x.Projects).FirstOrDefault(x => x.UserId == userId);
                var project = db.Projects.Include(x => x.Users).FirstOrDefault(p => p.Id == projectId);

                if (user != null && project != null)
                {
                    user.Projects.Remove(project);
                    project.Users.Remove(user);

                    db.SaveChanges();
                }
            }

        }
        public DailyActivity GetUsersCurrentDailyLog(int id, DateTime date)
        {
            using (RdDbContext db = new RdDbContext())
            {
                DailyActivity daily = db.DailyActivities.Include(x => x.HourlyActivity).FirstOrDefault(x => x.UserId == id && x.Date == date);

                return daily;

            }
        }

        //public Dictionary<DateTime, double> GetDailyWorkHours(int userId)
        //{
        //    using (RdDbContext db = new RdDbContext())
        //    {


        //        Users user = db.Users.Include(u => u.DailyActivities)
        //                                .FirstOrDefault(u => u.Id == userId);
        //        DailyActivity daily = db.DailyActivities.Include(d => d.HourlyActivities).FirstOrDefault(u => u.UserId == user.Id);
        //        double totalhours = daily.HourlyActivities.Count();
        //        if (user == null)
        //        {

        //            return null;
        //        }

        //        Dictionary<DateTime, double> dailyWorkHours = new Dictionary<DateTime, double>();

        //        foreach (var dailyActivity in user.DailyActivities)
        //        {
        //            double totalWorkHours = totalhours;
        //            dailyWorkHours[dailyActivity.Date] = totalWorkHours;
        //        }

        //        return dailyWorkHours;
        //    }
        //}

        public Dictionary<string, int> GetProjectRecordsForUser(int userId)
        {
            using (RdDbContext db = new RdDbContext())
            {
                var result = from dailyActivity in db.DailyActivities
                             join hourlyActivity in db.HourlyActivity on dailyActivity.Id equals hourlyActivity.DailyActivityId
                             where dailyActivity.UserId == userId
                             group hourlyActivity by hourlyActivity.ProjectCode into g
                             select new { ProjectCode = g.Key, RecordCount = g.Count() };
                return result.ToDictionary(x => x.ProjectCode, x => x.RecordCount);
            }

        }

        public Users GetUser(int id)
        {
            using (RdDbContext db = new RdDbContext())
            {
                Users user = db.Users
                               .Include(u => u.Projects)
                               .Include(u => u.DailyActivities)
                               .Include(u => u.OffDayForms)
                               .Include(u => u.Team)
                               .FirstOrDefault(x => x.UserId == id);
                return user;
            }
        }
        public string GetUserNameById(int Id)
        {
            using (RdDbContext db = new RdDbContext())
            {
                Users user = db.Users.FirstOrDefault(x => x.UserId == Id);
                string name = user.Name + " " + user.Surname;
                return name;
            }
        }

        public bool IsUserLeader(int userId)
        {
            bool state = new bool();
            using (var dbContext = new RdDbContext())
            {
                List<Users> users = GetUsers();
                foreach (Users user in users)
                {
                    if (user.IsTeamLeader)
                    {
                        return true;
                    }
                }
            }


            return state;
        }

    }
}
