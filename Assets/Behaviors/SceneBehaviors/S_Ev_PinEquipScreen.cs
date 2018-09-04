using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PostProcessing;

public class S_Ev_PinEquipScreen : MonoBehaviour {

	public GameObject returnButton;
	public GameObject tutorialPopup;
	public GameObject pin;
	public GameObject selectionArrow;
	public GameObject displayPin;
	public PostProcessingProfile blur;
	public GameObject mainCam;
	int arrowPos = 0;
	GameObject highlightedPin;
	public TextMeshProUGUI totalPPDisplay;
    List<GameObject> pinPageList = new List<GameObject>();

	void OnEnable () {
        if (PinManager.Instance.DebugPins)
        {
            GlobalVariableManager.Instance.PINS_DISCOVERED = PIN.ALL;
        }

        GlobalVariableManager.Instance.MENU_SELECT_STAGE = 10;

		if(GlobalVariableManager.Instance.ROOM_NUM != 112){
			GlobalVariableManager.Instance.ROOM_NUM = 101;
		}

        /*if(GlobalVariableManager.Instance.WORLD_SIGNS_READ[0][9] != 'o'){
			Instantiate(tutorialPopup,transform.position,Quaternion.identity);
			GlobalVariableManager.Instance.WORLD_SIGNS_READ[0].Replace(GlobalVariableManager.Instance.WORLD_SIGNS_READ[0][9],'o');
		}*/
		mainCam.GetComponent<PostProcessingBehaviour>().profile = blur;
        totalPPDisplay.text = GlobalVariableManager.Instance.PPVALUE.ToString();

        // Set up all the pin pages.
        var pinsPerPage = PinManager.Instance.PinRow * PinManager.Instance.PinCol;
        var pinPageCount = PinManager.Instance.pinConfig.pinList.Count / pinsPerPage;
        pinPageList = new List<GameObject>();

        // add a page for any remaining pins.
        if (PinManager.Instance.pinConfig.pinList.Count % pinsPerPage > 0)
            pinPageCount += 1;

        for (int i=0; i < pinPageCount; ++i)
        {
            GameObject pinPage = ObjectPool.Instance.GetPooledObject("PinPage");
            pinPage.transform.SetParent(PinManager.Instance.PageRoot.transform);
            pinPage.transform.position = PinManager.Instance.PageRoot.transform.position;
            pinPage.name = "Pin Page " + i;
            pinPage.SetActive(true);
            pinPageList.Add(pinPage);
        }

        for (int i = 0; i < PinManager.Instance.pinConfig.pinList.Count; i++){
            PinDefinition pinDefinition = PinManager.Instance.pinConfig.pinList[i];
            int pageNum = i / pinsPerPage;
            Vector3 pagePos = pinPageList[pageNum].transform.position;
            GameObject pin = ObjectPool.Instance.GetPooledObject("Pin");
            pin.transform.SetParent(pinPageList[pageNum].transform);

            // populate the pin data
            pin.name = pinDefinition.displayName;

            var sprite = pin.GetComponent<tk2dSprite>();
            sprite.SetSprite(pinDefinition.sprite);

            var behavior = pin.GetComponent<Ev_PinBehavior>();
            behavior.SetPinData(pinDefinition);

            pin.SetActive(true);

            var pinSize = pin.GetComponent<Collider2D>().bounds.size;
            // x, at the page origin, per column, spaced with an offset
            // y, at the page origin, per row, per page, projected downward.
            pin.transform.position = new Vector3(pagePos.x + i % PinManager.Instance.PinCol * (pinSize.x + PinManager.Instance.PinOffsetX),
                                                 pagePos.y + -i / PinManager.Instance.PinCol % PinManager.Instance.PinRow * (pinSize.y + PinManager.Instance.PinOffsetY),
                                                 pagePos.z);
            Debug.Log("Got Here Pin Spawn");
        }

        for (int i=0; i < pinPageList.Count; ++i)
        {
            // deactivate all but the first page.
            if (i != 0)
                pinPageList[i].SetActive(false);
        }

	}
	
	void Update () {
        if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT))
        {
            MoveArrow();

            if (highlightedPin.GetComponent<Ev_PinBehavior>().EquipPin())
            {
                // Untilt if equipped
                //if (highlightedPin != null)
                //    highlightedPin.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            }
            else
            {
                // Tilt if unequipped
                //if (highlightedPin != null)
                //    highlightedPin.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 15f));
            }
            totalPPDisplay.text = GlobalVariableManager.Instance.PPVALUE.ToString();
		}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.PAUSE)){
			GameObject.Find("hubWorld_pinCase").GetComponent<Ev_PinDisplayOption>().enabled = true;
			mainCam.GetComponent<PostProcessingBehaviour>().profile = null;
			this.gameObject.SetActive(false);
		}
        else
        {
            bool isNewPin = false;
            if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT)
             || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKLEFT))
            {
                arrowPos--;
                isNewPin = true;
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT)
                  || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKRIGHT))
            {
                arrowPos++;
                isNewPin = true;
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEDOWN)
                  || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKDOWN))
            {
                arrowPos += PinManager.Instance.PinCol;
                isNewPin = true;
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEUP)
                  || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKUP))
            {
                arrowPos -= PinManager.Instance.PinCol;
                isNewPin = true;
            }

            if (isNewPin)
            {
                // Wrap from 0 to pin max.
                if (arrowPos < 0)
                    arrowPos = PinManager.Instance.pinConfig.pinList.Count - 1;

                if (arrowPos > PinManager.Instance.pinConfig.pinList.Count - 1)
                    arrowPos = 0;

                // select the currentpage.
                for (int i=0; i < pinPageList.Count; ++i)
                {
                    if (i == arrowPos / (PinManager.Instance.PinCol * PinManager.Instance.PinRow))
                        pinPageList[i].SetActive(true);
                    else
                        pinPageList[i].SetActive(false);
                }

                if (highlightedPin != null)
                    highlightedPin.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));

                MoveArrow();
            }
        }

        totalPPDisplay.text = GlobalVariableManager.Instance.PPVALUE.ToString();
    }

	void MoveArrow(){
        int currentPage = arrowPos / (PinManager.Instance.PinCol * PinManager.Instance.PinRow);
        int currentPin = arrowPos % (PinManager.Instance.PinCol * PinManager.Instance.PinRow);
        GameObject pinPage = PinManager.Instance.PageRoot.transform.GetChild(currentPage).gameObject;

        highlightedPin = pinPage.transform.GetChild(currentPin).gameObject;
        highlightedPin.SetActive(true);
        selectionArrow.transform.position = new Vector3(highlightedPin.transform.position.x,
                                                        highlightedPin.transform.position.y,
                                                        selectionArrow.transform.position.z);

		highlightedPin.transform.rotation =  Quaternion.Euler(new Vector3(0f,0f,15f));
		highlightedPin.GetComponent<Ev_PinBehavior>().AtEquipScreen();
	}
}
