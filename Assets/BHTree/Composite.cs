using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.BHTree
{
    public abstract class Composite : Behaviour
    {
        protected List<IBehaviour> Children { get; set; }
        protected Composite()
        {
            Children = new List<IBehaviour>();
            Initialize = () => { };
            Terminate = status => { };
            Update = () => BHStatus.Running;
        }

        public IBehaviour GetChild(int index)
        {
            return Children[index];
        }

        public int ChildCount
        {
            get { return Children.Count; }
        }
    }
}

