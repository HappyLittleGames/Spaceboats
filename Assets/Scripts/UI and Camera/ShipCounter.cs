using UnityEngine;
using System.Collections.Generic;
using Assets.BHTree;

public class ShipCounter : MonoBehaviour {

    public Vector3 averagePosition { get; private set; }
    public Dictionary<int, List<GameObject>> fighterTeams;
    [SerializeField] GameObject m_explosion = null;

    private SpaceManager m_spaceManager = null;
    private Assets.BHTree.Behaviour m_countShips = new Assets.BHTree.Behaviour();

    void Start ()
    {
        fighterTeams = new Dictionary<int, List<GameObject>>();

        averagePosition = Vector3.zero;

        m_countShips.BUpdate = () =>
            {
                averagePosition = Vector3.zero;

                for (int i = 1; i <= fighterTeams.Keys.Count; i++)
                {
                    fighterTeams[i].Clear();
                }

                foreach (GameObject fighter in GameObject.FindGameObjectsWithTag("Fighter"))
                {
                    if (fighter.GetComponent<Fighter>().isExploding)
                    {
                        GameObject explosion = (GameObject)Instantiate(m_explosion, fighter.transform.position, fighter.transform.rotation);
                        explosion.GetComponent<Rigidbody>().velocity = fighter.GetComponent<Rigidbody>().velocity;                        
                        Destroy(explosion, 2f);
                        Destroy(fighter);
                        
                        // Debug.Log("Trigger Eksplozionz!");
                    }
                    else
                    {
                        if (fighter.GetComponent<Fighter>())
                        {
                            fighterTeams[fighter.GetComponent<Fighter>().teamNumber].Add(fighter);
                        }
                        averagePosition += fighter.transform.position;                        
                    }
                    averagePosition /= (fighterTeams[1].Count + fighterTeams[2].Count);
                }

                //try that freaky camera shit while we are at it

                return BHStatus.Success;
            };
        m_countShips = new Regulator(m_countShips, 0.1f);
	}
	
	
	void Update ()
    {
        m_countShips.BTick();
    }


    private void ExplodeFighters()
    {
        foreach (int key in fighterTeams.Keys)
        {
            for (int j = fighterTeams[key].Count; j >= 0; j--)
            {
                if (fighterTeams[key][j].GetComponent<Fighter>().isExploding)
                {
                    GameObject toExplode = fighterTeams[key][j];
                    fighterTeams[key].Remove(fighterTeams[key][j]);
                    Destroy(toExplode);
                }
            }
        }
    }
}
