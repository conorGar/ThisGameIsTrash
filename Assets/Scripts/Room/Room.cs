﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Information about a room.  A world is made up of a series of rooms.
public class Room : MonoBehaviour
{
    public RoomManager roomManager;
    public bool bossRoom;
    public int roomNum;
    public List<EnemySpawner> enemySpawners;
    public List<GarbageSpawner> garbageSpawners;
    public GameObject player;
    public BoxCollider2D roomCollider2D;
    public GameObject objectPool; //given to enemy, who needs it for EnemyTakeDamage hitStar spawn
    public GameObject cam; // used for cam zoom when enter boss room, can probably find a better way
    public GameObject tutPopup;
    public bool activateTutpopWhenEnter;
    public string tutPopUpToActivate;
	int waifuChance;
    private List<GameObject> enemies;

    //public List<Friend> friends;

    public void ActivateRoom()
    {
    	Debug.Log("ActivateRoom....activated");
        roomManager.mainCamera.ScreenCamera.ViewportToWorldPoint(transform.position);
        roomManager.SetCamFollowBounds(this);

        // enemy spawns
        bool allowArmoredEnemies = false;
        if (GlobalVariableManager.Instance.DAY_NUMBER < 2)
            allowArmoredEnemies = true;

        for (int i=0; i < enemySpawners.Count; ++i)
        {
            // get a random enemy from the enemy spawn list
            Enemy enemy = enemySpawners[i].enemies[0];


			if(GlobalVariableManager.Instance.IsPinEquipped(PIN.WAIFUWANTED)){
				//TODO: For now, waifu chance happens every time enter room, waifu isnt permanently set for that day.(players can keep entering room until waifu)
				waifuChance = Random.Range(1,3);
				Debug.Log("Waifu Chance: " + waifuChance);

				//enemySpawners[i].enemies.Add(waifu);
				//enemy = enemySpawners[i].enemies[1];

			}
            // ignore armored enemies if they aren't allowed to spawn yet.
          //  if (enemy.IsArmored && !allowArmoredEnemies)
              //  continue;

             //ignore if enemy for this spawner has been killed
             if(enemySpawners[i].CheckIfEnemyDead()){
             	Debug.Log("MY ENEMY IS DEAD!!!");
             	continue;
             }else{
            	Debug.Log("*** MY ENEMY IS NOT DEAD ***");
            	GameObject spawnedEnemy;
				if(waifuChance == 2){
					spawnedEnemy = ObjectPool.Instance.GetPooledObject("pObj_Waifu");

				}else{
					spawnedEnemy = ObjectPool.Instance.GetPooledObject(enemy.tag);
				}
				spawnedEnemy.SetActive(true);

	            if (spawnedEnemy != null)
	            {
	                enemies.Add(spawnedEnemy);
	                spawnedEnemy.transform.position = enemySpawners[i].transform.position;
					if(spawnedEnemy.GetComponent<EnemyTakeDamage>() != null){
		                spawnedEnemy.GetComponent<EnemyTakeDamage>().SetSpawnerID(enemySpawners[i].name);
		                spawnedEnemy.GetComponent<CannotExitScene>().SetLimits(this);
		                spawnedEnemy.GetComponent<EnemyTakeDamage>().objectPool = objectPool;
	                }
	            }
             }

            //GameObject spawnedEnemy = ObjectPool.Instance.GetPooledObject(enemy.tag);
           // spawnedEnemy.SetActive(true);


            
        }
        if(bossRoom){
        	for(int i = 0; i< transform.childCount;i++){
        		if(transform.GetChild(i).tag == "Boss"){
        			transform.GetChild(i).gameObject.SetActive(true);
					transform.GetChild(i).GetComponent<Boss>().ActivateBoss();
        		}
        	}
        	cam.GetComponent<Ev_MainCameraEffects>().ZoomInOut(1f,5f); //for debug testing
        }
 
    }

    public void DeactivateRoom()
    {	
    	if(enemies.Count > 0){
	        for (int i=0; i < enemies.Count; ++i)
	            enemies[i].SetActive(false);

	        enemies.Clear();
        }
    }


    // Use this for initialization
    void Start ()
    {
        enemies = new List<GameObject>();
        //Physics2D.IgnoreCollision(roomCollider2D, player.GetComponent<Collider2D>());
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public Rect GetRoomCameraBoundaries()
    {
        var vertExtent = roomManager.mainCamera.ScreenExtents.yMax;
        var horzExtent = roomManager.mainCamera.ScreenExtents.xMax;
        Rect rect = new Rect();

        // TODO: There's a Mathf.Max call in here to handle situations where the room is smaller than the size of the camera (the max_x or max_y become smaller than the min_x or min_y in this case).
        // Maybe there's a better way to handle this kind of issue?  If careful, will this come up much?
        rect.xMin = -roomCollider2D.bounds.size.x / 2.0f + horzExtent + transform.position.x;
        rect.xMax = Mathf.Max(rect.xMin, roomCollider2D.bounds.size.x / 2.0f - horzExtent + transform.position.x);
        rect.yMin = -roomCollider2D.bounds.size.y / 2.0f + vertExtent + transform.position.y;
        rect.yMax = Mathf.Max(rect.yMin, roomCollider2D.bounds.size.y / 2.0f - vertExtent + transform.position.y);

        return rect;
    }
}
