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
	private int spawnDelay = 2;
	public List<Enemy> EnemyList = new List<Enemy>();
	// Use this for initialization


	IEnumerator Spawn() {
		if(EnemiesPerSpawn > 0 && EnemyList.Count < TotalEnemies){
			for (var i = 0; i < EnemiesPerSpawn; i++){
				if(EnemyList.Count < MaxEnemiesOnScreen){
					var newEnemy = Instantiate(Enemies[Random.Range(0,3)]) as GameObject;
					newEnemy.transform.position = SpawnPoint.transform.position;
				}
			}
		}

		yield return new WaitForSeconds(spawnDelay); 
		StartCoroutine(Spawn());
	}
	void Start () {
		StartCoroutine(Spawn());
	}

	public void RegisterEmeny(Enemy enemy){
		EnemyList.Add(enemy);
	}

	public void UnRegisterEmeny(Enemy enemy){
		EnemyList.Remove(enemy);
		Destroy(enemy.gameObject);
	}


	public void DestroyAllEnemies() {
		EnemyList.ForEach(x=> {
			Destroy(x.gameObject);
		});

		EnemyList.Clear();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
