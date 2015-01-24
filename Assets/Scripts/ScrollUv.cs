using UnityEngine;
using System.Collections;

public class ScrollUv : MonoBehaviour
{

    public Vector2 Speed;
    private Vector4 offset;
    
	// Use this for initialization
	void Start ()
	{
	    offset = Vector4.zero;
        
	}
	
	// Update is called once per frame
	void Update ()
	{
	    Vector2 scaled = Speed*Time.deltaTime;
	    offset.x += scaled.x;
        offset.y += scaled.y;
       // renderer.material.SetVector("_UvOffs",offset);
        renderer.material.SetTextureOffset("_MainTex", offset);
	}
}
