using UnityEngine;
using System.Collections;

public class Weapon
{
    private Color m_color;
    private bool m_coolingDown = false;
    public float cooldown { get; private set; }
    public float range { get; private set; }
    public float accuracy { get; private set; }

    public Weapon(Color lazerColor, float range, float accuracy)
    {
        if (lazerColor != null)
            m_color = lazerColor;
        else
            m_color = Color.green;

        this.range = range;
        this.accuracy = accuracy;

    }


    public GameObject DebugPew(Vector3 start, Vector3 direction, float dur)
    {
        // if (whatever weapon cd)
        {
            Debug.DrawRay(start, (direction.normalized * range), m_color, dur);

            RaycastHit hit;
            if (Physics.Raycast(start, (direction.normalized), out hit, range))
            {
                return hit.collider.gameObject;
            }
            else
            {
                return null;
            }
        }
    }


}
