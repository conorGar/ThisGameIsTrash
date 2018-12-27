using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PostProcessing;

public class GUI_LargeTrashCollectedDisplay : MonoBehaviour
{
	int phase = 1;
	public GameObject starDisplay;
	public GameObject trashIcon;
	public GameObject trashTitlePaper;
	public PostProcessingProfile dialogBlur;

	public AudioClip trashDisplayChime;
	public AudioClip continueSfx;
	public AudioClip starFillSfx;

	int indexOfCurrentLargeTrash;
	int displayIndex;
	// Use this for initialization
	void Start ()
	{
	
	}

	void OnEnable(){
		CamManager.Instance.mainCamPostProcessor.profile = dialogBlur;
		displayIndex = GlobalVariableManager.Instance.LARGE_TRASH_LIST.Count-1;
		phase = 1;
		trashIcon.SetActive(true);
		trashTitlePaper.SetActive(true);
		trashIcon.GetComponent<Image>().sprite = (GlobalVariableManager.Instance.LARGE_TRASH_LIST[displayIndex].collectedDisplaySprite);
		trashTitlePaper.GetComponent<TextMeshProUGUI>().text = GlobalVariableManager.Instance.LARGE_TRASH_LIST[displayIndex].collectedTitle;
		SoundManager.instance.PlaySingle(trashDisplayChime);
		trashIcon.GetComponent<Animator>().Play("largeTrashCollected",-1,0f);

	}

	
	// Update is called once per frame
	void Update ()
	{
		if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)){
			if(phase == 1){
				SoundManager.instance.PlaySingle(continueSfx);
				StartCoroutine("NewStarFill");
				starDisplay.SetActive(true);
				for(int i = 0; i < starDisplay.transform.childCount;i++){
					LARGEGARBAGE largeGarbageType = LargeGarbage.ByIndex(i);
            		Debug.Log(LargeGarbage.ByIndex(i).ToString());
           			if ((GlobalVariableManager.Instance.LARGE_GARBAGE_DISCOVERED & largeGarbageType) == largeGarbageType || (GlobalVariableManager.Instance.LARGE_GARBAGE_VIEWED & largeGarbageType) == largeGarbageType) {
            			starDisplay.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().color = Color.white;
					}
				}
				StartCoroutine("NewStarFill");
				phase = 2;

			}else if(phase == 3){
				SoundManager.instance.PlaySingle(continueSfx);

				Close();
			}
		}
	}

	IEnumerator NewStarFill(){
		trashIcon.SetActive(false);
		trashTitlePaper.SetActive(false);
		yield return new WaitForSeconds(1f);
		starDisplay.transform.GetChild(indexOfCurrentLargeTrash).gameObject.GetComponent<Animator>().enabled = true;
		SoundManager.instance.PlaySingle(starFillSfx);
		GameObject stars = ObjectPool.Instance.GetPooledObject("effect_GUISelectStars",starDisplay.transform.GetChild(indexOfCurrentLargeTrash).position);
		starDisplay.transform.GetChild(indexOfCurrentLargeTrash).gameObject.GetComponent<Animator>().Play("largeTrashDisplay_starFill",-1,0);
		yield return new WaitForSeconds(1.5f);
		ObjectPool.Instance.ReturnPooledObject(stars);
		phase = 3;
	}

	void Close(){
		GameStateManager.Instance.PopState();
		starDisplay.SetActive(false);
		this.gameObject.SetActive(false);
		CamManager.Instance.mainCamPostProcessor.profile = null;
	}


}

