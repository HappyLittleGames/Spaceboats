using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.BHTree
{
    public class NavigationTree : Sequence
    {
        private FighterBlackboard m_blackboard = null;
        private Propulsion m_propulsion = null;
        private Navigator m_navigator = null;
        private Vector3 m_destination = Vector3.zero;
        private GameObject m_target = null;
        private float m_orbitDistance = 40;
        public NavigationTree(FighterBlackboard blackboard, Navigator navigator, Propulsion prop)
        {
            m_blackboard = blackboard;
            m_propulsion = prop;
            m_navigator = navigator;                       

            Selector navigationSelector = this.AddBehaviour<Selector>();

            Sequence engage = navigationSelector.AddBehaviour<Sequence>();
            engage.AddBehaviour<Condition>().BCanRun = HasTarget;
            //engage.AddBehaviour<Behaviour>().BUpdate = Stabilize;
            //engage.AddBehaviour<Behaviour>().BUpdate = SetDestination;
            //engage.AddBehaviour<Behaviour>().BUpdate = SetThrottle;

            Sequence idle = navigationSelector.AddBehaviour<Sequence>();
            idle.AddBehaviour<Condition>().BCanRun = HasMothership;
            idle.AddBehaviour<Behaviour>().BUpdate = FindOrbit;
            //idle.AddBehaviour<Behaviour>().BUpdate = Stabilize;
            //idle.AddBehaviour<Behaviour>().BUpdate = SetDestination;
            //idle.AddBehaviour<Behaviour>().BUpdate = SetThrottle;

            AddBehaviour<Behaviour>().BUpdate = Stabilize;
            AddBehaviour<Behaviour>().BUpdate = SetDestination;
            AddBehaviour<Behaviour>().BUpdate = SetThrottle;
            AddBehaviour<Behaviour>().BUpdate = AlwaysFails;
        }


        private bool HasMothership()
        {
            m_target = null;
            if (m_blackboard.mothership != null)
            {
                m_target = m_blackboard.mothership;
            }
            if (m_target.transform != null)
            {
                m_destination = (m_target.transform.position + m_blackboard.parentObject.transform.right.normalized * m_orbitDistance) - m_propulsion.rigidbody.velocity;
                return true;
            }
            else
                return false;
        }


        private BHStatus FindOrbit()
        {
            // not correct
            
            m_destination = m_target.transform.position - m_propulsion.rigidbody.velocity;
            return BHStatus.Success;
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


        private bool HasTarget()
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


        private BHStatus SetDestination()
        {               
            m_navigator.destination = m_destination;
            return BHStatus.Success;
        }


        private BHStatus SetThrottle()
        {
            if (Vector3.Angle(m_blackboard.parentObject.transform.forward, m_navigator.destination) < 15)
            {
                // needs some clever behaviour for throttling
                m_navigator.thrustThrottle = 1;                
            }
            return BHStatus.Success;
        }


        private BHStatus AlwaysFails()
        {
            // Debug.Log("End of NavTree");
            return BHStatus.Failure;
        }
    }
}
