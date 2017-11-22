using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBtn : MonoBehaviour {

[SerializeField]
private GameObject towerObject;
[SerializeField]
private Sprite dragSprite;

public Sprite DragSprite {
	get {
		return dragSprite;
		}
	}

public GameObject TowerObject {
	get {
		return towerObject;
		}
	}
}


