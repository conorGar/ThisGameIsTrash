using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using I2.TextAnimation;

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

	float delayBonus = 1.0f;

	public float secondsInTheDay = 240f;
    public float secondsPassed = 0f;
    public int timeDeathIncrease = 20;
	int finalCountdownNumber = 10;
	bool halfWayMark;

	void Start () {
		GlobalVariableManager.Instance.TIME_IN_DAY = 0;
		dayIcon.transform.localPosition = startPos.transform.localPosition;
		dayNumberDisplay.text = "Day: "+ GlobalVariableManager.Instance.DAY_NUMBER;
        secondsPassed = 0f;
    }//end of Start()

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
		SoundManager.instance.PlaySingle(halfWayDoneChime);
	}

}
