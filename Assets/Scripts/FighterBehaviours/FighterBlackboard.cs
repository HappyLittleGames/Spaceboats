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

        public Behaviour scanningBehaviour { get; private set; }
        public Behaviour weaponBehaviour { get; private set; }
        public Behaviour navigationTree { get; private set; }

        public Navigator navigator { get; set; }
        public GameObject target { get; set; }
        public GameObject wingMan { get; set; }
        public GameObject mothership { get; set; }
        public List<GameObject> enemies { get; set; }
        public List<GameObject> friendlies { get; set; }

        public FighterBlackboard(Fighter fighter, GameObject parentObject)
        {
            this.fighter = fighter;
            this.parentObject = parentObject;
            mothership = fighter.mothership;
            m_tickInterval = .1f; // some variance in update speed because some dudes are faster than others
            m_tickInterval = UnityEngine.Random.Range(-m_tickInterval * .1f, m_tickInterval * .1f) + m_tickInterval;
            enemies = new List<GameObject>();
            friendlies = new List<GameObject>();

        }

        public void AddScanner()
        {
            scanningBehaviour = new ScanningBehaviour(this);
            scanningBehaviour = new Regulator(scanningBehaviour, m_tickInterval);
        }

        public void AddWeapon(Weapon weapon)
        {
            weaponBehaviour = new WeaponBehaviour(this, weapon);
            weaponBehaviour = new Regulator(weaponBehaviour, m_tickInterval);
        }

        public void AddNavigation(Propulsion prop)
        {
            navigator = new Navigator(prop);
            navigationTree = new NavigationTree(this, navigator, prop);
            navigationTree = new Regulator(navigationTree, m_tickInterval);
        }        

        public override void BlackboardUpdate()
        {
            scanningBehaviour.BTick();
            weaponBehaviour.BTick();
            navigationTree.BTick();
        }
    }
}
