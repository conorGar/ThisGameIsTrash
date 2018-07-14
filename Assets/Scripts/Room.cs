using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Information about a room.  A world is made up of a series of rooms.
public class Room : MonoBehaviour
{
    public RoomManager roomManager;

    public int roomNum;
    public List<EnemySpawn> enemySpawns;
    List<Enemy> enemies;
    public List<Friend> friends;
    

    [System.Serializable]
    public class EnemySpawn
    {
        public List<Enemy> enemies;
    }

    private void ActivateRoom()
    {
        roomManager.mainCamera.ScreenCamera.ViewportToWorldPoint(transform.position);
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
