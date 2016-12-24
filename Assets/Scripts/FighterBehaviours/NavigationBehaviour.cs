using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.BHTree
{
    public class NavigationBehaviour : Sequence
    {
        private FighterBlackboard m_blackboard = null;
        private Propulsion m_propulsion = null;

        public NavigationBehaviour(FighterBlackboard blackboard, Propulsion prop)
        {
            m_blackboard = blackboard;
            m_propulsion = prop;

            AddBehaviour<Condition>().BCanRun = HasDestination;
            AddBehaviour<Behaviour>().BUpdate = PrintAngles;
        }


        private bool HasDestination()
        {
            if (GameObject.FindGameObjectWithTag("TargetPracticeTarget"))
            {
                return true;
            }
            else
            {
                Debug.Log("No destination");
                return false;
            }
        }


        private BHStatus PrintAngles()
        {
            GameObject testTarget = null;
            if (testTarget = GameObject.FindGameObjectWithTag("TargetPracticeTarget"))
            {

                Vector3 targetDirection = testTarget.transform.position - m_blackboard.ParentObject.transform.position;
                float pitchAngle = Mathf.Asin(Vector3.Cross(targetDirection.normalized, m_blackboard.ParentObject.transform.forward).x) * Mathf.Rad2Deg;
                float yawAngle = Mathf.Asin(Vector3.Cross(targetDirection.normalized, m_blackboard.ParentObject.transform.forward).y) * Mathf.Rad2Deg;
                float rollAngle = Mathf.Atan(Vector3.Cross(targetDirection.normalized, m_blackboard.ParentObject.transform.forward).z) * Mathf.Rad2Deg;

                // add 90 to pitch and roll angles if destination is behind transform.
                if (Vector3.Distance(testTarget.transform.position, m_blackboard.ParentObject.transform.position) <
                    Vector3.Distance(testTarget.transform.position, m_blackboard.ParentObject.transform.position + m_blackboard.ParentObject.transform.forward * .1f))
                {
                    Debug.Log("testTarget is behind transform");
                    pitchAngle = (180 - pitchAngle*1);
                    yawAngle = (180 - yawAngle*1);

                    // some elastic behaviour at 90-95 degrees
                }

                Debug.Log("Angles to target: - x: " + pitchAngle + " - y: " + yawAngle + " - z: " + rollAngle);


                // m_propulsion.Turn("pitch", 1 * pitchAngle);
                
                // m_propulsion.Turn("yaw", (1 * yawAngle);

                //if (rollAngle > 0)
                //{
                //    m_propulsion.Turn("roll", -0.1f);
                //}
                //else if (rollAngle < 0)
                //{
                //    m_propulsion.Turn("roll", 0.1f);
                //}

                return BHStatus.Running;
            }
            else
            {
                return BHStatus.Failure;
            }
        }
    }    
}

