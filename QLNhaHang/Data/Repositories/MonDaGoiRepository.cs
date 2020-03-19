using Data.Interfaces;
using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNhaHang.Data.Repositories
{
    public interface IMonDaGoiRepository : IRepository<MonDaGoi>
    {

    }
    public class MonDaGoiRepository : Repository<MonDaGoi>, IMonDaGoiRepository
    {
        public MonDaGoiRepository(QLNhaHangDbContext context) : base(context)
        {
        }
    }
}