using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Bus.Models
{
    public partial class Stop
    {
        [Key, Column(Order = 0)]
        public int RouteId { get; set; }
        [Key, Column(Order = 1)]
        public byte PathType { get; set; }
        [Key, Column(Order = 2)]
        public byte Sequence { get; set; }
        public int? ImageId { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public DateTime? ModificationDate { get; set; }
        public int? Modifier { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? Creator { get; set; }

        public virtual Person CreatorNavigation { get; set; }
        public virtual Image Image { get; set; }
        public virtual Person ModifierNavigation { get; set; }
        public virtual Path Path { get; set; }
    }
}
