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

	Dictionary<string, HashSet<Map>> m_mapDic;
	Dictionary<string, HashSet<Point>> m_pointDic;
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

		m_pointDic = new Dictionary<string, HashSet<Point>>();
		m_mapDic = new Dictionary<string, HashSet<Map>>();
		foreach(var m in m_maps)
		{
			var ps = m.GetComponentsInChildren<Point>();
			List<Point> tmp = new List<Point>();
			foreach (var p in ps)
			{
				//if (p.m_otherMapPoint.Length == 0)
					tmp.Add(p);
			}
			m.m_points = tmp.ToArray();
		}
		Debug.Log("edge ok");
		///<summary>
		///获取所有的点
		///连边建图
		/// </summary>
		foreach (Point p in m_points)
		{
			m_graph.Add(p, new List<Edge>());
		}
		Debug.Log(m_points.Length);
		foreach (Point p in m_points)
		{
			foreach (Point op in p.m_otherMapPoint)
			{
				if (op == null) continue;
				m_graph[p].Add(new Edge(op,1));
				m_graph[op].Add(new Edge(p, 1));
			}
			if (p.m_otherMapPoint.Length != 0)
			{
				foreach (Point mp in p.m_map.m_points)
				{
					float dis = p.m_map.m_nav.CalDis(p.transform, mp.transform);
					m_graph[p].Add(new Edge(mp,dis));
					m_graph[mp].Add(new Edge(p, dis));
					if (mp.m_pointName == "休息区") Debug.Log("休息区------"+p.m_pointName+" "+dis);
				}
			}
		}
		Debug.Log("edge ok");
		///<summary>
		///提取字符串中的信息
		/// </summary>
		foreach(var m in m_maps)
		{
			string[] tmp0 = m.m_mapName.Split('+');
			foreach (var s in tmp0)
			{
				string tmp1 = s.Split('-')[0];
				if (!m_mapDic.ContainsKey(tmp1))
				{
					m_mapDic.Add(tmp1, new HashSet<Map>());
				}
				m_mapDic[tmp1].Add(m);
			}
		}
		foreach(var p in m_points)
		{
			string[] tmp0 = p.m_pointName.Split('+');
			foreach (var s in tmp0)
			{
				string tmp1 = s.Split('-')[0];
				if (!m_pointDic.ContainsKey(tmp1))
				{
					m_pointDic.Add(tmp1, new HashSet<Point>());
				}
				m_pointDic[tmp1].Add(p);
			}
		}
	}

	public HashSet<Map> GetMapFromString(string _s,out string _mapname)
	{
		Debug.Log(m_mapDic.Count);
		foreach (var m in m_mapDic)
		{
			//Debug.Log(m.Key);
			if (_s.Contains(m.Key))
			{
				_mapname = _s.Replace(m.Key,"");
				return m.Value;
			}
		}
		_mapname = _s;
		return null;
	}

	public HashSet<Point> GetPointFromString(string _s,HashSet<Map> _m)
	{
		//Debug.Log(_s);
		HashSet<Point> res = new HashSet<Point>();
		foreach(var p in m_pointDic)
		{
			if ((_s.Contains(p.Key)))
			{
				foreach(var sp in p.Value)
				{
					if (_m == null || _m.Count == 0 || _m.Contains(sp.m_map))
					{
						res.Add(sp);
					}
				}
			}
		}
		return res;
	}



	public string FindWay(Point _st,HashSet<Point> _eds)
	{
		FindWayPoints.Clear();
		FindWayMaps.Clear();
		FindWayQueue.Clear();
		FindWayDis.Clear();
		FindWayFrom.Clear();
		FindWayVis.Clear();

		if (_eds.Contains(_st)) _eds.Remove(_st);

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

		foreach (var _ed in _eds)
		{
			if (_st.m_map == _ed.m_map)
			{
				//FindWayMaps.Add(_st.m_map);
				//FindWayPoints.Add(_st);
				//FindWayPoints.Add(_ed);
				//_st.m_map.m_nav.DrawLine(_st.transform, _ed.transform);
				//return;

				FindWayDis[_ed] = _st.m_map.m_nav.CalDis(_st.transform,_ed.transform);
				FindWayFrom[_ed] = _st;
			}
		}


		Point point = null;
		foreach(var p in _eds)
		{
			if (point==null || FindWayDis[point] > FindWayDis[p])
			{
				point = p;
			}
		}
		string res_ed = point.m_map.m_mapName.Split('+')[0].Split('-')[0] + point.m_pointName.Split('+')[0].Split('-')[0];
		FindWayPoints.Add(point);
		//FindWayMaps.Add(_ed.m_map);
		while (FindWayFrom.ContainsKey(point))
		{
			Debug.Log(point.m_pointName + "-->" + FindWayFrom[point].m_pointName);
			FindWayPoints.Add(FindWayFrom[point]);
			if (FindWayFrom[point].m_map == point.m_map)
			{
				if (!FindWayMaps.Exists((x)=>x.m_mapName == point.m_map.m_mapName))
					FindWayMaps.Add(FindWayFrom[point].m_map);
				point.m_map.m_nav.DrawLine(FindWayFrom[point].transform, point.transform);
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

		return res_ed;
	}

    // Update is called once per frame
    void Update()
    {
		//OnClick();
	}
}
