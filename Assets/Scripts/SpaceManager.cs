using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ShipCounter))]
public class SpaceManager : MonoBehaviour {


    [SerializeField] private Text teamOneCount;
    [SerializeField] private Text teamTwoCount;
    [SerializeField] private Camera m_camera = null;
    public ShipCounter shipCounter { get; private set; }
    [SerializeField] private GameObject m_planet = null;
    [SerializeField] private List<GameObject> m_spawners;
    float m_gravityAtPlanet = 65000000;

    public float m_dragSpeed = 16;
    private Vector3 m_dragOrigin;

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
                //Quaternion smoothRotation = Quaternion.LookRotation(shipCounter.averagePosition - m_camera.transform.position);
                //m_camera.transform.rotation = Quaternion.Slerp(m_camera.transform.rotation, smoothRotation, 1f * Time.deltaTime);                
                m_camera.transform.position += new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10,10)) * deltaTime;
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

            DragCamera();
        }
    }


    // Mr Percy McPersonface at https://forum.unity3d.com/threads/click-drag-camera-movement.39513/
    private void DragCamera()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        Vector3 pos = m_camera.ScreenToViewportPoint(Input.mousePosition - m_dragOrigin);
        Vector3 move = new Vector3(pos.x * m_dragSpeed, pos.z * m_dragSpeed, pos.y * m_dragSpeed);

        m_camera.transform.Translate(move, Space.World);
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
            Vector3 gravity = Vector3.zero;
            // use fields for torps though?
            // en.wikipedia.org/wiki/Newton's_law_of_universal_gravitation#Vector_form
            if (Input.GetKeyDown(KeyCode.F3))
            {
                float gravityOverDistance = 10000000 / Vector3.SqrMagnitude(new Vector3(0, 0, 0) - myPosition);
                gravity += (new Vector3(0, 0, 0) - myPosition).normalized * gravityOverDistance;
            }
            if (Input.GetKey(KeyCode.F4))
            {
                m_spawners[0].GetComponent<RandomScaler>().isScaling = true;

                float gravityOverDistance = 100000 / Vector3.SqrMagnitude(m_spawners[0].transform.position - myPosition);
                gravity += (m_spawners[0].transform.position - myPosition).normalized * gravityOverDistance;
            }
            else
            {
                m_spawners[1].GetComponent<RandomScaler>().isScaling = false;
            }
            if (Input.GetKey(KeyCode.F5))
            {
                m_spawners[1].GetComponent<RandomScaler>().isScaling = true;

                float gravityOverDistance = 100000 / Vector3.SqrMagnitude(m_spawners[1].transform.position - myPosition);
                gravity += (m_spawners[1].transform.position - myPosition).normalized * gravityOverDistance;
            }
            else
            {
                m_spawners[1].GetComponent<RandomScaler>().isScaling = false;
            }
            {
                float gravityOverDistance = m_gravityAtPlanet / Vector3.SqrMagnitude(m_planet.transform.position - myPosition);
                gravity += (m_planet.transform.position - myPosition).normalized * gravityOverDistance;
            }
            return gravity;
        }
        else
            return Vector3.zero;
    }
}
