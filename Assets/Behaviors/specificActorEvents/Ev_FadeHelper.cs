using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; //needed for white flash
public class Ev_FadeHelper : MonoBehaviour {

	public GameObject smallTruck;
	public GameObject upgradeActorTempEffects;
	public GameObject fader;
	public GameObject currentCanvas;
	public Texture2D fadeOutTexture; //texture that will overlay screen(black image)
	public float fadeSpeed;
	public PinFunctionsManager pfm;

	float alpha = 0f;
	int specialFade = 0;
	int signFade = 0;

	int fadeColor = 0;
	int whiteFlashOpacity = 0;
	int opacity = 0;
	float fadeMaxStrength = .7f;
	Color alphaColor;
	public bool fading;
	public bool fadeBack;
	GameObject tempFlash;
	Image faderImage;

	void Start(){
		faderImage = fader.GetComponent<Image>();
		//FadeToScene("IntroCredits");
	}

	void OnGUI (){
		/*alpha += specialFade * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp(alpha, 0, fadeMaxStrength); //force the alpha to stay between 0 and 1

		//set color of texture
		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = -100; // a lower number means draw on top
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), fadeOutTexture );
		*/
	}
	void Update(){
		if(whiteFlashOpacity > 0){
			tempFlash.GetComponent<Image>().color = Color.Lerp(tempFlash.GetComponent<Image>().color ,alphaColor,2f*Time.deltaTime);

		}else{
			Destroy(tempFlash);
		}

		if(fading){
			faderImage.color = Color.Lerp(faderImage.color,alphaColor,7f*Time.deltaTime);
		}else if(fadeBack){
			faderImage.color = Color.Lerp(faderImage.color,alphaColor,10f*Time.deltaTime);
		}
	}

	public void FadeIn(){
		fading = false;
		alphaColor = faderImage.color;
		alphaColor.a = 0f;
		fadeBack = true;
	}



	public void WhiteFlash(){
		//GameObject currentCanvas = GameObject.Find("Canvas").gameObject;
		//tempFlash = Instantiate(whiteFlash,currentCanvas.transform.position,Quaternion.identity);
		tempFlash.transform.parent = currentCanvas.transform;
		alphaColor = tempFlash.GetComponent<Image>().color;
		alphaColor.a = 0f;
		whiteFlashOpacity = 100;
	}

	public void BlackFade(){
		
		fading = true;
		alphaColor = faderImage.color;
		alphaColor.a = 1f;
		fader.SetActive(true);
	}

	public IEnumerator Fade(){
		specialFade = 1;
		yield return new WaitForSeconds(.5f);
		specialFade = 0;
	}

	public void EndOfDayFade(){
		Debug.Log("End of Day fade activated");
		if(specialFade != 1){
			specialFade = 1;
			//player.GetComponent<Ev_Jim>().CantPause();
//			if(GlobalVariableManager.Instance.CURRENT_HP > 0){
				//GlobalVariableManager.Instance.ROOM_PLAYER_DIED_IN = 99;
		//	}

			GameObject truckInstance = Instantiate(smallTruck,new Vector2(CamManager.Instance.mainCam.transform.position.x - 5, PlayerManager.Instance.player.transform.position.y), Quaternion.identity);
			truckInstance.GetComponent<Ev_SmallTruck>().EndDay();
	
		}
	}

	public void ReturnToDumpster(){
		Debug.Log("Truck- return to dumpster activated");
		GameObject truckInstance = Instantiate(smallTruck,new Vector2(CamManager.Instance.mainCam.transform.position.x - 5, PlayerManager.Instance.player.transform.position.y), Quaternion.identity);

		if(GlobalVariableManager.Instance.IsPinEquipped(PIN.DEVILSDEAL)){
				//'Devil's Deal' Pin equipped
					GameObject effectInstance;
					effectInstance = Instantiate(upgradeActorTempEffects,new Vector2(0f,0f),Quaternion.identity);
				//	effectInstance.GetComponent<Ev_UpgradeActorTempEffects>().Follow();
			
				truckInstance.GetComponent<Ev_SmallTruck>().EndDay();
		}else{
			truckInstance.GetComponent<Ev_SmallTruck>().ReturnToDumpster();
		}
	}

	public void SignFade(){
		fadeColor = 0;
		signFade = 1;
	}

	public void SignFadeReturn(){
		signFade = 0;
	}

	public void FadeToScene(string sceneName){
		SoundManager.instance.FadeMusic();
		Initiate.Fade(sceneName,Color.black,0.5f);

	}
	public void JumpToScene(string sceneName){
		SceneManager.LoadScene(sceneName);
	}
}
