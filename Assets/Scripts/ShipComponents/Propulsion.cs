using UnityEngine;


public class Propulsion
{
    private bool m_debugLogs = false;    

    public float turnRate { get; set; }
    public float thrust { get; set; }
    public float vectoringThrust { get; set; }

    public Rigidbody rigidbody { get; private set; }
    private GameObject m_rootObject = null;

    public Propulsion(Rigidbody rigidBody, GameObject rootObject, float thrust, float turnRate)
    {
        rigidbody = rigidBody;
        m_rootObject = rootObject;
        this.thrust = thrust;
        this.turnRate = turnRate;
        vectoringThrust = 1;       
        
    }


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
                Debug.Log("current velocity = " + rigidbody.velocity.magnitude);
            }
            float amount = Mathf.Clamp(throttle, -1f, 1f);
            rigidbody.AddForce((m_rootObject.transform.forward * thrust * amount) * fixedDeltaTime, ForceMode.Impulse);
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
        float amount = 1 - Mathf.Clamp(throttle, 0f, 1f);
        Vector3 reverseThrust = -rigidbody.velocity.normalized;
        rigidbody.AddForce((reverseThrust * thrust * amount) * fixedDeltaTime, ForceMode.Impulse);
        Debug.DrawRay(m_rootObject.transform.position, rigidbody.velocity * (thrust * -amount * fixedDeltaTime));
    }


    /// <summary>
    ///  Applies vectoring thrust in any direction, determined by strength of thrust in propulsion and it's throttle.
    /// </summary>
    ///     /// <param name="direction">
    /// Direction vector to apply thrust.
    /// </param>
    /// <param name="throttle">
    /// Desired throttle, clamped from -1 to 1 (inverse to actual).
    /// </param>
    /// <param name="fixedDeltaTime">
    /// Fixed Delta Seconds from UnityEngine.
    /// </param>
    public void VectoringThrust(Vector3 direction, float throttle, float fixedDeltaTime)
    {
        float amount = 1 - Mathf.Clamp(throttle, -1f, 1f);
        rigidbody.AddForce((direction.normalized * (vectoringThrust * amount)) * fixedDeltaTime, ForceMode.Acceleration); // impulse mode
    }


    /// <summary>
    ///  Applies some torque, per axis
    /// </summary>
    ///     /// <param name="direction">
    /// Direction vector to apply thrust.
    /// </param>
    /// <param name="input">
    /// "pitch", "yaw", or "roll" along the transform axis.
    /// </param>
    public void Rotate(string input, Vector3 rotation)
    {
        //Rigidbody.AddRelativeTorque(rotation.normalized * m_turnRate * amount, ForceMode.Force);

        if (rotation != Vector3.zero)
        {
            switch (input)
            {
                case "pitch":
                    rigidbody.AddTorque((m_rootObject.transform.right * turnRate) * rotation.x, ForceMode.Force);
                    break;
                case "yaw":
                    rigidbody.AddTorque((m_rootObject.transform.up * turnRate) * rotation.y, ForceMode.Force);
                    break;
                case "roll":
                    rigidbody.AddTorque((m_rootObject.transform.forward * turnRate) * rotation.z, ForceMode.Force);
                    break;
                default:
                    Debug.Log(this + ", Rotate took invalid string");
                    break;
            }
        }
    }


    /// <summary>
    ///  Poorly implemented Slerping towards a direction.
    /// </summary>
    /// <param name="desiredDirection">
    /// The point to turn towards.
    /// </param>
    public void FakeRotate(Vector3 desiredDirection, float deltaTime)
    {
        if (desiredDirection != Vector3.zero && desiredDirection.magnitude != 0.0f)
        {
            Quaternion direction = Quaternion.LookRotation(desiredDirection);
            direction = Quaternion.Slerp(rigidbody.rotation, direction, turnRate * deltaTime);
            rigidbody.angularVelocity = Vector3.zero;

            rigidbody.MoveRotation(direction);
        }
    }
}

