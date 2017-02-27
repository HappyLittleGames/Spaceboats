using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class SpawnShips : MonoBehaviour
    {
        [SerializeField] private string m_spawnKey = null;
        [SerializeField] private GameObject m_shipType = null;
        [SerializeField] public GameObject enemyMothership = null;
        [SerializeField] private int m_teamNumber = 0;
        [SerializeField] private float m_thrust = 10;
        [SerializeField] private float m_turnRate = 4;
        [SerializeField] private SpaceManager m_spaceManager = null;

        public void Start()
        {
            m_spaceManager = GameObject.FindGameObjectWithTag("SpaceManager").GetComponent<SpaceManager>();
            m_spaceManager.shipCounter.fighterTeams.Add(m_teamNumber, new List<GameObject>());
        }

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
            if (Input.GetButton(m_spawnKey))
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
            fighter.spaceManager = m_spaceManager;
            fighter.enemyMothership = enemyMothership;
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
