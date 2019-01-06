using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaptainBadgerFriend : Friend
{
	public GameObject rockPile;
	public GameObject largeTrashMineCart;

	public Sprite rockPile_full;
	public Sprite rockPile_half;
	public Sprite rockPile_almostGone;

	int hope;
	int pride;


	public new void OnEnable(){

		rockPile.GetComponent<SpriteRenderer>().sprite = rockPile_almostGone; // this is the sprite for rock pile after the first two istances.
		switch (GetFriendState()) {
			case "INTRO":
                rockPile.GetComponent<SpriteRenderer>().sprite = rockPile_full;
                gameObject.transform.position = new Vector2(gameObject.transform.position.x - 10f,gameObject.transform.position.y); //captain badger is further away to begin with...
                break;
			case "TAKING_SOME_TIME":
                rockPile.GetComponent<SpriteRenderer>().sprite = rockPile_half;
				gameObject.transform.position = new Vector2(gameObject.transform.position.x - 5f,gameObject.transform.position.y); //captain badger is further away to begin with...
                break;
            case "END_PRIDE":
            	//spawn black rat enemy
            	break;
        }

        if(GlobalVariableManager.Instance.DAY_NUMBER > day){ //day of visit does not matter after time has passed.
        	day = GlobalVariableManager.Instance.DAY_NUMBER;
        }
	}

	
	private void Update()
    {
        OnUpdate();
    }

	public override void OnUpdate ()
	{
		switch (GetFriendState()) {
            case "INTRO":
                nextDialog = "Start";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
            case "TAKING_SOME_TIME":
                nextDialog = "Jumbo2";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
			case "LOSING_HOPE":
                nextDialog = "Jumbo3";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
            case "HOPEFUL":
				nextDialog = "JumboMissed";
           	    GetComponent<ActivateDialogWhenClose>().Execute();
            	break;
           	case "PRIDEFUL":
				nextDialog = "JumboMissed2_1";
				GetComponent<ActivateDialogWhenClose>().Execute();

           		break;
			case "END_TUNNEL":
				nextDialog = "JumboMissed2_1";
				GetComponent<ActivateDialogWhenClose>().Execute();

           		break;
			case "END_PRIDE":
				nextDialog = "JumboMissed2_1";
				GetComponent<ActivateDialogWhenClose>().Execute();

           		break;
            case "END":
                break;
        }
	}

	public override void OnWorldStart(World world){
		switch (GetFriendState()) {
          	case "END_PRIDE":
          		rockPile.SetActive(false);
          		break;
			case "END_TUNNEL":
          		rockPile.SetActive(false);
          		break;
			case "END_TUNNEL_FIN":
          		rockPile.SetActive(false);
          		break;
			case "END_PRIDE_DEAD":
          		rockPile.SetActive(false);
          		break;
            case "END_GIVE_UP":
            	largeTrashMineCart.GetComponent<Ev_LargeTrash>().enabled = true;
                break;
        }
	}

	public override IEnumerator OnFinishDialogEnumerator(bool panToPlayer = true){
		CheckValues();
		yield return base.OnFinishDialogEnumerator();


	}

	void CheckValues(){
		switch (GetFriendState()) {
            case "INTRO":  
				day += Random.Range(2,4);        
                break;
            case "TAKING_SOME_TIME":
                if(pride > 5){
					SetFriendState("PRIDEFUL");
                }else if(hope < 5){
					SetFriendState("LOSING_HOPE");
                }else{
                	SetFriendState("HOPEFUL");
                }
				day += Random.Range(2,4);  
                break;
			case "LOSING_HOPE":
                if(hope > 7){
					SetFriendState("END_TUNNEL");
                }else{
					SetFriendState("END_GIVE_UP");
                }
				day += Random.Range(2,4);  
                break;
            case "HOPEFUL":
				if(pride > 8){
					SetFriendState("END_PRIDE");
                }else if(hope < 7){
					SetFriendState("END_GIVE_UP");
                }else{
                	SetFriendState("END_TUNNEL");
                }
				day += Random.Range(2,4);  
            	break;
           	case "PRIDEFUL":
				if(pride > 8){
					SetFriendState("END_PRIDE");
                }else if(hope < 7){
					SetFriendState("END_GIVE_UP");
                }else{
                	SetFriendState("END_TUNNEL");
                }
				day += Random.Range(2,4);
           		break;
			case "END_TUNNEL":
           		break;
			case "END_PRIDE":
           		break;
            case "END":
                break;
        }
	}

}

