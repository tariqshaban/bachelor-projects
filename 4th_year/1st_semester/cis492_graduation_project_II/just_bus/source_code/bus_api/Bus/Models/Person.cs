using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

#nullable disable

namespace Bus.Models
{
    public partial class Person
    {
        public Person()
        {
            Configurations = new HashSet<Configuration>();
            DriverCreatorNavigations = new HashSet<Driver>();
            DriverModifierNavigations = new HashSet<Driver>();
            InverseCreatorNavigation = new HashSet<Person>();
            InverseModifierNavigation = new HashSet<Person>();
            PathCreatorNavigations = new HashSet<Path>();
            PathModifierNavigations = new HashSet<Path>();
            RefreshTokens = new HashSet<RefreshToken>();
            RouteCreatorNavigations = new HashSet<Route>();
            RouteModifierNavigations = new HashSet<Route>();
            StopCreatorNavigations = new HashSet<Stop>();
            StopModifierNavigations = new HashSet<Stop>();
        }

        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string Number { get; set; }
        public string Password { get; set; }
        [JsonIgnore]
        public string Salt { get; set; }
        public byte? Role { get; set; }
        public DateTime? ModificationDate { get; set; }
        public int? Modifier { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? Creator { get; set; }

        public virtual Person CreatorNavigation { get; set; }
        public virtual Person ModifierNavigation { get; set; }
        public virtual Driver DriverPerson { get; set; }
        public virtual ICollection<Configuration> Configurations { get; set; }
        public virtual ICollection<Driver> DriverCreatorNavigations { get; set; }
        public virtual ICollection<Driver> DriverModifierNavigations { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Person> InverseCreatorNavigation { get; set; }
        public virtual ICollection<Person> InverseModifierNavigation { get; set; }
        public virtual ICollection<Path> PathCreatorNavigations { get; set; }
        public virtual ICollection<Path> PathModifierNavigations { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        public virtual ICollection<Route> RouteCreatorNavigations { get; set; }
        public virtual ICollection<Route> RouteModifierNavigations { get; set; }
        public virtual ICollection<Stop> StopCreatorNavigations { get; set; }
        public virtual ICollection<Stop> StopModifierNavigations { get; set; }
        public virtual ICollection<Vehicle> VehicleCreatorNavigations { get; set; }
        public virtual ICollection<Vehicle> VehicleModifierNavigations { get; set; }
        public virtual ICollection<Image> ImageCreatorNavigations { get; set; }
        public virtual ICollection<Image> ImageModifierNavigations { get; set; }
    }
}
