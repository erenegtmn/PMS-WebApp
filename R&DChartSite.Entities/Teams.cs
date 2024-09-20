using RDChartSite.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace RDChartSite.Entities
{
    public class Teams
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamId { get; set; }

        [DisplayName("Grup İsmi")]
        public string TeamName { get; set; }

        [ForeignKey("Leader")]
        public int? LeaderId { get; set; }
        public Users Leader { get; set; }

        [InverseProperty("Team")]
        public ICollection<Users> Users { get; set; } = new List<Users>();

    }
}
