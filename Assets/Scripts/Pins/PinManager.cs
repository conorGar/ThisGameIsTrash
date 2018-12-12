using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PinManager : MonoBehaviour {
    public static PinManager Instance;
    public int PinRow = 6;
    public int PinCol = 4;
    public float PinOffsetX = 0.1f;
    public float PinOffsetY = 0.1f;
    public bool DebugPins = false;
    public S_Ev_shop Shop;
    public GameObject PageRoot;
    public TextMeshProUGUI DescriptionText;
    public TextMeshProUGUI PinTitle;
    public Ev_PPDisplay PPDisplay;
    public tk2dSprite PinDisplaySprite;
    public PinConfig pinConfig;
    public ParticleSystem equipSpark;
    public GameObject newPinIcon;
    private Dictionary<PIN, PinDefinition> PINLOOKUP = new Dictionary<PIN, PinDefinition>();

	void Awake () {
        Instance = this;

        // sort by display priority
        pinConfig.pinList.Sort((x, y) => x.displayPriority - y.displayPriority);

        // populate the pin look up table.
        for (int i = 0; i < pinConfig.pinList.Count; ++i)
        {
            PINLOOKUP[pinConfig.pinList[i].Type] = pinConfig.pinList[i];
			
        }
    }

    private void Start()
    {
        // Get the current pp stat by evaluating the pins equipped.
        // TODO: PinManager is so coupled with the store that it can't exist outside the hub scene.  Need to move all the visual stuff back into the EquipScreen  so the PinManager can be used as a real Singleton.
        GlobalVariableManager.Instance.PP_STAT.ResetCurrent();
        GlobalVariableManager.Instance.PP_STAT.UpdateCurrent(-PinManager.Instance.GetAllocatedPP());
        RefreshNewPinIcon();
    }

    public void RefreshNewPinIcon(){
		for (int i = 0; i < pinConfig.pinList.Count; ++i)
        {
			if(!newPinIcon.activeInHierarchy && GlobalVariableManager.Instance.IsPinDiscovered(pinConfig.pinList[i].Type) && !GlobalVariableManager.Instance.IsPinViewed(pinConfig.pinList[i].Type)){
				Debug.Log("New pin Icon set active because of:" + pinConfig.pinList[i].displayName);
				newPinIcon.SetActive(true);
                return;
			}
        }
    }

    public PinDefinition GetPin(PIN type)
    {
        if (PINLOOKUP.ContainsKey(type))
            return PINLOOKUP[type];
        return null;
    }

    // amount of allocated PP inferred from equipped pins.
    public int GetAllocatedPP()
    {
        int count = 0;
        for (int i = 0; i < pinConfig.pinList.Count; i++) {
            var definition = pinConfig.pinList[i];

            if (GlobalVariableManager.Instance.IsPinEquipped((PIN)definition.pinValue)) {
                count += definition.ppValue;
            }
        }

        return count;
    }

    // Returns a random pin from the range specified that fits the criteria.
    public PinDefinition GetRandomPin(Predicate<PinDefinition> predicate = null, int min = -1, int max = -1)
    {
        // Default min
        if (min == -1)
            min = 0;

        // Default max
        if (max == -1)
            max = pinConfig.pinList.Count;

        // subset of the list
        var shuffledPinList = pinConfig.pinList.GetRange(min, max - min);

        // trimmed based on the passed in comparer function.
        if (predicate != null)
            shuffledPinList.RemoveAll(predicate);

        // shuffled for a random pin.
        shuffledPinList.Shuffle();

        // assuming their are any pins left, return the first one.
        if (shuffledPinList.Count > 0)
            return shuffledPinList[0];
        else
            return null;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
