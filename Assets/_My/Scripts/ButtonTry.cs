using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTry : MonoBehaviour
{
	public GameObject _obj;
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
		//gameObject.SetActive(!gameObject.activeSelf);
		var tmp = _obj.AddComponent<UIMove>();
		tmp.GoToAndShow(new Vector2(0,-50),1f);
		tmp = gameObject.AddComponent<UIMove>();
		tmp.GoToAndFade(new Vector2(0, -50), 1f);
	}

}
