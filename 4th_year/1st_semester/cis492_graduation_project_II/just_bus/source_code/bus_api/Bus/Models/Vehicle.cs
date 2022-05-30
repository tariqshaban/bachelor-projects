using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

#nullable disable

namespace Bus.Models
{
    public partial class Vehicle
    {
        public Vehicle()
        {
            Drivers = new HashSet<Driver>();
        }

        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        public int? ImageId { get; set; }
        public string PlateNumber { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerAr { get; set; }
        public string Model { get; set; }
        public string ModelAr { get; set; }
        public string Color { get; set; }
        public string SecondaryColor { get; set; }
        public byte? Capacity { get; set; }
        public DateTime? ModificationDate { get; set; }
        public int? Modifier { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? Creator { get; set; }

        public virtual Person CreatorNavigation { get; set; }
        public virtual Image Image { get; set; }
        public virtual Person ModifierNavigation { get; set; }
        public virtual ICollection<Driver> Drivers { get; set; }
    }
}
