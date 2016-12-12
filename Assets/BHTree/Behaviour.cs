using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.BHTree
{
    public class Behaviour : IBehaviour
    {
        public Action Initialize { protected get; set; }
        public Func<BHStatus> Update { protected get; set; }
        public Action<BHStatus> Terminate { protected get; set; }
        public BHStatus Status { get; set; }
        public BHStatus Tick()
        {
            return Status;
        }
    }
}