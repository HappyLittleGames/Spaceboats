using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.BHTree
{
    class FighterBlackboard : MonoBehaviour
    {
        private Munitions m_munitions = new Munitions();
        private float m_tickInterval = 0.1f;
        private float m_tickTimer = 0.0f;

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
        private void Update() // how to use?????
        {

            m_tickInterval += Time.deltaTime;
            if (m_tickTimer > m_tickInterval)
            {
                m_munitions.Tick();
                m_tickTimer = 0.0f;
            }
        }


    }
}
