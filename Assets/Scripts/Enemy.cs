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
	[SerializeField]
	private int healthPoints;
	private bool isDead = false;
	private Collider2D enemyCollider;
	private Animator animator; 
	[SerializeField]
	private int RewardAmount;

	public bool IsDead {
		get {
			return isDead;
		}
	}

	private Transform enemy; 
	private float NavigationTime; 
	// Use this for initialization
	void Start () {
		enemy = GetComponent<Transform>();
		enemyCollider = GetComponent<Collider2D>();
		animator = GetComponent<Animator>();
		GameManager.Instance.RegisterEmeny(this);	}
	
	// Update is called once per frame
	void Update () {
		if (Waypoints !=  null && !isDead){
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
			GameManager.Instance.RoundEscaped += 1;
			GameManager.Instance.TotalEscaped += 1;
			GameManager.Instance.UnRegisterEmeny(this);
			GameManager.Instance.IsWaveOver();
		} else if (other.tag  == "Projectile"){
			Projectile newP = other.gameObject.GetComponent<Projectile>();
			EnemyHit(newP.AttackStrength);
			Destroy(other.gameObject);
		}
	}

	public void EnemyHit(int hitPoints){
		if(healthPoints - hitPoints > 0) {
			healthPoints -= hitPoints;
			//call hurt animation
			animator.Play("Hurt");
			GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Hit);
		} else {
			//play die animation
			animator.SetTrigger("DidDie");
			//enemy should die
			Die();
		}
	}

	public void Die () {
		isDead = true;
		enemyCollider.enabled = false;
		GameManager.Instance.TotalKilled += 1;
		GameManager.Instance.AddMoney(RewardAmount);
		GameManager.Instance.IsWaveOver();
		GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Death);
	}


}
