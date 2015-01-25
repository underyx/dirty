using System;
using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    public bool open;
    private OffMeshLink link;
    private Transform doorLink0;
    private Transform doorLink1;
    private Transform doorFrame;

    public const int PATH_LAYER = 3;
    public const int PATH_MASK = 1 << PATH_LAYER;
    public Region region0;
    public Region region1;

    private Animator doorAnimator;

    public void Awake()
    {
    //    Debug.Log("Door controller awake");
        RaycastHit hit;
        Physics.Raycast(new Ray(transform.position, Vector3.down), out hit);
      //  transform.position = hit.point + Vector3.up * 0.05f;
        doorLink0 = transform.FindChild("DoorLink0");
        doorLink1 = transform.FindChild("DoorLink1");

        var pos0 = doorLink0.position;        
        var pos1 = doorLink1.position;

        pos0.y = hit.point.y + 0.05f;
        pos1.y = hit.point.y + 0.05f;

        doorLink0.position = pos0;
        doorLink1.position = pos1;

        link = GetComponent<OffMeshLink>();

        link.navMeshLayer = PATH_LAYER;
        link.activated = true;
    }

    // Use this for initialization
	void Start ()
	{
       // DoorManager manager = GameObject.Find("DoorController Manager").GetComponent<DoorManager>();
	    doorFrame = transform.FindChild("FrameMaster");
	    doorFrame.transform.GetChild(0).collider.isTrigger = true;
        doorAnimator = doorFrame.GetComponent<Animator>();
	    var regions = GameObject.FindObjectsOfType<Region>();
	    region0 = region1 = null;

	    region0 = Region.FindRegion(doorLink0.transform);
        region1 = Region.FindRegion(doorLink1.transform);

	}

    

    public bool Open
    {
        get { return open; }
        set
        {
            open = value;
          //  link.activated = value;
        }
    }

    public bool Locked;

    public float AnimDuration = 0.5f;

    private float animState = 0;
    private float OpenDirection;

    public void OpenAwayFrom(Transform target)
    {
        var localvec = transform.InverseTransformPoint(target.position);

        OpenDirection = -Math.Sign(localvec.x);
    }

    // Update is called once per frame
	void Update ()
	{
	    link.activated = !Locked;

        animState += (Open ? Time.deltaTime : -Time.deltaTime) / AnimDuration;
        animState= Mathf.Clamp01(animState);
     //   Debug.Log("AnimState:"+animState);
        //doorFrame.GetComponent<Animator>().enabled = !Open;
        //var rot = doorFrame.rotation;
        doorFrame.transform.localRotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(0, 90 * OpenDirection, 0), animState);
	    //  rot.y += Open ? AnimDuration * Time.deltaTime : -AnimDuration * Time.deltaTime;
	    //rot.y = Mathf.Clamp(rot.y, 0, 90);
	    //doorFrame.rotation = Quaternion.Euler(rot);
	}

    private void OnDestroy()
    {
    }
}
