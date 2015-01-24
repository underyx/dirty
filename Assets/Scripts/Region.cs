using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class Region : MonoBehaviour
{

    public List<DoorController> doors;
    private NavMeshPath path;

    public bool InRegion(Transform target)
    {
        bool result =  (NavMesh.CalculatePath(transform.position, target.position, int.MaxValue, path));

        return path.status == NavMeshPathStatus.PathComplete;
    }

    private void Awake()
    {
     path = new NavMeshPath();
     RaycastHit hit;
     Physics.Raycast(new Ray(transform.position, Vector3.down), out hit);
     transform.position = hit.point+Vector3.up*0.05f;
    }

    private bool HasDoor(DoorController _doorController)
    {
        return this == _doorController.region0 || this == _doorController.region1;
    }

    // Use this for initialization
	void Start () {
        doors = new List<DoorController>();




	    foreach (var go in GameObject.FindObjectsOfType<DoorController>())
	    {
	        if (this.HasDoor(go))
	        {
	            doors.Add(go);
	        }
	    }	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    [ExecuteInEditMode]
    public void OnDrawGizmos()
    {
        RaycastHit hit;
        Vector3 downvec = Physics.Raycast(new Ray(transform.position, Vector3.down), out hit)?hit.point:Vector3.down * 1000.0f;
        var oldColor = Gizmos.color;
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(transform.position, 0.3f);
        Gizmos.DrawLine(transform.position, downvec);
        Gizmos.color = oldColor;
        
    }
}
