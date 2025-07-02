using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MovieLibrary.Shared
{
    public class Genre
    {
        public Genre(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Name is too long.")]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [JsonIgnore]
        public List<Movie> Movies { get; set; } = new List<Movie>();
    }
}
