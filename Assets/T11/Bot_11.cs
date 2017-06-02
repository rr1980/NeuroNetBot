using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot_11 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnDrawGizmos()
	{
		DebugExtension.DrawArrow(transform.position, transform.forward * 2);
	}
}
