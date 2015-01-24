using UnityEngine;
using System.Collections;

public class SampleAgent : MonoBehaviour
{

    private Camera mainCamera;
    private Vector3 targetPos;
    private NavMeshAgent agent;
    
	// Use this for initialization
	void Start () {
        mainCamera = Camera.main;
	    agent = this.GetComponent<NavMeshAgent>();
	    agent.autoTraverseOffMeshLink = false;
	}

    private Vector3 clickPos;
	// Update is called once per frame
	void Update ()
	{
	    
	    if (Input.GetMouseButtonDown(0))
	    {
	        RaycastHit info;
            bool result = Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out info);
	        if (!result)
	            return;
            clickPos = info.point;
            if(Vector3.Distance(clickPos,transform.position) > 0.02)
            {
                agent.SetDestination(clickPos);
            }
	    }


	}

    public void OnDrawGizmos()
    {
        var oldColor = Gizmos.color;
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(clickPos,0.3f);
        Gizmos.color = oldColor;
    }
}
