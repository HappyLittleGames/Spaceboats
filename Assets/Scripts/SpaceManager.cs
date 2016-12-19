using System.Collections.Generic;
using UnityEngine;

class SpaceManager : MonoBehaviour
{
    private Teams m_teams; // do not want this.
    public Teams GetTeams() { return m_teams; }

    // sweep all of space for instances of T, do not call too often!
    public List<T> DetectSpaceThings<T>() where T : MonoBehaviour
    {
        List<T> spaceThingsInSpace = new List<T>();

        foreach (T spaceThing in GameObject.FindObjectsOfType<T>())
        {
            spaceThingsInSpace.Add(spaceThing);
        }

        return spaceThingsInSpace;
    }
}

