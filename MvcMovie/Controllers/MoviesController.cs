using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcMovie.Models;


namespace MvcMovie.Controllers
{
	public class MoviesController : Controller
	{

		private MovieDBContext db = new MovieDBContext();

		public ActionResult Index(string movieGenre, string searchString)
		{

			var GenreList = new List<string>();

			var GenreQuery = from d in db.Movies
							 orderby d.Genre
							 select d.Genre;

			GenreList.AddRange(GenreQuery.Distinct());
			ViewBag.movieGenre = new SelectList(GenreList);

			var movies = from m in db.Movies
						 select m;

			if (!String.IsNullOrEmpty(searchString))
			{
				movies = movies.Where(s => s.Title.Contains(searchString));
			}

			if (!string.IsNullOrEmpty(movieGenre))
			{
				movies = movies.Where(x => x.Genre == movieGenre);
			}

			return View(movies);

		}

		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var movie = db.Movies.Find(id);

			if (movie == null)
			{
				return HttpNotFound();
			}

			return View(movie);

		}

		public ActionResult Create()
		{
			return View(new Movie() {
				Genre = "Test",
				Price = 5.99M,
				ReleaseDate = DateTime.Now,
				Rating = "G",
				Title = "Ghost Busters"
			});
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "ID,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
		{
			if (ModelState.IsValid)
			{
				db.Movies.Add(movie);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(movie);
		}


		public ActionResult Edit(int? id)
		{

			if (id == null)
			{ return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

			var movie = db.Movies.Find(id);
			if (movie == null)
			{
				return HttpNotFound();
			}

			return View(movie);

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "ID,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
		{
			if (ModelState.IsValid)
			{
				db.Entry(movie).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(movie);
		}

		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var movie = db.Movies.Find(id);
			if (movie == null)
			{
				return HttpNotFound();
			}

			return View(movie);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			var movie = db.Movies.Find(id);
			db.Movies.Remove(movie);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

	}
}
