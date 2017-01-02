using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ShipCounter))]
public class SpaceManager : MonoBehaviour {


    [SerializeField] private Text teamOneCount;
    [SerializeField] private Text teamTwoCount;
    [SerializeField] private Camera m_camera = null;
    public ShipCounter shipCounter { get; private set; }
    [SerializeField] private GameObject m_planet = null;
    float m_gravityAtPlanet = 65000000;

    void Start()
    {
        shipCounter = gameObject.GetComponent<ShipCounter>();

        teamOneCount.text = "#0";
        teamTwoCount.text = "#0";
    }
    

    void FixedUpdate()
    {
       
    }


    void Update()
    {
        if (teamOneCount != null)        
            teamOneCount.text = "#" + shipCounter.fighterTeams[1].Count;
        if (teamTwoCount != null)
            teamTwoCount.text = "#" + shipCounter.fighterTeams[2].Count;

        UpdateCamera(Time.deltaTime);
    }


    private void UpdateCamera(float deltaTime)
    {
        if (m_camera != null)
        {
            if (shipCounter.averagePosition != Vector3.zero)
            {
                Quaternion smoothRotation = Quaternion.LookRotation(shipCounter.averagePosition - m_camera.transform.position);
                m_camera.transform.rotation = Quaternion.Slerp(m_camera.transform.rotation, smoothRotation, 1f * Time.deltaTime);                
                m_camera.transform.position += new Vector3(Random.Range(0, 20), Random.Range(0, 20), Random.Range(0, 20)) * deltaTime;
            }

            var change = Input.GetAxis("Mouse ScrollWheel");
            if (change > 0f)
            {
                AlterZoom(-2);
            }
            else if (change < 0f)
            {
                AlterZoom(2);
            }
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
            // use fields for torps though
            // en.wikipedia.org/wiki/Newton's_law_of_universal_gravitation#Vector_form

            float gravityOverDistance = m_gravityAtPlanet / Vector3.SqrMagnitude(m_planet.transform.position - myPosition);
            Vector3 gravity = (m_planet.transform.position - myPosition).normalized * gravityOverDistance;
            return gravity;
        }
        else
            return Vector3.zero;
    }
}
