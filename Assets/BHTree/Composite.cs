using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.BHTree
{
    public abstract class Composite : Behaviour
    {
        protected List<IBehaviour> m_children { get; set; }

        protected Composite()
        {
            m_children = new List<IBehaviour>();
            Initialize = () => { };
            Terminate = status => { };
            Update = () => BHStatus.Running;
        }

        public IBehaviour GetChild(int index)
        {
            return m_children[index];
        }

        public int ChildCount()
        {
            return m_children.Count;
        }
    }
}

