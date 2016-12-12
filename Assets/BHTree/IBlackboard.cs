using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.BHTree
{
    abstract public class IBlackboard
    {
        protected float m_tickInterval = 0.1f;  // decorator for these
        protected float m_tickTimer = 0.0f;
        abstract public void BlackboardTick(float deltaTime);
    }
}
