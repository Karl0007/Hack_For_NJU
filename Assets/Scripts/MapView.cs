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

	static MapView Instance;
	private void Awake()
	{
		if (!Instance)
		{
			Instance = this;
		}
	}

	public Camera m_camera;
	private int m_currentMap;
	private MapStates m_mapStates;

	public void FindWay(Point _st,Point _ed)
	{
		m_currentMap = 0;
		m_mapStates = MapStates.Showing;
		MapManager.Instance.FindWay(_st, _ed);
	}


	public void NextMap()
	{
		//Debug.Log(MapManager.Instance.FindWayMaps.Count);
		if (m_mapStates == MapStates.Showing)
		{
			m_currentMap++;
			if (m_currentMap >= MapManager.Instance.FindWayMaps.Count) m_currentMap = 0;
			Debug.Log(m_currentMap);
			m_camera.transform.position = new Vector3(MapManager.Instance.FindWayMaps[m_currentMap].transform.position.x, 10, MapManager.Instance.FindWayMaps[m_currentMap].transform.position.z);
			//Camera.main.transform.Translate(
			//	-MapManager.Instance.FindWayMaps[m_currentMap].transform.position.x + Camera.main.transform.position.x,0,
			//	-MapManager.Instance.FindWayMaps[m_currentMap].transform.position.z + Camera.main.transform.position.z);
		}
	}

	public void LastMap()
	{
		FindWay(st, ed);

		if (m_mapStates == MapStates.Showing)
		{
			m_currentMap--;
			if (m_currentMap < 0) m_currentMap = MapManager.Instance.FindWayMaps.Count-1;
			Debug.Log(m_currentMap);
			m_camera.transform.position = new Vector3(MapManager.Instance.FindWayMaps[m_currentMap].transform.position.x, 10, MapManager.Instance.FindWayMaps[m_currentMap].transform.position.z);
		}
	}

	public void BackToNone()
	{
		m_mapStates = MapStates.None;
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
