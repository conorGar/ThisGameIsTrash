using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Something simple to contain GUI elements in the scene so references aren't passed all over the place.
// TODO: Maybe break this up into a manager for each world since they each have specific GUIs.
public class GUIManager : MonoBehaviour {
    public static GUIManager Instance;

    public HUD_Calendar CalendarHUD;
    public GUI_DayDisplay DayDisplay;
    public GUI_DeathDisplay DeathDisplay;
    public GUI_TrashCollectedDisplay TrashCollectedDisplayGameplay; // At the top left of the screen during gameplay
    public GUI_TrashCollectedDisplay TrashCollectedDisplayDeath; // On the death display

    // Friend related
    public GUI_RockItemHUD rockItemHUD;
    public GUI_SlabTrashGiveHUD slabTrashGiveHUD;

	void Awake () {
        Instance = this;
	}
}
