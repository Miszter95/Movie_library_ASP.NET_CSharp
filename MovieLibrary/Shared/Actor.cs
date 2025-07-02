using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MovieLibrary.Shared
{
    public class Actor
    {
        public Actor(string name, string placeOfBirth, string partner)
        {
            Name = name;
            PlaceOfBirth = placeOfBirth;
            Partner = partner;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Name is too long.")]
        public string Name { get; set; }

        [Required]
        public DateTime Born { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Place of birth is too long.")]
        public string PlaceOfBirth { get; set; }

        public double Height { get; set;}

        public string Partner { get; set;}

        [JsonIgnore]
        public List<Movie> Movies { get; set; } = new List<Movie>();
    }
}
