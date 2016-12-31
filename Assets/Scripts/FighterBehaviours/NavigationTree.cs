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
        private Vector3 m_desiredPosition = Vector3.zero;       
        //private float m_orbitDistance = 10;
        private float m_topSpeed = 40;
        public NavigationTree(FighterBlackboard blackboard, Navigator navigator, Propulsion prop)
        {
            m_blackboard = blackboard;
            m_propulsion = prop;
            m_topSpeed = prop.thrust * 5; // this is just bad

            AddBehaviour<Behaviour>().BUpdate = SetTarget;
            Selector shootOrTraverse = AddBehaviour<Selector>();
            {
                // completely redundant
                //Sequence shoot = shootOrTraverse.AddBehaviour<Sequence>();
                //{
                //    shoot.AddBehaviour<Condition>().BCanRun = TargetEnteringRange;
                //    shoot.AddBehaviour<Behaviour>().BUpdate = TurnToTarget;
                //}

                Sequence traverse = shootOrTraverse.AddBehaviour<Sequence>();
                {                    
                    traverse.AddBehaviour<Behaviour>().BUpdate = Stabilize;
                    traverse.AddBehaviour<Behaviour>().BUpdate = SetDestination;
                }
            }

            AddBehaviour<Behaviour>().BUpdate = SetThrottle;

            //Sequence stayInFormation = AddBehaviour<Sequence>();
            //{
            //    stayInFormation.AddBehaviour<Condition>().BCanRun = HasWingman;
            //    stayInFormation.AddBehaviour<Behaviour>().BUpdate = VectoringThrust;
            //}            
        }


        private BHStatus SetTarget()
        {
            if (m_blackboard.target != null)
            {
                if (m_blackboard.target.GetComponent<Rigidbody>() != null)
                {
                    Vector3 behindTarget = -(m_blackboard.target.GetComponent<Rigidbody>().velocity.normalized * (m_blackboard.fighter.weapon.range / 2)); // Good spot for _boldness
                    m_desiredPosition = m_blackboard.target.transform.position - m_blackboard.parentObject.transform.position - m_propulsion.rigidbody.velocity + behindTarget;
                }
                else
                {
                    m_desiredPosition = m_blackboard.target.transform.position - m_blackboard.parentObject.transform.position - m_propulsion.rigidbody.velocity;
                }
                return BHStatus.Success;
            }
            else
                return BHStatus.Failure;
        }


        private bool TargetEnteringRange()
        {
            if (Vector3.Distance(m_blackboard.parentObject.transform.position, m_blackboard.target.transform.position) < m_blackboard.fighter.weapon.range)
            {
                return true;
            }
            return false;
        }


        private BHStatus TurnToTarget()
        {
            m_blackboard.navigator.destination = m_blackboard.target.transform.position;
            return BHStatus.Success;
        }


        private BHStatus Stabilize()
        {
            // Debug.Log("magnitude of velocity = " + m_propulsion.rigidbody.velocity.magnitude + ", velocity taking us further from destination.");
            if (Vector3.Distance(m_blackboard.parentObject.transform.position + m_propulsion.rigidbody.velocity.normalized, m_desiredPosition) >
                Vector3.Distance(m_blackboard.parentObject.transform.position, m_desiredPosition))
            {
                // Debug.Log("Setting destination to opposite velocity");
                m_desiredPosition = (-m_propulsion.rigidbody.velocity.normalized * m_topSpeed * 1000) - m_blackboard.parentObject.transform.position;
                return BHStatus.Success;
            }
            // but if speed is too damn high, the tickrate on scans is too low for updating the destinations like this
            if (m_propulsion.rigidbody.velocity.magnitude > m_topSpeed)
            {
                // Debug.Log("Setting destination to opposite velocity");
                m_desiredPosition = (-m_propulsion.rigidbody.velocity.normalized * m_topSpeed * 1000) - m_blackboard.parentObject.transform.position;
                return BHStatus.Success;
            }

            // else if stable
            return BHStatus.Success;
        }        


        private BHStatus SetDestination()
        {               
            m_blackboard.navigator.destination = m_desiredPosition;
            return BHStatus.Success;
        }


        private BHStatus SetThrottle()
        {
            if (Vector3.Angle(m_blackboard.parentObject.transform.forward, m_blackboard.navigator.destination) < 15)
            {
                // maybe some more clever throttling?
                m_blackboard.navigator.thrustThrottle = 1;                
            }
            return BHStatus.Success;
        }


        private bool HasWingman()
        {
            if (m_blackboard.wingMan != null)
            {
                return true;
            }
            return false;
        }


        private BHStatus VectoringThrust()
        {
            float distance = Vector3.Distance(m_blackboard.parentObject.transform.position, m_blackboard.wingMan.transform.position);
            float m_squadTightness = 10;            
            Vector3 direction = m_blackboard.wingMan.transform.position - m_blackboard.parentObject.transform.position;  // maintain distance
            float vectoringAmount = distance - m_squadTightness;
            m_propulsion.VectoringThrust(direction, vectoringAmount, Time.fixedDeltaTime);

            Debug.DrawRay(m_blackboard.parentObject.transform.position, direction, Color.red);
                
            
            return BHStatus.Success;
        }


        private BHStatus AlwaysFails()
        {
            // Debug.Log("End of NavTree");
            return BHStatus.Failure;
        }
    }
}
