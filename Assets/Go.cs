using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Go : MonoBehaviour
{
    private Vector3 m_velocity, m_position, m_gravity, m_destination;
    private Quaternion m_rotation;
    private GameObject m_bearing = null;
    private GameObject m_star = null;
    private Rigidbody m_rigidBody = null;

    [SerializeField]private GameObject m_destObject = null;
    [SerializeField]private bool m_debugRays = false;

    [SerializeField]private float m_turnRate = 30;
    [SerializeField]private float m_horsePower = 300;
    // [SerializeField]private float m_topSpeed = 30;
    [SerializeField]private float m_squadTightness = 10;

    [SerializeField]private List<GameObject> m_wingMen = null;


    void Start()
    {
        m_star = GameObject.FindGameObjectWithTag("Star"); // This needs a physics implementation

        m_velocity = Vector3.zero;
        m_position = gameObject.transform.position;
        m_rotation = transform.rotation;
        m_gravity = Vector3.zero;
        m_destination = Vector3.zero;
        MakeBearing();

        if(!m_rigidBody)
        {
            m_rigidBody = gameObject.AddComponent<Rigidbody>();
            m_rigidBody.useGravity = false;

        }
    }


    void FixedUpdate()
    {
        StayInFormation();
        ApplyThrottle(1);
    }

    void Update()
    {
        SetValues(); // save transform values to variables for no reason

        if (m_destObject != null)
        {
            TurnToBearing(m_destObject.transform.position);
        }

        gameObject.transform.rotation = m_rotation;
    }







    private void TurnToBearing(Vector3 destination)
    {
        if (m_destination != null)
        {
            SetBearing(m_destination);
            Quaternion compDirection = m_bearing.transform.rotation;
            float smoothTurn = (Quaternion.Angle(gameObject.transform.rotation, compDirection) * m_turnRate) * Time.deltaTime;
            m_rotation = Quaternion.RotateTowards(gameObject.transform.rotation, compDirection, smoothTurn);
        }
    }

    private void SetBearing(Vector3 destination)
    {
        m_bearing.transform.LookAt(destination, gameObject.transform.position - ((m_star) ? m_star.transform.position : gameObject.transform.up));
        if (m_debugRays)
        {
            Debug.DrawRay(m_bearing.transform.position, m_bearing.transform.forward * 10);
        }
    }


    private void SetValues()
    {
        m_rotation = transform.rotation;
        m_position = transform.position;
        if (m_destination != null)
        {
            m_destination = m_destObject.transform.position;
        }
        SetGravity();
    }


    private void SetGravity()
    {
        if (m_star != null)
        {
            m_gravity = transform.position - m_star.transform.position;
        }
    }


    private void MakeBearing()
    {
        if (!m_bearing)
        {
            m_bearing = new GameObject();
            m_bearing.name = "Bearing of " + gameObject.name;
            m_bearing.transform.parent = gameObject.transform;
        }
        m_bearing.transform.position = gameObject.transform.position;
        m_bearing.transform.rotation = gameObject.transform.rotation;
    }


    private void ApplyThrottle(float throttle)
    {
        // if (m_rigidBody.velocity.magnitude < m_topSpeed)
        {
            if (m_debugRays)
            {
                Debug.Log("current velocity = " + m_rigidBody.velocity.magnitude);
            }
            m_rigidBody.AddForce((gameObject.transform.forward * m_horsePower * throttle) * Time.fixedDeltaTime, ForceMode.Impulse);
        }
    }


    private void StayInFormation()
    {
        if (m_wingMen != null)
        {

            GameObject wingMan = m_wingMen[0];
            float distance = Vector3.Distance(gameObject.transform.position, wingMan.transform.position);
            if (m_wingMen.Count > 1)
            {
                for (int i = 1; i < m_wingMen.Count; i++)
                {
                    if (Vector3.Distance(gameObject.transform.position, m_wingMen[i].transform.position) < distance)
                        wingMan = m_wingMen[i];
                }
            }
            distance = Vector3.Distance(gameObject.transform.position, wingMan.transform.position);
            // if (location != Vector3.zero)
            {
                Vector3 direction = wingMan.transform.position - gameObject.transform.position;
                float vectoringAmount = distance - m_squadTightness;
                VectoringThrust(direction, vectoringAmount);
                if (m_debugRays)
                {
                    Debug.DrawRay(gameObject.transform.position, direction, Color.red);
                }
            }
        }
    }

    private void VectoringThrust(Vector3 direction, float amount)
    {
        m_rigidBody.AddForce(direction.normalized * (amount * Time.fixedDeltaTime), ForceMode.Impulse);
    }


}
