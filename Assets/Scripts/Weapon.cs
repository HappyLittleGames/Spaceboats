using UnityEngine;
using System.Collections;

public class Weapon
{
    public Weapon(Color lazerColor)
    {
        if (lazerColor != null)
            m_color = lazerColor;
        else
            m_color = Color.green;
       
    }

    private Color m_color;
    private bool m_coolingDown = false;
    private float m_cooldown = 0.15f;
    private float m_range = 20f;


    public GameObject DebugPew(Vector3 start, Vector3 direction, float dur)
    {
        // if (whatever weapon cd)
        {
            Debug.DrawRay(start, (direction.normalized * m_range), m_color, dur);
            RaycastHit hit;

            if (Physics.Raycast(start, (direction.normalized), out hit, m_range))
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
