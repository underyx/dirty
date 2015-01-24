using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class Region : MonoBehaviour
{

    public List<Door> doors;
    private NavMeshPath path;

    public bool InRegion(Transform target)
    {
        return (NavMesh.CalculatePath(transform.position, target.position, int.MaxValue, path));
    }

    private void Awake()
    {
     path = new NavMeshPath();
     RaycastHit hit;
     Physics.Raycast(new Ray(transform.position, Vector3.down), out hit);
     transform.position = hit.point;
    }

    private bool HasDoor(Door door)
    {
        return this == door.region0 || this == door.region1;
    }

    // Use this for initialization
	void Start () {
        doors = new List<Door>();




	    foreach (var go in GameObject.FindObjectsOfType<Door>())
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
