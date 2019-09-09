using UnityEngine;
using System.Collections;

public class Hub_SuitVariantStand : MonoBehaviour
{

	public GUI_SuitUpgradeGUI suitUpgradeGui;
	public GameObject spaceIcon;


	
	// Update is called once per frame
	void Update ()
	{
		// RatWithAHat will control if this component is enabled or disabled.  Once enabled it works as a way to launch the base stat upgrade shop gui.
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
                if (Mathf.Abs(transform.position.x - PlayerManager.Instance.player.transform.position.x) < 4f &&
                    Mathf.Abs(transform.position.y - PlayerManager.Instance.player.transform.position.y) < 7f) {

                    if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT) && GUIManager.Instance.BaseStatHUD.activeInHierarchy != true) {
                        suitUpgradeGui.gameObject.SetActive(true);
                        suitUpgradeGui.Navigate("");
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

