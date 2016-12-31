using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class SpawnShips : MonoBehaviour
    {
        [SerializeField] private GameObject m_shipType = null;
        [SerializeField] private int m_teamNumber = 0;
        [SerializeField] private float m_thrust = 10;
        [SerializeField] private float m_turnRate = 4;
        




        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                SpawnShip();
            }
            if (Input.GetKey(KeyCode.F2))
            {
                SpawnShip();
            }
        }

        public void SpawnShip()
        {
            GameObject ship = (GameObject)Instantiate(m_shipType, transform.position, Quaternion.identity);
            Fighter fighter = ship.AddComponent<Fighter>();
            fighter.SetThrust(m_thrust);
            fighter.SetTurnRate(m_turnRate);
            fighter.mothership = gameObject;
            fighter.teamNumber = m_teamNumber;
        }

        public void AlterTurnRate(float change)
        {
            m_turnRate += change;
        }

        public void AlterThrust(float change)
        {
            m_thrust += change;
        }
    }
}
