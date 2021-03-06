﻿using UnityEngine;
using System.Collections;

public class DoorOpener : MonoBehaviour
{

    private DoorController _doorController;
	// Use this for initialization
	void Start ()
	{
	    _doorController = GetComponent<DoorController>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Space))
	    {
	        _doorController.Open = !_doorController.Open;
	    }
	}
}
