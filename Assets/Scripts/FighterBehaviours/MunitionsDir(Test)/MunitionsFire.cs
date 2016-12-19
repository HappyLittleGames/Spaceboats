using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.BHTree
{
    class MunitionsFire : Behaviour
    {
        public MunitionsFire(Weapon weapon)
        {
            BUpdate = () =>
            {
                GameObject practiceTarget = GameObject.FindGameObjectWithTag("TargetPracticeTarget");
                if (practiceTarget)
                {
                    Debug.Log("Weapon Discharged");
                    GameObject.Destroy(practiceTarget);
                    return BHStatus.Success;
                }
                else
                    return BHStatus.Failure;
            };
        }
        /*
            should be more like "opening fire" though??

        */
    }
}
