using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.BHTree
{
    class MunitionsAim : Behaviour
    {
        public MunitionsAim(Weapon weapon)
        {
            BUpdate = () =>
            {
                GameObject practiceTarget = GameObject.FindGameObjectWithTag("TargetPracticeTarget");
                if (practiceTarget)
                {
                    Debug.Log("Taking Aim");
                    //GameObject.Destroy(practiceTarget);
                    if (Input.anyKeyDown)
                    {
                        Debug.Log("Opening Fire");
                        return BHStatus.Success;
                    }
                    return BHStatus.Running; 
                }
                else
                    return BHStatus.Failure;
            };
        }
        /*
            should be "taking aim".
            also clearing the shot to prevent friendly fire
        */

    }
}
