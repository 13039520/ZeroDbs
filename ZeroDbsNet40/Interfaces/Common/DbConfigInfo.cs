﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Interfaces.Common
{
    public class DbConfigInfo
    {
        public List<DbConfigDatabaseInfo> Dbs { get; set; }
        public List<DbConfigDataviewInfo> Dvs { get; set; }
    }
}
