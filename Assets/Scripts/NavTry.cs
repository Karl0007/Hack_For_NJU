using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavTry : MonoBehaviour
{
	public Transform tr;
	public NavMeshAgent nag;
	public LineRenderer lr;
	public NavMeshPath pa;

	// Start is called before the first frame update
	void Start()
    {
		pa = new NavMeshPath();
		nag = gameObject.GetComponent<NavMeshAgent>();
		lr = gameObject.GetComponent<LineRenderer>();
		Debug.Log(nag.gameObject.name);
    }

	public void GoDes()
	{
		nag.SetDestination(tr.position);
		nag.CalculatePath(tr.position, pa);
		Debug.Log(pa.corners.Length);
		var tmp = pa.corners;
		lr.positionCount = tmp.Length;
		lr.SetPositions(tmp);
		float distance=0;
		for (int i= 0; i<pa.corners.Length-1;i++)
		{
			distance += (pa.corners[i] - pa.corners[i + 1]).magnitude;
		}
		Debug.Log(distance);

	}

	// Update is called once per frame
	void Update()
    {
		//nag.SetDestination(tr.position);

		//nag.CalculatePath(tr.position, pa);
		//lr.SetPositions(pa.corners);

	}
}
