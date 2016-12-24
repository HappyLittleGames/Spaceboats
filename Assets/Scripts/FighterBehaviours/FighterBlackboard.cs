using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.BHTree
{
    public class FighterBlackboard : Blackboard
    {
        public Go goScript { get; set; } // not like this
        public WeaponBehaviour weaponBehaviour { get; private set; }
        public NavigationBehaviour navigationBehaviour { get; private set; }
        public GameObject target { get; set; }

        public FighterBlackboard(Go go, GameObject parentObject)
        {
            this.goScript = go;
            ParentObject = parentObject;
            m_tickTimer = 0.0f;
            m_tickInterval = .1f;
        }

        public void AddWeapon(Weapon weapon)
        {
            weaponBehaviour = new WeaponBehaviour(this, weapon);
        }

        public void AddNavigation(Propulsion prop)
        {
            navigationBehaviour = new NavigationBehaviour(this, prop);
        }

        //fakeUpdate
        public override void BlackboardUpdate(float deltaTime)
        {

            m_tickTimer += deltaTime;
            if (m_tickTimer > m_tickInterval)
            {
                weaponBehaviour.BTick();
                navigationBehaviour.BTick();
                m_tickTimer = 0.0f;
            }

        }


    }
}
