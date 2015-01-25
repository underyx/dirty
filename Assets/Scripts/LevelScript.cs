using UnityEngine;
using System.Collections;

public class LevelScript : MonoBehaviour {
    private Color originalcolor;
    private Collider originalobject = null;
    private Collider grabobject = null;
    private Transform grabtransform;
    private Vector3 screenPoint;
    private Vector3 offset;

    private Conveyor conveyor;

	// Use this for initialization
	void Start ()
	{
	    conveyor = GameObject.FindObjectOfType<Conveyor>();
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

		if (rayhit && originalobject == null && hit.collider.CompareTag("Draggable"))
        {
            originalobject = hit.collider;
            originalcolor = originalobject.renderer.material.color;
            originalobject.renderer.material.color = Color.yellow;
        }

        if (rayhit && hit.collider.CompareTag("Draggable") && Input.GetMouseButtonDown(0))
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
                            objects[j].renderer.material.color = Color.blue;
                        }
                    }
                }

                screenPoint = Camera.main.WorldToScreenPoint(grabobject.transform.position);
                offset = grabobject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
				grabobject.collider.enabled = false;
                grabobject.GetComponent<Draggable>().BeingDragged = true;
            }

        }
        else if (grabobject != null && Input.GetMouseButtonUp(0))
        {
            if (rayhit && hit.collider.CompareTag("DropTarget"))
            {
                bool result = hit.collider.GetComponent<DropTarget>().Drop(grabobject);

                if (result)
                {
                    conveyor.ItemRemove(grabobject.GetComponent<Draggable>());
                }
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
                        objects[j].renderer.material.color = Color.red; // FIXME
                    }
                }
            }

            grabobject.collider.enabled = true;
            grabobject.GetComponent<Draggable>().BeingDragged = false;
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
