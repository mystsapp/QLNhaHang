using Data.Interfaces;
using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNhaHang.Data.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {

    }
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(QLNhaHangDbContext context) : base(context)
        {
        }
    }
}