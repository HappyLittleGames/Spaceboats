using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.BHTree
{
    public class Sequence : Composite
    {
        private int m_sequence = 0;
        public Sequence()
        {
            Update = () =>
            {
                for (;;)
                {
                    BHStatus status = GetChild(m_sequence).Tick();
                    if (status != BHStatus.Success)
                    {
                        if (status == BHStatus.Failure)
                        {
                            m_sequence = 0;
                        }
                        return status;
                    }
                    if (++m_sequence == ChildCount())
                    {
                        m_sequence = 0;
                        return BHStatus.Success;
                    }
                }
            };

            Initialize = () => { m_sequence = 0; };  // less delegates maybe?
        }

        public void SetSequence(List<IBehaviour> behaviours)
        {
            foreach (Behaviour behaviour in behaviours)
            {
                m_children.Add(behaviour);
            }
        }
    }

}