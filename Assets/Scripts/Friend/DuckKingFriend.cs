using UnityEngine;
using System.Collections;

public class DuckKingFriend : Friend
{
	int quackCounter;

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
            case "DUCK_INTRO":
                nextDialog = "Duck1";
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

    public void Quack(){
    	CamManager.Instance.mainCamPostProcessor.profile = null;
    	StartCoroutine("QuackSequence");
    }

    IEnumerator QuackSequence(){
		if(quackCounter == 0){
	    	CamManager.Instance.mainCamEffects.ZoomInOut(1.3f,1f);
	    	CamManager.Instance.mainCam.ScreenShake(2f);
	    	yield return new WaitForSeconds(1f);
	    	dialogManager.ReturnFromAction();
	    	quackCounter++;
    	}else{
			GameObject deathSmoke = ObjectPool.Instance.GetPooledObject("effect_SmokePuff",gameObject.transform.position); 
			deathSmoke.transform.position = new Vector3((transform.position.x), transform.position.y, transform.position.z);
			AudioClip smokeSFX = deathSmoke.GetComponent<KillSelfAfterTime>().mySound;
			SoundManager.instance.PlaySingle(smokeSFX);

			GameObject deathGhost = ObjectPool.Instance.GetPooledObject("effect_DeathGhost",new Vector3((transform.position.x), transform.position.y, transform.position.z));
			deathGhost.GetComponent<Ev_DeathGhost>().OnSpawn();
			GameObject body = ObjectPool.Instance.GetPooledObject("enemyBody",gameObject.transform.position);
			body.GetComponent<tk2dSprite>().SetSprite("duck");
			yield return new WaitForSeconds(.1f);
			//CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
    	}

    }
}

