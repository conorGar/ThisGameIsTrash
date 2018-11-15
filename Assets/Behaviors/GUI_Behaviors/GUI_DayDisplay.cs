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
	public bool forHub;
	public AudioClip truckSmokeSfx;
	public AudioClip truckDoor;
	public AudioClip truckSfx;

	int phase = 0;

	// Use this for initialization
	void Start ()
	{
		if(!forHub){
		dayNumDisplay.text = "- Day " + GlobalVariableManager.Instance.DAY_NUMBER.ToString() + " -";
		dayNumDisplay.GetComponent<TextAnimation>().PlayAnim();
		}
		StartCoroutine("TruckEnter");
		SoundManager.instance.PlaySingle(truckSfx);
		//player.GetComponent<EightWayMovement>().enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(phase == 1){
			if(forHub){
				truck.transform.position = Vector2.MoveTowards(truck.transform.position,new Vector2(13f,15f),13*Time.deltaTime);

			}else{
				truck.transform.position = Vector2.MoveTowards(truck.transform.position,new Vector2(.1f,8.2f),13*Time.deltaTime);
			
			}

		}else if(phase == 3){
			if(forHub){
				truck.transform.Translate(Vector3.left);
			}else{
				truck.transform.position = Vector2.MoveTowards(truck.transform.position,new Vector2(27f,8.2f),24*Time.deltaTime);
			}
		}
	}

	IEnumerator TruckEnter(){
		InvokeRepeating("SmokePuffSfx",.1f,.5f);
		phase = 1;
		if(forHub){
			yield return new WaitUntil(() => truck.transform.localPosition.x < 0f);

		}else{
			yield return new WaitUntil(() => truck.transform.localPosition.x > -1.3f);
		}

		phase = 2;
		StartCoroutine("TruckLeave");
		//yield return new WaitForSeconds(1.5f);

	}

	IEnumerator TruckLeave(){
		playerDummy.SetActive(true);
		SoundManager.instance.PlaySingle(truckDoor);
		yield return new WaitForSeconds(.3f);
		phase = 3;
		yield return new WaitForSeconds(.6f);
		back.GetComponent<Animator>().enabled = true;
		back.GetComponent<SpriteRenderer>().enabled = false;
		//player.GetComponent<EightWayMovement>().enabled = true;
		playerDummy.SetActive(false);
		yield return new WaitForSeconds(.5f);
		CancelInvoke();
		GameStateManager.Instance.PushState(typeof(GameplayState));

		yield return new WaitForSeconds(1.5f);
		gameObject.SetActive(false);
	}

	void SmokePuffSfx(){
		SoundManager.instance.PlaySingle(truckSmokeSfx);
	}
}

