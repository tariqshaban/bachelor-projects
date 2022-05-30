using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Bus.Models
{
    public partial class Driver
    {
        [Key, Column(Order = 0)]
        public int PersonId { get; set; }
        [Key, Column(Order = 1)]
        public int VehicleId { get; set; }
        [Key, Column(Order = 2)]
        public int? RouteId { get; set; }
        public int? ImageId { get; set; }
        public decimal? LastLatitude { get; set; }
        public decimal? LastLongitude { get; set; }
        public DateTime? LastLocationUpdate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public int? Modifier { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? Creator { get; set; }

        public virtual Person CreatorNavigation { get; set; }
        public virtual Image Image { get; set; }
        public virtual Person ModifierNavigation { get; set; }
        public virtual Person Person { get; set; }
        public virtual Vehicle Vehicle { get; set; }

        public Driver ShowOnlyPersonName()
        {
            if (Person != null)
            {
                Person.Id = -1;
                Person.Number = null;
                Person.Password = null;
                Person.Salt = null;
                Person.Role = null;
            }

            return this;
        }
    }
}
