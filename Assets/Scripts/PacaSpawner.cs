using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PacaSpawner : MonoBehaviour
{
	public GameObject spawnedobject;
	public List<GameObject> Spawnables;

	void Start ()
	{
		this.spawnedobject = null;
	}

	void Update ()
	{
		Debug.Log(GameObject.Find("Capsule").GetComponent<SampleAgent>().CurrentRegion);
		Debug.Log(Region.FindRegion(this.transform));
		if (
			this.spawnedobject == null
			&& GameObject.Find("Capsule").GetComponent<SampleAgent>().CurrentRegion != Region.FindRegion(this.transform) 
			&& Random.value < 0.02
		) {
			GameObject newpaca = Instantiate (
				Spawnables[Random.Range(0, 2)], // paca should be an invisible reference object, with a mesh renderer 
				this.transform.position,
				this.transform.rotation
			) as GameObject;
			this.spawnedobject = newpaca;
			MeshRenderer newpacarenderer = newpaca.GetComponent<MeshRenderer>();
			newpacarenderer.enabled = true;
		}
	}
}