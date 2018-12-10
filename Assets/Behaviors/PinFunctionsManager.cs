using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PinFunctionsManager : MonoBehaviour {

	/// <summary>
	/// Keeps a method for each pins effect that is called when needed by whatever script. Attatched to Jim.
	/// </summary>
	GameObject objectToKill;
	float delay;
	int effectDisplayOverride;
	float passivePillageBonus;

	public bool inWorld; //Dont bother adding some functions in rooms outside of the worlds
	public GameObject pinEffectDisplayHUD;
	public GameObject pinObjectPool;
	public GameObject devil;
	public Ev_CurrentWeapon currentWeaponDisplay;
	public GameObject spinAttack;
	public GameObject decoyObject;
	//public PinManager pinManager;
	public Sprite[] displaySprites;
	Sprite displaySprite;
	int displayHudCalledAgain;


	GameObject decoyInstance;
	int decoyTimer;


	int dashCounter = 0;
	INPUTACTION dashKey;

	//LinkToTheTrash
	INPUTACTION heldKey;
	bool chargingSpin;

	public void Awake(){
		if(inWorld){
			if(GlobalVariableManager.Instance.IsPinEquipped(PIN.BULKYBAG)){
				GlobalVariableManager.Instance.BAG_SIZE += 2; //subtracted again at results.cs
			}

			if(GlobalVariableManager.Instance.IsPinEquipped(PIN.DIRTYDECOY)){
				decoyInstance = Instantiate(decoyObject,gameObject.transform.position,Quaternion.identity); //ask if this is better since this object isnt needed unless has the pin?
				decoyObject.SetActive(false);
				InvokeRepeating("DirtyDecoyTimer",1,1);
			}

		}
	}

	void Update(){

		if(chargingSpin && ControllerManager.Instance.GetKeyUp(heldKey)){
			chargingSpin = false;
			StopCoroutine("SpinAttack");
			gameObject.GetComponent<MeleeAttack>().cantAttack = false;
			gameObject.GetComponent<MeleeAttack>().ReturnFromSwing();

		}


	}

	public IEnumerator DumpsterDash(INPUTACTION givenKey){
		if(dashCounter == 0){
			Debug.Log("Dumpster Dash - 0");
			dashCounter = 1;
			dashKey = givenKey;
			yield return new WaitForSeconds(.2f);
			if(dashCounter == 1){ //after a breif period if key isn't pressed again it resets
				dashCounter = 0;
			}
		}else if(dashCounter == 1){
			Debug.Log("Dumpster Dash - 1");

			if(givenKey == dashKey){
				//-------------Dash-----------------//
				dashCounter = 2;
				Debug.Log("Dumpster Dash - 2");

				gameObject.GetComponent<EightWayMovement>().enabled = false; // I know we dont wanna do stuff like this, just felt like for this instance it was more appropriate doing this than creating a gamestate...?
				ObjectPool.Instance.GetPooledObject("effect_enemyLand",new Vector2(gameObject.transform.position.x,gameObject.transform.position.y-1));
				if(dashKey == INPUTACTION.MOVELEFT){
					gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-30f,0f),ForceMode2D.Impulse);
				}else if(dashKey == INPUTACTION.MOVERIGHT){
					gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(30f,0f),ForceMode2D.Impulse);
				}else if(dashKey == INPUTACTION.MOVEDOWN){
					gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f,-30f),ForceMode2D.Impulse);
				}else if(dashKey == INPUTACTION.MOVEUP){
					gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f,30f),ForceMode2D.Impulse);
				}
				yield return new WaitForSeconds(.1f);
				dashCounter = 0;
				gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				gameObject.GetComponent<EightWayMovement>().enabled = true;

			}else{
				dashCounter = 0; //reset if press different direction
			}
		}
	}

	public void HeroOfGrime(int direction, Vector3 spawnPos){//right = 1, left = 2, up = 3, down = 4
		GameObject beam = pinObjectPool.GetComponent<ObjectPool>().GetPooledObject("pObj_hogBeam",spawnPos);
		if(direction == 1){
			beam.GetComponent<Rigidbody2D>().velocity = new Vector2(15f,0f);
			beam.transform.localScale = new Vector3(.87f,beam.transform.localScale.y,beam.transform.localScale.z);

		}else if(direction == 2){
			beam.GetComponent<Rigidbody2D>().velocity = new Vector2(-15f,0f);
			beam.transform.localScale = new Vector3(-.87f,beam.transform.localScale.y,beam.transform.localScale.z);
		}else if(direction == 3){
			beam.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,15f);
			beam.transform.Rotate(0f,0f,90f);
		}else if(direction == 4){
			beam.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,-15f);
			beam.transform.Rotate(0f,0f,-90f);
		}
		SetDelay(1f);
		StartCoroutine("KillObject",beam);
	}

	public void DevilsDeal(){
		GameObject devilInstance = Instantiate(devil);
		devilInstance.transform.position = gameObject.transform.position;
		devilInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,1f);

	}

	void DirtyDecoy(){
		Debug.Log("Dirty Decoy Activate");
		decoyInstance.SetActive(false);
		decoyInstance.transform.position = gameObject.transform.position;
		decoyInstance.SetActive(true);
		//decoyInstance.transform.parent = null;

	}

	void DirtyDecoyTimer(){
		if(GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)){
			decoyTimer++;
			if(decoyTimer >= 25){
			DirtyDecoy();
			decoyTimer = 0;
			}
		}
	}

	public void HungryForMore(){
		WorldManager.Instance.amountTrashHere += 5;
		Sprite mySprite = GetSprite("pin_hungry");
		StartCoroutine("DisplayEffectHud",mySprite);
	}

	public void ApplePlus(){
		GlobalVariableManager.Instance.Max_HP += 1;
		Sprite mySprite = GetSprite("pin_applePlus");
		StartCoroutine("DisplayEffectHud",mySprite);
	}

	public void FaithfulWeapin(){
		currentWeaponDisplay.UpdateMelee();
	}

	public void SneakyScrapper(){
		Debug.Log("Sneaky Scrapper Activated");
		GlobalVariableManager.Instance.IS_HIDDEN = true;
		gameObject.GetComponent<EightWayMovement>().myLegs.GetComponent<tk2dSprite>().color = new Color(1,1,1,.5f);
		gameObject.GetComponent<tk2dSprite>().color = new Color(1,1,1,.5f);
	}
	public void SneakyScrapperReturn(){
		Debug.Log("Sneaky Scrapper De-Activated");

		gameObject.GetComponent<EightWayMovement>().myLegs.GetComponent<tk2dSprite>().color = new Color(1,1,1,1);

		gameObject.GetComponent<tk2dSprite>().color = new Color(1,1,1,1);
		GlobalVariableManager.Instance.IS_HIDDEN = false;
	}


	public IEnumerator SpinAttack(INPUTACTION givenKey){ //called at Swing() in 'MeleeAttack.cs'
		gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		heldKey = givenKey;
		chargingSpin = true;
		yield return new WaitForSeconds(.3f);
		spinAttack.SetActive(true);
		chargingSpin = false;
		yield return new WaitForSeconds(.5f);
		gameObject.GetComponent<MeleeAttack>().cantAttack = false;
		gameObject.GetComponent<MeleeAttack>().ReturnFromSwing();
		spinAttack.SetActive(false);
	}




	public void PassivePillage(bool increase){
		if(increase){
			passivePillageBonus += .5f;
			Sprite mySprite = GetSprite("pin_passivePillage");
			StartCoroutine("DisplayEffectHud",mySprite);
			gameObject.GetComponent<EightWayMovement>().UpdateSpeed(.5f);
		}else{
			passivePillageBonus = passivePillageBonus*-1;
			gameObject.GetComponent<EightWayMovement>().UpdateSpeed(passivePillageBonus); //subtracts all previously added bonus
			passivePillageBonus = 0;


		}

	}

	public void QuenpinTarantino(){
		for(int i = 0; i<3;i++){
			GameObject bullet = ObjectPool.Instance.GetPooledObject("pObj_bullet",gameObject.transform.position);
			int randomDirection = Random.Range(-1,1);
			if(randomDirection == -1){
				bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(-5,Random.Range(-6f,6f));

			}else{
				bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(5,Random.Range(-4f,4f));

			}
		}
	}
	public void ReturnPinValues(){ //Resets needed pin values at end of day(mostly just stat upgrade types

		if(GlobalVariableManager.Instance.IsPinEquipped(PIN.APPLEPLUS)){
			GlobalVariableManager.Instance.Max_HP -= 1;
		}


	} 



	Sprite GetSprite(string spriteName){
		for(int i = 0; i < displaySprites.Length;i++){
			if(displaySprites[i].name == spriteName){
			displaySprite = displaySprites[i];
			break;
			}
		}
		return displaySprite;

	}

	public IEnumerator DisplayEffectHud(Sprite pinIcon){
		if(displayHudCalledAgain == 0){
			displayHudCalledAgain = 1;
		}else{
			displayHudCalledAgain++;
		}
		/*if(pinEffectDisplayHUD.activeInHierarchy){//if just activated by previous pin
			pinEffectDisplayHUD.SetActive(false);
			CancelInvoke();
			StartCoroutine("DisplayEffectHud",pinIcon);
		}*/
		pinEffectDisplayHUD.SetActive(true);
		pinEffectDisplayHUD.GetComponent<Image>().sprite = pinIcon;
		pinEffectDisplayHUD.GetComponent<GUIEffects>().Start();
		yield return new WaitForSeconds(2.5f);
		if(displayHudCalledAgain == 1){
			pinEffectDisplayHUD.SetActive(false);
			displayHudCalledAgain = 0;
		}else{
			displayHudCalledAgain--; // display hud activation goes down. Theoretically, should go down for each one called until it reaches 1
		}
	}

	void SetDelay(float thisDelay){
		delay = thisDelay;
	}


	IEnumerator KillObject(GameObject objToKill){
		
		yield return new WaitForSeconds(delay);

		objToKill.SetActive(false);
	}
	

}
