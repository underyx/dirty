using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(SampleAgent))]
public class PathPlanner : MonoBehaviour
{
    public List<Region> PlannedPath;
    private SampleAgent agent;
    private System.Random random;
    private Dictionary<Region, int> visitCounts;
    public bool RunningPath;
    public Region target;

    

    private void Awake()
    {
        agent = FindObjectOfType<SampleAgent>();        
    }

    // Use this for initialization
	void Start () {
        visitCounts = new Dictionary<Region, int>();
        random = new System.Random();
        PlannedPath = new List<Region>(64);
	    foreach (var region in FindObjectsOfType<Region>())
	    {
	        visitCounts[region] = 0;
	    }
        //For now, generate a path at the beginning
	    var startRegion = GameObject.Find("Canteen").GetComponent<Region>();
        PlanPath(startRegion);
	}
	
	// Update is called once per frame
	void Update () {
	    if (RunningPath)
	    {
	        if (PlannedPath.Count == 0)
	        {
	            RunningPath = false;
	        }
            else if (agent.MoveFinished )
	        {
	            target = PlannedPath[0];
                PlannedPath.RemoveAt(0);
                agent.GoToPoint(target.transform.position);
	        }
	    }
	}


    public List<Region> UnvisitedNodes()
    {
        var list = new List<Region>(32);

        foreach (var elem in visitCounts)
        {
            if(elem.Value<=0)
                list.Add(elem.Key);
        }
        return list;
    }

    int[] Shuffle(int cnt)
    {
        int[] array = new int[cnt];
        for (int i = 0; i < cnt; i++)
        {
            array[i] = i;
        }
        int n = array.Length;
        for (int i = 0; i < n; i++)
        {
            // NextDouble returns a random number between 0 and 1.
            // ... It is equivalent to Math.random() in Java.
            int r = i + (int)(random.NextDouble() * (n - i));
            int t = array[r];
            array[r] = array[i];
            array[i] = t;
        }
        return array;
    }

    public void PlanPath(Region startRegion)
    {
        var nodesToVisit = UnvisitedNodes();
        PlannedPath.Clear();
        var shuf = Shuffle(nodesToVisit.Count);
        for (int i = 0; i < nodesToVisit.Count; i++)
        {
            PlannedPath.Add(nodesToVisit[shuf[i]]);
        }

    }

    public void ExecutePath()
    {

    }
}
