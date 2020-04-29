using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebmotorsTeste.Web.Models;

namespace WebmotorsTeste.Web.Data
{
    public class WebmotorsContext : DbContext
    {
        public WebmotorsContext(DbContextOptions<WebmotorsContext> options) : base(options) { }

        public DbSet<Anuncio> Anuncios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Anuncio>().ToTable("tb_AnuncioWebmotors");
        }
    }
}
