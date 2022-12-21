using Microsoft.EntityFrameworkCore;
using MSysICTSBM.Dal.DataContexts.Models.DB.MainModels;
using MSysICTSBM.Dal.DataContexts.Models.DB.MainSPModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSysICTSBM.Dal.DataContexts.Models.DB
{
    public class MSysMainDb : MSysMainEntities
    {

        public MSysMainDb()
        {

        }

        public MSysMainDb(DbContextOptions<MSysMainEntities> options)
            : base(options)
        {
        }


        public DbSet<sp_Get_ActiveULB_result> sp_Get_ActiveULB_results { get; set; }
        public DbSet<sp_getULB_statusById_Result> sp_getULB_statusById_Results { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<sp_Get_ActiveULB_result>().HasNoKey();
            modelBuilder.Entity<sp_getULB_statusById_Result>().HasNoKey();

        }

    }
}
