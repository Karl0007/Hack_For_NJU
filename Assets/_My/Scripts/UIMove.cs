using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMove : MonoBehaviour
{
	private Image[] m_img;
	private Text[] m_text;
	private RectTransform m_tran;
	private float m_deltacolor;
	private float m_time;
	private Vector2 m_speed;

	public void GoToAndFade(Vector2 _pos,float _time)
	{
		m_tran = gameObject.GetComponent<RectTransform>();
		m_img = (gameObject.GetComponentsInChildren<Image>());
		m_text = (gameObject.GetComponentsInChildren<Text>());
		m_deltacolor = -1.0f/_time;
		m_time = _time;
		m_speed = (_pos-m_tran.anchoredPosition)/_time;
	}

	public void GoToAndShow(Vector2 _pos, float _time)
	{
		m_tran = gameObject.GetComponent<RectTransform>();
		m_img = (gameObject.GetComponentsInChildren<Image>());
		m_text = (gameObject.GetComponentsInChildren<Text>());
		m_deltacolor = 1.0f / _time;
		m_time = _time;
		m_speed = (_pos-m_tran.anchoredPosition) / _time;
	}

	public void GoTo(Vector2 _pos,float _col)
	{
		m_tran = gameObject.GetComponent<RectTransform>();
		m_img = (gameObject.GetComponentsInChildren<Image>());
		m_text = (gameObject.GetComponentsInChildren<Text>());
		m_tran.anchoredPosition += _pos;
		Color tmp;
		foreach (var x in m_img)
		{
			tmp=x.color;
			tmp.a = _col;
			x.color = tmp;
		}

		foreach (var x in m_text)
		{
			tmp = x.color;
			tmp.a = _col;
			x.color = tmp;
		}
		Destroy(this);
	}

	// Start is called before the first frame update
	void Start()
    {
		m_tran = gameObject.GetComponent<RectTransform>();
		m_img = (gameObject.GetComponentsInChildren<Image>());
		m_text = (gameObject.GetComponentsInChildren<Text>());
	}

	// Update is called once per frame
	void Update()
    {
		if (m_time > 0)
		{
			m_time -= Time.deltaTime;
			m_tran.anchoredPosition += m_speed * Time.deltaTime;
			Color tmp;
			foreach (var x in m_img)
			{
				tmp = x.color;
				tmp.a += m_deltacolor * Time.deltaTime;
				x.color = tmp;
			}

			foreach (var x in m_text)
			{
				tmp = x.color;
				tmp.a += m_deltacolor * Time.deltaTime;
				x.color = tmp;
			}
		}
		else
		{
			if (m_deltacolor < 0) this.gameObject.SetActive(false);
			Destroy(this);
		}
	}
}
