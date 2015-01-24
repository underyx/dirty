using System;
using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public bool open;
    private OffMeshLink link;
    private Transform doorLink0;
    private Transform doorLink1;
    private Transform doorFrame;


    public Region region0;
    public Region region1;

    private Animator doorAnimator;
    // Use this for initialization
	void Start ()
	{
       // DoorManager manager = GameObject.Find("Door Manager").GetComponent<DoorManager>();
	    link = GetComponent<OffMeshLink>();
	    doorLink0 = transform.FindChild("DoorLink0");
        doorLink1 = transform.FindChild("DoorLink1");
	    doorFrame = transform.FindChild("FrameMaster");
        doorAnimator = doorFrame.GetComponent<Animator>();
	    var regions = GameObject.FindObjectsOfType<Region>();
	    region0 = region1 = null;

	    foreach (Region reg in regions)
	    {
	        NavMeshPath path = new NavMeshPath();
	        if (region0 == null && reg.InRegion(doorLink0.transform))
	        {
	            region0 = reg;
	        }
            else if (region1 == null && reg.InRegion(doorLink1.transform))
            {
                region1 = reg;
            }
	    }
	}

    public bool Open
    {
        get { return open; }
        set
        {
            open = value;
            link.activated = value;
            /*if (value)
            {
                doorAnimator.Play("DoorOpen");               
            }
            else
            {
                doorAnimator.Play("DoorClose");
            }*/
        }
    }

    public float AnimDuration = 0.5f;

    private float animState = 0;

    // Update is called once per frame
	void Update ()
	{
        animState += (Open ? Time.deltaTime : -Time.deltaTime) / AnimDuration;
        animState= Mathf.Clamp01(animState);
     //   Debug.Log("AnimState:"+animState);
        //doorFrame.GetComponent<Animator>().enabled = !Open;
        //var rot = doorFrame.rotation;
        doorFrame.transform.localRotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(0, 90, 0), animState);
	    //  rot.y += Open ? AnimDuration * Time.deltaTime : -AnimDuration * Time.deltaTime;
	    //rot.y = Mathf.Clamp(rot.y, 0, 90);
	    //doorFrame.rotation = Quaternion.Euler(rot);
	}

    private void OnDestroy()
    {
    }
}
