using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

[SerializeField]
private float timeBetweenAttacks; 
[SerializeField]
private float attackRadius; 
[SerializeField]
private Projectile projectile; 
private Enemy targetEnemy;
private float attackCounter;
private bool isAttacking = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		attackCounter -= Time.deltaTime;
		if(targetEnemy == null || targetEnemy.IsDead){
			Enemy nearestEnemy = GetNearestEnemyInRange();
			if(nearestEnemy!= null && Vector2.Distance(transform.localPosition, nearestEnemy.transform.localPosition) <= attackRadius) {
				targetEnemy = nearestEnemy;
			}
		} else {
			if(attackCounter <= 0) {
				isAttacking = true;
				//reset attack counter
				attackCounter = timeBetweenAttacks;
			} else {
				isAttacking = false;
			}
			if(Vector2.Distance(transform.localPosition, targetEnemy.transform.localPosition) > attackRadius){
			targetEnemy = null;
			}
		}
		
	}

	public void Attack() {
		Projectile newProjectile = Instantiate(projectile) as Projectile; 
		newProjectile.transform.localPosition = transform.localPosition;
		PlayProjectileSound(newProjectile);
		if(targetEnemy == null) {
			Destroy(newProjectile);
		} else {
			StartCoroutine(MoveProjectile(newProjectile));
		}
	}

	public void PlayProjectileSound(Projectile projectile){
		if (projectile.ProjectileType == ProjectileType.Arrow) {
			GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Arrow);

		} else if (projectile.ProjectileType == ProjectileType.Fireball) {
			GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.FireBall);

		} else if (projectile.ProjectileType == ProjectileType.Rock) {
			GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Rock);

		}
	}

	void FixedUpdate() {
		if(isAttacking) {
			Attack();
		}
	}

	IEnumerator MoveProjectile (Projectile projectile) {
		while(GetTargetDistance(targetEnemy) > 0.20f && projectile != null && targetEnemy != null) {
			 var dir = targetEnemy.transform.localPosition - transform.localPosition;
			 var angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; 
			 projectile.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);
			 projectile.transform.localPosition = Vector2.MoveTowards(projectile.transform.localPosition, targetEnemy.transform.localPosition, 5f * Time.deltaTime);
			 yield return null;
		}

		if(projectile != null || targetEnemy == null){
			Destroy(projectile);
		}
	}

	private float GetTargetDistance(Enemy thisEnemy){
		if(thisEnemy == null){
			thisEnemy = GetNearestEnemyInRange();
		}

		return thisEnemy == null ? 0f : Mathf.Abs(Vector2.Distance(transform.localPosition, thisEnemy.transform.localPosition));
	}

	private List<Enemy> GetEnemiesInRange() {
		List<Enemy> enemiesInRange = new List<Enemy>();
		GameManager.Instance.EnemyList.ForEach(x=> {
			if(Vector2.Distance(transform.localPosition, x.transform.localPosition) <= attackRadius) {
				enemiesInRange.Add(x);
			}
		});
		return enemiesInRange;
	}

	private Enemy GetNearestEnemyInRange() {
		Enemy nearestEnemy  = null;
		float smallestDistance = float.PositiveInfinity;
		GetEnemiesInRange().ForEach(x=> {
			if(Vector2.Distance(transform.localPosition, x.transform.localPosition) < smallestDistance){
				smallestDistance = Vector2.Distance(transform.localPosition, x.transform.localPosition);
				nearestEnemy = x;
			}
		});

		return nearestEnemy;
	}
}
