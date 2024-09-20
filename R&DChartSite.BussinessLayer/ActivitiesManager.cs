
using RDChartSite.BussinessLayer.Abstract;
using RDChartSite.DataAccessLayer.EntitiyFramework;
using RDChartSite.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Data.Entity;
using RDChartSite.Entities.ValueObjects;
using System.Runtime.Remoting.Contexts;
using System.Runtime.InteropServices.ComTypes;

namespace RDChartSite.BussinessLayer
{
    public class ActivitiesManager : ManagerBase<DailyActivity>
    {
        private static readonly Random random = new Random();

        Repository<DailyActivity> repositoryDaily = new Repository<DailyActivity>();
        Repository<HourlyActivity> repositoryHourly = new Repository<HourlyActivity>();


        public List<DailyActivity> GetActivities()
        {
            using (var context = new RdDbContext())
            {
                

                var activities = context.DailyActivities.Include(x=>x.HourlyActivity).ToList();

                return activities;
            }
        }
        public List<HourlyActivity> Filterbyteam(String projectCode, int? _teamId, DateTime? startDate, DateTime? endDate)
        {
            using (var context = new RdDbContext())
            {
                var teamActivities = context.HourlyActivity
                    .Where(ha => ha.ProjectCode == projectCode)
                    .Include(ha => ha.DailyActivity).Where(a => a.DailyActivity.Date >= startDate && a.DailyActivity.Date <= endDate)
                    .Include(da => da.DailyActivity.User)
                    .Include(u => u.DailyActivity.User.Team)
                    .Where(ha => ha.DailyActivity.User.Team.TeamId == _teamId)
                    .OrderBy(da => da.DailyActivity.Date)
                    .ToList();

                return teamActivities;
            }
        }

        public List<HourlyActivity> FilterbyTeamAndUser(String projectCode, int? _teamId,int? userId, DateTime? startDate, DateTime? endDate)
        {
            using (var context = new RdDbContext())
            {
                var teamActivities = context.HourlyActivity
                    .Where(ha => ha.ProjectCode == projectCode)
                    .Include(ha => ha.DailyActivity).Where(a => a.DailyActivity.Date >= startDate && a.DailyActivity.Date <= endDate)
                    .Include(da => da.DailyActivity.User).Where(u=> u.DailyActivity.User.UserId == userId)
                    .Include(u => u.DailyActivity.User.Team)
                    .Where(ha => ha.DailyActivity.User.Team.TeamId == _teamId)
                    .OrderBy(da => da.DailyActivity.Date)
                    .ToList();

                return teamActivities;
            }
        }
        public List<HourlyActivity> GetDailyActivitiesHourlyLog(DateTime date)
        {
            List<HourlyActivity> hourly;

            using (var context = new RdDbContext())
            {
                hourly = context.DailyActivities
                    .Where(x => x.Date == date)
                    .SelectMany(da => da.HourlyActivity)
                    .ToList();
            }

            return hourly;
        }


        public int CountMonthlyWorkHour(int id)
        {

            using (var context = new RdDbContext())
            {
                DateTime now = DateTime.Now;
                int currentMonth = now.Month;
                int currentYear = now.Year;

                int totalWorkHour = context.DailyActivities
                    .Where(da => da.UserId == id && da.Date.Month == currentMonth && da.Date.Year == currentYear)
                    .SelectMany(da => da.HourlyActivity)
                    .Where(ha => ha.Time != null)
                    .Count();

                return totalWorkHour;
            }
        }

        public string GenerateRandomString()
        {
            const int minLength = 60;
            int randomStringLength = random.Next(minLength, 150);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string result = "";

            for (int i = 0; i < randomStringLength; i++)
            {
                int randomIndex = random.Next(chars.Length);
                result += chars[randomIndex];
            }

            return result;
        }



 

        public int GetHourlyActivityCount(int userId)
        {
            using (var context = new RdDbContext())
            {
                var count = context.DailyActivities
                    .Where(d => d.UserId == userId)
                    .SelectMany(d => d.HourlyActivity)      
                    .Count();

                return count;
            }
        }
        public int ProjectHoursCount(string ProjectCode)
        {
            using (var context = new RdDbContext())
            {
                var count = context.HourlyActivity
                    .Where(d => d.ProjectCode == ProjectCode)
                    .Count();

                return count;
            }
        }

