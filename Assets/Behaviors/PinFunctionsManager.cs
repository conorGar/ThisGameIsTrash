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
	//public PinManager pinManager;
	public Sprite[] displaySprites;
	Sprite displaySprite;
	int displayHudCalledAgain;

	public void Awake(){
		if(inWorld){
			if(GlobalVariableManager.Instance.IsPinEquipped(PIN.BULKYBAG)){
				GlobalVariableManager.Instance.BAG_SIZE += 2; //subtracted again at results.cs
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
