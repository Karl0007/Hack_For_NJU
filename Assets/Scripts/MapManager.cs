using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
	class Edge : IComparer<Edge>
	{
		public Point To;
		public float Distance;
		public Edge() { }
		public Edge(Point to,float dis)
		{
			To = to;
			Distance = dis;
		}

		public int Compare(Edge x, Edge y)
		{
			return x.Distance.CompareTo(y.Distance);
			//throw new NotImplementedException();
		}


		public static bool operator <(Edge l, Edge r)
		{
			return (l.Distance < r.Distance);
		}
		public static bool operator >(Edge l, Edge r)
		{
			return (l.Distance > r.Distance);
		}

	}

	static public MapManager Instance;
	private void Awake()
	{
		if(!Instance)
		{
			Instance = this;
		}
	}

	Map[] m_maps;
	Point[] m_points;

	Dictionary<string, Map> m_mapDic;
	Dictionary<string, Point> m_pointDic;
	Dictionary<Point, List<Edge>> m_graph;
	
	SortedSet<Edge> FindWayQueue;
	Dictionary<Point,float> FindWayDis;
	Dictionary<Point, bool> FindWayVis;
	Dictionary<Point, Point> FindWayFrom;

	public List<Map> FindWayMaps;
	public List<Point> FindWayPoints;


    // Start is called before the first frame update
    void Start()
    {
		
		m_maps = FindObjectsOfType<Map>();
		m_points = FindObjectsOfType<Point>();
		m_graph = new Dictionary<Point, List<Edge>>();
		FindWayQueue = new SortedSet<Edge>(new Edge());
		FindWayMaps = new List<Map>();
		FindWayPoints = new List<Point>();
		FindWayDis = new Dictionary<Point, float>();
		FindWayVis = new Dictionary<Point, bool>();
		FindWayFrom = new Dictionary<Point, Point>();

		///<summary>
		///获取所有的点
		///连边建图
		/// </summary>
		foreach (Point p in m_points)
		{
			m_graph.Add(p, new List<Edge>());
		}
		foreach (Point p in m_points)
		{
			foreach (Point op in p.m_otherMapPoint)
			{
				m_graph[p].Add(new Edge(op,0));
			}
			if (p.m_otherMapPoint.Length != 0)
			{
				foreach (Point mp in p.m_map.m_points)
				{
					float dis = p.m_map.m_nav.CalDis(p.transform, mp.transform);
					m_graph[p].Add(new Edge(mp,dis));
					m_graph[mp].Add(new Edge(p, dis));
				}
			}
		}

	}

	public void FindWay(Point _st,Point _ed)
	{
		FindWayPoints.Clear();
		FindWayMaps.Clear();
		FindWayQueue.Clear();
		FindWayDis.Clear();
		FindWayFrom.Clear();
		FindWayVis.Clear();

		if (_st.m_map == _ed.m_map)
		{
			FindWayMaps.Add(_st.m_map);
			FindWayPoints.Add(_st);
			FindWayPoints.Add(_ed);
			_st.m_map.m_nav.DrawLine(_st.transform, _ed.transform);
			return;
		}

		foreach (var p in m_points)
		{
			FindWayDis.Add(p, 99999);
			FindWayVis.Add(p, false);
		}
		FindWayDis[_st] = 0;
		//FindWayVis[_st] = true;
		FindWayQueue.Add(new Edge(_st,0));

		//Debug.Log("step1");

		while(FindWayQueue.Count != 0)
		{
			int i = 0;
			var tmp = FindWayQueue.Min;
			FindWayQueue.Remove(FindWayQueue.Min);
			if (FindWayVis[tmp.To]) continue;
			FindWayVis[tmp.To] = true;
			foreach (var p in m_graph[tmp.To])
			{
				//Debug.Log(tmp.To.m_pointName +"--1323-->" + p.To.m_pointName +"  " +FindWayDis[p.To]+" "+FindWayDis[tmp.To]+" "+p.Distance + (FindWayDis[p.To] > FindWayDis[tmp.To] + p.Distance? "T" : "F"));
				if (FindWayDis[p.To] > FindWayDis[tmp.To] + p.Distance)
				{
					FindWayDis[p.To] = FindWayDis[tmp.To] + p.Distance;
					FindWayQueue.Add(new Edge(p.To, FindWayDis[p.To]));
					FindWayFrom[p.To] = tmp.To;
				}
				//Debug.Log(tmp.To.m_pointName + "--1323-->" + p.To.m_pointName + "  " + FindWayDis[p.To]);
				//Debug.Log(FindWayQueue.Min.To.m_pointName,FindWayQueue.Min.To);
			}
		}


		Point point = _ed;
		FindWayPoints.Add(_ed);
		//FindWayMaps.Add(_ed.m_map);
		while (FindWayFrom.ContainsKey(point))
		{
			//Debug.Log(point.name+" "+point.m_pointName);
			FindWayPoints.Add(FindWayFrom[point]);
			if (FindWayFrom[point].m_map == point.m_map)
			{
				//if (!FindWayMaps.Exists((x)=>x.m_mapName == point.m_map.m_mapName))
					FindWayMaps.Add(FindWayFrom[point].m_map);
				point.m_map.m_nav.DrawLine(FindWayFrom[point].transform, point.transform);
			}
			point = FindWayFrom[point];
		}
		FindWayMaps.Reverse();
		FindWayPoints.Reverse();
		//Debug.Log(FindWayPoints);
		//Debug.Log("MAPs"+FindWayMaps.Count);
		foreach (var p in FindWayMaps)
		{
			Debug.Log(p.name);
		}
		

	}

    // Update is called once per frame
    void Update()
    {
		//OnClick();
	}
}
