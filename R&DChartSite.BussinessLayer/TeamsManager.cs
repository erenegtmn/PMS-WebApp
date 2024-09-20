using RDChartSite.Entities;
using RDChartSite.Entities.ValueObjects;
using RDChartSite.BussinessLayer.Abstract;
using RDChartSite.BussinessLayer.Results;
using RDChartSite.DataAccessLayer.EntitiyFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace RDChartSite.BussinessLayer
{
    public class TeamsManager : ManagerBase<Teams>
    {
        Repository<Teams> repositoryteams = new Repository<Teams>();
        Repository<Users> repositoryusers = new Repository<Users>();
        public List<Teams> GetTeams() {

            using (RdDbContext db = new RdDbContext())
            {
                List<Teams> teams = db.Teams
                    .Include(x =>x.Users)
                    .Include(x=>x.Leader)
                    .ToList();
                return teams;
            }

        }
        public List<Teams> GetTeamsForProject(String projectCode)
        {
            using (RdDbContext db = new RdDbContext())
            {
                var teams = db.Projects
             .Where(p => p.ProjectCode == projectCode)
             .SelectMany(p => p.Users)
             .Where(u => u.Projects.Any(proj => proj.ProjectCode == projectCode))
             .Select(u => u.Team)
             .Distinct()
             .Include(t => t.Users)
             .ToList();
                return teams;
            }
        }
        public void CreateTeam(Teams team, int leaderId)
        {
            Users user = repositoryusers.Find(x=>x.UserId == leaderId);
            Teams newteam = new Teams
            {

                TeamName = team.TeamName
            };
            repositoryteams.Insert(newteam);
            user.TeamId = newteam.TeamId;
            user.IsTeamLeader = true;
            repositoryusers.Update(user);

            newteam.LeaderId = leaderId;
            newteam.Leader = user;
            repositoryteams.Update(newteam);

            user.Team = newteam;
            repositoryusers.Update(user);

        }


        public Teams GetTeam(int id) 
        {
            using (RdDbContext db = new RdDbContext())
            {
                Teams team = db.Teams.Include(t => t.Users).FirstOrDefault(t => t.TeamId == id);
                return team;
            }
        }

        public Teams GetTeamByName(string name)
        {
            using (RdDbContext db = new RdDbContext())
            {
                Teams team = db.Teams.Include(t => t.Users).FirstOrDefault(t => t.TeamName == name);
                return team;
            }
        }

        public string IdtoName(int? Id)
        {
            if (Id == null)
            {
                return "system";
            }
            else
            {

                Teams team = Find(x => x.TeamId == Id);
                return team.TeamName;
            }
        }

        public List<int> GetLeadersList()
        {
            using (var dbContext = new RdDbContext())
            {
                var leaderIds = dbContext.Teams
                    .Where(team => team.LeaderId.HasValue)
                    .Select(team => team.LeaderId.Value)
                    .ToList();

                return leaderIds;
            }
        }

        public bool IsUserLeader(int userId)
        {
             bool state = new bool();
            using (var dbContext = new RdDbContext())
            {
                List<Teams> teams = GetTeams();
                foreach (Teams team in teams)
                {
                    if (team.LeaderId == userId)
                    {
                        return true;
                    }
                }
            }


            return state;
        }


    }
}
