using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour
{
    [SerializeField] private GameObject m_beholder;
    [SerializeField] private GameObject m_beholden;

	void Update()
    {
        m_beholder.transform.LookAt(m_beholden.transform.position);	
	}
}
