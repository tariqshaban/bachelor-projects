using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Bus.Models
{
    public partial class Path
    {
        public Path()
        {
            Stops = new HashSet<Stop>();
        }

        [Key, Column(Order = 0)]
        public int RouteId { get; set; }
        [Key, Column(Order = 1)]
        public byte Type { get; set; }
        public int? ImageId { get; set; }
        public string StartName { get; set; }
        public string StartNameAr { get; set; }
        public string EndName { get; set; }
        public string EndNameAr { get; set; }
        public string Path1 { get; set; }
        public bool? IsCircular { get; set; }
        public decimal? AverageSpeed { get; set; }
        public DateTime? ModificationDate { get; set; }
        public int? Modifier { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? Creator { get; set; }

        public virtual Person CreatorNavigation { get; set; }
        public virtual Image Image { get; set; }
        public virtual Person ModifierNavigation { get; set; }
        public virtual Route Route { get; set; }
        public virtual ICollection<Stop> Stops { get; set; }
    }
}
