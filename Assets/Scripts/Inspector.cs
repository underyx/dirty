using UnityEngine;
using System.Collections;

public class Inspector : MonoBehaviour {
	private GameObject target = null;
	private float turnSpeed = 0.2f;
	private Color originalcolor;
	private Collider originalobject = null;
	private Collider grabobject = null;
	private Transform grabtransform;
	private Vector3 screenPoint;
	private Vector3 offset;
	
	void Start () {
		target = GameObject.Find("target");
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		bool rayhit = Physics.Raycast (ray, out hit);
		if (!rayhit || originalobject != hit.collider) {
			if(originalobject != null) {
				//Debug.Log ("setback");
				originalobject.renderer.material.color = originalcolor;
				originalobject = null;
			}
		}
		
		if(rayhit && originalobject == null) {
			originalobject = hit.collider;
			originalcolor = originalobject.renderer.material.color;
			originalobject.renderer.material.color = Color.yellow;
		}

		if(rayhit && hit.collider.name == "block" && Input.GetMouseButton(0)) {
			if(grabobject == null) {
				grabobject = hit.collider;
				grabtransform = grabobject.transform;
				GameObject.Find("blocktarget").renderer.material.color = Color.red;
				screenPoint = Camera.main.WorldToScreenPoint(grabobject.transform.position);
				offset = grabobject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
				grabobject.collider.enabled = false;
			}
			
		} else if(grabobject != null && !Input.GetMouseButton(0)) {
			if(rayhit && hit.collider.name == "blocktarget") {
				hit.collider.GetComponent<droptarget>().Drop(grabobject);
			}

			GameObject.Find("blocktarget").renderer.material.color = originalcolor;
			grabobject.collider.enabled = true;
			grabobject = null;
		}
		
		if(grabobject != null) {
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
			grabobject.transform.position = curPosition;
		}

		if (target == null)
			return;
		if ((target.transform.localPosition - transform.localPosition).magnitude < 1.0f) {
			target = GameObject.Find("target2");
		}

		var targetDir = target.transform.position - transform.position;
		var newDir = Vector3.RotateTowards(transform.forward, targetDir, turnSpeed * Time.deltaTime, 0.0f);

		//Debug.Log (Vector3.Angle (targetDir, newDir));
		if (transform.rotation != Quaternion.LookRotation(newDir)) {
			transform.rotation = Quaternion.LookRotation(newDir);
			return;
		}
		transform.rotation = Quaternion.LookRotation(targetDir);
		transform.Translate (Vector3.forward * Time.deltaTime);
	}

	void OnGUI()
	{
		//Debug.Log ("GUI");
		//GUI.DrawTexture( position, crosshairTexture );
	}
}
