using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using I2.TextAnimation;
using UnityEngine.UI;


public class Ev_DayMeter : MonoBehaviour {
	public GameObject dayIcon;
	public TextMeshProUGUI countdownNumber;
	public GameObject fadeHelper;
	public GameObject tutPopup;
    public GameObject startPos;
    public GameObject endPos;
	public AudioClip countdownTick;
	public TextMeshProUGUI dayNumberDisplay;
	public ParticleSystem halfWayDonePS;
	public AudioClip halfWayDoneChime;
	public Sprite nightIcon;
	public Image dayColorTint;


	float delayBonus = 1.0f;
	Color sunsetColor = new Color(.85f,.64f,.18f,.13f);
	Color nightColor = new Color(.18f,.26f,.85f,.13f);
	Color startColor;

	public float secondsInTheDay = 240f;
    public float secondsPassed = 0f;
    public int timeDeathIncrease = 20;
	int finalCountdownNumber = 10;
	bool halfWayMark;
	bool nightMark;

	void Start () {
		GlobalVariableManager.Instance.TIME_IN_DAY = 0;
		dayIcon.transform.localPosition = startPos.transform.localPosition;
		dayNumberDisplay.text = "Day: "+ GlobalVariableManager.Instance.DAY_NUMBER;
        secondsPassed = 0f;
        startColor = dayColorTint.color;

        GameStateManager.Instance.RegisterChangeStateEvent(OnChangeState);
    }//end of Start()

    void OnDestroy()
    {
        GameStateManager.Instance.UnregisterChangeStateEvent(OnChangeState);
    }

    void Update()
    {
        Type state = GameStateManager.Instance.GetCurrentState();
        if (state == typeof(GameplayState) || state == typeof(DethKlokState)) {
            // Add the change in time but only in the states that are allowed to pass time.
            // If it's the deathclock state, time moves at an accelerated rate.
            secondsPassed += GameStateManager.Instance.GetCurrentState() == typeof(GameplayState) ? Time.deltaTime
                : Time.deltaTime * timeDeathIncrease;
            secondsPassed = Mathf.Min(secondsPassed, secondsInTheDay * delayBonus);

            // Update day icon position
            dayIcon.transform.localPosition = Vector3.Lerp(startPos.transform.localPosition, endPos.transform.localPosition, secondsPassed / secondsInTheDay * delayBonus);

            // Update time of day in seconds.
            GlobalVariableManager.Instance.TIME_IN_DAY = Mathf.Min(Mathf.RoundToInt(secondsPassed), Mathf.RoundToInt(secondsInTheDay * delayBonus));

            //change Color
            if(dayColorTint !=null) //would be null for the day meter shown at player death
				dayColorTint.color = Color.Lerp(startColor, nightColor,secondsPassed / secondsInTheDay * delayBonus);


            // Trigger events based on the time.
            if (state == typeof(GameplayState)) {
                if (secondsPassed < secondsInTheDay * delayBonus) {
                    //Debug.Log("Seconds Passed: " + GlobalVariableManager.Instance.TIME_IN_DAY);
                    if (secondsPassed >= secondsInTheDay * delayBonus - finalCountdownNumber) {
                        Debug.Log("Trigger final countdown number: " + finalCountdownNumber);
                        countdownNumber.gameObject.SetActive(true);

                        SoundManager.instance.RandomizeSfx(countdownTick, .7f, 1.3f);
                        countdownNumber.text = finalCountdownNumber.ToString();
                        countdownNumber.color = new Color(152f, 152f, 152f, .64f);
                        countdownNumber.gameObject.GetComponent<TextAnimation>().PlayAnim(0);
                        finalCountdownNumber--;

                        //InvokeRepeating("FinalCountdown", .1f, 2f + (delayBonus * 2));
                    }
                }
                else {
                    Debug.Log("*** END DAY FADE ACTIVATE ***");
                    fadeHelper.GetComponent<Ev_FadeHelper>().EndOfDayFade();
                    this.enabled = false;
                }

                if (secondsPassed > (secondsInTheDay * delayBonus / 2) && !halfWayMark) {
                    Debug.Log("HALFWAY Seconds Passed: " + GlobalVariableManager.Instance.TIME_IN_DAY);
                    HalfWay();
                }

				if(secondsPassed > (secondsInTheDay * delayBonus / 1.3f) && !nightMark){
					NightMark();
				}

            }
        }
    }

	public void Slowdown(){
		//For 'Workin' Hard' Pin
		delayBonus = 1.1f;
	}

    // Helpers
	void HalfWay(){
		halfWayMark = true;
		halfWayDonePS.Play();
		dayIcon.GetComponent<Animator>().Play("dayChangeEmphasis",-1,0f);
		SoundManager.instance.PlaySingle(halfWayDoneChime);
	}

	void NightMark(){
		nightMark = true;
		dayIcon.GetComponent<Image>().sprite= nightIcon;
		dayIcon.GetComponent<Animator>().Play("dayChangeEmphasis",-1,0f);
		SoundManager.instance.PlaySingle(halfWayDoneChime);

	}

    void OnChangeState(System.Type stateType, bool isEntering)
    {
        // Show only if the current state is gameplay.
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (countdownNumber != null) {
                countdownNumber.GetComponent<CanvasRenderer>().SetAlpha(1f);
            }
        } else {
            if (countdownNumber != null) {
                countdownNumber.GetComponent<CanvasRenderer>().SetAlpha(0f);
            }
        }
    }
}
