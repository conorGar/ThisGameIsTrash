using UnityEngine;
using System.Collections;

public class GeneralGrubFriend : Friend
{

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
        }
	}
	public override IEnumerator OnFinishDialogEnumerator(bool panToPlayer = true)
    {
        yield return new WaitForSeconds(.3f);

        switch (GetFriendState()) {
            case "DUCK_INTRO":
				StartCoroutine("QuackSequence");
				yield return new WaitForSeconds(.5f);
                SetFriendState("KING_INTRO");
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

