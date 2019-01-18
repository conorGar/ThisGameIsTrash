using UnityEngine;
using System.Collections;

public class BossFriendMoleKing : Friend
{

	public BossMoleKing bossScript;

	bool healthHandicap;
	bool damageHandicap;
	// Use this for initialization
	void OnEnable ()
	{
		base.OnEnable();
       

		switch (GetFriendState())
        {
			case "INTRO":
				break;
			case "PHASE1":
				break;
			case "PHASE1_LEFT":
				break;
			case "PHASE1_AFTERTALK":
				break;
			case "PHASE1_AFTERTALK_LEFT":
				break;
            case "PHASE2":
           		bossScript.ActivateMoleSpawners();
           		break;
		    case "PHASE2_LEFT":
           		bossScript.ActivateMoleSpawners();
           		break;
            case "END":           	
           		break;
        }
	}

	public override void OnWorldStart(World world){
		switch (GetFriendState())
        {
			
           case "PHASE1_LEFT":
           	// Move the Mole King near to the player and activate proper dialog
           	break;
		   case "PHASE1_DIED":
			// Move the Mole King near to the player and activate proper dialog
           	break;
           
           case "END":           	
           	break;
        }
	}

	// Update is called once per frame
	private void Update ()
	{
		OnUpdate();
	}

	public override void OnUpdate(){
		switch (GetFriendState())
        {
			case "INTRO":
				nextDialog = "MK_intro";
                GetComponent<ActivateDialogWhenClose>().Execute();
				break;
			case "PHASE1":
				break;
			case "PHASE1_LEFT":
				break;
			case "PHASE1_AFTERTALK":
				break;
			case "PHASE1_AFTERTALK_LEFT":
				break;
            case "PHASE2":
           		bossScript.ActivateMoleSpawners();
           		break;
		    case "PHASE2_LEFT":
           		bossScript.ActivateMoleSpawners();
           		break;
            case "END":           	
           		break;
         }
	}

	public override void OnDeactivateRoom()
    {
        // TODO
    }

	public override IEnumerator OnFinishDialogEnumerator(bool panToPlayer = true)
    {
        yield return new WaitForSeconds(.5f);
        //TODO
        switch (GetFriendState())
        {
			case "PHASE1":
				break;

			case "PHASE2":
           		bossScript.ActivateMoleSpawners();
           		break;
        }

        yield return base.OnFinishDialogEnumerator();
        gameObject.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
        gameObject.SetActive(false);
    }
}

