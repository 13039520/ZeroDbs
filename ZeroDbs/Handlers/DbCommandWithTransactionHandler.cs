using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    public delegate bool DbCommandWithTransactionHandler(IDbCommand cmd);
}
