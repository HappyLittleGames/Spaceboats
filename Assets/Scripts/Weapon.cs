﻿using UnityEngine;
using System.Collections;

public class Weapon
{
    private Color m_color;
    private bool m_coolingDown = false;
    public float cooldown { get; private set; }
    public float range { get; private set; }
    public float accuracy { get; private set; }
    private GameObject m_weaponVisuals = null; 

    public Weapon(Color lazerColor, float range, float accuracy, GameObject weaponVisuals)
    {
        if (lazerColor != null)
            m_color = lazerColor;
        else
            m_color = Color.green;

        this.range = range;
        this.accuracy = accuracy;
        if (weaponVisuals)
        {
            weaponVisuals.transform.localRotation = Quaternion.Euler(90, 0, 0);
            weaponVisuals.transform.localScale = new Vector3(0.0f, range/2, 0.0f);
            weaponVisuals.transform.localPosition += new Vector3(0, 0, range/2);
            this.m_weaponVisuals = weaponVisuals;
        }
    }


    public GameObject DebugPew(Vector3 start, Vector3 direction, float dur)
    {
        {
            Debug.DrawRay(start, (direction.normalized * range), m_color, dur);
            Pew(start, direction);
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


    public void Pew(Vector3 start, Vector3 direction)
    {
        if (m_weaponVisuals)
            m_weaponVisuals.transform.localScale = new Vector3(0.6f, m_weaponVisuals.transform.localScale.y, 0.6f);
    }
}
