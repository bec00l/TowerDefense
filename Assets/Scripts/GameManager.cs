using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameStatus{
	Next, 
	Play, 
	Gameover,
	Win
}
public class GameManager : Singleton<GameManager> {
	[SerializeField] 
	private int TotalWaves = 10; 
	[SerializeField]
	private Text TotalMoneyLbl;
	[SerializeField]
	private Text CurrentWaveLbl; 
	[SerializeField]
	private Text PlayBtnLbl;
	[SerializeField]
	private Button PlayButton;
	[SerializeField]
	private Text TotalEscapedLbl;    

	private int waveNumber = 0; 
	private int totalMoney = 0;	
	private int totalEscaped = 0;
	private int roundEscaped = 0;	
	private int totalKilled = 0;
	private int whichEnemyToSpawn = 0;
	private GameStatus currentStatus = GameStatus.Play;
	private AudioSource audioSource; 

	public AudioSource AudioSource {
		get{return audioSource;}
	}

	public int TotalMoney {
		get {
			return totalMoney;
		}

		set {
			totalMoney = value;
			TotalMoneyLbl.text = totalMoney.ToString();
		}
	}


	public int TotalEscaped {
		get {
			return totalEscaped;
		}

		set {
			totalEscaped = value;
		}
	}

	public int RoundEscaped {
		get {
			return roundEscaped;
		}

		set {
			roundEscaped = value;
		}
	}

	public int TotalKilled {
		get {
			return totalKilled;
		}

		set {
			totalKilled = value;
		}
	}

	[SerializeField]
	private GameObject SpawnPoint;
	[SerializeField]
	private GameObject[] Enemies;
	[SerializeField]
	private int TotalEnemies = 3; 
	[SerializeField]
	private int EnemiesPerSpawn; 
	private int spawnDelay = 2;
	public List<Enemy> EnemyList = new List<Enemy>();
	// Use this for initialization


	IEnumerator Spawn() {
		if(EnemiesPerSpawn > 0 && EnemyList.Count < TotalEnemies){
			for (var i = 0; i < EnemiesPerSpawn; i++){
				if(EnemyList.Count < TotalEnemies){
					var newEnemy = Instantiate(Enemies[0]) as GameObject;
					newEnemy.transform.position = SpawnPoint.transform.position;
				}
			}
		}

		yield return new WaitForSeconds(spawnDelay); 
		StartCoroutine(Spawn());
	}
	void Start () {
		PlayButton.gameObject.SetActive(false);
		audioSource = GetComponent<AudioSource>();
		ShowMenu();
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

	public void AddMoney(int amount) {
		TotalMoney += amount;
	}

	public void ReduceMoney(int amount) {
		TotalMoney -= amount;
	}

	public void ShowMenu () {
		switch(currentStatus) {
			case GameStatus.Gameover : 
				PlayBtnLbl.text = "Play Again";
				//add gameover sound
				AudioSource.PlayOneShot(SoundManager.Instance.GameOver);
				break;
			case GameStatus.Next : 
				PlayBtnLbl.text = "Next Wave";
				break;
			case GameStatus.Play : 
				PlayBtnLbl.text = "Play Game";
				break;
			case GameStatus.Win : 
				PlayBtnLbl.text = "Play";
				break;
			default:
				break;
		}
		PlayButton.gameObject.SetActive(true);
	}

	public void IsWaveOver() {
		TotalEscapedLbl.text = "Escaped " + TotalEscaped + "/10";

		if((RoundEscaped + TotalKilled) == TotalEnemies)
		{
			SetCurrentGameState();
			ShowMenu();
		}
	}

	public void SetCurrentGameState() {
		if(TotalEscaped >= 10) {
			currentStatus = GameStatus.Gameover;
			AudioSource.PlayOneShot(SoundManager.Instance.GameOver);
		}
		else if (waveNumber == 0 && (TotalKilled + RoundEscaped) == 0 ) {
			currentStatus = GameStatus.Play;
		}
		else if (waveNumber >= TotalWaves) {
			currentStatus = GameStatus.Win;
		} 
		else {
			currentStatus = GameStatus.Next;
		}

	}

	public void PlayBtnPressed() {
		switch (currentStatus) {
			case GameStatus.Next: 
				waveNumber += 1;
				TotalEnemies += waveNumber;
				break;
			default: 
				TotalEnemies = 3;
				TotalEscaped = 0;
				TotalMoney = 10;
				TowerManager.Instance.DestroyAllTower();
				TowerManager.Instance.RenameTagBuildSites();
				TotalMoneyLbl.text = TotalMoney.ToString();
				TotalEscapedLbl.text = "Escaped " + TotalEscaped + "/10";
				audioSource.PlayOneShot(SoundManager.Instance.NewGame);
				break;

		}

		DestroyAllEnemies();
		TotalKilled = 0;
		RoundEscaped = 0; 
		CurrentWaveLbl.text = "Wave " + (waveNumber + 1);
		StartCoroutine(Spawn());
		PlayButton.gameObject.SetActive(false);
	}

	private void HandleEscape() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			TowerManager.Instance.DisableDragSprite();
			TowerManager.Instance.towerBtnPressed = null;
		}
	}
	
	// Update is called once per frame
	void Update () {
		HandleEscape();
	}
}
