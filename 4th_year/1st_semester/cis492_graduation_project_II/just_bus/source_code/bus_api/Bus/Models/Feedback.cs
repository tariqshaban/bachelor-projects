using System;
using Newtonsoft.Json;

#nullable disable

namespace Bus.Models
{
    public partial class Feedback
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int? PersonId { get; set; }
        public string Message { get; set; }
        public string Model { get; set; }
        public string Os { get; set; }
        public string Phone { get; set; }
        public DateTime DateCreated { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Person Person { get; set; }
    }
}
