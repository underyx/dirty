using UnityEngine;
using System.Collections;

public class ScrollUv : MonoBehaviour
{

    public Vector2 Speed;
    private Vector4 offset;
    private Mesh mesh;
	// Use this for initialization
	void Start ()
	{
	    offset = Vector4.zero;
	    mesh = GetComponent<MeshFilter>().mesh??GetComponent<MeshFilter>().sharedMesh;
	    float xscale = this.transform.localScale.x;
	    Vector2[] uv = mesh.uv;
	    for (int i = 0; i < mesh.uv.Length; i++)
	    {
	        uv[i].x *= xscale;
	    }
	    mesh.uv = uv;
	        GetComponent<MeshFilter>().sharedMesh = null;
            GetComponent<MeshFilter>().mesh = mesh;

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
