using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavTry : MonoBehaviour
{
	public Transform tr;
	public NavMeshAgent nag;
    // Start is called before the first frame update
    void Start()
    {
		nag = gameObject.GetComponent<NavMeshAgent>();
		Debug.Log(nag.gameObject.name);
    }

	public void GoDes()
	{
		nag.SetDestination(tr.position);

	}

	// Update is called once per frame
	void Update()
    {
    }
}
