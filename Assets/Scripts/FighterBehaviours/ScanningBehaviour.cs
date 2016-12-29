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

            AddBehaviour<Behaviour>().BUpdate = FindFriendlies;
            AddBehaviour<Behaviour>().BUpdate = FindEnemies;
            AddBehaviour<Condition>().BCanRun = HasEnemy;
            AddBehaviour<Behaviour>().BUpdate = TargetClosestEnemy;
            AddBehaviour<Behaviour>().BUpdate = KnowsWhereToGo;
        }


        // make unified method for finding both friendlies and enemies, and sort more than 2 teams
        private BHStatus FindFriendlies()
        {
            m_blackboard.friendlies.Clear();
            if (m_blackboard.parentObject.tag == "Team1")
                m_blackboard.friendlies = GameObject.FindGameObjectsWithTag("Team1").ToList<GameObject>();
            else if (m_blackboard.parentObject.tag == "Team2")
                m_blackboard.friendlies = GameObject.FindGameObjectsWithTag("Team2").ToList<GameObject>();
            if (m_blackboard.friendlies.Count > 0)
            {
                return BHStatus.Success;
            }
            else
            {
                return BHStatus.Failure;
            }
        }

        private BHStatus FindEnemies()
        {
            m_blackboard.enemies.Clear();
            if (m_blackboard.parentObject.tag == "Team1")
                m_blackboard.enemies = GameObject.FindGameObjectsWithTag("Team2").ToList<GameObject>();
            else if (m_blackboard.parentObject.tag == "Team2")
                m_blackboard.enemies = GameObject.FindGameObjectsWithTag("Team1").ToList<GameObject>();
            if (m_blackboard.enemies.Count > 0)
            {
                return BHStatus.Success;
            }
            else
            {
                return BHStatus.Failure;
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

        private BHStatus KnowsWhereToGo()
        {
            if (m_blackboard.target != null)
            {
                return BHStatus.Success;
            }
            else
            {
                // if it doesnT know, set course home?
                m_blackboard.target = m_blackboard.mothership;
                if (m_blackboard.target == null)
                {
                    return BHStatus.Failure;
                }
                return BHStatus.Success;
            }            
        }
    }
}
