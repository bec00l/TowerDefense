using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

	[SerializeField]
	private GameObject SpawnPoint;
	[SerializeField]
	private GameObject[] Enemies;
	[SerializeField]
	private int MaxEnemiesOnScreen; 
	[SerializeField]
	private int TotalEnemies; 
	[SerializeField]
	private int EnemiesPerSpawn; 
	private int enemiesOnScreen;
	private int spawnDelay = 2;
	// Use this for initialization


	IEnumerator Spawn() {
		if(EnemiesPerSpawn > 0 && enemiesOnScreen < TotalEnemies){
			for (var i = 0; i < EnemiesPerSpawn; i++){
				if(enemiesOnScreen < MaxEnemiesOnScreen){
					var newEnemy = Instantiate(Enemies[Random.Range(0,3)]) as GameObject;
					newEnemy.transform.position = SpawnPoint.transform.position;
					enemiesOnScreen += 1;
				}
			}
		}

		yield return new WaitForSeconds(spawnDelay); 
		StartCoroutine(Spawn());
	}
	void Start () {
		StartCoroutine(Spawn());
	}

	void SpawnEnemy() {
		
	}

	public void RemoveEnemyFromScreen() {
		if(enemiesOnScreen > 0) {
			enemiesOnScreen -=1; 
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
