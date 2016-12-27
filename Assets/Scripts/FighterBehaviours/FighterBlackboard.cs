using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.BHTree
{
    public class FighterBlackboard : Blackboard
    {
        public Fighter fighter { get; set; } // not like this
        public ScanningBehaviour scanningBehaviour { get; private set; }
        public WeaponBehaviour weaponBehaviour { get; private set; }
        public NavigationBehaviour navigationBehaviour { get; private set; }
        public Selector navigationSelector { get; set; }
        public Navigator navigator { get; set; }
        public GameObject target { get; set; }
        public GameObject mothership { get; set; }
        public List<GameObject> enemies { get; set; }

        public FighterBlackboard(Fighter fighter, GameObject parentObject)
        {
            this.fighter = fighter;
            base.parentObject = parentObject;
            m_tickTimer = 0.0f;
            m_tickInterval = .1f;
            enemies = new List<GameObject>();
        }

        public void AddScanner()
        {
            scanningBehaviour = new ScanningBehaviour(this);
        }

        public void AddWeapon(Weapon weapon)
        {
            weaponBehaviour = new WeaponBehaviour(this, weapon);
        }

        public void AddNavigation(Propulsion prop)
        {
            navigator = new Navigator(prop);
            navigationBehaviour = new NavigationBehaviour(this, navigator, prop);
            
        }        

        //fakeUpdate
        public override void BlackboardUpdate(float deltaTime)
        {
            m_tickTimer += deltaTime;
            if (m_tickTimer > m_tickInterval)
            {
                scanningBehaviour.BTick();
                weaponBehaviour.BTick();
                navigationBehaviour.BTick();
                m_tickTimer = 0.0f;
            }            
        }
    }
}
