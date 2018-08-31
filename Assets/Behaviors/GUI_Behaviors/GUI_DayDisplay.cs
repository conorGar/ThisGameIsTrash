using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using I2.TextAnimation;
public class GUI_DayDisplay : MonoBehaviour
{
	public GameObject truck;
	public TextMeshProUGUI dayNumDisplay;
	public GameObject back;
	public GameObject player;
	public GameObject playerDummy;

	int phase = 0;

	// Use this for initialization
	void Start ()
	{
		dayNumDisplay.text = "- Day " + GlobalVariableManager.Instance.DAY_NUMBER.ToString() + " -";
		dayNumDisplay.GetComponent<TextAnimation>().PlayAnim();
		StartCoroutine("TruckEnter");
		player.GetComponent<EightWayMovement>().enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(phase == 1){
			truck.transform.Translate(Vector3.right);
			if(truck.transform.localPosition.x > 0f){
				phase = 2;
				StartCoroutine("TruckLeave");
			}
		}else if(phase == 3){
			truck.transform.Translate(Vector3.right);
		}
	}

	IEnumerator TruckEnter(){
		yield return new WaitForSeconds(1.5f);
		phase = 1;
	}

	IEnumerator TruckLeave(){
		playerDummy.SetActive(true);
		yield return new WaitForSeconds(.3f);
		phase = 3;
		back.GetComponent<Animator>().enabled = true;
		player.GetComponent<EightWayMovement>().enabled = true;
		playerDummy.SetActive(false);
		yield return new WaitForSeconds(.5f);
		yield return new WaitForSeconds(1.5f);
		gameObject.SetActive(false);
	}
}

