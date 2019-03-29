using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ev_DroppedPin : MonoBehaviour {

	PinDefinition pinData = null; //usually given by trash can

	GameObject pinUnlockHud; //given by trash can
    public GameObject shadow;
    [SerializeField] bool canBeGrabbed = false;
	Sprite mySprite; //given by trash can, used by unlock hud
    
    TimedLeapifier timedLeapifier;  // leaping, in a bouncy manner
    public float maxBounceHeight; // maximum bounce height
    public float totalBounceTime; // how long the whole bounce animation will run
    public GameObject bounceDest; // where the pin will be after it stops bouncing
    public AnimationCurve bounceCurve; // bouncy y-value curve

    // Use this for initialization
    void OnEnable () {
        canBeGrabbed = false;
        Bounce();
	}

    void OnDisable()
    {
        if (timedLeapifier != null) {
            timedLeapifier.Reset();
        }
    }

    // Update is called once per frame
    void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (timedLeapifier != null) {
                if (timedLeapifier.OnUpdate()) {
                    timedLeapifier.Reset();
                    timedLeapifier = null;
                    canBeGrabbed = true;
                }
            }
        }
	}

	void OnTriggerStay2D(Collider2D collider){
		if(canBeGrabbed && collider.gameObject.tag == "Player"){
            if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
                pinUnlockHud.SetActive(true);
                pinUnlockHud.GetComponent<GUI_PinUnlockDisplay>().SetValues(pinData.displayName, pinData.description, mySprite);
                GlobalVariableManager.Instance.PINS_DISCOVERED |= pinData.Type;
                GameStateManager.Instance.PushState(typeof(PopupState));
                gameObject.SetActive(false);
            }
		}
	}

	public void SetPinData(PinDefinition data, GameObject unlockHUD, Sprite pinsprite){
        pinData = data;
        pinUnlockHud = unlockHUD;
        mySprite = pinsprite;
	}//end of pin data set


	void Bounce(){
        timedLeapifier = new TimedLeapifier(gameObject, shadow, maxBounceHeight, totalBounceTime, bounceDest.transform.position, bounceCurve);
	}
}
