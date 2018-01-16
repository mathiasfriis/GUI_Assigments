using JavaJamAjax.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaJamAjax.DataAccess
{
    public class MusicDBContext : DbContext
    {
        // Use the default connectionstring (and perhaps rename the file name)
        public MusicDBContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Music> MusicEntetainments { get; set; }
    }
}