        public int ProjectWeeklyHoursCount(string ProjectCode)
        {

            DateTime today = DateTime.Today;

            Tuple<DateTime, DateTime> weekBounds = GetWeekBounds(today);
            DateTime weekStart = weekBounds.Item1;
            DateTime weekEnd = weekBounds.Item2;
            int count = 0;
            using (var context = new RdDbContext())
            {
                count = context.DailyActivities
               .Where(d => d.Date >= weekStart && d.Date <= weekEnd)
               .SelectMany(d => d.HourlyActivity)
               .Where(h => h.ProjectCode == ProjectCode)
               .Count();
                return count;
            }
        }

        public int ProjectHoursCount(int id)
        {
            ProjectsManager projectsManager = new ProjectsManager();
            string projectcode = projectsManager.FindProjectCodeById(id);
            using (var context = new RdDbContext())
            {
                var count = context.HourlyActivity
                    .Where(d => d.ProjectCode == projectcode)
                    .Count();

                return count;
            }
        }

        public int ProjectUsersCount(string ProjectCode)
        {
            using (var context = new RdDbContext())
            {
                var count = context.Projects
                    .Where(p => p.ProjectCode == ProjectCode)
                    .SelectMany(p => p.Users)
                    .Count();

                return count;
            }
        }

        private static Tuple<DateTime, DateTime> GetWeekBounds(DateTime date)
        {
            int currentWeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int currentYear = date.Year;

            DateTime weekStart = new DateTime(currentYear, 1, 1).AddDays((currentWeekNumber - 1) * 7);
            DayOfWeek startDayOfWeek = CultureInfo.CurrentCulture.Calendar.GetDayOfWeek(weekStart);

            if (startDayOfWeek != DayOfWeek.Monday)
                weekStart = weekStart.AddDays(7 - (int)startDayOfWeek + 1);

            DateTime weekEnd = weekStart.AddDays(6);

            return Tuple.Create(weekStart, weekEnd);
        }

        public List<DailyActivity> GetDailyActivitiesCurrentWeek()
        {
            DateTime today = DateTime.Today;

            Tuple<DateTime, DateTime> weekBounds = GetWeekBounds(today);
            DateTime weekStart = weekBounds.Item1;
            DateTime weekEnd = weekBounds.Item2;

            using (RdDbContext db = new RdDbContext())
            {

                List<DailyActivity> weeklyActivities = db.DailyActivities
                 .Include(da => da.HourlyActivity)
                 .Include(da => da.User)
                 .Where(da => da.Date >= weekStart && da.Date <= weekEnd)
                 .OrderBy(da => da.Date)
                 .ToList();

                return weeklyActivities;
            }
        }

        public int GetHourlyActivityCountInWeek()
        {
            DateTime today = DateTime.Today;

            Tuple<DateTime, DateTime> weekBounds = GetWeekBounds(today);
            DateTime weekStart = weekBounds.Item1;
            DateTime weekEnd = weekBounds.Item2;

            using (var context = new RdDbContext())
            {
                var weeklyData = context.DailyActivities
                    .Where(d => d.Date >= weekStart && d.Date <= weekEnd)
                    .ToList();

                return weeklyData.Count;
            }
        }


        public int GetHourlyActivityCount(int userId, DateTime date)
        {
            using (var context = new RdDbContext())
            {
                var count = context.DailyActivities
                    .Where(d => d.UserId == userId && d.Date == date)
                    .SelectMany(d => d.HourlyActivity)
                    .Count();

                return count;
            }
        }
        public bool HourlyControl(int userId, DateTime date, TimeSpan time)
        {
            using (var context = new RdDbContext())
            {
                bool existingRecord = context.HourlyActivity
                    .Any(a => a.DailyActivity.UserId == userId && a.DailyActivity.Date == date && a.Time == time);

                return existingRecord;
            }
        }
        public bool DailyExistControl(DateTime date, int UserId)
        {
            bool existingdaily;
            using (var context = new RdDbContext())
            {

                existingdaily = context.DailyActivities.Any(x => x.Date == date && x.User.UserId == UserId);
                return existingdaily;
            }
        }

        public Dictionary<string, int> GetTimeCountByProjectCode()
        {
            Dictionary<string, int> timeCountByProjectCode = new Dictionary<string, int>();
            using (RdDbContext db = new RdDbContext())
            {
                var hourlyActivities = db.HourlyActivity.GroupBy(x => x.ProjectCode);
                foreach (var group in hourlyActivities)
                {
                    string projectCode = group.Key;
                    int timeCount = group.Count();
                    timeCountByProjectCode.Add(projectCode, timeCount);
                }
            }
            return timeCountByProjectCode;
        }
    }
}
