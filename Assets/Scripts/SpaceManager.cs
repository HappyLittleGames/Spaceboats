using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(ShipCounter))]
public class SpaceManager : MonoBehaviour {


    [SerializeField] private Text teamOneCount;
    [SerializeField] private Text teamTwoCount;
    [SerializeField] private Camera m_camera = null;
    private ShipCounter m_shipCounter = null;
    [SerializeField] private GameObject m_planet = null;
    float m_gravityAtPlanet = 65000000;

    void Start()
    {
        m_shipCounter = gameObject.GetComponent<ShipCounter>();

        teamOneCount.text = "#0";
        teamTwoCount.text = "#0";


    }



    void FixedUpdate()
    {
       
    }


    void Update()
    {
        if (teamOneCount != null)        
            teamOneCount.text = m_shipCounter.teamOneCount;
        if (teamTwoCount != null)
            teamTwoCount.text = m_shipCounter.teamTwoCount;
            
        if (m_camera != null)
        {
            Quaternion smoothRotation = Quaternion.LookRotation(m_shipCounter.averagePosition - m_camera.transform.position);
            m_camera.transform.rotation = Quaternion.Slerp(m_camera.transform.rotation, smoothRotation, 1f * Time.deltaTime);
        }
    }


    public void AlterZoom(float amount)
    {
        if (m_camera != null)
        {
            m_camera.fieldOfView = Mathf.Clamp(m_camera.fieldOfView + amount, 1, 179);
        }
    }


    public Vector3 GetGravity(Vector3 myPosition)
    {
        if (m_planet != null)
        {
            // use fields for torps
            // en.wikipedia.org/wiki/Newton's_law_of_universal_gravitation#Vector_form

            float gravityOverDistance = m_gravityAtPlanet / Vector3.SqrMagnitude(m_planet.transform.position - myPosition);
            Vector3 gravity = (m_planet.transform.position - myPosition).normalized * gravityOverDistance;
            return gravity;
        }
        else
            return Vector3.zero;
    }
}
