using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

#nullable disable

namespace Bus.Models
{
    public partial class Configuration
    {
        [Key]
        [JsonIgnore]
        public bool Id { get; set; }
        public string AppVersion { get; set; }
        public bool? IsPeak { get; set; }
        public byte? Weather { get; set; }
        public byte? SpeedContributionFactor { get; set; }
        public string ImageDrawerDirectory { get; set; }
        public byte? Timeout { get; set; }
        public byte? DriverTimeout { get; set; }
        public byte? DriverLocationGetterInterval { get; set; }
        public byte? DriverLocationSetterInterval { get; set; }
        public DateTime? ModificationDate { get; set; }
        public int? Modifier { get; set; }

        public virtual Person ModifierNavigation { get; set; }
    }
}
