using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public enum DESIRED_OBJECT
{
    NONE = 0,
    LIPSTICK,
    WIG,
    EYELINER,
    SUIT
}*/


// Covers the 3 rock-based friends (Rock, Stone, Slab)
public class RockFriend : Friend {

   
    public List<SpecialFriendObject> desiredObject = new List<SpecialFriendObject>();//the needed objects start off as the friend's children
    private List<SpecialFriendObject> deliveredObjects = new List<SpecialFriendObject>();
    List<SpecialFriendObject> pickedUpObjects = new List<SpecialFriendObject>();

    public GUI_RockItemHUD itemHUD;

    public override void GenerateEventData()
    {
        // These guys show up every day.
        day = CalendarManager.Instance.currentDay;
    }

    public void DeliverObject(SpecialFriendObject obj)
    {
    	for(int i = 0; i < pickedUpObjects.Count;i++){
    		deliveredObjects.Add(pickedUpObjects[i]);
    	}
    	pickedUpObjects.Clear();
        //deliveredObjects.Add(obj);
    }

    public override void StartingEvents(){

    	//spawn the 4(or how ever many left) objects at 4 random garbage spawner locations
		List<GarbageSpawner> myChosenSpawners =GarbageManager.Instance.garbageSpawners;
		myChosenSpawners.Shuffle();
    	for(int i = 0; i< desiredObject.Count;i++){
  			desiredObject[i].transform.position = myChosenSpawners[i].transform.position;
    		desiredObject[i].gameObject.SetActive(true);
    	}

    	//give current needed info to collected HUD
    	for(int i = 0; i< pickedUpObjects.Count;i++){
    		itemHUD.UpdateItemsCollected(pickedUpObjects[i].gameObject.GetComponent<SpriteRenderer>().sprite);
    	}
		for(int i = 0; i< deliveredObjects.Count; i++){
    		itemHUD.UpdateItemsCollected(deliveredObjects[i].gameObject.GetComponent<SpriteRenderer>().sprite);
    	}

    }

    public void PickUpObject(SpecialFriendObject go){ //activated by 'SpecialFriendObject'
    	pickedUpObjects.Add(go);
    	desiredObject.Remove(go);
    	if(!itemHUD.gameObject.activeInHierarchy){
    		itemHUD.gameObject.SetActive(true);
    	}
    	itemHUD.UpdateItemsCollected(go.GetComponent<SpriteRenderer>().sprite);
    }

}
