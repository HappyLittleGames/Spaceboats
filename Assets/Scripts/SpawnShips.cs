using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class SpawnShips : MonoBehaviour
    {
        [SerializeField] private GameObject shipType = null;        
        [SerializeField] private float thrust = 10;
        [SerializeField] private float turnRate = 4;


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
            GameObject ship = (GameObject)Instantiate(shipType, transform.position, Quaternion.identity);
            Fighter fighter = ship.AddComponent<Fighter>();
            fighter.SetThrust(thrust);
            fighter.SetTurnRate(turnRate);
            fighter.mothership = gameObject;
        }

        public void AlterTurnRate(float change)
        {
            turnRate += change;
        }

        public void AlterThrust(float change)
        {
            thrust += change;
        }
    }
}
