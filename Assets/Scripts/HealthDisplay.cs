using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class HealthDisplay : MonoBehaviour
{
	public Text textobject;
	public int health;
	public float starttime;

	void Start ()
	{
		this.starttime = Time.time;
		this.health = 500;
		this.textobject = GetComponent<Text>();
	}

	void Update ()
	{
		this.textobject.text = health.ToString();
	}

	public void ChangeHealth (int amount) {
		this.health += amount;
		if (this.health <= 0 && !GameObject.Find("GameOver").GetComponent<Text>().enabled) {
			Text gameovertext = GameObject.Find("GameOver").GetComponent<Text> ();
			float seconds = Time.time - this.starttime;
			gameovertext.text = String.Format("GAME OVER\nYou lasted {0} seconds", (int)seconds);
			gameovertext.enabled = true;
		}
	}
}