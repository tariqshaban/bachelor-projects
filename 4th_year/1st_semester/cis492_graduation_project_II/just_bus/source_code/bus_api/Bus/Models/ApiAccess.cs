using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

#nullable disable

namespace Bus.Models
{
    public partial class ApiAccess
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        public string Action { get; set; }
        public string Path { get; set; }
        public DateTime DateAccessed { get; set; }
    }
}
