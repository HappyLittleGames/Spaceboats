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
        public GameObject target { get; set; }

        public FighterBlackboard(Go go, GameObject parentObject)
        {
            this.goScript = go;
            ParentObject = parentObject;
        }

        public void AddWeapon(Weapon weapon)
        {
            weaponBehaviour = new WeaponBehaviour(this, weapon);
        }

        //fakeUpdate
        public override void BlackboardUpdate(float deltaTime)
        {

            m_tickTimer += deltaTime;
            //if (m_tickTimer > m_tickInterval)
            //{
                
                weaponBehaviour.BTick();
                m_tickTimer = 0.0f;  
            //}

            // m_munitions.Update();
        }


    }
}
