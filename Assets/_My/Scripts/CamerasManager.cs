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

	public int m_curCamera;


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

		//SetMainCamera(0);

	}

	public void MoveToPos(Vector2 _pos,float _time)
	{
		m_moveTime = _time;
		m_moveTo = (new Vector2(-m_cameras[m_curCamera].transform.position.x + _pos.x, -m_cameras[m_curCamera].transform.position.z + _pos.y)) / _time;
	}

	public void TransTo(Vector2 _pos)
	{
		Vector3 tmp = m_cameras[m_curCamera^1].transform.localPosition;
		Debug.Log(tmp);
		tmp.x = _pos.x+.5f;
		tmp.y = -6.1693f;
		tmp.z = _pos.y-8.5f;
		Debug.Log(tmp);
		m_cameras[m_curCamera^1].transform.localPosition = tmp;
		Debug.Log(m_cameras[m_curCamera ^ 1].transform.localPosition);
		//m_cameras[m_curCamera ^ 1].transform.LookAt(new Vector3(_pos.x,0,_pos.y));

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
	}
}
