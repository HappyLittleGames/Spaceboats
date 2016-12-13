using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.BHTree
{
    class Navigation : Sequence
    {
        private List<Weapon> m_weaponry = new List<Weapon>();

        public Navigation()
        {
            Initialize = () =>
            {
                SetSequence(new List<IBehaviour>
                {
                });
            };
        }

    }
}
