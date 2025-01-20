﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieGame
{
    public interface IIdentificable
    {
        int? Id { get; set; }
        bool IsFromPool { get; set; }
    }
}
