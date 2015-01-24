using UnityEngine;
using System.Collections;

public class DoorOpener : MonoBehaviour
{

    private Door door;
	// Use this for initialization
	void Start ()
	{
	    door = GetComponent<Door>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Space))
	    {
	        door.Open = !door.Open;
	    }
	}
}
