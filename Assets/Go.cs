using UnityEngine;
using System.Collections;

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

    }


    void Update()
    {
        SetValues(); // save transform values to variables for no reason

        if (m_destObject != null)
        {
            TurnToBearing(m_destObject.transform.position);
        }

        gameObject.transform.rotation = m_rotation;

        ApplyThrottle(1);
    }


    private void TurnToBearing(Vector3 destination)
    {
        SetBearing(m_destination);
        Quaternion compDirection = m_bearing.transform.rotation;
        float smoothTurn = (Quaternion.Angle(gameObject.transform.rotation, compDirection) * m_turnRate) * Time.deltaTime;
        m_rotation = Quaternion.RotateTowards(gameObject.transform.rotation, compDirection, smoothTurn);
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
        m_destination = m_destObject.transform.position;
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
        m_rigidBody.AddForce((gameObject.transform.forward * m_horsePower * throttle) * Time.deltaTime, ForceMode.Impulse);
    }
}
