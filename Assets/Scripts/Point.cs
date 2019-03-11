using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
	public Map m_map;
	public Point[] m_otherMapPoint;
	public string m_pointName;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		//m_map.m_nav.DrawLine(m_map.m_nav.transform, transform);
		//Debug.Log(m_map.m_nav.CalDis(m_map.m_nav.transform, transform));
	}
}
