using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.Shared
{
    public class Movie
    {
        public Movie(string title, string directors, string writers, string plot)
        {
            Title = title;
            Directors = directors;
            Writers = writers;
            Plot = plot;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Movie title is required.", AllowEmptyStrings = false)]
        public string Title { get; set; }

        public List<Genre> Genres { get; set; } = new List<Genre>();

        public int Length { get; set; }

        public int ReleaseDate { get; set; }

        public string Directors { get; set; }

        public string Writers { get; set; }

        public string Plot { get; set; }

        public int Certificate { get; set; }

        public int MetaScore { get; set; }

        public List<Actor> Actors { get; set; } = new List<Actor>();

    }
}
