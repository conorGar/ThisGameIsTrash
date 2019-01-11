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
	void Update ()
	{
	
	}
}

