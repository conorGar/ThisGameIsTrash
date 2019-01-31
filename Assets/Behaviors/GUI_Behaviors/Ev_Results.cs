using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class Ev_Results : MonoBehaviour {

	public TextMeshProUGUI trashCollected;
	public TextMeshProUGUI largeTrashCollected;
	public TextMeshProUGUI totalTrash;
	//public Text enemiesDefeated;
	//public TextMeshProUGUI nextUnlockNeeded;
	public TextMeshProUGUI currentStars;
	//public GameObject largeTrashTextDisplay;
	//public GameObject treasureCollectedDisplay;
	//public GameObject largeTrashCollectedDisplay;
	//public LargeTrashManager ltManager;
	public int currentWorld; //needed for largeTrashManager
    public Image backPaper;
    public Image image;
    public ParticleSystem clouds;
    public AudioClip resultsMusic;
    public AudioClip closeSfx;
    public GameObject starDisplay;
    public int startIndexOfLargeTrash;
    public Sprite filledStarSprite;


	int trashCollectedValue;
	int phase = 0;
	int spawnLargeTrashOnce = 0;
	int displayIndex = 0;


	void Start () {
        GameStateManager.Instance.RegisterChangeStateEvent(OnChangeState);
        // TODO: Unity doesn't run start on disabled gameobjects because it's lame and weird.  Is there a better way to do this
        // Then starting active and disabling it immediately?
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        for (int i = 0; i < GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count; i++) {
            if (i != 1) {
                //^ doesnt count scrap
                trashCollectedValue += GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[i];
            }
        }

        for (int i = 0; i < GlobalVariableManager.Instance.LARGE_TRASH_LIST.Count; i++) {
            // Add trash to the discover list and award a star for each.
            GlobalVariableManager.Instance.LARGE_GARBAGE_DISCOVERED |= GlobalVariableManager.Instance.LARGE_TRASH_LIST[displayIndex].type;
            GlobalVariableManager.Instance.STAR_POINTS_STAT.UpdateMax(+1);
            GlobalVariableManager.Instance.STAR_POINTS_STAT.UpdateCurrent(+1);
        }

        largeTrashCollected.text = "+" + GlobalVariableManager.Instance.LARGE_TRASH_LIST.Count;
        currentStars.text = GlobalVariableManager.Instance.STAR_POINTS_STAT.GetMax().ToString();

        totalTrash.text = "/" + GlobalVariableManager.Instance.TOTAL_TRASH.ToString();
        trashCollected.text = "+" +trashCollectedValue;

       /* if (GlobalVariableManager.Instance.PROGRESS_LV == 0) {
            nextUnlockNeeded.text = "/2";
        }
        else if (GlobalVariableManager.Instance.PROGRESS_LV == 1) {
            nextUnlockNeeded.text = "/5";
        }
        else if (GlobalVariableManager.Instance.PROGRESS_LV == 2) {
            nextUnlockNeeded.text = "/10";
        }
        /*if (GlobalVariableManager.Instance.WORLD_ROOM_DISCOVER.Count > 5) {
            //in case player dies during race
            GlobalVariableManager.Instance.WORLD_ROOM_DISCOVER.RemoveAt(5);
        }*/

	

    }

    void OnDestroy()
    {
        GameStateManager.Instance.UnregisterChangeStateEvent(OnChangeState);
    }

    void OnChangeState(System.Type stateType, bool isEntering )
    {
        if (isEntering) {
            if (stateType == typeof(EndDayState)) {
                gameObject.SetActive(true);
				for(int i = 0; i < starDisplay.transform.childCount;i++){
					LARGEGARBAGE largeGarbageType = LargeGarbage.ByIndex(i);
            		Debug.Log(LargeGarbage.ByIndex(i).ToString());
           			if ((GlobalVariableManager.Instance.LARGE_GARBAGE_DISCOVERED & largeGarbageType) == largeGarbageType || (GlobalVariableManager.Instance.LARGE_GARBAGE_VIEWED & largeGarbageType) == largeGarbageType) {
            			starDisplay.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = filledStarSprite;
					}
			}
                CamManager.Instance.mainCamEffects.ZoomInOut(.7f,.5f);
				SoundManager.instance.backupMusicSource.clip = resultsMusic;
				SoundManager.instance.backupMusicSource.volume = GlobalVariableManager.Instance.MASTER_MUSIC_VOL;
				SoundManager.instance.backupMusicSource.Play();
				SoundManager.instance.musicSource.Stop();
                StartCoroutine("InteractDelay"); // wait for the truck to get a bit up the road.
            }
        }
    }

    void Update () {
		if(GameStateManager.Instance.GetCurrentState() == typeof(EndDayState)) {
            if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)) {
                if (phase == 2) {

						FriendManager.Instance.OnWorldEnd();
						SoundManager.instance.backupMusicSource.Stop();
                       // largeTrashTextDisplay.SetActive(false);
                        backPaper.enabled = true;
                        image.enabled = true;
                        GlobalVariableManager.Instance.ENEMIES_DEFEATED = 0;
                        for (int i = 0; i < GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count; i++) {
                            GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[i] = 0;
                        }
                        //GlobalVariableManager.Instance.CURRENT_HP = 0;
                 
                        GlobalVariableManager.Instance.MENU_SELECT_STAGE = 1;
                        if (GlobalVariableManager.Instance.DEJAVUCOUNT - 3 <= 0) {
                            //Deja Vu pin
                            GlobalVariableManager.Instance.DAY_NUMBER++;
                        }
                        else {
                            GlobalVariableManager.Instance.DEJAVUCOUNT--;
                        }


                        //-------Reset today's trash collected---------//
                        if (GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count > 3) {
                            if (GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count > 4) {
                                GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.RemoveAt(4);
                            }
                            GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.RemoveAt(3);
                        }
                        LargeTrashManager.Instance.DisableProperTrash(currentWorld);
                        FriendManager.Instance.DisableAllFriends();
                        //FriendManager.Instance.DisableFriends(currentWorld);

                        //---------------------------------------------//
                        GlobalVariableManager.Instance.ARROW_POSITION = 1;

                        UserDataManager.Instance.SetDirty();
                        GameStateManager.Instance.PopAllStates();

						if (GlobalVariableManager.Instance.DAY_NUMBER == 2) { // was 2
                            GameObject.Find("fadeHelper").GetComponent<Ev_FadeHelper>().FadeToScene("Hub");//supposed to be intro credits scene, changed for testing
                        }
                        else if (GlobalVariableManager.Instance.DAY_NUMBER >= 10) {
							if(GlobalVariableManager.Instance.LARGE_TRASH_COLLECTED < 3){
                           		 GameObject.Find("fadeHelper").GetComponent<Ev_FadeHelper>().FadeToScene("DemoEndRoom"); //if havent collected enough large trash by the demos end, game ends
                           	}else{
								GameObject.Find("fadeHelper").GetComponent<Ev_FadeHelper>().FadeToScene("Hub");
                           	}
                        }
                        else {
                            GameObject.Find("fadeHelper").GetComponent<Ev_FadeHelper>().FadeToScene("Hub");
                        }

                        SoundManager.instance.PlaySingle(closeSfx);
                        // Fading needs time to pass.
                        Time.timeScale = 1f;
                    
                }// end of phase = 2 check
            }
		}
	}//end of update

	IEnumerator InteractDelay(){
		yield return new WaitForSeconds(.3f);
		clouds.gameObject.SetActive(true);
        clouds.Play();
		yield return new WaitForSeconds(1f);
        //Time.timeScale = 0f;  //took this out because large trash display wasnt working well with it, needed?
		phase = 2;
	}


}
