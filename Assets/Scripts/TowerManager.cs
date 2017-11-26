
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : Singleton<TowerManager> {

	public  TowerBtn towerBtnPressed {get; set;}
	private SpriteRenderer spriteRenderer;
	private List<Tower> TowerList = new List<Tower>();
	private List<Collider2D> BuildList = new List<Collider2D>();
	private Collider2D buildTile;
	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		buildTile = GetComponent<Collider2D>();
		spriteRenderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

			if(hit.collider.tag == "BuildSite"){
				buildTile = hit.collider;
				buildTile.tag = "BuildSiteFull";
				RegisterBuildSite(buildTile);
				PlaceTower(hit);
			}
		}

		if(spriteRenderer.enabled) {
				FollowMouse();
		}
			
	}

	public void FollowMouse() {
		transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = new Vector2(transform.position.x, transform.position.y);
	}

	public void EnableDragSprite(Sprite sprite) {
		spriteRenderer.enabled = true;
		spriteRenderer.sprite = sprite; 
	}

	public void DisableDragSprite() {
		spriteRenderer.enabled = false;
	}

	public void RegisterBuildSite(Collider2D buildTag) {
		BuildList.Add(buildTag);
	}

	public void RegisterTower(Tower tower) {
		TowerList.Add(tower);
	}

	public void RenameTagBuildSites() {
		BuildList.ForEach(x=> {
			x.tag = "BuildSite";
		});
		BuildList.Clear();
	}

	public void DestroyAllTower() {
		TowerList.ForEach(x=> {
			Destroy(x);
		});
		TowerList.Clear();
	}

	public void PlaceTower(RaycastHit2D hit) {
		if(!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed != null){
			Tower newTower = Instantiate(towerBtnPressed.TowerObject);
			newTower.transform.position = hit.transform.position;
			GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.TowerBuilt);
			BuyTower(towerBtnPressed.TowerPrice);
			RegisterTower(newTower);
			DisableDragSprite();
		}
		
	}

	public void BuyTower(int price) {
		GameManager.Instance.ReduceMoney(price);
	}

	public void SelectedTower(TowerBtn towerSelected){
		if(towerSelected.TowerPrice <= GameManager.Instance.TotalMoney) {
			towerBtnPressed = towerSelected;
			EnableDragSprite(towerSelected.DragSprite);
		}
		
	}
}
