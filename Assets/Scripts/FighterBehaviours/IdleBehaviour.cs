using UnityEngine;
using System.Collections;

namespace Assets.BHTree
{
    public class IdleBehaviour : Sequence
    {
        private FighterBlackboard m_blackboard = null;
        private Propulsion m_propulsion = null;
        private Navigator m_navigator = null;
        private Vector3 m_destination = Vector3.zero;
        private float m_orbitDistance = 40;
        public IdleBehaviour(FighterBlackboard blackboard, Navigator navigator, Propulsion prop)
        {
            m_blackboard = blackboard;
            m_propulsion = prop;
            m_navigator = navigator;
            
            AddBehaviour<Condition>().BCanRun = HasMothership;
            AddBehaviour<Behaviour>().BUpdate = FindOrbit;
            AddBehaviour<Behaviour>().BUpdate = Stabilize;
            AddBehaviour<Behaviour>().BUpdate = TurnToDestination;
            AddBehaviour<Behaviour>().BUpdate = SetThrottle;
        }


        private bool HasMothership()
        {            
            if (m_blackboard.mothership != null)
            {               
                return true;
            }
            else
                return false;
        }


        private BHStatus FindOrbit()
        {
            m_destination = m_blackboard.mothership.transform.position + (m_blackboard.parentObject.transform.right.normalized * m_orbitDistance);
            return BHStatus.Failure;
        }


        private BHStatus Stabilize()
        {
            // Debug.Log("magnitude of velocity = " + m_propulsion.rigidbody.velocity.magnitude);
            if (Vector3.Distance(m_blackboard.parentObject.transform.position + m_propulsion.rigidbody.velocity.normalized, m_destination) >
                Vector3.Distance(m_blackboard.parentObject.transform.position, m_destination))
            {
                // Debug.Log("Reducing Speed");
                // this arbitrary "30" needs implementation, is currently thrust*.75 or whatever but meh.
                m_destination = (-m_propulsion.rigidbody.velocity.normalized * 30) - m_blackboard.parentObject.transform.position;
            }
            return BHStatus.Success;
        }


        private BHStatus TurnToDestination()
        {
            if (m_blackboard.mothership != null)
            {
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
                // needs some clever behaviour for throttling
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