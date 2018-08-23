using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class S_Ev_TitleScreen : MonoBehaviour {
    bool isInteractable = true;
    int navigationPosition = 1;
	int phase = 0;
	int backCamPhase = 0;

	public GameObject title;
	public GameObject choicesBox;
	public GameObject backCam;
	public GameObject fadeHelper;
	public GameObject GUIcam;

	public GameObject playOption;
	public GameObject optionsOption;
	public GameObject extrasOptions;

    public TextMeshProUGUI loadingGameDataVisual;

	GameObject currentSelected;

	// Use this for initialization
	void Start () {
		currentSelected = playOption;
	}
	
    IEnumerator LoadUserData()
    {
        isInteractable = false;
        loadingGameDataVisual.gameObject.SetActive(true);

        // Testing data loading!
        UserDataManager.Instance.SetSlot(0);
        yield return UserDataManager.Instance.ReadAsync();

        // Simulate async loading time for testing.
        yield return new WaitForSeconds(2);

        loadingGameDataVisual.gameObject.SetActive(false);

        phase = 2;
        fadeHelper.GetComponent<Ev_FadeHelper>().FadeToScene("1_1");
    }

	// Update is called once per frame
	void Update () {
        if (isInteractable)
        {
            if (phase == 1) {
                if (Input.GetKeyDown(KeyCode.UpArrow)) {
                    if (navigationPosition > 1) {
                        navigationPosition--;
                        UpdateSelected();
                    }
                } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
                    if (navigationPosition < 3) {
                        navigationPosition++;
                        UpdateSelected();
                    }
                } else if (Input.GetKeyDown(KeyCode.Space)) {
                    if (navigationPosition == 1) {
                        StartCoroutine(LoadUserData());
                    }
                }
            } else if (phase == 0) {
                if (Input.GetKeyDown(KeyCode.Space)) {
                    Debug.Log(title.GetComponent<SpecialEffectsBehavior>() == null);
                    Vector3 topLimit = GUIcam.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(title.transform.position.x, Screen.height * -1, 0));

                    Debug.Log(topLimit);
                    Debug.Log(topLimit * -1);

                    title.transform.localPosition = topLimit * -1;
                    //title.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(title.transform.position.x,title.transform.localPosition.y + topLimit.y,.2f);
                    choicesBox.SetActive(true);
                    phase = 1;

                }
            }
        }



		//----------------------Camera pan over world----------------------------//

		if(backCamPhase == 0){
			backCam.transform.position = Vector3.MoveTowards(backCam.transform.position,new Vector3(51f,27f,-10f),10*Time.deltaTime);
			if(backCam.transform.position.x > 50.8f){
				backCamPhase = 1;
			}
		}else if(backCamPhase == 1){
			backCam.transform.position = Vector3.MoveTowards(backCam.transform.position,new Vector3(51f,-49.4f,-10f),10*Time.deltaTime);
			if(backCam.transform.position.y < -49.1f){
				backCamPhase = 2;
			}
		}else if(backCamPhase == 2){
			backCam.transform.position = Vector3.MoveTowards(backCam.transform.position,new Vector3(-79f,-49.4f,-10f),10*Time.deltaTime);
			if(backCam.transform.position.x < -78.8f){
				backCamPhase = 3;
			}
		}else if(backCamPhase == 3){
			backCam.transform.position = Vector3.MoveTowards(backCam.transform.position,new Vector3(-79f,27,-10f),10*Time.deltaTime);
			if(backCam.transform.position.y > 26.9f){
				backCamPhase = 0;
			}
		}


		//----------------------------------------------------------------------//
	}

	void UpdateSelected(){
		currentSelected.GetComponent<SpecialEffectsBehavior>().SetGrowValues(new Vector3(.5f,.5f,.5f),10f);//need new Grow function that takes two different values
		currentSelected.GetComponent<SpecialEffectsBehavior>().StartCoroutine("Grow",.01f);
		currentSelected.GetComponent<Image>().color = new Color(currentSelected.GetComponent<Image>().color.r,currentSelected.GetComponent<Image>().color.g,currentSelected.GetComponent<Image>().color.b,.7f);
		currentSelected.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(255f,255f,255f,.7f);
		if(navigationPosition == 1){
			currentSelected = playOption;
		}else if(navigationPosition == 2){
			currentSelected = optionsOption;
		}else if(navigationPosition == 3){
			currentSelected = extrasOptions;
		}
		Debug.Log(navigationPosition);

		currentSelected.GetComponent<SpecialEffectsBehavior>().SetGrowValues(new Vector3(1.5f,1.5f,1.5f),10f);
		currentSelected.GetComponent<SpecialEffectsBehavior>().StartCoroutine("Grow",.01f);
		currentSelected.GetComponent<Image>().color = new Color(currentSelected.GetComponent<Image>().color.r,currentSelected.GetComponent<Image>().color.g,currentSelected.GetComponent<Image>().color.b,1f);
		currentSelected.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(255f,255f,255f,1f);

	}
}
