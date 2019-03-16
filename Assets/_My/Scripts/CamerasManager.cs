using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CameraTransitions;

public class CamerasManager : MonoBehaviour
{
	public static CamerasManager Instance;
	private void Awake()
	{
		if (!Instance)
		{
			Instance = this;
		}
	}

	public Camera[] m_cameras;
	public Canvas[] m_canvas;
	private Vector2 m_moveTo;
	private float m_moveTime;
	private CameraTransition m_transition;
	private CameraTransitionsAssistant m_transitionsAssistant;
	private Vector3 m_target;
	private Vector3 m_delta;
	private RaycastHit m_hit;


	public int m_curCamera;
	public GameObject m_text;

	// Start is called before the first frame update
	void Start()
    {
		m_transitionsAssistant = gameObject.GetComponent<CameraTransitionsAssistant>();
		m_transition = gameObject.GetComponent<CameraTransition>();
		m_canvas = FindObjectsOfType<Canvas>();
		m_transitionsAssistant.cameraA = m_cameras[m_curCamera];
		m_transitionsAssistant.cameraB = m_cameras[m_curCamera ^ 1];
		m_cameras[m_curCamera ^ 1].gameObject.SetActive(false);
		m_cameras[m_curCamera ^ 1].enabled = true;
		m_cameras[m_curCamera].enabled = true;
		Camera.SetupCurrent( m_cameras[m_curCamera^1]);
		m_target = new Vector3();
		//SetMainCamera(0);
		TransTo(new Vector2(115, 0));

	}

	public void MoveToPos(Vector2 _pos,float _time)
	{
		m_moveTime = _time;
		m_moveTo = (new Vector2(-m_cameras[m_curCamera].transform.position.x + _pos.x, -m_cameras[m_curCamera].transform.position.z + _pos.y)) / _time;
	}

	void CameraGoTO(Vector2 _pos,Camera cam)
	{
		Vector3 tmp = cam.transform.localPosition;
		//Debug.Log(tmp);
		tmp.x = _pos.x + .5f;
		tmp.y = -6.1693f;
		tmp.z = _pos.y - 8.5f;
		//Debug.Log(tmp);
		cam.transform.localPosition = tmp;
		m_target = new Vector3(_pos.x, 0, _pos.y);
	}

	public void TransTo(Vector2 _pos)
	{
		CameraGoTO(_pos,m_cameras[m_curCamera^1]);
		//Debug.Log(m_cameras[m_curCamera ^ 1].transform.localPosition);

		m_cameras[m_curCamera].fieldOfView = 60;
		m_delta = new Vector3(0,0,0);
		//m_cameras[m_curCamera^1].transform.position = tmp;


		m_transitionsAssistant.cameraA = m_cameras[m_curCamera];
		m_transitionsAssistant.cameraB = m_cameras[m_curCamera ^ 1];

		foreach (var can in m_canvas)
		{
			can.worldCamera = m_cameras[m_curCamera^1];
		}

		m_transitionsAssistant.ExecuteTransition();

		m_curCamera ^= 1;
	}


	public void SetMainCamera(int _x)
	{
		m_curCamera = 0;
		m_moveTime = 0;
		m_cameras[m_curCamera].enabled = true;
		m_cameras[m_curCamera^1].enabled = false;
		foreach (var can in m_canvas)
		{
			can.worldCamera = m_cameras[m_curCamera];
		}
	}

	// Update is called once per frame
	void Update()
    {
        if (m_moveTime > 0)
		{
			m_moveTime -= Time.deltaTime;
			Vector3 tmp = m_cameras[m_curCamera].transform.position;
			tmp.x += m_moveTo.x * Time.deltaTime;
			tmp.z += m_moveTo.y * Time.deltaTime;
			m_cameras[m_curCamera].transform.position = tmp;
		}
		if (!m_transitionsAssistant.IsRunning)
		{
			Ray tmp = m_cameras[m_curCamera].ScreenPointToRay(Input.mousePosition);
			Vector3 tmppos;
			Point near = null;
			if (Physics.Raycast(tmp, out m_hit))
			{
				tmppos = m_hit.point;
				m_cameras[m_curCamera].transform.LookAt((m_target + tmppos + m_delta) / 2);
				if (Input.GetAxis("Mouse ScrollWheel") != 0)
				{
					Vector3 look = m_target;
					look.x += (+Input.mousePosition.x - Screen.width / 2) / Screen.width * 5;
					look.z += (+Input.mousePosition.y - Screen.height / 2) / Screen.height * 10;
					Debug.Log((Input.mousePosition.x - Screen.width / 2) / Screen.width);
					if (Input.GetAxis("Mouse ScrollWheel") > 0 && m_cameras[m_curCamera].fieldOfView >= 7)
					{
						m_delta += (-m_target - m_delta + tmppos) * 0.1f;
						m_cameras[m_curCamera].fieldOfView -= 2;
					}
					if (Input.GetAxis("Mouse ScrollWheel") < 0 && m_cameras[m_curCamera].fieldOfView <= 70)
					{
						m_delta *= 0.8f;
						m_cameras[m_curCamera].fieldOfView += 2;
					}
				}

	
				foreach(var p in MapView.Instance.GetCurMap().m_points)
				{
					if (near == null || Vector3.Distance(near.transform.position,tmppos) > Vector3.Distance(p.transform.position, tmppos))
					{
						if (Vector3.Distance(p.transform.position, tmppos) < 1)
						{
							near = p;
						}
					}
				}
				//Debug.Log(near.m_pointName);
			}
			if (near == null)
			{
				m_text.SetActive(true);
				m_text.GetComponent<TextMesh>().text = MapView.Instance.GetCurMap().m_mapName.Split('+')[0].Split('-')[0];
				m_text.transform.position = MapView.Instance.GetCurMap().transform.position;

			}
			else
			{
				m_text.SetActive(true);
				m_text.GetComponent<TextMesh>().text =
					near.m_map.m_mapName.Split('+')[0].Split('-')[0] + "\n" +
					near.m_pointName.Split('+')[0].Split('-')[0];
				m_text.transform.position = near.transform.position;
			}

		}
	}
}
