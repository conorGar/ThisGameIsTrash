using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Information about a room.  A world is made up of a series of rooms.
public class Room : MonoBehaviour
{
    public RoomManager roomManager;

    public int roomNum;
    public List<EnemySpawn> enemySpawns;

    /*public GameObject leftPortal;
    public GameObject rightPortal;
    public GameObject topPortal;
    public GameObject botPortal;
    */

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
        //roomManager.SetCamFollowBounds(leftPortal.transform.position.x,rightPortal.transform.position.x,
        							//	topPortal.transform.position.y,botPortal.transform.position.y);
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
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
