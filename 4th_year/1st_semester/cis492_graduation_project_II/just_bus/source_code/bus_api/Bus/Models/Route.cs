using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

#nullable disable

namespace Bus.Models
{
    public partial class Route
    {
        public Route()
        {
            Paths = new HashSet<Path>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public int? Views { get; set; }
        public DateTime? ModificationDate { get; set; }
        public int? Modifier { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? Creator { get; set; }

        public virtual Person CreatorNavigation { get; set; }
        public virtual Person ModifierNavigation { get; set; }
        public virtual ICollection<Path> Paths { get; set; }
    }
}
