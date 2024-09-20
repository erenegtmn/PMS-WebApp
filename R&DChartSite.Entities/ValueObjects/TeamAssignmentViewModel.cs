using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RDChartSite.Entities;


namespace RDChartSite.Entities.ValueObjects
{
    public class TeamAssignmentViewModel
    {
        [Required(ErrorMessage = "Kullanıcı seçimi zorunludur.")]
        public int SelectedUserId { get; set; }

        [Required(ErrorMessage = "Takım seçimi zorunludur.")]
        public int SelectedTeamId { get; set; }

        public List<Users> Users { get; set; }
        public List<Teams> Teams { get; set; }
    }
}
