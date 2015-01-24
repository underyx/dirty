using UnityEngine;
using System.Collections;

public class droptarget : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Drop(Collider dragObject) {
		Debug.Log (dragObject.name);
	}
}
