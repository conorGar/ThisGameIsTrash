using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using I2.TextAnimation;

public class IntroManager : MonoBehaviour {

	public List<GameObject> slides;
	int slideNumber = 1;
	int skipNumber;
	public TextMeshProUGUI textShown;
	public Animator fader;
	public AudioClip music;
	public AudioClip typeSfx;
	public GameObject skipDisplay;


	// Use this for initialization
	void Start () {
		StartCoroutine(SkipTimer());
	}
	
	// Update is called once per frame
	void Update () {
		if(ControllerManager.Instance.GetKeyDown(INPUTACTION.PAUSE) || ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)){
			Debug.Log("Skip Pressed" + skipNumber);
			if(skipNumber == 0){
				skipDisplay.SetActive(true);
				skipNumber = 1;
				StartCoroutine(SkipTimer());
			}else if(skipNumber == 1){	
				SoundManager.instance.FadeMusic();
				Initiate.Fade("TitleScreen",Color.black,0.5f);
			}
		} 
	}

	public void NextSlide(){
		if(slides[slideNumber].name != slides[slideNumber-1].name){// dont disable if it's the same slide(and just change the text...)
			slides[slideNumber-1].SetActive(false);
			slideNumber++;
			slides[slideNumber-1].SetActive(true);
			fader.SetTrigger("Fade");
		}else{
			slideNumber++;
		}
		if(slideNumber == 1){
			textShown.text = "Beauty. Power. Majesty.";
		}else if(slideNumber == 2){
			textShown.text = "These are the words that describe the modern garbageman.";
		}else if(slideNumber == 3){
			textShown.text = "Hailed as the heroes of our time, they protect the world from the gross, the filthy, and the ratty.";
		}else if(slideNumber == 4){
			textShown.text = "In doing so, the world rewards them with all the luxuries it has available.";
		}else if(slideNumber == 5){

			textShown.text = "They get The MONEY!";
			/*textShown.gameObject.GetComponent<TextAnimation>().PlayAnim(2)._Sequences[0]._TargetRangeStart = textShown.text.IndexOf("NOBODY");
			textShown.gameObject.GetComponent<TextAnimation>().PlayAnim(2)._Sequences[0]._TargetRangeStart = 17;
			textShown.gameObject.GetComponent<TextAnimation>().PlayAnim(2)._Sequences[1]._TargetRangeAmount = textShown.text.IndexOf("NOBODY");
			textShown.gameObject.GetComponent<TextAnimation>().PlayAnim(2)._Sequences[1]._TargetRangeAmount = 17;
			textShown.gameObject.GetComponent<TextAnimation>().PlayAnim(2)._Sequences[0]._TargetRangeStart = textShown.text.IndexOf("NOBODY");
			textShown.gameObject.GetComponent<TextAnimation>().PlayAnimation(2);*/
		}else if(slideNumber == 6){
			textShown.text = "They get The FAME!";
		}else if(slideNumber == 7){
			textShown.text = "They get The WOMEN!";
		}else if(slideNumber == 8){
			textShown.text = "...Or MEN!";
		}else if(slideNumber == 9){
			textShown.text = "...Or whatever they were into!<color=#DC3232> NOBODY JUDGED THEM!</color>";
		}else if(slideNumber == 10){
			textShown.text = "And every year, the best amoung them was chosen and given the prestigious <color=#78FF32>Garbageman Of The Year Award</color>.";
		}else if(slideNumber == 11){
			textShown.text = "Because of all this, many go out to try their hand at becoming a garbagemen...to follow their dumpster dreams...";
		}else if(slideNumber == 12){
			textShown.text = "...many fail.";
		}else if(slideNumber == 13){
			textShown.text = "But you, <color=#78FF32>Jim</color>, hope to succeed where others have failed. You have the heart. The passion. The F I R E!";
		}else if(slideNumber == 14){
			textShown.text = "You'll need it too. The trash of the Earth has gotten so out of hand that its creatures have gone crazy.";
		}else if(slideNumber == 15){
			textShown.text = "Angry at the humans for trashing their homes.";
		}else if(slideNumber == 16){
			textShown.text = "Worse yet, a malicious group calling themselves <color=#DC3232>White Trash</color> are rising to power.";
		}else if(slideNumber == 17){
			textShown.text = "They hope to set up their mysterious leader, known only as <color=#DC3232>The Greasy Prince</color>, to be the winner of the Garbageperson Of The Year Award!";
		}else if(slideNumber == 18){
			textShown.text = "And with only <color=#78FF32>30 days until the award ceremony</color>, will you really have the time to make your mark on the trashy world?!?";
		}else if(slideNumber == 19){
			textShown.text = "Or will you be forced to face the reality that you're just not cut out to be a garbageman...";
		}else if(slideNumber == 20){
			slides[slideNumber-1].GetComponent<ChangingSlide>().changeDelay = 3;
			textShown.gameObject.GetComponent<TextAnimation>().PlayAnim(1);//fast type
			textShown.text = "And all your dreams will die!";
		}else if(slideNumber == 21){
			slides[slideNumber-1].GetComponent<ChangingSlide>().changeDelay = 6;
			textShown.text = "And you'll have wasted all this time!";
		}else if(slideNumber == 22){
			slides[slideNumber-1].GetComponent<ChangingSlide>().changeDelay = 6;

			textShown.text = "And all your other friends will have real jobs like an engineer or doctor or something!";
		}else if(slideNumber == 23){
			slides[slideNumber-1].GetComponent<IntroSlide>().camPanSpeed = 2;

			textShown.text = "AND YOU'LL REALIZE YOUR DAD WAS RIGHT AND THAT YOU SHOULD'VE JUST WENT TO GRAD SCHOOL!";
		}else if(slideNumber == 24){
			slides[slideNumber-1].GetComponent<ChangingSlide>().changeDelay = 3;

			textShown.text = "AND YOU'LL ENTRENCH YOURSELF IN A NEVERENDING SPIRAL OF EXISTENTIAL DREAD! FOREVER!";
		}else if(slideNumber == 25){
			textShown.text = "...";
		}else if(slideNumber == 26){
			textShown.text = "...or maybe not. I don't know. Let's see!";
		}else if(slideNumber == 27){
			textShown.text = " ";
			SoundManager.instance.FadeMusic();
			Initiate.Fade("TitleScreen",Color.black,0.5f);
			textShown.text = "";
		}

	
		textShown.gameObject.GetComponent<TextAnimation>().PlayAnim();
		PlayTalkSounds();
		Debug.Log("Fade");


	}

	IEnumerator SkipTimer(){
		yield return new WaitForSeconds(3f);
		if(skipNumber == 1){
			skipDisplay.SetActive(false);
			skipNumber = 0;
		}
	}

	private void PlayTalkSounds()
    {
        CancelInvoke();
        InvokeRepeating("TalkSound", 0.1f, .05f);
    }

    private void TalkSound(){
		SoundManager.instance.RandomizeSfx(typeSfx,.8f,1.2f);
	}

	public void FinishedDisplay(){
		Debug.Log("finish display activaed");
			CancelInvoke();

	}
}
