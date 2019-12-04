using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// Something simple to contain GUI elements in the scene so references aren't passed all over the place.
// TODO: Maybe break this up into a manager for each world since they each have specific GUIs.
public class GUIManager : MonoBehaviour {
    public static GUIManager Instance;

    public Ev_HUD HUD;
    public HUD_Calendar CalendarHUD;
    public GUI_DayDisplay DayDisplay;
    public GUI_DeathDisplay DeathDisplay;
    public Ev_DayMeter DayMeterDisplay;
    public GUI_TrashCollectedDisplay TrashCollectedDisplayGameplay; // At the top left of the screen during gameplay
    public GUI_TrashCollectedDisplay TrashCollectedDisplayDeath; // On the death display
    public GUI_HPdisplay hpDisplay;
    public GUI_SuitSelect suitSelectDisplay;
    public GameObject checkPointDisplay;
    public GUI_BossHpDisplay bossHPDisplay;
  	public GUI_TutPopup tutorialPopup;
  	public GUI_LargeTrashCollectedDisplay largeTrashCollectDisplay;
  	public Camera miniMapCam;

   	public TextMeshProUGUI lostTrash;


    // Friend related
    public GUI_RockItemHUD rockItemHUD;
    public GUI_SlabTrashGiveHUD slabTrashGiveHUD;
	public GameObject SlabTrashNeededDisplay;
	public GameObject StoneHandNeededDisplay;
	public GUI_RockItemHUD coonScavengerHUD;

    // Hub related
    public GameObject BaseStatHUD;
    public GUI_BaseStatUpgrade GUI_BaseStatUpgrade;
    public Hub_UpgradeStand Hub_UpgradeStand;
    public GUI_TimeUpgrade GUI_TimeUpgrade;
    public GameObject areaTitle;


	void Awake () {
        Instance = this;
	}

    void Start()
    {
        if (BaseStatHUD != null) {
            BaseStatHUD.SetActive(false);
        }
    }

    public void LeaveTownHUDSpawns(){
    	DayMeterDisplay.gameObject.SetActive(true);
    	TrashCollectedDisplayGameplay.gameObject.SetActive(true);
    }
}
