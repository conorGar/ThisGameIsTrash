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
    public GameObject player;
    public BoxCollider2D roomCollider2D;
    public GameObject objectPool; //given to enemy, who needs it for EnemyTakeDamage hitStar spawn
    public GameObject cam; // used for cam zoom when enter boss room, can probably find a better way
    public GameObject tutPopup;
    public bool activateTutpopWhenEnter;
    public string tutPopUpToActivate;

    private List<GameObject> enemies;
    private List<GameObject> friends;

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

            // ignore armored enemies if they aren't allowed to spawn yet.
            //  if (enemy.IsArmored && !allowArmoredEnemies)
            //    continue;

            //ignore if enemy for this spawner has been killed
            if(enemySpawners[i].CheckIfEnemyDead()){
                Debug.Log("MY ENEMY IS DEAD!!!");
                continue;
            }else{
                Debug.Log("*** MY ENEMY IS NOT DEAD ***");
			    GameObject spawnedEnemy = ObjectPool.Instance.GetPooledObject(enemy.tag);
			    spawnedEnemy.SetActive(true);

	            if (spawnedEnemy != null)
	            {
	                enemies.Add(spawnedEnemy);
	                spawnedEnemy.transform.position = enemySpawners[i].transform.position;
	                spawnedEnemy.GetComponent<EnemyTakeDamage>().SetSpawnerID(enemySpawners[i].name);
	                spawnedEnemy.GetComponent<CannotExitScene>().SetLimits(this);
	                spawnedEnemy.GetComponent<EnemyTakeDamage>().objectPool = objectPool;
	            }
            }   
        }

        for (int i=0; i < friendSpawners.Count; ++i)
        {
            if (friendSpawners[i].friend.IsVisiting)
            {
                var spawnedFriend = FriendManager.Instance.GetFriendObject(friendSpawners[i].friend);

                if (spawnedFriend != null)
                {
                    spawnedFriend.transform.position = friendSpawners[i].transform.position;
                    spawnedFriend.SetActive(true);
                    friends.Add(spawnedFriend);
                }
            }
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
	    for (int i=0; i < enemies.Count; ++i)
	        enemies[i].SetActive(false);

        for (int i = 0; i < friends.Count; ++i)
            friends[i].SetActive(false);

        friends.Clear();
	    enemies.Clear();
    }


    // Use this for initialization
    void Awake ()
    {
        enemies = new List<GameObject>();
        friends = new List<GameObject>();
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
