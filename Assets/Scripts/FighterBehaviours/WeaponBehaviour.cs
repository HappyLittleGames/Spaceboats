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
            if (m_blackboard.target)
            {                
                if ((m_blackboard.target.transform.position - m_blackboard.parentObject.transform.position).magnitude <= m_weapon.range)
                {
                    // Debug.Log("Taking Aim (AnyKey to fire)");
                    if (Vector3.Angle(m_blackboard.target.transform.position - m_blackboard.parentObject.transform.position, m_blackboard.parentObject.transform.forward) < m_weapon.accuracy)
                    {
                            // Debug.Log("Target locked, Weapons Free");
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
                    // Debug.Log("Target not in Range");
                    return BHStatus.Running;
                }                
            }
            else
                return BHStatus.Failure;
        }


        private BHStatus OpeningFire()
        {
            // Debug.Log("Opening Fire");
            GameObject hit = m_weapon.DebugPew(m_blackboard.parentObject.transform.position, m_blackboard.parentObject.transform.forward, 0.1f);
            if (hit != null)
            {
                GameObject.Destroy(hit);
            }
            return BHStatus.Success;
        }


        private BHStatus Reloading()
        {
            // Debug.Log("Attempting to recharge");
            return BHStatus.Success;
        }
    }
}
