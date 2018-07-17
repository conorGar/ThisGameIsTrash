using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Information about a room.  A world is made up of a series of rooms.
public class Room : MonoBehaviour
{
    public RoomManager roomManager;

    public int roomNum;
    public List<EnemySpawn> enemySpawns;
//<<<<<<< HEAD

    /*public GameObject leftPortal;
    public GameObject rightPortal;
    public GameObject topPortal;
    public GameObject botPortal;
    */

//=======
    public GameObject player;
    public BoxCollider2D roomCollider2D;
    
//>>>>>>> refs/remotes/origin/digital_smash
    List<Enemy> enemies;
    //public List<Friend> friends;
    

    [System.Serializable]
    public class EnemySpawn
    {
        public List<Enemy> enemies;
    }

    public void ActivateRoom()
    {
    	Debug.Log("ActivateRoom....activated");
        roomManager.mainCamera.ScreenCamera.ViewportToWorldPoint(transform.position);
<<<<<<< HEAD
//<<<<<<< HEAD
        roomManager.SetCamFollowBounds(leftPortal.transform.position.x,rightPortal.transform.position.x,
        								topPortal.transform.position.y,botPortal.transform.position.y);
//=======
        roomManager.SetCamFollowBounds(this);
//>>>>>>> refs/remotes/origin/digital_smash
=======
        //roomManager.SetCamFollowBounds(leftPortal.transform.position.x,rightPortal.transform.position.x,
        							//	topPortal.transform.position.y,botPortal.transform.position.y);
>>>>>>> refs/heads/MeleeSystemImprovements
    }


    // Use this for initialization
    void Start ()
    {
        var enemies = new List<Enemy>();
        bool allowArmoredEnemies = false;
        if (GlobalVariableManager.Instance.DAY_NUMBER < 2)
            allowArmoredEnemies = true;

        // enemy spawns
        for (int i=0; i < enemySpawns.Count; ++i)
        {
            // get a random enemy from the enemy spawn list
            Enemy enemy = enemySpawns[i].enemies.RandomElement();

            // ignore armored enemies if they aren't allowed to spawn yet.
            if (enemy.IsArmored && !allowArmoredEnemies)
                continue;

            enemies.Add(enemy);  
        }

        Physics2D.IgnoreCollision(roomCollider2D, player.GetComponent<Collider2D>());
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
