using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class Ev_SignPost : MonoBehaviour {
	public float distanceUntilGlow;
	public GameObject signPostHUD;
	public TextMeshProUGUI signText;
	public string myText;
	public string myName = "- Your Pal, Stuart";
	public TextMeshProUGUI nameDisplay;
	public Sprite myPicture;
	public AudioClip signRise;
	public PostProcessingProfile blur;
	float lastRealTimeSinceStartup;
	int glowCheck;
	tk2dSpriteAnimator myAnim;
	GameObject speechIcon;

	// Use this for initialization
	void Start () {
		myAnim = gameObject.GetComponent<tk2dSpriteAnimator>();

	}

    // Update is called once per frame
    void Update() {

        // Only interact with a sign post if you are close!
        if (Mathf.Abs(PlayerManager.Instance.player.transform.position.x - gameObject.transform.position.x) < distanceUntilGlow && Mathf.Abs(PlayerManager.Instance.player.transform.position.y - gameObject.transform.position.y) < distanceUntilGlow) {
            if (glowCheck == 0) {
                myAnim.Play("glow");
                speechIcon = ObjectPool.Instance.GetPooledObject("speechIcon", new Vector2(gameObject.transform.position.x + 2f, gameObject.transform.position.y + 1f));
                glowCheck = 1;
            }

            // Toggle from gameplay to dialog states.
            if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
                if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)) {
                    ObjectPool.Instance.ReturnPooledObject(speechIcon);
                    SoundManager.instance.PlaySingle(signRise);
                    CamManager.Instance.mainCamPostProcessor.profile = blur;
                    signPostHUD.SetActive(true);
					if( signPostHUD.transform.GetChild(0).GetComponent<TextMeshProUGUI>() !=null)
                    	signPostHUD.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = myText;
                    else if(signText != null)
                    	signText.text = myText;
                    if(nameDisplay != null)
                  	  nameDisplay.text = myName;
                    if (myPicture != null) {
                        signPostHUD.transform.GetChild(1).GetComponent<Image>().sprite = myPicture;
                    }
                    lastRealTimeSinceStartup = Time.realtimeSinceStartup;
                    GameStateManager.Instance.PushState(typeof(DialogState));
                    Time.timeScale = 0;
                }
            }
            else if (GameStateManager.Instance.GetCurrentState() == typeof(DialogState)) {
                if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)) {
                    GameStateManager.Instance.PopState();
                    Time.timeScale = 1;
                    signPostHUD.SetActive(false);
                    CamManager.Instance.mainCamPostProcessor.profile = null;
                    signPostHUD.transform.localPosition = new Vector3(13f, -203f, 10f);
                }
            }
        } else {
            if (glowCheck > 0) {
                myAnim.Play("idle");
                if (speechIcon != null)
                    speechIcon.SetActive(false);
                glowCheck = 0;
            }
        }

		if(signPostHUD.activeInHierarchy){
			signPostHUD.transform.localPosition = Vector3.Lerp(signPostHUD.transform.localPosition,new Vector3(13f,-1f,10f),.1f*(Time.realtimeSinceStartup - lastRealTimeSinceStartup));
		}

	}


}
