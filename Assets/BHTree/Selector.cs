using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.BHTree
{
    public class Selector : Composite
    {
        private int m_selection;
        public Selector()
        {
            Update = () =>
            {
                for (;;)
                {
                    BHStatus status = GetChild(m_selection).Tick();
                    if (status != BHStatus.Failure)
                    {
                        if (status == BHStatus.Success)
                        {
                            m_selection = 0;
                        }
                    }
                    if (++m_selection == ChildCount())
                    {
                        m_selection = 0;
                        return BHStatus.Failure;
                    }
                }
            };
            Initialize = () =>
            {
                m_selection = 0;
            };
        }
    }
}
