﻿using Data.Interfaces;
using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNhaHang.Data.Repositories
{
    public interface IPhaCheRepository : IRepository<PhaChe>
    {

    }
    public class PhaCheRepository : Repository<PhaChe>, IPhaCheRepository
    {
        public PhaCheRepository(QLNhaHangDbContext context) : base(context)
        {
        }
    }
}