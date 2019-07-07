using UnityEngine;
using System.Collections;

public class GeneralGrubFriend : Friend
{

	public GameObject beetleSteve;

	public override void GenerateEventData()
    {
		
		day = CalendarManager.Instance.currentDay;
		
        
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
                BossPool.Instance.GetPooledBoss("boss_beetleSteve",gameObject.transform.position);
                gameObject.GetComponent<ActivateDialogWhenClose>().distanceThreshold = 15; // smaller start radius for beetle steve dialog
                break;
			case "KING_INTRO":
             
                SetFriendState("END");
                break;
            case "END":
                break;
        }


        yield return base.OnFinishDialogEnumerator();


    }
}

