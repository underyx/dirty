using UnityEngine;
using System.Collections;
[RequireComponent(typeof(NavMeshAgent))]
public class SampleAgent : MonoBehaviour
{

    private Camera mainCamera;    
    private NavMeshAgent agent;
    private bool passingDoor;
    private bool moveFinished;

    public Region CurrentRegion;


    public bool MoveFinished
    {
        get { return moveFinished; }
    }

    public bool enableMouse = true;

	// Use this for initialization
	void Start () {
        mainCamera = Camera.main;
	    agent = this.GetComponent<NavMeshAgent>();
	    agent.autoTraverseOffMeshLink = false;
	    agent.walkableMask = int.MaxValue;
        passingDoor = false;
	    moveFinished = true;
	    CurrentRegion = Region.FindRegion(transform);
	}

    public Vector3 targetPos;
	// Update is called once per frame
	public void Update ()
	{
        if (enableMouse && Input.GetMouseButtonDown(0))
	    {
	        RaycastHit info;
	        if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out info))
	        {
	            targetPos = info.point;
	            GoToPoint(targetPos);
	        }
	    }
	    if (agent.isOnOffMeshLink && !passingDoor)
	    {
	        StartCoroutine(PassDoor());
	        passingDoor = true;
	    }

        if (CloseToTarget() && !passingDoor)
	    {
            moveFinished = true;
	    }

		foreach (GameObject penalizer in GameObject.FindGameObjectsWithTag("Penalizer"))
		{            
			if (this.CurrentRegion == Region.FindRegion(penalizer.transform))
			{
				HealthDisplay healthdisplay = GameObject.Find("HealthDisplay").GetComponent<HealthDisplay>();;
				healthdisplay.ChangeHealth(-1);
			}
		}
	}

    bool CloseToTarget()
    {
        var tpos = transform.position;
        float xdiff = targetPos.x - tpos.x;
        float zdiff = targetPos.z - tpos.z;
        return (Mathf.Sqrt(xdiff*xdiff + zdiff*zdiff) < 0.02);
    }

    public void GoToPoint(Vector3 pos)
    {
        targetPos = pos;
        var tpos = transform.position;
        float xdiff = pos.x - tpos.x;
        float zdiff = pos.z - tpos.z;

        if (!CloseToTarget())
        {
            agent.SetDestination(pos);
            moveFinished = false;
        }        
    }


    public IEnumerator PassDoor()
    {
        Debug.Log("On meshlink:" + agent.currentOffMeshLinkData.offMeshLink.name);        
        DoorController controller = agent.currentOffMeshLinkData.offMeshLink.GetComponent<DoorController>();
        controller.OpenAwayFrom(transform);
        controller.Open = true;
        yield return new WaitForSeconds(1.0f);
        Debug.Log("Door open");

        Vector3 targetPos = agent.currentOffMeshLinkData.endPos;
        targetPos.y = transform.position.y;
        while (Vector3.Distance(transform.position,targetPos) > 0.02f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime*agent.speed);
            yield return new WaitForEndOfFrame();
        }
        
        agent.CompleteOffMeshLink();        
        yield return new WaitForSeconds(1.0f);
        controller.Open = false;
        Debug.Log("Door closed");
        passingDoor = false;
        //The next current region is the one we are not in
        CurrentRegion = controller.region0 == CurrentRegion ? controller.region1 : controller.region0;
    }

    public void OnDrawGizmos()
    {
        var oldColor = Gizmos.color;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,targetPos);
        Gizmos.DrawSphere(targetPos,0.3f);
        Gizmos.color = oldColor;
    }
}
