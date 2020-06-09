using Data.Interfaces;
using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNhaHang.Data.Repositories
{
    public interface IBepRepository : IRepository<Bep>
    {

    }
    public class BepRepository : Repository<Bep>, IBepRepository
    {
        public BepRepository(QLNhaHangDbContext context) : base(context)
        {
        }
    }
}