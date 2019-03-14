using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavManager : MonoBehaviour
{
	private LineRenderer m_lineRender;
	private NavMeshAgent m_navMeshAgent;
	private NavMeshPath m_path;
	// Start is called before the first frame update
	private void Awake()
	{
		m_path = new NavMeshPath();
		m_lineRender = gameObject.GetComponent<LineRenderer>();
		m_navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
		//Debug.Log(m_navMeshAgent, gameObject);
	}

	void Start()
    {
    }

	public void DrawLine(Transform _st,Transform _ed)
	{
		gameObject.transform.position = _st.position;
		m_navMeshAgent.CalculatePath(_ed.position, m_path);
		m_lineRender.positionCount = m_path.corners.Length;
		m_lineRender.SetPositions(m_path.corners);
	}

	public float CalDis(Transform _st,Transform _ed)
	{
		gameObject.transform.position = _st.position;
		Debug.Log(m_navMeshAgent,gameObject);
		m_navMeshAgent.CalculatePath(_ed.position, m_path);
		float res = 0;
		for (int i = 0; i < m_path.corners.Length-1; i++)
		{
			res += (m_path.corners[i + 1] - m_path.corners[i]).magnitude;
		}
		return res;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
