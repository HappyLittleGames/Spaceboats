using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.BHTree
{
    public class EngagementBehaviour : Sequence
    {
        private FighterBlackboard m_blackboard = null;
        private Propulsion m_propulsion = null;
        private Navigator m_navigator = null;
        private Vector3 m_destination = Vector3.zero;
        private GameObject m_target = null;
        public EngagementBehaviour(FighterBlackboard blackboard, Navigator navigator, Propulsion prop)
        {
            m_blackboard = blackboard;
            m_propulsion = prop;
            m_navigator = navigator;

            AddBehaviour<Condition>().BCanRun = FindTarget;
            AddBehaviour<Behaviour>().BUpdate = Stabilize;            
            AddBehaviour<Behaviour>().BUpdate = TurnToDestination;
            AddBehaviour<Behaviour>().BUpdate = SetThrottle;
        }


        private bool FindTarget()
        {
            m_target = null;
            if (m_blackboard.target != null)
            {
                m_target = m_blackboard.target;
            }
            if (m_target != null)
            {
                m_destination = m_target.transform.position - m_blackboard.parentObject.transform.position - m_propulsion.rigidbody.velocity;
                return true;
            }
            else
                return false;
        }


        private BHStatus Stabilize()
        {
            // Debug.Log("magnitude of velocity = " + m_propulsion.rigidbody.velocity.magnitude);
            if (Vector3.Distance(m_blackboard.parentObject.transform.position + m_propulsion.rigidbody.velocity.normalized, m_destination) >
                Vector3.Distance(m_blackboard.parentObject.transform.position, m_destination))
            {
                // Debug.Log("Reducing Speed");
                m_destination = (-m_propulsion.rigidbody.velocity.normalized * 30) - m_blackboard.parentObject.transform.position;
            }
            return BHStatus.Success;
        }


        private BHStatus TurnToDestination()
        {
            if (m_target != null)
            {
                // Vector3 targetVelocity = (testTarget.GetComponent<Rigidbody>().velocity);
                
                m_navigator.destination = m_destination;
                return BHStatus.Success;
            }
            else 
            {
                return BHStatus.Failure;
            }
        }


        private BHStatus SetThrottle()
        {
            if (Vector3.Angle(m_blackboard.parentObject.transform.forward, m_navigator.destination) < 15)
            {
                m_navigator.thrustThrottle = 1;
                return BHStatus.Success;
            }
            else
            {
                return BHStatus.Failure;
            }
        }
    }    
}

