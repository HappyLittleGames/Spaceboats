using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Propulsion
{
    private Rigidbody m_rigidbody = null;
    private Transform m_centreOfMass = null;

    public Propulsion(Rigidbody rigidBody, Transform rootTransform)
    {
        m_rigidbody = rigidBody;
        m_centreOfMass = rootTransform;
    }

    [SerializeField] private bool m_debugLogs = false;
    [SerializeField] private bool m_debugRays = false;
    [SerializeField] private float m_turnRate = 4;
    [SerializeField] private float m_thrust = 10;
    [SerializeField] private float m_vectoringThrust = 1;


    /// <summary>
    ///  Applies throttle in the direction of the centre of mass.
    /// </summary>
    /// <param name="throttle">
    /// Throttle clamped 0-1 to scale the trottle amount
    /// <param name="fixedDeltaTime">
    /// Fixed Delta Seconds from UnityEngine.
    /// </param>
    public void ApplyThrottle(float throttle, float fixedDeltaTime)
    {
        {
            if (m_debugLogs)
            {
                Debug.Log("current velocity = " + m_rigidbody.velocity.magnitude);
            }
            m_rigidbody.AddForce((m_centreOfMass.forward * m_thrust * throttle) * fixedDeltaTime, ForceMode.Impulse);
        }
    }


    /// <summary>
    ///  Reduces momentum by applying a throttled force in the direction inverse to current velocity.
    /// </summary>
    /// <param name="throttle">
    /// Throttle clamped 0-1 to scale the velocity per second.
    /// </param>
    /// <param name="fixedDeltaTime">
    /// Fixed Delta Seconds from UnityEngine.
    /// </param>
    public void ApplyGravBreak(float throttle, float fixedDeltaTime)
    {
        if (m_debugLogs)
            Debug.Log("Breaking!");

        float amount = 1 - Mathf.Clamp(throttle, 0f, 1f);
        Vector3 reverseThrust = -m_rigidbody.velocity.normalized;
        m_rigidbody.AddForce((reverseThrust * m_thrust * amount) * fixedDeltaTime, ForceMode.Impulse);
        Debug.DrawRay(m_centreOfMass.position, m_rigidbody.velocity * (m_thrust * -amount * fixedDeltaTime));
    }


    /// <summary>
    ///  Applies vectoring thrust in any direction, determined by thust in propulsion.
    /// </summary>
    ///     /// <param name="direction">
    /// Direction vector to apply thrust.
    /// </param>
    /// <param name="throttle">
    /// Throttle clamped from 0 to 1 to scale the velocity per second.
    /// </param>
    /// <param name="fixedDeltaTime">
    /// Fixed Delta Seconds from UnityEngine.
    /// </param>
    public void VectoringThrust(Vector3 direction, float throttle, float fixedDeltaTime)
    {
        m_rigidbody.AddForce((direction.normalized * (m_vectoringThrust * throttle)) * fixedDeltaTime, ForceMode.Impulse); // impulse mode
    }


    /// <summary>
    ///  Applies some torque vessel, per axis
    /// </summary>
    ///     /// <param name="direction">
    /// Direction vector to apply thrust.
    /// </param>
    /// <param name="input">
    /// "pitch", "yaw", or "roll" along the transform axis.
    /// </param>
    /// <param name="amount">
    /// Desired throttle, clamped from -1 to 1 (left to right).
    /// </param>
    public void Turn(string input, float amount)
    {
        switch (input)
        {
            case "pitch":
                m_rigidbody.AddTorque((m_centreOfMass.right * m_turnRate) * amount, ForceMode.Acceleration);
                break;
            case "yaw":
                m_rigidbody.AddTorque((m_centreOfMass.up * m_turnRate) * amount, ForceMode.Acceleration);
                break;
            case "roll":
                m_rigidbody.AddTorque((m_centreOfMass.forward * m_turnRate) * amount, ForceMode.Acceleration);
                break;
            default:
                Debug.Log(this + ", TurnToBearing took invalid string");
                break;
        }
    }
    //private void SetBearing(Vector3 desiredDestination)
    //{
    //    Quaternion bearing = new Quaternion(m_centreOfMass.rotation.x, m_centreOfMass.rotation.y, m_centreOfMass.rotation.z, m_centreOfMass.rotation.w);
    //    m_bearing.transform.LookAt(desiredDestination, gameObject.transform.position - ((m_star) ? m_star.transform.position : gameObject.transform.up));
    //    if (m_debugRays)
    //    {
    //        Debug.DrawRay(m_bearing.transform.position, m_bearing.transform.forward * 10);
    //    }
    //}
}

