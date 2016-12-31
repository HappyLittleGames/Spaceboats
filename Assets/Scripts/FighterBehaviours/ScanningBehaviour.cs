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

            AddBehaviour<Behaviour>().BUpdate = FindFighters; // is somehow worse?
            //AddBehaviour<Behaviour>().BUpdate = FindFriendlies;
            //AddBehaviour<Behaviour>().BUpdate = FindEnemies;
            
            Selector engageOrRetreat = AddBehaviour<Selector>();
            {
                Sequence engage = engageOrRetreat.AddBehaviour<Sequence>();
                {
                    engage.AddBehaviour<Condition>().BCanRun = HasEnemy;
                    engage.AddBehaviour<Behaviour>().BUpdate = TargetClosestEnemy;
                }

                Selector retreat = engageOrRetreat.AddBehaviour<Selector>();
                {
                    //Sequence formUp = retreat.AddBehaviour<Sequence>();
                    //{
                    //    formUp.AddBehaviour<Condition>().BCanRun = HasFriendly;
                    //    formUp.AddBehaviour<Behaviour>().BUpdate = TargetClosestFriendly;
                    //}

                    Sequence goHome = retreat.AddBehaviour<Sequence>();
                    {
                        goHome.AddBehaviour<Condition>().BCanRun = HasMothership;
                        goHome.AddBehaviour<Behaviour>().BUpdate = TargetMothership;
                    }
                }
            }
        }


        private BHStatus AlwaysFails()
        {
            Debug.Log("End of ScanTree");
            return BHStatus.Failure;
        }



        private bool HasEnemy()
        {
            if (m_blackboard.enemies.Count > 0)
                return true;
            else
                return false;
        }


        private bool HasFriendly()
        {
            if (m_blackboard.friendlies.Count > 0)
                return true;
            else
                return false;
        }


        //private BHStatus FindFriendlies()
        //{
        //    m_blackboard.friendlies.Clear();
        //    if (m_blackboard.parentObject.tag == "Team1")
        //        m_blackboard.friendlies = GameObject.FindGameObjectsWithTag("Team1").ToList<GameObject>();
        //    else if (m_blackboard.parentObject.tag == "Team2")
        //        m_blackboard.friendlies = GameObject.FindGameObjectsWithTag("Team2").ToList<GameObject>();
        //    if (m_blackboard.friendlies.Count > 0)
        //    {
        //        return BHStatus.Success;
        //    }
        //    else
        //    {
        //        return BHStatus.Failure;
        //    }
        //}

        //private BHStatus FindEnemies()
        //{
        //    m_blackboard.enemies.Clear();
        //    if (m_blackboard.parentObject.tag == "Team1")
        //        m_blackboard.enemies = GameObject.FindGameObjectsWithTag("Team2").ToList<GameObject>();
        //    else if (m_blackboard.parentObject.tag == "Team2")
        //        m_blackboard.enemies = GameObject.FindGameObjectsWithTag("Team1").ToList<GameObject>();
        //    if (m_blackboard.enemies.Count > 0)
        //    {
        //        return BHStatus.Success;
        //    }
        //    else
        //    {
        //        return BHStatus.Failure;
        //    }
        //}


        private BHStatus FindFighters()
        {
            m_blackboard.friendlies.Clear();
            m_blackboard.enemies.Clear();

            foreach (GameObject fighter in GameObject.FindGameObjectsWithTag("Fighter"))
            {
                if ((fighter.GetComponent<Fighter>().teamNumber == m_blackboard.fighter.teamNumber) && (fighter.gameObject != m_blackboard.parentObject.gameObject))
                    m_blackboard.friendlies.Add(fighter);
                if (fighter.GetComponent<Fighter>().teamNumber != m_blackboard.fighter.teamNumber)
                    m_blackboard.enemies.Add(fighter);
            }
            if ((m_blackboard.enemies.Count + m_blackboard.friendlies.Count) == 0)
            {
                Debug.Log("NO FRIENDS OR ENEMIES  :(");
                return BHStatus.Running;
            }
            return BHStatus.Success;
        }


        private BHStatus TargetClosestEnemy()
        {
            if (m_blackboard.enemies.Count > 0)
            {
                List<GameObject> sortedByRange = m_blackboard.enemies.OrderBy(x => Vector3.Distance(m_blackboard.parentObject.transform.position, x.transform.position)).ToList();
                m_blackboard.target = sortedByRange[0];
                return BHStatus.Success;
            }
            else
            {
                m_blackboard.target = null;
                return BHStatus.Failure;
            }            
        }


        private BHStatus TargetClosestFriendly()
        {
            List<GameObject> sortedByRange = m_blackboard.friendlies.OrderBy(x => Vector3.Distance(m_blackboard.parentObject.transform.position, x.transform.position)).ToList();
            m_blackboard.wingMan = sortedByRange[0];
            return BHStatus.Success;
        }


        private BHStatus TargetMothership()
        {
            m_blackboard.target = m_blackboard.mothership;
            return BHStatus.Success;
        }


        private bool HasMothership()
        {
            if (m_blackboard.mothership != null)
            {
                return true;
            }
            else
            {
                return false;
            }         
        }
    }
}
