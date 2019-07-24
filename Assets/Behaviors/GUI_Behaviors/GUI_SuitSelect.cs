using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GUI_SuitSelect : MonoBehaviour
{
	int weaponsAvailable = 1;
	protected int arrowpos = 1;

	public List<GUI_SuitOption> weaponBoxes = new List<GUI_SuitOption>();
	//public GameObject mopBox;
	public GameObject highlightBox;
	public GameObject selectArrow;
	protected Vector3 startPos;

	public AudioClip selectSound;
    public AudioClip paperSlide;


    GUI_SuitOption selectedSuit;

	void Awake() {
        startPos = gameObject.transform.localPosition;

        // TODO: Unity doesn't run start on disabled gameobjects because it's lame and weird.  Is there a better way to do this
        // Then starting active and disabling it immediately?
        gameObject.SetActive(false);



        GameStateManager.Instance.RegisterChangeStateEvent(OnChangeState);
    }

	void OnEnable(){
		weaponsAvailable = 1;
	
		if(GlobalVariableManager.Instance.IsSuitAvailable(GlobalVariableManager.WEAPONS.HAZMAT)){
			Debug.Log("HAZMAT IS AVAILABLE");
			weaponsAvailable++;
			weaponBoxes[1].gameObject.SetActive(true);
			if(GlobalVariableManager.Instance.IsSuitEquipped(GlobalVariableManager.WEAPONS.HAZMAT)){
				highlightBox.transform.position = weaponBoxes[1].transform.position;
			}
		}else{
			Debug.Log("Hazmat not available:" + GlobalVariableManager.Instance.SUITS_DISCOVERED);
		}
	}
	
	void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(PauseMenuState)) {

            if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT)
            || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKRIGHT)) {
            Debug.Log("Registered key right- suit select");
            	if(arrowpos < weaponsAvailable){
					Debug.Log("Registered key right- suit select 2");

            		arrowpos++;
            		selectArrow.transform.position = weaponBoxes[arrowpos-1].gameObject.transform.position;
            	}

             
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT)
                 || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKLEFT)) {
				Debug.Log("Registered key left- suit select");

                if (arrowpos > 1) {
                    arrowpos--;
					selectArrow.transform.position = weaponBoxes[arrowpos-1].gameObject.transform.position;

                    SoundManager.instance.PlaySingle(selectSound);
               
                }
            
			}else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)) {
		
				GlobalVariableManager.Instance.WEAPON_EQUIPPED = weaponBoxes[arrowpos-1].mySuit;
				selectedSuit = weaponBoxes[arrowpos-1];
				Debug.Log("Set WEAPON_EQUIPPED to..." + GlobalVariableManager.Instance.WEAPON_EQUIPPED);

            }
            if (ControllerManager.Instance.GetKeyDown(INPUTACTION.PAUSE)) {
                arrowpos = 1;
                gameObject.transform.localPosition = startPos;
                UpdateJimVisual();
                GameStateManager.Instance.PopState();
                GameStateManager.Instance.PushState(typeof(GameState));
            }
        }


		//gameObject.transform.localPosition = Vector2.Lerp(gameObject.transform.localPosition,Vector2.zero,.1f*(Time.realtimeSinceStartup - Time.deltaTime));
	}

	void UpdateJimVisual(){
		Debug.Log(PlayerManager.Instance.player);
		PlayerManager.Instance.player.GetComponent<tk2dBaseSprite>().SetSprite(selectedSuit.mySuitsSpriteCollection,0);
		PlayerManager.Instance.player.GetComponent<tk2dSpriteAnimator>().Library = selectedSuit.mySuitSpriteAnimation;
		PlayerManager.Instance.player.GetComponent<tk2dSpriteAnimator>().Play("ani_jimIdle");
	}

	private void OnDestroy()
    {

        GameStateManager.Instance.UnregisterChangeStateEvent(OnChangeState);
    }

    void OnPopupCloseEvent()
    {
    }

    void OnPopupOptionsEvent(int optionNum)
    {
        switch (optionNum) {
            case 0:
                GameObject fadeHelp = GameObject.Find("fadeHelper");
                fadeHelp.GetComponent<Ev_FadeHelper>().EndOfDayFade();
                break;
        }
    }

    void OnChangeState(System.Type stateType, bool isEntering)
    {
        if (isEntering) {
            if (stateType == typeof(PauseMenuState)) {
                gameObject.SetActive(true);
                SoundManager.instance.PlaySingle(paperSlide);
                SoundManager.instance.musicSource.volume = SoundManager.instance.musicSource.volume / 2;
                Time.timeScale = 0;
            }
            else if (stateType == typeof(EndDayState)) {
                gameObject.SetActive(false);
            }
        }
        else {
            if (stateType == typeof(PauseMenuState)) {
                gameObject.SetActive(false);
                SoundManager.instance.PlaySingle(paperSlide);
                SoundManager.instance.musicSource.volume = SoundManager.instance.musicSource.volume * 2;
                Time.timeScale = 1;
            }
        }
    }
}

