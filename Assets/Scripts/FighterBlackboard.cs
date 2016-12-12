using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.BHTree
{
    public class FighterBlackboard : IBlackboard
    {
        private Munitions m_munitions = new Munitions();
        private void Initialize()
        {
            // sorry about this style :D::D
            m_munitions.SetSequence(new List<IBehaviour>
            {
                new MunitionsAim(),
                new MunitionsFire(),
                new MunitionsReload()
            });
        }

        //fakeUpdate
        public override void BlackboardTick(float deltaTime) // how to use?????
        {

            m_tickInterval += deltaTime;
            if (m_tickTimer > m_tickInterval)
            {
                m_munitions.Tick();
                m_tickTimer = 0.0f;
            }
        }


    }
}
