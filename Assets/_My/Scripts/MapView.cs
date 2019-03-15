using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapView : MonoBehaviour
{
	public Point st, ed;

	public enum MapStates
	{
		None,
		StartFirstPosition,
		StartSecondPosition,
		EndFirstPosition,
		EndPosition,
		Showing,
	}

	public static MapView Instance;
	private void Awake()
	{
		if (!Instance)
		{
			Instance = this;
		}
	}

	//public Camera m_camera;
	private int m_currentMap;
	private MapStates m_mapStates;

	public string FindWay(Point _st,HashSet<Point> _ed)
	{
		m_currentMap = 0;
		m_mapStates = MapStates.Showing;
		return MapManager.Instance.FindWay(_st, _ed);
	}


	public void NextMap()
	{
		if (m_mapStates == MapStates.Showing)
		{
			m_currentMap++;
			if (m_currentMap >= MapManager.Instance.FindWayMaps.Count) m_currentMap = 0;
			CamerasManager.Instance.TransTo(new Vector2(MapManager.Instance.FindWayMaps[m_currentMap].transform.position.x, MapManager.Instance.FindWayMaps[m_currentMap].transform.position.z));
			//Debug.Log(m_currentMap);
			//m_camera.transform.position = new Vector3(MapManager.Instance.FindWayMaps[m_currentMap].transform.position.x, 10, MapManager.Instance.FindWayMaps[m_currentMap].transform.position.z);
		}
	}

	public void LastMap()
	{
		//FindWay(st, ed);

		if (m_mapStates == MapStates.Showing)
		{
			m_currentMap--;
			if (m_currentMap < 0) m_currentMap = MapManager.Instance.FindWayMaps.Count-1;
			//Debug.Log(m_currentMap);
			//m_camera.transform.position = new Vector3(MapManager.Instance.FindWayMaps[m_currentMap].transform.position.x, 10, MapManager.Instance.FindWayMaps[m_currentMap].transform.position.z);
			CamerasManager.Instance.TransTo(new Vector2(MapManager.Instance.FindWayMaps[m_currentMap].transform.position.x, MapManager.Instance.FindWayMaps[m_currentMap].transform.position.z));
		}
	}

	public void BackToNone()
	{
		m_mapStates = MapStates.None;
		foreach (var map in MapManager.Instance.FindWayMaps)
		{
			map.m_nav.DelLine();
		}
	}


	// Start is called before the first frame update
	void Start()
    {

	}

	// Update is called once per frame
	void Update()
    {
    }
}
