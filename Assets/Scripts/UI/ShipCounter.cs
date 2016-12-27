using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShipCounter : MonoBehaviour {

	[SerializeField] private Text teamOneCount;
    [SerializeField] private Text teamTwoCount;

    void Start ()
    {
        teamOneCount.text = "#0";
        teamTwoCount.text = "#0";
	}
	
	
	void Update ()
    {
        teamOneCount.text ="#" + GameObject.FindGameObjectsWithTag("Team1").Length;
        teamTwoCount.text = "#" + GameObject.FindGameObjectsWithTag("Team2").Length;
    }
}
