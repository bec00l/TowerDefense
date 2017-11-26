using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ProjectileType {
	Rock, Arrow, Fireball
}
public class Projectile : MonoBehaviour {

	[SerializeField]
	private ProjectileType projectileType; 

	[SerializeField]
	private int attackStrength; 


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int AttackStrength {
		get {
			return attackStrength;
		}
	}

	public ProjectileType ProjectileType {
		get {
			return projectileType;
		}
	}
}
