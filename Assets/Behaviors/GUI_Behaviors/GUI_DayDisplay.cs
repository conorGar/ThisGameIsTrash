using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using I2.TextAnimation;
public class GUI_DayDisplay : MonoBehaviour
{
	public GameObject truck;
	public TextMeshProUGUI dayNumDisplay;
	public GameObject back;
	public GameObject player;
	public GameObject playerDummy;
	public bool forHub;
	public AudioClip truckSmokeSfx;
	public AudioClip truckDoor;
	public AudioClip truckSfx;
	public GameObject worldTitle;
	public GameObject underline;
	//public AudioClip typeSound;

	public GameObject demoGoalDisplay;
	public GameObject Hud;
	int phase = 0;

	// Use this for initialization
	void Start ()
	{
		Hud.SetActive(false);
		if(GlobalVariableManager.Instance.DAY_NUMBER == 1){
			truck.SetActive(false);

			dayNumDisplay.gameObject.SetActive(false);
			StartCoroutine("DemoGoalDisplay");
		}else{

			if(!forHub){
			dayNumDisplay.text = "- Day " + GlobalVariableManager.Instance.DAY_NUMBER.ToString() + " -";
			dayNumDisplay.GetComponent<TextAnimation>().PlayAnim();
				SoundManager.instance.musicSource.clip = SoundManager.instance.worldMusic;
    			SoundManager.instance.musicSource.volume = GlobalVariableManager.Instance.MASTER_MUSIC_VOL;
    			SoundManager.instance.musicSource.Play();
			}
			StartCoroutine("DisplaySequence");
			//SoundManager.instance.PlaySingle(truckSfx);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(phase == 1){
			if(forHub){
				truck.transform.position = Vector2.MoveTowards(truck.transform.position,new Vector2(13f,15f),13*Time.deltaTime);

			}else{
				truck.transform.position = Vector2.MoveTowards(truck.transform.position,new Vector2(.1f,8.2f),13*Time.deltaTime);
			
			}

		}else if(phase == 3){
			if(forHub){
				truck.transform.Translate(Vector3.left);
			}else{
				truck.transform.position = Vector2.MoveTowards(truck.transform.position,new Vector2(27f,8.2f),24*Time.deltaTime);
			}
		}
	}

	IEnumerator TruckEnter(){
		InvokeRepeating("SmokePuffSfx",.1f,.5f);
		phase = 1;
		if(forHub){
			yield return new WaitUntil(() => truck.transform.localPosition.x < 0f);

		}else{
			yield return new WaitUntil(() => truck.transform.localPosition.x > -1.3f);

		}
		Debug.Log("Day display got to this point...");
		phase = 2;


		StartCoroutine("TruckLeave");        
	}

	IEnumerator TruckLeave(){
		playerDummy.SetActive(true);
		SoundManager.instance.PlaySingle(truckDoor);
		yield return new WaitForSeconds(.3f);
		phase = 3;
		yield return new WaitForSeconds(.6f);
		Hud.SetActive(true);
		back.GetComponent<Animator>().enabled = true;
		back.GetComponent<SpriteRenderer>().enabled = false;
		playerDummy.SetActive(false);
		yield return new WaitForSeconds(.5f);
		CancelInvoke();

        // Leaving pop up state.
        GameStateManager.Instance.PopAllStates();
        GameStateManager.Instance.PushState(typeof(GameplayState));

        yield return new WaitForSeconds(1.5f);

        gameObject.SetActive(false);
	}

	void SmokePuffSfx(){
		SoundManager.instance.PlaySingle(truckSmokeSfx);
	}

	IEnumerator DisplaySequence(){
		
		if(!forHub){
			underline.SetActive(true);
			yield return new WaitForSeconds(.3f);
			worldTitle.SetActive(true);
			truck.SetActive(true);
			dayNumDisplay.gameObject.SetActive(true);
			dayNumDisplay.text = "- Day " + GlobalVariableManager.Instance.DAY_NUMBER.ToString() + " -";
			dayNumDisplay.GetComponent<TextAnimation>().PlayAnim();
		}
			StartCoroutine("TruckEnter");
			SoundManager.instance.PlaySingle(truckSfx);
	
	}

	IEnumerator DemoGoalDisplay(){
        GameStateManager.Instance.PushState(typeof(PopupState));
        PlayTalkSounds();
		demoGoalDisplay.SetActive(true);
		yield return new WaitForSeconds(2f);
		CancelInvoke("TalkSound");
		yield return new WaitForSeconds(3f);
		truck.SetActive(true);
		//Hud.SetActive(true);
		demoGoalDisplay.SetActive(false);
		yield return new WaitForSeconds(.3f);

		underline.SetActive(true);
		yield return new WaitForSeconds(.3f);
		worldTitle.SetActive(true);
		SoundManager.instance.musicSource.clip = SoundManager.instance.worldMusic;
    	SoundManager.instance.musicSource.volume = GlobalVariableManager.Instance.MASTER_MUSIC_VOL;
    	SoundManager.instance.musicSource.Play();
		dayNumDisplay.gameObject.SetActive(true);
		if(!forHub){
			dayNumDisplay.text = "- Day " + GlobalVariableManager.Instance.DAY_NUMBER.ToString() + " -";
			dayNumDisplay.GetComponent<TextAnimation>().PlayAnim();
		}
		StartCoroutine("TruckEnter");
		SoundManager.instance.PlaySingle(truckSfx);
	}


	private void PlayTalkSounds()
    {
        CancelInvoke();
        InvokeRepeating("TalkSound", 0.1f, .05f);
    }

    private void TalkSound(){
		SoundManager.instance.RandomizeSfx(SFXBANK.VOICE_TICK,.8f,1.2f);
	}
}

