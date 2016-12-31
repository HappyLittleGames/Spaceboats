using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.BHTree;

public class ShipCounter : MonoBehaviour {

    public string teamOneCount { get; private set; }
    public string teamTwoCount { get; private set; }
    public Vector3 averagePosition { get; private set; }

    private Assets.BHTree.Behaviour m_countShips = new Assets.BHTree.Behaviour();

    void Start ()
    {
        teamOneCount = "#0";
        teamTwoCount = "#0";
        averagePosition = Vector3.zero;

        m_countShips.BUpdate = () =>
            {
                int total = 0;
                int team1 = 0;
                averagePosition = Vector3.zero;
                foreach (GameObject fighter in GameObject.FindGameObjectsWithTag("Fighter"))
                {
                    total++;
                    if (fighter.GetComponent<Fighter>().teamNumber == 1)
                        team1++;

                    averagePosition += fighter.transform.position;
                    averagePosition /= total;
                }
                teamOneCount = "#" + team1;
                teamTwoCount = "#" + (total - team1);

                //try that freaky camera shit while we are at it

                return BHStatus.Success;
            };
        m_countShips = new Regulator(m_countShips, 0.1f);
	}
	
	
	void Update ()
    {
        m_countShips.BTick();
    }
}
