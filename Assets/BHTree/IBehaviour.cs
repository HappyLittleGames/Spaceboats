using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.BHTree
{
    public interface IBehaviour
    {
        BHStatus Tick();
        BHStatus Status { get; }

        Action Initialize { set; }
        Func<BHStatus> Update { set; }
        Action<BHStatus> Terminate { set; }
    }
}