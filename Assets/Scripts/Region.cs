using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class Region : MonoBehaviour
{

    public List<DoorController> doors;
    public List<Region> neighbors;

    private NavMeshPath path;

    public static Region FindRegion(Transform transform)
    {        
        foreach (Region reg in GameObject.FindObjectsOfType<Region>())
        {            
            if (reg.InRegion(transform))
            {
                return reg;
            }
        }
        return null;
    }

    public bool InRegion(Transform target)
    {
        bool result =  (NavMesh.CalculatePath(transform.position, target.position, int.MaxValue & (~(DoorController.PATH_MASK)), path));

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

    private Region GetDoorTarget(DoorController door)
    {
        return door.region0 == this ? door.region1 : door.region0;
    }

    public DoorController DoorToRegion(Region region)
    {
        var door = doors.Find((d) => d.region0 == region || d.region1 == region);
        return door;
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
