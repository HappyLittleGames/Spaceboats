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
        public EngagementBehaviour engagementBehaviour { get; private set; }
        //public IdleBehaviour idleBehaviour { get; private set; }
        public NavigationTree navigationTree { get; private set; }
        public Selector navigationSelector { get; set; }
        public Navigator navigator { get; set; }
        public GameObject target { get; set; }
        public GameObject mothership { get; set; }
        public List<GameObject> enemies { get; set; }

        public FighterBlackboard(Fighter fighter, GameObject parentObject)
        {
            this.fighter = fighter;
            this.parentObject = parentObject;
            mothership = fighter.mothership;
            m_tickTimer = 0.0f;
            m_tickInterval = .1f;
            m_tickInterval = UnityEngine.Random.Range(-m_tickInterval * .1f, m_tickInterval * .1f) + m_tickInterval;
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
            //navigationSelector = new Selector();
            navigator = new Navigator(prop);
            engagementBehaviour = new EngagementBehaviour(this, navigator, prop);
            //idleBehaviour = new IdleBehaviour(this, navigator, prop);
            navigationTree = new NavigationTree(this, navigator, prop);

        }        

        //fakeUpdate
        public override void BlackboardUpdate(float deltaTime)
        {
            m_tickTimer += deltaTime;
            if (m_tickTimer > m_tickInterval)
            {
                scanningBehaviour.BTick();
                weaponBehaviour.BTick();
                // engagementBehaviour.BTick();
                navigationTree.BTick();
                m_tickTimer = 0.0f;
            }            
        }
    }
}
