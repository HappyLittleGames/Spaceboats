using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.BHTree
{
    abstract public class Blackboard
    {
        public GameObject parentObject { get; set; }
        protected float m_tickInterval = 0.1f;  // put these in decorator for tick-intervals instead
        protected float m_tickTimer = 0.0f;
        abstract public void BlackboardUpdate();
    }
}
