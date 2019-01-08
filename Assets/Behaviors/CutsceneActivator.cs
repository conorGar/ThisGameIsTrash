using UnityEngine;
using System.Collections;

public class CutsceneActivator : MonoBehaviour
{
	
	public GameObject player;
	//public IEnumerator coroutine;
	public ParticleSystem hidingClouds;
	public ParticleSystem backHidingClouds;
	public GameObject title;
	public GameObject peakViewStars;
	public AudioClip musicToPlay;
	public AudioClip starShow;
	public GameObject demoEndFader;
	public GameObject demoEndText;
	public Ev_FadeHelper fader;

	bool movePlayer;
	Vector2 playerDestination;
	int starShowIndex;
	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		if(movePlayer){
			if(Vector2.Distance(player.transform.position,playerDestination) > 2f)
				player.transform.position = Vector2.MoveTowards(player.transform.position,playerDestination,4*Time.deltaTime);
			else{
				StartCoroutine("PollutedPeakView");
				movePlayer = false;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.gameObject == player && !movePlayer){
		GameStateManager.Instance.PushState(typeof(DialogState));
		SoundManager.instance.backupMusicSource.clip = musicToPlay;
		SoundManager.instance.backupMusicSource.Play();
		CamManager.Instance.mainCamEffects.CameraPan(player,true);
		playerDestination = new Vector2(67.9f,47.7f);

		movePlayer = true;
		}

	}

	IEnumerator PollutedPeakView(){
		CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
		yield return new WaitForSeconds(.2f);
		CamManager.Instance.mainCamEffects.CameraPan(new Vector3(78.8f,53.8f,-10.8f),"");
		CamManager.Instance.mainCamEffects.ZoomInOut(.9f,2f);
		player.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimIdle",true);
		player.GetComponent<EightWayMovement>().legAnim.Play("ani_jimIdle");
		hidingClouds.Stop();
		yield return new WaitForSeconds(.5f);
		backHidingClouds.Stop();
		yield return new WaitForSeconds(5f);
		InvokeRepeating("PeakStarShow",0f,.2f);
		yield return new WaitForSeconds(5f);
		title.SetActive(true);
		yield return new WaitForSeconds(2f);
		demoEndFader.SetActive(true);
		yield return new WaitForSeconds(3.5f);
		demoEndText.SetActive(true);
		yield return new WaitForSeconds(1.5f);
		fader.FadeToScene("TitleScreen");

	}

	void PeakStarShow(){
		if(starShowIndex < peakViewStars.transform.childCount){
			peakViewStars.transform.GetChild(starShowIndex).gameObject.SetActive(true);
			SoundManager.instance.PlaySingle(starShow);
			starShowIndex++;
		}else{
			CancelInvoke();
		}
	}
}

