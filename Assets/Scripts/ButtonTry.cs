using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTry : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void OnButtonClick()
	{
		gameObject.SetActive(!gameObject.activeSelf);
	}

}
