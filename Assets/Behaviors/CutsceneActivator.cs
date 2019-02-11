using UnityEngine;
using System.Collections;
using System.IO;

public class CutsceneActivator : MonoBehaviour
{
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
			if(Vector2.Distance(PlayerManager.Instance.player.transform.position,playerDestination) > 2f)
                PlayerManager.Instance.player.transform.position = Vector2.MoveTowards(PlayerManager.Instance.player.transform.position,playerDestination,4*Time.deltaTime);
			else{
				StartCoroutine("PollutedPeakView");
				movePlayer = false;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.gameObject == PlayerManager.Instance.player && !movePlayer){
		GameStateManager.Instance.PushState(typeof(DialogState));
		SoundManager.instance.backupMusicSource.clip = musicToPlay;
		SoundManager.instance.backupMusicSource.Play();
		CamManager.Instance.mainCamEffects.CameraPan(PlayerManager.Instance.player, true);
		playerDestination = new Vector2(67.9f,47.7f);

		movePlayer = true;
		}

	}

	IEnumerator PollutedPeakView(){
		CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
		yield return new WaitForSeconds(.2f);
		CamManager.Instance.mainCamEffects.CameraPan(new Vector3(78.8f,53.8f,-10.8f),"");
		CamManager.Instance.mainCamEffects.ZoomInOut(.9f,2f);
        PlayerManager.Instance.player.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimIdle",true);
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
		int currentSaveSlot = UserDataManager.Instance.GetSlot();
		yield return new WaitForSeconds(4.5f);
		ResetData(currentSaveSlot);
		GlobalVariableManager.Instance.SetDefaultStats();
		SoundManager.instance.backupMusicSource.Stop();
		GameStateManager.Instance.PopAllStates();
		//GameStateManager.Instance.PushState(typeof(TitleState));
		Application.Quit();
		//fader.FadeToScene("TitleScreen2");

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


	static void ResetData(int slot)
    {
        string directory_path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "TGIT");
        if (!Directory.Exists(directory_path)) {
            return;
        }

        string fileName = Path.Combine(directory_path, "UserData_" + slot + ".json");

        if (File.Exists(fileName)) {
            File.Delete(fileName);
        }

        Debug.Log("Data in Slot: " + slot + " has been deleted!");
    }
}

