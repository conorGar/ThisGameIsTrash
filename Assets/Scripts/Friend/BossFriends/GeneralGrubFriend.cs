using UnityEngine;
using System.Collections;

public class GeneralGrubFriend : Friend
{

	public int startRoom;
	public int beetleSteveRoom;
	public int trailRoom;
	public int flamingPhilRoom;
	public int generalRoom = 206;

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
                return room.roomNum == flamingPhilRoom;
            case "CICADA_SAM_INTRO":
				return room.roomNum == trailRoom;
			case "GENERAL_FIGHT_INTRO":
				return room.roomNum == generalRoom;
			case "GENERAL_FIGHT":
				return room.roomNum == generalRoom;
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
			case "CICADA_SAM_INTRO":
                nextDialog = "GeneralGrub2";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
			case "CICADA_SAM":
                nextDialog = "FlamingPhil";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
			case "GENERAL_FIGHT_INTRO":
                nextDialog = "GeneralFightIntro";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
			case "GENERAL_FIGHT_END":
                nextDialog = "GrubFightEnd";
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
                SetFriendState("CICADA_SAM");

                gameObject.GetComponent<ActivateDialogWhenClose>().distanceThreshold = 15; 
                break;
			case "CICADA_SAM":
				yield return new WaitForSeconds(.5f);
                SetFriendState("CICADA_SAM_FIGHT");
				GameObject flamingPhilBounds = BossPool.Instance.GetPooledBoss("boss_flamingPhil_bounds",gameObject.transform.position);
				flamingPhilBounds.SetActive(true);
                GameObject flamingPhil = BossPool.Instance.GetPooledBoss("boss_flamingPhil",gameObject.transform.position);
	

                flamingPhil.GetComponent<BossFlamingPhil>().ActivateBoss();
				flamingPhil.GetComponent<BossFlamingPhil>().friend = this;

                gameObject.GetComponent<ActivateDialogWhenClose>().distanceThreshold = 15; 
                break;
			case "GENERAL_FIGHT_INTRO":
				yield return new WaitForSeconds(.5f);
                SetFriendState("GENERAL_FIGHT");
				GameObject generalGrubTank = BossPool.Instance.GetPooledBoss("boss_generalGrubTank",gameObject.transform.position);

                GameObject generalGrub = BossPool.Instance.GetPooledBoss("boss_generalGrub");
				generalGrub.GetComponent<Boss>().ActivateBoss();
				generalGrub.GetComponent<BossGeneralGrub>().grubFriend = this;
                gameObject.GetComponent<ActivateDialogWhenClose>().distanceThreshold = 45; 
                break;
            case "GENERAL_FIGHT_END":
            	LargeTrashManager.Instance.EnableSpecificTrash("testChair",1);
				SetFriendState("END");

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
           case "GENERAL_FIGHT":
				GameObject generalGrubTank = BossPool.Instance.GetPooledBoss("boss_generalGrubTank",gameObject.transform.position);
                GameObject generalGrub = BossPool.Instance.GetPooledBoss("boss_generalGrub");
				generalGrub.GetComponent<BossGeneralGrub>().grubFriend = this;

				generalGrub.GetComponent<Boss>().ActivateBoss();
				gameObject.SetActive(false);

           	break;
        }
    }
}

