using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class Ev_Results : MonoBehaviour {

	public Text trashCollected;
	public Text largeTrashCollected;
	public Text enemiesDefeated;
	public TextMeshProUGUI nextUnlockNeeded;
	public TextMeshProUGUI currentStars;
	public GameObject largeTrashTextDisplay;
	public GameObject treasureCollectedDisplay;
	public GameObject largeTrashCollectedDisplay;
	public LargeTrashManager ltManager;
	public int currentWorld; //needed for largeTrashManager
    public Image backPaper;
    public Image image;

    public AudioClip largeTrashCollectedSfx;

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
            GlobalVariableManager.Instance.STAR_POINTS++;
        }

        currentStars.text = GlobalVariableManager.Instance.STAR_POINTS.ToString();
        if (GlobalVariableManager.Instance.PROGRESS_LV == 0) {
            nextUnlockNeeded.text = "/2";
        }
        else if (GlobalVariableManager.Instance.PROGRESS_LV == 1) {
            nextUnlockNeeded.text = "/5";
        }
        else if (GlobalVariableManager.Instance.PROGRESS_LV == 2) {
            nextUnlockNeeded.text = "/10";
        }
        if (GlobalVariableManager.Instance.WORLD_ROOM_DISCOVER.Count > 5) {
            //in case player dies during race
            GlobalVariableManager.Instance.WORLD_ROOM_DISCOVER.RemoveAt(5);
        }
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
                StartCoroutine("InteractDelay"); // wait for the truck to get a bit up the road.
            }
        }
    }

    void Update () {
		if(GameStateManager.Instance.GetCurrentState() == typeof(EndDayState)) {
            if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)) {
                if (phase == 2) {
                    if (GlobalVariableManager.Instance.LARGE_TRASH_LIST.Count > displayIndex) {

                        if (spawnLargeTrashOnce == 0) {
                        	Debug.Log("L trash display got here - 1");
                            //spawn large trash collected display
                            //treasureCollectedDisplay.SetActive(true);
                            largeTrashCollectedDisplay.SetActive(true);
                            backPaper.enabled = false;
                            image.enabled = false;
                            largeTrashTextDisplay.SetActive(true);
                            spawnLargeTrashOnce = 1;
                        }
                        else {
							Debug.Log("L trash display got here - 2");

                            treasureCollectedDisplay.GetComponent<Animator>().Play("largeTrashCollected",-1,0f);
                        }

                        SoundManager.instance.PlaySingle(largeTrashCollectedSfx);

                        //change sprite of the large trash display
                        treasureCollectedDisplay.GetComponent<Image>().sprite = (GlobalVariableManager.Instance.LARGE_TRASH_LIST[displayIndex].collectedDisplaySprite);
                        //	treasureCollectedDisplay.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(); //TODO: working on this..
						largeTrashTextDisplay.GetComponent<TextMeshProUGUI>().text = GlobalVariableManager.Instance.LARGE_TRASH_LIST[displayIndex].collectedTitle;
                        displayIndex++;
                    }
                    else {
                        largeTrashTextDisplay.SetActive(false);
                        backPaper.enabled = true;
                        image.enabled = true;
                        GlobalVariableManager.Instance.ENEMIES_DEFEATED = 0;
                        for (int i = 0; i < GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count; i++) {
                            GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[i] = 0;
                        }
                        //GlobalVariableManager.Instance.CURRENT_HP = 0;
                        if (GlobalVariableManager.Instance.WORLD_ROOM_DISCOVER.Count > 4)
                            GlobalVariableManager.Instance.WORLD_ROOM_DISCOVER.RemoveAt(4); // remove temporary room discover string
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
                                //resets hp after cassie gives you bonus
                                GlobalVariableManager.Instance.Max_HP -= 1;
                                GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.RemoveAt(4);
                            }
                            GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.RemoveAt(3);
                        }
                        LargeTrashManager.Instance.DisableProperTrash(currentWorld);
                        FriendManager.Instance.DisableAllFriends();
                        //FriendManager.Instance.DisableFriends(currentWorld);

                        //---------------------------------------------//
                        GlobalVariableManager.Instance.ARROW_POSITION = 1;


                        if (GlobalVariableManager.Instance.IsPinEquipped(PIN.BULKYBAG)) {
                            GlobalVariableManager.Instance.BAG_SIZE -= 2;
                        }

                        UserDataManager.Instance.SetDirty();
                        GameStateManager.Instance.PopAllStates();

                        if (GlobalVariableManager.Instance.DAY_NUMBER == 2) {
                            GameObject.Find("fadeHelper").GetComponent<Ev_FadeHelper>().FadeToScene("Hub");//supposed to be intro credits scene, changed for testing
                        }
                        else if (GlobalVariableManager.Instance.DAY_NUMBER == 3) {
                            //StartCoroutine("HomelessHarry"); TODO
                            GameObject.Find("fadeHelper").GetComponent<Ev_FadeHelper>().FadeToScene("Hub");
                        }
                        else {
                            GameObject.Find("fadeHelper").GetComponent<Ev_FadeHelper>().FadeToScene("Hub");
                        }

                        // Fading needs time to pass.
                        Time.timeScale = 1f;
                    }
                }// end of phase = 2 check
            }
		}
	}//end of update

	IEnumerator InteractDelay(){
		yield return new WaitForSeconds(1f);
        //Time.timeScale = 0f;  //took this out because large trash display wasnt working well with it, needed?
		phase = 2;
	}


}
