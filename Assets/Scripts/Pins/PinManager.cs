using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PinManager : MonoBehaviour {
    public static PinManager Instance;
    public int PinRow = 6;
    public int PinCol = 4;
    public float PinOffsetX = 0.1f;
    public float PinOffsetY = 0.1f;
    public bool DebugPins = false;
    public S_Ev_shop Shop;
    public GameObject PageRoot;
    public Text DescriptionText;
    public tk2dSprite PinTitleSprite;
    public Ev_PPDisplay PPDisplay;
    public tk2dSprite PinDisplaySprite;
    public PinConfig pinConfig;
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

    public PinDefinition GetPin(PIN type)
    {
        if (PINLOOKUP.ContainsKey(type))
            return PINLOOKUP[type];
        return null;
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
