using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.BHTree;

public class Fighter : MonoBehaviour
{
    private FighterBlackboard m_blackboard = null;
    private Propulsion m_propulsion = null;
    private Weapon m_weapon = null;
    private Rigidbody m_rigidbody;
    private float m_thrust = 10;
    private float m_turnRate = 4;

    public GameObject mothership { get; set; }

    void Start()
    {
        m_weapon = (gameObject.tag == "Team1") ? new Weapon(Color.red, 20.0f, 10.0f) : new Weapon(Color.green, 20.0f, 10.0f);
        m_rigidbody = gameObject.AddComponent<Rigidbody>();
        m_rigidbody.useGravity = false;
        m_propulsion = new Propulsion(m_rigidbody, gameObject, m_thrust, m_turnRate);
        mothership = null;
        InitializeBlackboard();
    }


    void FixedUpdate()
    {
        m_blackboard.navigator.Navigate(Time.fixedDeltaTime);
    }


    private void InitializeBlackboard()
    {
        m_blackboard = new FighterBlackboard(this, gameObject);
        m_blackboard.AddScanner();
        m_blackboard.AddWeapon(m_weapon);
        m_blackboard.AddNavigation(m_propulsion);
    }


    void Update()
    {
        m_blackboard.BlackboardUpdate(Time.deltaTime);
    }


    public void SetThrust(float amount)
    {
        m_thrust = amount;
    }


    public void SetTurnRate(float amount)
    {
        m_turnRate = amount;
    }
}

