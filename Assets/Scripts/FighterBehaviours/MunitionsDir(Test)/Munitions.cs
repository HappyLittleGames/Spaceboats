using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.BHTree
{
    class Munitions : Sequence
    {
        private Blackboard m_blackboard;

        public Munitions(Blackboard blackboard, Weapon weapon)
        {
            m_blackboard = blackboard;
            SetSequence(new List<IBehaviour>
            {
                new MunitionsAim(weapon),
                new MunitionsFire(weapon),
                new MunitionsReload(weapon)
            });

        }
    }
}
