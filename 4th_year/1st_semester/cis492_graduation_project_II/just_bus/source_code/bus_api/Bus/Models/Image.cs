using System;
using System.Collections.Generic;
using Newtonsoft.Json;

#nullable disable

namespace Bus.Models
{
    public partial class Image
    {
        public Image()
        {
            Drivers = new HashSet<Driver>();
            Paths = new HashSet<Path>();
            Stops = new HashSet<Stop>();
            Vehicles = new HashSet<Vehicle>();
        }

        [JsonIgnore]
        public int ImageId { get; set; }
        public string Directory { get; set; }
        public string Name { get; set; }
        public bool? Is360 { get; set; }
        public DateTime? ModificationDate { get; set; }
        public int? Modifier { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? Creator { get; set; }

        public virtual Person CreatorNavigation { get; set; }
        public virtual Person ModifierNavigation { get; set; }
        [JsonIgnore]
        public virtual ICollection<Driver> Drivers { get; set; }
        [JsonIgnore]
        public virtual ICollection<Path> Paths { get; set; }
        [JsonIgnore]
        public virtual ICollection<Stop> Stops { get; set; }
        [JsonIgnore]
        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}
