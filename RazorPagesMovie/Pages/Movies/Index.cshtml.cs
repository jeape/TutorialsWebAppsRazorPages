using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieApp.Data;

namespace MovieApp.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly MovieContext _context;

        public IndexModel(MovieContext context)
        {
            _context = context;
        }

        public IList<Models.Movie> Movie { get;set; }

        [BindProperty(SupportsGet = true)]
        public String SearchString { get; set; }

        public SelectList Genres { get; set; }

        [BindProperty(SupportsGet = true)]
        public String MovieGenre { get; set; }
        
        public async Task OnGetAsync()
        {
            var movies = from movie in _context.Movie
                select movie;

            IQueryable<String> genreQuery = from m in _context.Movie
                             orderby m.Genre
                             select m.Genre;

            if (!String.IsNullOrEmpty(SearchString))
            {
                movies = movies.Where(m => m.Title.Contains(SearchString));
            }

            if (!String.IsNullOrEmpty(MovieGenre))
            {
                movies = movies.Where(m => m.Genre.Equals(MovieGenre));
            }
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());
            Movie = await movies.ToListAsync();
        }
    }
}
