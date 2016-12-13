using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Go : MonoBehaviour
{
    private Vector3 m_velocity, m_position, m_gravity, m_destination;
    private Quaternion m_rotation;
    private GameObject m_bearing = null;
    private GameObject m_star = null;
    private Rigidbody m_rigidBody = null;
    private Weapon m_weapon = null;
 
    [SerializeField]private GameObject m_destObject = null;
    [SerializeField]private bool m_debugLogs = false;
    [SerializeField]private bool m_debugRays = false;

    [SerializeField]private float m_turnRate = 30;
    [SerializeField]private float m_horsePower = 300;
    // [SerializeField]private float m_topSpeed = 30;
    [SerializeField]private float m_squadTightness = 10;

    [SerializeField]private Teams m_teams = null;


    void Start()
    {
        m_star = GameObject.FindGameObjectWithTag("Star"); // This needs a physics implementation
        m_weapon = (gameObject.tag == "Team1") ? new Weapon(Color.blue) : new Weapon(Color.red);

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

        SweepForEntities();

    }


    void FixedUpdate()
    {
        StayInFormation();
        Navigate();
    }

    void Update()
    {
        SetValues(); // save transform values to variables for no reason

        if (m_destObject != null)
        {
            TurnToBearing(m_destObject.transform.position);
        }

        gameObject.transform.rotation = m_rotation;

        LazerTrigger();

        SortEnemies();
    }

    private List<GameObject> Friendlies()
    {
        if (m_teams != null)
            return m_teams.GetTeam(gameObject.tag);
        else
            return null;
    }

    private List<GameObject> Hostiles()
    {
        if (m_teams != null)
            return m_teams.GetOtherTeams(gameObject.tag);
        else
            return null;
    }


    private void SweepForEntities() //DEBUG METHOD
    {
        m_teams = GameObject.FindGameObjectWithTag("TeamManager").GetComponent<Teams>();
    }


    private void SortEnemies() //DEBUG METHOD
    {
        if (Hostiles().Count > 0)
        {
            List<GameObject> sortedByRange = Hostiles().OrderBy(x => Vector3.Distance(gameObject.transform.position, x.transform.position)).ToList();
            m_destObject = sortedByRange[0];
        }
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
        if ((m_destination != null) && (m_destObject != null))
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
            if (m_debugLogs)
            {
                Debug.Log("current velocity = " + m_rigidBody.velocity.magnitude);
            }
            m_rigidBody.AddForce((gameObject.transform.forward * m_horsePower * throttle) * Time.fixedDeltaTime, ForceMode.Impulse);
        }
    }


    private void ApplyGravBreak(float pressure)
    { 
        if(m_debugLogs)
            Debug.Log("Breaking!");

        float amount = 1 - Mathf.Clamp(pressure, 0f, 1f);
        Vector3 reverseThrust = -m_rigidBody.velocity.normalized;
        m_rigidBody.AddForce((reverseThrust * m_horsePower * amount) * Time.fixedDeltaTime, ForceMode.Impulse);
        Debug.DrawRay(gameObject.transform.position, m_rigidBody.velocity * (m_horsePower * -amount * Time.fixedDeltaTime));
    }


    private void StayInFormation()
    {
        if (Friendlies().Count > 0)
        {
            GameObject wingMan = Friendlies()[0];
            float distance = Vector3.Distance(gameObject.transform.position, wingMan.transform.position);
            if (Friendlies().Count > 1)
            {
                for (int i = 1; i < Friendlies().Count; i++) //  sort for closest wingman
                {
                    if (Vector3.Distance(gameObject.transform.position, Friendlies()[i].transform.position) < distance)
                        wingMan = Friendlies()[i];
                }
            }
            distance = Vector3.Distance(gameObject.transform.position, wingMan.transform.position);
            // if (location != Vector3.zero)
            {
                Vector3 direction = wingMan.transform.position - gameObject.transform.position;  // maintain distance
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
        m_rigidBody.AddForce((direction.normalized * amount) * Time.fixedDeltaTime, ForceMode.Impulse); // impulse mode
    }


    private void Navigate()
    {
        // Debug.Log("comparing expected distance" + Vector3.Distance(m_destination, transform.position + m_rigidBody.velocity) + "and current distance" + Vector3.Distance(m_destination, transform.position));
        if (Vector3.Distance(m_destination, transform.position + m_rigidBody.velocity) >
            Vector3.Distance(m_destination, transform.position))
        { 
            ApplyGravBreak(0.5f);  
        }

        ApplyThrottle(1);
    }

    private void LazerTrigger()  // DEBUG METHOD
    {
        float accuracy = 10;
        float range = 20;
        if (m_destObject != null)
        {
            if (Vector3.Angle(m_destObject.transform.position - gameObject.transform.position, gameObject.transform.forward) < accuracy)
            {
                if (Vector3.Distance(gameObject.transform.position, m_destObject.transform.position) < range)
                {
                    if (m_debugLogs)
                        Debug.Log("Fire lazor!");

                    m_teams.Kill(m_weapon.DebugPew(m_position, gameObject.transform.forward, 0.1f));
                }
            }
        }
    }


}
