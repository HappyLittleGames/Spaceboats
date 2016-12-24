using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.BHTree;

public class Go : MonoBehaviour
{
    private Vector3 m_velocity, m_position, m_gravity, m_destination;
    private Quaternion m_rotation;
    private GameObject m_bearing = null;
    private GameObject m_star = null;
    private GameObject m_teamBase = null;
    private Rigidbody m_rigidBody = null;
    private List<Weapon> m_weapons = null;
 
    [SerializeField]private GameObject m_destObject = null;
    [SerializeField]private GameObject m_leadWingMan = null;
    [SerializeField]private bool m_debugLogs = false;
    [SerializeField]private bool m_debugRays = false;

    [SerializeField]private float m_turnRate = 30;
    [SerializeField]private float m_horsePower = 300;
    // [SerializeField]private float m_topSpeed = 30;
    [SerializeField]private float m_squadTightness = 10;

    [SerializeField]private Teams m_teams = null;


    /*
        BHTreeStuff:
         - Update toward blackboard that holds the trees.
         - Separate trees for Navigation, Sensors, and Weapons.
         - Variable tick rates for different modules, and during engagements or not
    */
    private FighterBlackboard m_blackboard = null;
    [SerializeField] private Propulsion m_propulsion = null;



    void Start()
    {
        m_teams = GameObject.FindGameObjectWithTag("TeamManager").GetComponent<Teams>();
        if (m_teams)
        {
            if (m_teams.AddToTeam(gameObject))
            {
                Debug.Log(gameObject.name + " added to TeamManager");
            }
        }

        m_star = GameObject.FindGameObjectWithTag("Star"); // This needs a physics implementation
        m_weapons = new List<Weapon>();
        m_weapons.Add((gameObject.tag == "Team1") ? new Weapon(Color.red, 20.0f, 10.0f) : new Weapon(Color.green, 20.0f, 10.0f));

        m_velocity = Vector3.zero;
        m_position = gameObject.transform.position;
        m_rotation = transform.rotation;
        m_gravity = Vector3.zero;
        m_destination = Vector3.zero;
        MakeBearing();

        if(m_rigidBody == null)
        {
            m_rigidBody = gameObject.AddComponent<Rigidbody>();
            m_rigidBody.useGravity = false;
            m_propulsion = new Propulsion(m_rigidBody, gameObject.transform);
        }

        SweepForEntities();
        InitializeBlackboard();
    }


    void FixedUpdate()
    {
        //FindClosestWingman();
        //StayInFormation();
        //if ((m_destObject) && (m_destObject.tag != "TargetPracticeTarget"))
        //    //Navigate();
        ManualSteering();
    }

    void Update()
    {
        SetValues(); // save transform values to variables for no reason

        if (m_destObject != null)
        {
            TurnToBearing(m_destObject.transform.position);
        }

        // gameObject.transform.rotation = m_rotation;

        // LazerTrigger();

        SortEnemies();

        m_blackboard.BlackboardUpdate(Time.deltaTime);
    }


    private void InitializeBlackboard()
    {
        m_blackboard = new FighterBlackboard(this, gameObject);

        foreach (Weapon weapon in m_weapons)
        {
            m_blackboard.AddWeapon(weapon);
        }
        m_blackboard.AddNavigation(new Propulsion(m_rigidBody, gameObject.transform));
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
            m_blackboard.target = m_destObject;
        }
    }


    public void SetDestObject(GameObject destObject)
    {
        m_destObject = destObject;
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
    }


    private void FindClosestWingman()
    {
        if (Friendlies().Count > 0)
        {
            m_leadWingMan = Friendlies()[0];
            float distance = Vector3.Distance(gameObject.transform.position, m_leadWingMan.transform.position);
            if (Friendlies().Count > 1)
            {
                for (int i = 1; i < Friendlies().Count; i++) //  sort for closest wingman
                {
                    if (Vector3.Distance(gameObject.transform.position, Friendlies()[i].transform.position) < distance)
                        m_leadWingMan = Friendlies()[i];
                }
            }
        }
        else
            m_leadWingMan = null;
    }


    private void StayInFormation()
    {
        // behaviour for this in navigation
        if (Friendlies().Count > 0)
        {
            float distance = Vector3.Distance(gameObject.transform.position, m_leadWingMan.transform.position);

            distance = Vector3.Distance(gameObject.transform.position, m_leadWingMan.transform.position);
            // if (location != Vector3.zero)
            {
                Vector3 direction = m_leadWingMan.transform.position - gameObject.transform.position;  // maintain distance
                float vectoringAmount = distance - m_squadTightness;
                m_propulsion.VectoringThrust(direction, vectoringAmount, Time.fixedDeltaTime);
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
        if (m_destObject)
        {
            if (Vector3.Distance(m_destination, transform.position + m_rigidBody.velocity) > Vector3.Distance(m_destination, transform.position))
            {
                if (m_propulsion != null)
                    m_propulsion.ApplyGravBreak(0.5f, Time.fixedDeltaTime);
                else if (m_debugLogs)
                {
                    Debug.Log(gameObject.name + " is attempting apply throttle but can't access propulsion!");
                }
            }

            if (m_propulsion != null)
                m_propulsion.ApplyThrottle(1, Time.fixedDeltaTime);
            else if (m_debugLogs)
            {
                Debug.Log(gameObject.name + " is attempting apply throttle but can't access propulsion!");
            }
        }
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
                        Debug.Log("Fire lazors!");
                    foreach (Weapon weapon in m_weapons)
                    {
                        m_teams.Kill(weapon.DebugPew(m_position, gameObject.transform.forward, 0.1f));
                    }
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            foreach (Weapon weapon in m_weapons)
            {
                m_teams.Kill(weapon.DebugPew(m_position, gameObject.transform.forward, 0.2f));
            }
        }
    }


    public void KillMe()
    {
        m_teams.Kill(gameObject);
    }


    private void ManualSteering()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            m_propulsion.Turn("pitch", Input.GetAxis("Vertical"));
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            m_propulsion.Turn("yaw", Input.GetAxis("Horizontal"));
        }
        if (Input.GetAxis("Roll") != 0)
        {
            m_propulsion.Turn("roll", Input.GetAxis("Roll"));
        }
        if (Input.GetButton("Jump"))
        {
            m_propulsion.ApplyThrottle(1, Time.fixedDeltaTime);
        }
        if (Input.GetButton("Fire1"))
        {
            m_propulsion.ApplyGravBreak(1, Time.fixedDeltaTime);
        }
    }

}
