using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private int Target = 0; 
	[SerializeField]
	private Transform ExitPoint; 
	[SerializeField]
	private Transform[] Waypoints; 
	[SerializeField]
	private float NavigationUpdate; 


	private Transform enemy; 
	private float NavigationTime; 
	// Use this for initialization
	void Start () {
		enemy = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Waypoints !=  null){
			NavigationTime += Time.deltaTime;
			if(NavigationTime > NavigationUpdate){
				if(Target < Waypoints.Length){
					enemy.position = Vector2.MoveTowards(enemy.position, Waypoints[Target].position, NavigationTime);
				} else {
					enemy.position = Vector2.MoveTowards(enemy.position, ExitPoint.position, NavigationTime);
				}

				NavigationTime = 0;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Checkpoint"){
			Target += 1; 
		} else if(other.tag == "Finish"){
			Destroy(gameObject);
			GameManager.Instance.RemoveEnemyFromScreen();
		}
	}
}
