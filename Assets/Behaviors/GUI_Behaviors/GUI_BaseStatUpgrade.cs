using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using I2.TextAnimation;
public class GUI_BaseStatUpgrade : GUI_MenuBase {


	public TextMeshProUGUI positiveText;
	public Hub_UpgradeStand stand;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			SelectUpgrade();
		}
	}

	void SelectUpgrade(){
		if(GlobalVariableManager.Instance.STAR_POINTS == 0){
			if(arrowPos == 0){
				GlobalVariableManager.Instance.BAG_SIZE +=2;
			}else if(arrowPos == 1){
				GlobalVariableManager.Instance.Max_HP +=1;
			}else if(arrowPos == 2){
				GlobalVariableManager.Instance.PPVALUE +=3;
			}else if(arrowPos == 3){
				this.gameObject.SetActive(false);
				stand.enabled = true;
			}
			GlobalVariableManager.Instance.STAR_POINTS--;
			PositiveText();
		}
	}

	void PositiveText(){
		int whichText = Random.Range(1,6);
		string positiveQuote = null;
		if(whichText == 1){
			positiveQuote = "You got this. Keep it up. Proud of you.";
		}else if(whichText == 2){
			positiveQuote = "Just keep going. One of these days you'll get it right, I'm sure of it.";
		}else if(whichText == 3){
			positiveQuote = "I believe in you. If you can't do it, nobody can.";
		}else if(whichText == 4){
			positiveQuote = "You smell very nice today.";
		}else if(whichText == 5){
			positiveQuote = "Wow, you're doing so great. It's really inspiring.";
		}else if(whichText == 6){
			positiveQuote = "The difference between winners and losers is that winners try one more time.";
		}
		positiveText.text = positiveQuote;
		positiveText.GetComponent<TextAnimation>().StartAgain();

	}
}
