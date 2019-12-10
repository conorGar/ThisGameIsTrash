using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Information about a room.  A world is made up of a series of rooms.
public class Room : MonoBehaviour
{
    public RoomManager roomManager;
    public bool bossRoom;
    public int roomNum;
    public List<EnemySpawner> enemySpawners;
    public List<FriendSpawner> friendSpawners;
    public List<GarbageSpawner> garbageSpawners;
    public BoxCollider2D roomCollider2D;
    public GameObject objectPool; //given to enemy, who needs it for EnemyTakeDamage hitStar spawn
    public GameObject cam; // used for cam zoom when enter boss room, can probably find a better way
    public GameObject tutPopup;
    public Transform miniMapPosition;
    public bool activateTutpopWhenEnter;
    public string tutPopUpToActivate;
    public GameObject myMapClouds;
    public GlobalVariableManager.ROOM myRoom;
    public EnemyKillBlockManager roomKillBlockManager;

	int waifuChance;
	[HideInInspector]
    public List<GameObject> enemies; //needs to be public to be accessible for things that change the behavior of enemies(Dirty Decoy, for example)


    private List<GameObject> friends;

    public List<GameObject> bosses;



    public void ActivateRoom()
    {
    	Debug.Log("ActivateRoom: " + gameObject.name);

    	//Set room as visited
    	if((GlobalVariableManager.Instance.WORLD_ROOMS_DISCOVERED & myRoom) != myRoom){
    		Debug.Log("Room has not been discovered previously:" + gameObject.name);
    		GlobalVariableManager.Instance.WORLD_ROOMS_DISCOVERED |= myRoom;
			if(myMapClouds != null){
    			myMapClouds.SetActive(false);
    		}else{
    			Debug.Log("Room:" +gameObject.name + "has No map clouds assigned to it! ***");
    		}
    	}
        CamManager.Instance.tk2dMainCam.ScreenCamera.ViewportToWorldPoint(transform.position);
        roomManager.SetCamFollowBounds(this);

        // enemy spawns
        bool allowArmoredEnemies = false;
        if (GlobalVariableManager.Instance.DAY_NUMBER < 2)
            allowArmoredEnemies = true;
		if(roomKillBlockManager){
		    roomKillBlockManager.neededKillCount = 0; //reset kill blocker counter(so it doesnt keep incrementing every time you enter room...)
		}
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
				int deathDate = enemySpawners[i].GetDeathDate();
				if(deathDate != 0){//if body has not yet been spawned
					
	             	if(deathDate == 2 && enemySpawners[i].myEnemyHasNoDeadBody != true){
						GameObject body = ObjectPool.Instance.GetPooledObject("enemyBody",enemySpawners[i].transform.position);
						body.GetComponent<ThrowableBody>().SetSpawnerID(enemySpawners[i].name);
						body.GetComponent<tk2dSprite>().SetSprite(enemySpawners[i].enemyBodyName);
	             		body.GetComponent<ThrowableBody>().Poison();
	             		
	             	}//TODO: else spawn skeleton
             	}
             	continue;
             }else{
            	Debug.Log("*** MY ENEMY IS NOT DEAD ***");
            	GameObject spawnedEnemy;
                Vector3 spawnPos = new Vector3();

                // Try to spawn on the path finding grid if one is defined.  If not, spawn on the spawner position.
                var spawnerPathGrid = enemySpawners[i].pathGrid;
                if (spawnerPathGrid != null) {
                    Point enemyPoint = spawnerPathGrid.WorldToClosestGridPoint(enemySpawners[i].transform.position);

                    if (enemyPoint != null) {
                        spawnPos = spawnerPathGrid.GridToWorld(enemyPoint);
                    } else {
                        spawnPos = enemySpawners[i].transform.position;
                    }
                } else {
                    spawnPos = enemySpawners[i].transform.position;
                }

                if (waifuChance == 2){
					spawnedEnemy = ObjectPool.Instance.GetPooledObject("pObj_Waifu", spawnPos);

				}else{
					spawnedEnemy = ObjectPool.Instance.GetPooledObject(enemy.tag, spawnPos);
				}
            
	            if (spawnedEnemy != null)
	            {
                    // Give the enemy get the path grid reference if it needs it.
                    if (spawnerPathGrid != null) {
                        var enemyPath = spawnedEnemy.GetComponent<EnemyPath>();
                        if (enemyPath != null) {
                            enemyPath.pathGrid = spawnerPathGrid;
                        }
                    }

                    enemies.Add(spawnedEnemy);

                    //------check if spawned as an UpperEnemy or just an Enemy Layer based on given EnemySpawner value
					spawnedEnemy.layer = enemySpawners[i].GetEnemyLayer();
					spawnedEnemy.GetComponent<Renderer>().sortingLayerName = enemySpawners[i].GetEnemySortingLayer();
                    //---------------


                    Debug.Log("Current enemy spawned at layer: " + enemySpawners[i].GetEnemyLayer()); 

					if(spawnedEnemy.GetComponent<EnemyTakeDamage>() != null){
		                spawnedEnemy.GetComponent<EnemyTakeDamage>().SetSpawnerID(enemySpawners[i].name);
		                if(spawnedEnemy.GetComponent<CannotExitScene>())
		                	spawnedEnemy.GetComponent<CannotExitScene>().SetLimits(this);
		                spawnedEnemy.GetComponent<EnemyTakeDamage>().objectPool = objectPool;


		                //tell it to increase the kill counter if in a room with a kill count blocker
		                if(roomKillBlockManager){
		               	 spawnedEnemy.GetComponent<EnemyTakeDamage>().killBlockManager = roomKillBlockManager;
		                	roomKillBlockManager.neededKillCount++;
		                }
	                }
					if(enemySpawners[i].gameObject.GetComponent<WanderZone>() != null && spawnedEnemy.GetComponent<WanderWithinBounds>() != null) {	
						Rect wanderZone = enemySpawners[i].gameObject.GetComponent<WanderZone>().GetWanderBounds();
						spawnedEnemy.GetComponent<WanderWithinBounds>().SetWalkBounds(wanderZone);
					}else if(enemySpawners[i].gameObject.GetComponent<PathingMarks>() != null){
						List<GameObject> pathMarks = enemySpawners[i].gameObject.GetComponent<PathingMarks>().wanderpoints;
						spawnedEnemy.GetComponent<WanderOnPath>().SetPathMarks(pathMarks);
						spawnedEnemy.transform.position = enemySpawners[i].gameObject.GetComponent<PathingMarks>().startingPoint.transform.position;
					}
	            }
            }   
        }

        if (bossRoom) {
            for (int i = 0; i < bosses.Count; i++) {
                bosses[i].GetComponent<Boss>().ActivateBoss();
                bosses[i].GetComponent<Boss>().currentRoom = this;

            }
            CamManager.Instance.mainCamEffects.ZoomInOut(1f, 5f); //for debug testing
        }

        for (int i=0; i < friendSpawners.Count; ++i)
        {
            var spawnedFriend = FriendManager.Instance.GetFriend(friendSpawners[i].friend);
			Debug.Log("Friend Spawn got here *0*- " + friendSpawners[i].friend.name + spawnedFriend.IsVisiting);
            if (spawnedFriend != null && spawnedFriend.IsVisiting)
            {
            	Debug.Log("Friend Spawn got here *1* - " + friendSpawners[i].friend.name);
                if (spawnedFriend.IsCurrentRoom(this))
                {
					Debug.Log("Friend Spawn got here *2*- " + friendSpawners[i].friend.name);

                    spawnedFriend.gameObject.transform.position = friendSpawners[i].transform.position;
                    spawnedFriend.gameObject.SetActive(true);
                    spawnedFriend.OnActivateRoom();

                    friends.Add(spawnedFriend.gameObject);
                }
            }
        } 
    }

    public void DeactivateRoom()
    {	
    	if(bossRoom){
    		Debug.Log("Deactivate Room activated for boss room");
			for(int i = 0; i< bosses.Count;i++){
        			if(bosses[i].GetComponent<Boss>() != null){
						Debug.Log("Boss Deactivate event should be activated...?" + bosses[i].name);

        				bosses[i].GetComponent<Boss>().BossDeactivateEvent();
        			}
        			bosses[i].gameObject.SetActive(false);
					
        	}
    	}

	    for (int i=0; i < enemies.Count; ++i)
	        enemies[i].SetActive(false);

        for (int i = 0; i < friends.Count; ++i)
        {
            friends[i].GetComponent<Friend>().OnDeactivateRoom();
            friends[i].SetActive(false);
        }

        friends.Clear();
	    enemies.Clear();

        for (int i = 0; i < RoomManager.Instance.ObjectsClearedOnLeavingRoom.Count; i++)
        {
            ObjectPool.Instance.ClearPooledObjects(RoomManager.Instance.ObjectsClearedOnLeavingRoom[i]);
        }
    }


    // Use this for initialization
    void Awake ()
    {
        enemies = new List<GameObject>();
        friends = new List<GameObject>();
    }

    private void Start()
    {
    	if(roomCollider2D != this.gameObject.GetComponent<BoxCollider2D>()){
    		//default in case i forgot to assign proper box collider in Inspector
    		roomCollider2D = this.gameObject.GetComponent<BoxCollider2D>();
    	}


        if ((GlobalVariableManager.Instance.WORLD_ROOMS_DISCOVERED & myRoom) == myRoom) {
            if (myMapClouds != null) {
                myMapClouds.SetActive(false);
            } else {
                Debug.Log("Room:" + gameObject.name + "has No map clouds assigned to it! ***");
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {
		
	}

    public Rect GetRoomCameraBoundaries()
    {
        var vertExtent = CamManager.Instance.tk2dMainCam.ScreenExtents.yMax;
        var horzExtent = CamManager.Instance.tk2dMainCam.ScreenExtents.xMax;
        Rect rect = new Rect();

        // TODO: There's a Mathf.Max call in here to handle situations where the room is smaller than the size of the camera (the max_x or max_y become smaller than the min_x or min_y in this case).
        // Maybe there's a better way to handle this kind of issue?  If careful, will this come up much?
        rect.xMin = transform.position.x + roomCollider2D.offset.x - roomCollider2D.bounds.extents.x + horzExtent;
        rect.xMax = Mathf.Max(rect.xMin, transform.position.x + roomCollider2D.offset.x + roomCollider2D.bounds.extents.x - horzExtent);
        rect.yMin = -roomCollider2D.bounds.extents.y + vertExtent + transform.position.y + roomCollider2D.offset.y;
        rect.yMax = Mathf.Max(rect.yMin, transform.position.y + roomCollider2D.offset.y + roomCollider2D.bounds.extents.y - vertExtent);

        return rect;
    }

    public Rect GetRoomBoundaries()
    {
        Rect rect = new Rect();

        rect.xMin = roomCollider2D.bounds.min.x;
        rect.xMax = roomCollider2D.bounds.max.x;
        rect.yMin = roomCollider2D.bounds.min.y;
        rect.yMax = roomCollider2D.bounds.max.y;

        return rect;
    }

    // Returns true if there is trash in at least one spawner.  TODO: if we want a trash number this function could change to return that instead.
    public bool HasTrash()
    {
        for (int i = 0; i < garbageSpawners.Count; i++) {
            if (garbageSpawners[i].spawnedGarbage != null)
                return true;
        }

        return false;
    }
}
