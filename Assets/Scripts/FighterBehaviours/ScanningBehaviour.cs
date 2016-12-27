using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.BHTree
{
    public class ScanningBehaviour : Sequence
    {
        private FighterBlackboard m_blackboard = null;
        
        public ScanningBehaviour(FighterBlackboard blackboard)
        {
            m_blackboard = blackboard;

            AddBehaviour<Behaviour>().BUpdate = SweepForEnemies;
            AddBehaviour<Condition>().BCanRun = HasEnemy;
            AddBehaviour<Behaviour>().BUpdate = TargetClosestEnemy;
        }

        private BHStatus SweepForEnemies()
        {
            m_blackboard.enemies.Clear();
            if (m_blackboard.parentObject.tag == "Team1")
                m_blackboard.enemies = GameObject.FindGameObjectsWithTag("Team2").ToList<GameObject>();
            else if (m_blackboard.parentObject.tag == "Team2")
                m_blackboard.enemies = m_blackboard.enemies = GameObject.FindGameObjectsWithTag("Team1").ToList<GameObject>();
            if (m_blackboard.enemies.Count > 0)
            {
                return BHStatus.Success;
            }
            else
            {
                return BHStatus.Running;
            }
        }


        private bool HasEnemy()
        {
            if (m_blackboard.enemies.Count > 0)
                return true;
            else
                return false;
        }


        private BHStatus TargetClosestEnemy()
        {
            List<GameObject> sortedByRange = m_blackboard.enemies.OrderBy(x => Vector3.Distance(m_blackboard.parentObject.transform.position, x.transform.position)).ToList();
            m_blackboard.target = sortedByRange[0];
            return BHStatus.Success;
        }
    }
}
