using UnityEngine;
using System.Collections;

public class GeneralGrubFriend : Friend
{

	public int startRoom;
	public int beetleSteveRoom;
	public int trailRoom;
	public int flamingPhilRoom;


	public override void GenerateEventData()
    {
		
		day = CalendarManager.Instance.currentDay;
		
        
    }
	public override bool IsCurrentRoom(Room room)
    {
        switch (GetFriendState())
        {
            case "GRUB_INTRO":
            	Debug.Log(room.roomNum + "< current room| start room >" + startRoom);
                return room.roomNum == startRoom;
			case "BEETLE_STEVE_INTRO":
				Debug.Log(room.roomNum + "< current room| beetleSteve room >" + beetleSteveRoom);

                return room.roomNum == beetleSteveRoom;
			case "BEETLE_STEVE_FIGHT":
                return room.roomNum == beetleSteveRoom;
			case "CICADA_SAM":
                return room.roomNum == trailRoom;
            case "CICADA_SAM_INTRO":
				return room.roomNum == flamingPhilRoom;
        }

		return room.roomNum == startRoom;
    }
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		OnUpdate();
	}

	public override void OnUpdate(){
		switch (GetFriendState()) {
            case "GRUB_INTRO":
                nextDialog = "Grub1";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
			case "BEETLE_STEVE_INTRO":
                nextDialog = "BeetleSteve";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
			case "CICADA_SAM":
                nextDialog = "GeneralGrub2";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
			case "CICADA_INTRO":
                nextDialog = "FlamingPhil";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
        }
	}
	public override IEnumerator OnFinishDialogEnumerator(bool panToPlayer = true)
    {
        yield return new WaitForSeconds(.3f);

        switch (GetFriendState()) {
            case "GRUB_INTRO":
				yield return new WaitForSeconds(.5f);
                SetFriendState("BEETLE_STEVE_INTRO");
                gameObject.GetComponent<ActivateDialogWhenClose>().distanceThreshold = 15; // smaller start radius for beetle steve dialog
               
                break;
			case "BEETLE_STEVE_INTRO":
				yield return new WaitForSeconds(.5f);
                SetFriendState("BEETLE_STEVE_FIGHT");
                GameObject beetleSteve = BossPool.Instance.GetPooledBoss("boss_beetleSteve",gameObject.transform.position);
                beetleSteve.GetComponent<BossBeetleSteve>().ActivateBoss();
				beetleSteve.GetComponent<BossBeetleSteve>().friend = this;

                gameObject.GetComponent<ActivateDialogWhenClose>().distanceThreshold = 45; 
                break;
			case "CICADA_SAM_INTRO":
				yield return new WaitForSeconds(.5f);
                SetFriendState("CICADA_SAM_FIGHT");
                GameObject flamingPhil = BossPool.Instance.GetPooledBoss("boss_flamingPhil",gameObject.transform.position);
                flamingPhil.GetComponent<BossBeetleSteve>().ActivateBoss();
				flamingPhil.GetComponent<BossBeetleSteve>().friend = this;

                gameObject.GetComponent<ActivateDialogWhenClose>().distanceThreshold = 45; 
                break;
			case "KING_INTRO":
             
                SetFriendState("END");
                break;
            case "END":
                break;
        }


        yield return base.OnFinishDialogEnumerator();
        gameObject.SetActive(false);

    }


    public override void OnActivateRoom(){
		switch (GetFriendState())
        {
            case "BEETLE_STEVE_FIGHT":
				GameObject beetleSteve = BossPool.Instance.GetPooledBoss("boss_beetleSteve",gameObject.transform.position);
                beetleSteve.GetComponent<BossBeetleSteve>().ActivateBoss();
                beetleSteve.GetComponent<BossBeetleSteve>().friend = this;
				gameObject.SetActive(false);
                break;
        }
    }
}

