using UnityEngine;
using System.Collections;

public class LevelScript : MonoBehaviour {
    private Color originalcolor;
    private Collider originalobject = null;
    private Collider grabobject = null;
    private Transform grabtransform;
    private Vector3 screenPoint;
    private Vector3 offset;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool rayhit = Physics.Raycast(ray, out hit);
        if (!rayhit || originalobject != hit.collider)
        {
            if (originalobject != null)
            {
                //Debug.Log ("setback");
                originalobject.renderer.material.color = originalcolor;
                originalobject = null;
            }
        }

        if (rayhit && originalobject == null)
        {
            originalobject = hit.collider;
            originalcolor = originalobject.renderer.material.color;
            originalobject.renderer.material.color = Color.yellow;
        }

        if (rayhit && hit.collider.CompareTag("Draggable") && Input.GetMouseButton(0))
        {
            if (grabobject == null)
            {
                grabobject = hit.collider;
                grabtransform = grabobject.transform;

                string [] highlightTypes = hit.collider.GetComponent<Draggable>().highlightTypes.Split(',');

                GameObject [] objects = GameObject.FindGameObjectsWithTag("DropTarget");
                for (int j = 0; j < objects.Length; j++) {
                    for (int i = 0; i < highlightTypes.Length; i++)
                    {
                        string highlightType = highlightTypes[i];
                        if (objects[j].GetComponent<DropTarget>().dropType.Equals(highlightType)) {
                            objects[j].renderer.material.color = Color.red;
                        }
                    }
                }

                screenPoint = Camera.main.WorldToScreenPoint(grabobject.transform.position);
                offset = grabobject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
                grabobject.collider.enabled = false;
            }

        }
        else if (grabobject != null && !Input.GetMouseButton(0))
        {
            if (rayhit && hit.collider.CompareTag("DropTarget"))
            {
                hit.collider.GetComponent<DropTarget>().Drop(grabobject);
            }

            string[] highlightTypes = grabobject.GetComponent<Draggable>().highlightTypes.Split(',');

            GameObject[] objects = GameObject.FindGameObjectsWithTag("DropTarget");
            for (int j = 0; j < objects.Length; j++)
            {
                for (int i = 0; i < highlightTypes.Length; i++)
                {
                    string highlightType = highlightTypes[i];
                    if (objects[j].GetComponent<DropTarget>().dropType.Equals(highlightType))
                    {
                        objects[j].renderer.material.color = originalcolor;
                    }
                }
            }

            grabobject.collider.enabled = true;
            grabobject = null;
        }

        if (grabobject != null)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            grabobject.transform.position = curPosition;
        }

	}
}
