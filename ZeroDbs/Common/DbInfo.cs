﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    public class DbInfo
    {
        public string Key { get; set; }
        public string ConnectionString { get; set; }
        public DbType Type { get; set; }
    }
}