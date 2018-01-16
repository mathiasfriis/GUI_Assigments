using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using JavaJamAjax.DataAccess;
using JavaJamAjax.Models;

namespace JavaJamAjax.Controllers
{
    public class MusicsController : ApiController
    {
        private MusicDBContext db = new MusicDBContext();

        // GET: api/Musics
        public IQueryable<Music> GetMusicEntetainments()
        {
            var upcommingMusics = from music in db.MusicEntetainments
                                  where music.Year > DateTime.Now.Year || (music.Year == DateTime.Now.Year && music.Month >= DateTime.Now.Month)
                                  orderby music.Year ascending, music.Month ascending
                                  select music;
            return upcommingMusics;

            //return db.MusicEntetainments;
        }

        // GET: api/Musics/5
        [ResponseType(typeof(Music))]
        public IHttpActionResult GetMusic(int id)
        {
            Music music = db.MusicEntetainments.Find(id);
            if (music == null)
            {
                return NotFound();
            }

            return Ok(music);
        }

        // PUT: api/Musics/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMusic(int id, Music music)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != music.Id)
            {
                return BadRequest();
            }

            db.Entry(music).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MusicExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Musics
        [ResponseType(typeof(Music))]
        public IHttpActionResult PostMusic(Music music)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MusicEntetainments.Add(music);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = music.Id }, music);
        }

        // DELETE: api/Musics/5
        [ResponseType(typeof(Music))]
        public IHttpActionResult DeleteMusic(int id)
        {
            Music music = db.MusicEntetainments.Find(id);
            if (music == null)
            {
                return NotFound();
            }

            db.MusicEntetainments.Remove(music);
            db.SaveChanges();

            return Ok(music);
        }

        // GET: api/Musics/Date/2015/08
        [HttpGet]
        public IQueryable<Music> Date(int year, int month = 0, int day = 0)
        {
            IQueryable<Music> musicsOnDate;
            if (month == 0)
                musicsOnDate = from music in db.MusicEntetainments
                               where music.Year == year
                               select music;
            else
                musicsOnDate = from music in db.MusicEntetainments
                               where music.Year == year && music.Month == month
                               select music;
            return musicsOnDate;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MusicExists(int id)
        {
            return db.MusicEntetainments.Count(e => e.Id == id) > 0;
        }
    }
}