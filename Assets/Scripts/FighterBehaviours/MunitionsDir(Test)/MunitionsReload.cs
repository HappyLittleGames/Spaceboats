using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.BHTree
{
    class MunitionsReload : Behaviour
    {
        public MunitionsReload(Weapon weapon)
        {
            BUpdate = () =>
            {
                Debug.Log("Attempting to recharge");
                return BHStatus.Success;
            };
        }
        // should not be a behaviour, handled by respective weapons
        // Or if possible make CanRun based on timers in respective weapon.
    }
}
