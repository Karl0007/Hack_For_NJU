using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public GameObject[] m_objs;
	public GameObject m_back;
	private int m_curState;
	private bool m_cureff;
	public InputField m_curPos;
	public InputField m_stPos;
	public InputField m_edPos;
	

	public void StartFindWayClick()
	{
		m_curState = 1;
		m_objs[1].SetActive(true);
		m_objs[0].AddComponent<UIMove>().GoToAndFade(new Vector2(0,-10),0.4f);
		m_objs[1].AddComponent<UIMove>().GoToAndShow(new Vector2(0,0),0.4f);
		BackShow();
		if (m_cureff)
		{
			m_stPos.text = m_curPos.text;
		}

	}

	public void SetCurPosClick()
	{
		m_curState = 2;
		m_objs[2].SetActive(true);
		m_objs[0].AddComponent<UIMove>().GoToAndFade(new Vector2(0, -10), 0.4f);
		m_objs[2].AddComponent<UIMove>().GoToAndShow(new Vector2(0, 0), 0.4f);
		BackShow();
	}

	void BackShow()
	{
		m_back.AddComponent<UIMove>().GoToAndShow(new Vector2(0, 0), 0.4f);
		m_back.SetActive(true);
	}

	void BackFade()
	{
		m_back.AddComponent<UIMove>().GoToAndFade(new Vector2(0, 0), 0.4f);
	}

	public void BackClick()
	{
		if (m_curState == 1)
		{
			m_curState = 0;
			m_objs[0].SetActive(true);
			m_objs[0].AddComponent<UIMove>().GoToAndShow(new Vector2(0, 0), 0.4f);
			m_objs[1].AddComponent<UIMove>().GoToAndFade(new Vector2(-10, 0), 0.4f);
			BackFade();
		}
		if (m_curState == 2)
		{
			m_curState = 0;
			m_objs[0].SetActive(true);
			m_objs[0].AddComponent<UIMove>().GoToAndShow(new Vector2(0, 0), 0.4f);
			m_objs[2].AddComponent<UIMove>().GoToAndFade(new Vector2(10, 0), 0.4f);
			BackFade();
		}
		if (m_curState == 3)
		{
			m_curState = 0;
			m_objs[0].SetActive(true);
			m_objs[0].AddComponent<UIMove>().GoToAndShow(new Vector2(0, 0), 0.4f);
			m_objs[3].AddComponent<UIMove>().GoToAndFade(new Vector2(0, -10), 0.4f);
			MapView.Instance.BackToNone();
			BackFade();
		}

	}
	
	public HashSet<Point> FindPointsByString(string _s)
	{
		var tmp = MapManager.Instance.GetMapFromString(_s, out string ot);
		//Debug.Log(ot);
		return MapManager.Instance.GetPointFromString(ot,tmp);
	}

	public void SetCurPosOKClick()
	{
		var p = FindPointsByString(m_curPos.text);
		if (p == null || p.Count == 0)
		{
			m_curPos.text = "地点不存在";
			m_cureff = false;
			return;
		}
		if (p.Count > 1)
		{
			m_curPos.text = "地点不唯一";
			m_cureff = false;
			return;
		}
		m_cureff = true;
		MapManager.Instance.FindWayMaps.Clear();
		foreach (var np in p)
		{
			MapManager.Instance.FindWayMaps.Add(np.m_map);
			MapView.Instance.NextMap();
		}

		BackClick();
	}

	public void FindWayGOClick()
	{
		var st = FindPointsByString(m_stPos.text);
		var ed = FindPointsByString(m_edPos.text);
		if (st == null || st.Count == 0)
		{
			m_stPos.text = "起点不存在";
			return;
		}
		if (st.Count > 1)
		{
			m_stPos.text = "起点不唯一";
			return;
		}
		if (ed == null || ed.Count == 0)
		{
			m_edPos.text = "终点不存在";
			return;
		}
		foreach (var tmp in st)
		{
			m_edPos.text = MapView.Instance.FindWay(tmp, ed);
		}
		Debug.Log(233);
		m_curState = 3;
		m_objs[3].SetActive(true);
		m_objs[1].AddComponent<UIMove>().GoToAndFade(new Vector2(-10, 0), 0.4f);
		m_objs[3].AddComponent<UIMove>().GoToAndShow(new Vector2(0, 0), 0.4f);
		BackShow();
		MapView.Instance.CurMap();
	}

	public void GoNextMap()
	{
		MapView.Instance.NextMap();
	}

	public void GoLastMap()
	{
		MapView.Instance.LastMap();
	}

	public void FindWayDone()
	{
		m_curPos.text = m_edPos.text;
		m_edPos.text = "";
		BackClick();
	}
	void Start()
    {
		m_curState = 0;
		m_cureff = false;
		m_back.AddComponent<UIMove>().GoTo(new Vector2(0, 0), 0);
		m_objs[0].AddComponent<UIMove>().GoTo(new Vector2(0, 0), 1);
		m_objs[1].AddComponent<UIMove>().GoTo(new Vector2(-10, 0), 0);
		m_objs[2].AddComponent<UIMove>().GoTo(new Vector2(10, 0), 0);
		m_objs[3].AddComponent<UIMove>().GoTo(new Vector2(0, -10), 0);

		m_objs[0].SetActive(true);
		m_objs[1].SetActive(false);
		m_objs[2].SetActive(false);
		m_objs[3].SetActive(false);
		m_back.SetActive(false);

	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
