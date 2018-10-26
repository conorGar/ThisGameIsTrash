using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Hub_UpgradeStand : MonoBehaviour {

    public GameObject spaceIcon;
	public GameObject player;
    public RatWithHatFriend shopKeeper;

    void Start()
    {
        shopKeeper = (RatWithHatFriend)FriendManager.Instance.GetFriend("RatWithAHat");
    }

    // Update is called once per frame
    void Update () {
        // RatWithAHat will control if this component is enabled or disabled.  Once enabled it works as a way to launch the base stat upgrade shop gui.
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (shopKeeper.GetFriendState() == "OPEN_FOR_BUSINESS") {
                if (Mathf.Abs(transform.position.x - player.transform.position.x) < 4f && Mathf.Abs(transform.position.y - player.transform.position.y) < 7f) {

                    if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT) && GUIManager.Instance.BaseStatHUD.activeInHierarchy != true) {
                        GUIManager.Instance.BaseStatHUD.SetActive(true);
                        GUIManager.Instance.GUI_BaseStatUpgrade.GetComponent<GUI_BaseStatUpgrade>().Navigate("");
                        spaceIcon.SetActive(false);
                    }
                    else if (!spaceIcon.activeInHierarchy) {
                        spaceIcon.SetActive(true);
                    }
                }
                else if (spaceIcon.activeInHierarchy) {
                    spaceIcon.SetActive(false);
                }
            }
        }
    }

}
