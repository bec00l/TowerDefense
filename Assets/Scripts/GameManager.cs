using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager Instance = null;
	public GameObject SpawnPoint;
	public GameObject[] Enemies;
	public int MaxEnemiesOnScreen; 
	public int TotalEnemies; 
	public int EnemiesPerSpawn; 
	private int enemiesOnScreen;
	private int spawnDelay = 2;
	// Use this for initialization

	void Awake () {
		if(Instance == null){
			Instance = this; 
		}
		else if (Instance != this){
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

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
