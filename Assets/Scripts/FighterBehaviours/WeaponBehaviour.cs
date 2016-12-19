using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.BHTree
{
    public class WeaponBehaviour : Sequence
    {
        private FighterBlackboard m_blackboard = null;
        private Weapon m_weapon = null;

        public WeaponBehaviour(FighterBlackboard blackboard, Weapon weapon)
        {
            m_blackboard = blackboard;
            m_weapon = weapon;

            AddBehaviour<Behaviour>().BUpdate = Reloading;
            AddBehaviour<Behaviour>().BUpdate = TakingAim;
            AddBehaviour<Behaviour>().BUpdate = OpeningFire;      
        }


        private BHStatus TakingAim()
        {
            // GameObject practiceTarget = GameObject.FindGameObjectWithTag("TargetPracticeTarget");
            if (m_blackboard.target)
            {                
                if ((m_blackboard.target.transform.position - m_blackboard.ParentObject.transform.position).magnitude <= m_weapon.range)
                {
                    Debug.Log("Taking Aim, AnyKey to fire");
                    if (Vector3.Angle(m_blackboard.target.transform.position - m_blackboard.ParentObject.transform.position, m_blackboard.ParentObject.transform.forward) < m_weapon.accuracy)
                    {
                            Debug.Log("Target locked, Weapons Free");
                            return BHStatus.Success;
                    }
                    else if (Input.anyKeyDown)
                    {
                        return BHStatus.Success;
                    }
                    return BHStatus.Running;
                }
                else
                {
                    Debug.Log("Target not in Range");
                    return BHStatus.Running;
                }                
            }
            else
                return BHStatus.Failure;
        }


        private BHStatus OpeningFire()
        {
            // make this call some destroy or take damage in collider (maybe SendMessage?)
            Debug.Log("Opening Fire");
            GameObject hit = m_weapon.DebugPew(m_blackboard.ParentObject.transform.position, m_blackboard.ParentObject.transform.forward, 0.1f);
            if (hit != null)
            {
                hit.BroadcastMessage("KillMe");

            }
            else if ((hit != null) && (hit.tag == "TargetPracticeTarget"))
            {
                GameObject.Destroy(hit);
            }
            return BHStatus.Success;
        }


        private BHStatus Reloading()
        {
            Debug.Log("Attempting to recharge");
            return BHStatus.Success;
        }
    }
}
